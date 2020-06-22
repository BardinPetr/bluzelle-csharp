using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BluzelleCSharp;
using BluzelleCSharp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TestAPI.Interfaces;
using TestAPI.Models;

namespace TestAPI.Controllers
{
    [ApiController]
    [Route("/")]
    [Produces("application/json")]
    public class MainController : ControllerBase
    {
        private readonly BluzelleApi _bz;
        private GasInfo _gas;

        public MainController(IBlzApi blzApi)
        {
            _bz = blzApi.Api;
        }
        
        public override OkObjectResult Ok(object value)
        {
            Console.WriteLine("RESPONDED WITH");
            Console.WriteLine(JsonConvert.SerializeObject(value));
            Console.WriteLine("----------------------------END----------------------------");
            return base.Ok(value);
        }

        public override ObjectResult StatusCode(int statusCode, object value)
        {
            Console.WriteLine($"ERROR {statusCode}");
            Console.WriteLine(JsonConvert.SerializeObject(value));
            Console.WriteLine("----------------------------END----------------------------");
            return base.StatusCode(statusCode, value);
        }

        [HttpPost]
        public async Task<IActionResult> PostTest(MethodRunRequest req)
        {
            try
            {
                var args = req.Args.Select(i => i.ToString()).ToList();
                Console.Write($"{req.Method} -> ");
                foreach (var s in args) Console.Write($"{s} ");
                Console.WriteLine("");

                _gas = new GasInfo {GasPrice = 10};
                for (var i = 0; i < args.Count; i++)
                {
                    if (!args[i].Contains("gas") && !args[i].Contains("fee")) continue;
                    _gas = JsonConvert.DeserializeObject<GasInfo>(args[i]);
                    args.RemoveAt(i);
                    i--;
                }

                LeaseInfo leaseInfo;
                switch (req.Method.ToLower())
                {
                    case "create":
                        if ((req.Args[0] as dynamic).ValueKind == JsonValueKind.Number)
                            return StatusCode(400, "Key must be a string");
                        if ((req.Args[1] as dynamic).ValueKind == JsonValueKind.Number)
                            return StatusCode(400, "Value must be a string");
                        leaseInfo = args.Count == 3
                            ? JsonConvert.DeserializeObject<LeaseInfo>(args[2]!)
                            : new LeaseInfo();
                        leaseInfo = new LeaseInfo{Days = 10};
                        await _bz.Create(
                            args[0],
                            args[1],
                            leaseInfo,
                            _gas);
                        break;
                    case "update":
                        if ((req.Args[0] as dynamic).ValueKind == JsonValueKind.Number ||
                            (req.Args[1] as dynamic).ValueKind == JsonValueKind.Number)
                            return StatusCode(400);
                        leaseInfo = args.Count == 3
                            ? JsonConvert.DeserializeObject<LeaseInfo>(args[^1]!)
                            : new LeaseInfo();
                        await _bz.Update(
                            args[0],
                            args[1],
                            leaseInfo,
                            _gas);
                        break;
                    case "delete":
                        if ((req.Args[0] as dynamic).ValueKind == JsonValueKind.Number)
                            return StatusCode(400);
                        await _bz.Delete(args[0], _gas);
                        break;
                    case "rename":
                        if ((req.Args[0] as dynamic).ValueKind == JsonValueKind.Number ||
                            (req.Args[1] as dynamic).ValueKind == JsonValueKind.Number)
                            return StatusCode(400);
                        await _bz.Rename(args[0], args[1], _gas);
                        break;
                    case "read":
                        return Ok(await _bz.Read(args[0], args.Count == 2));
                    case "txread":
                        return Ok(await _bz.TxRead(args[0], _gas));
                    case "has":
                        return Ok(await _bz.HasKey(args[0]));
                    case "txhas":
                        return Ok(await _bz.TxHas(args[0], _gas));
                    case "keys":
                        return Ok(await _bz.Keys());
                    case "txkeys":
                        return Ok(await _bz.TxKeys(_gas));
                    case "count":
                        return Ok(await _bz.Count());
                    case "txcount":
                        return Ok(await _bz.TxCount(_gas));
                    case "deleteall":
                        await _bz.DeleteAll(_gas);
                        break;
                    case "keyvalues":
                        var reskv0 = new List<KeyVal>();
                        foreach (var (key, value) in await _bz.GetKeyValues())
                            reskv0.Add(new KeyVal {Key = key, Value = value});
                        return Ok(reskv0);
                    case "txkeyvalues":
                        var reskv1 = new List<KeyVal>();
                        foreach (var (key, value) in await _bz.TxGetKeyValues(_gas))
                            reskv1.Add(new KeyVal {Key = key, Value = value});
                        return Ok(reskv1);
                    case "multiupdate":
                        var res = JsonConvert.DeserializeObject<List<JObject>>(args[0]);
                        var data = res.ToDictionary(i => (string) i["key"], i => (string) i["value"]);
                        await _bz.UpdateMany(data, _gas);
                        break;
                    case "getlease":
                        return Ok(await _bz.GetLease(args[0]));
                    case "txgetlease":
                        return Ok(await _bz.TxGetLease(args[0], _gas));
                    case "renewlease":
                        leaseInfo = args.Count == 2
                            ? JsonConvert.DeserializeObject<LeaseInfo>(args[1]!)
                            : new LeaseInfo();
                        await _bz.Renew(args[0], leaseInfo, _gas);
                        break;
                    case "renewleaseall":
                        leaseInfo = args.Count == 1
                            ? JsonConvert.DeserializeObject<LeaseInfo>(args[0]!)
                            : new LeaseInfo();
                        await _bz.RenewAll(leaseInfo, _gas);
                        break;
                    case "getnshortestleases":
                        return Ok((await _bz.GetNShortestLease(int.Parse(args[0])))
                            .Select(x => new KeyLease {Key = x.Key, Lease = x.Value}));
                    case "txgetnshortestleases":
                        return Ok((await _bz.TxGetNShortestLease(int.Parse(args[0]), _gas))
                            .Select(x => new KeyLease {Key = x.Key, Lease = x.Value}));
                    case "account":
                        return Content(JsonConvert.SerializeObject(await _bz.GetAccount()));
                    case "version":
                        return Ok(await _bz.GetVersion());
                    default:
                        return StatusCode(404, $"Method {req.Method} not found");
                }
            }
            catch (Exception exception)
            {
                return StatusCode(400, exception.Message);
            }

            return Content("null");
        }
    }
}