using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BluzelleCSharp;
using BluzelleCSharp.Models;
using Microsoft.AspNetCore.Mvc;
using TestAPI.Interfaces;

namespace TestAPI.Controllers
{
    [ApiController]
    [Route("/")]
    [Produces("application/json")]
    public class MainController : ControllerBase
    {
        private readonly BluzelleApi _bz;
        private readonly GasInfo _gas;

        public MainController(IBlzApi blzApi)
        {
            _gas = blzApi.Gas;
            _bz = blzApi.Api;
        }

        [HttpPost]
        public async Task<IActionResult> PostTest(MethodRunRequest req)
        {
            try
            {
                var args = req.Args.Select(i => i.ToString()).ToList();
                switch (req.Method.ToLower())
                {
                    case "create":
                        await _bz.Create(
                            args[0],
                            args[1],
                            new LeaseInfo(args.Count == 3 ? long.Parse(args[2]) : 0),
                            _gas);
                        break;
                    case "update":
                        await _bz.Update(
                            args[0],
                            args[1],
                            new LeaseInfo(args.Count == 3 ? long.Parse(args[2]) : 0),
                            _gas);
                        break;
                    case "delete":
                        await _bz.Delete(args[0], _gas);
                        break;
                    case "rename":
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
                        var data = new Dictionary<string, string>();
                        for (var i = 0; i < args.Count; i += 2)
                            data.Add(args[i], args[i + 1]);
                        await _bz.UpdateMany(data, _gas);
                        break;
                    case "getlease":
                        return Ok(await _bz.GetLease(args[0]));
                    case "txgetlease":
                        return Ok(await _bz.TxGetLease(args[0], _gas));
                    case "renewlease":
                        await _bz.Renew(args[0], new LeaseInfo(long.Parse(args[1])), _gas);
                        break;
                    case "renewleaseall":
                        await _bz.RenewAll(new LeaseInfo(long.Parse(args[0])), _gas);
                        break;
                    case "getnshortestlease":
                        return Ok((await _bz.GetNShortestLease(int.Parse(args[0])))
                            .Select(x => new KeyLease {Key = x.Key, Lease = x.Value}));
                    case "txgetnshortestlease":
                        return Ok((await _bz.TxGetNShortestLease(int.Parse(args[0]), _gas))
                            .Select(x => new KeyLease {Key = x.Key, Lease = x.Value}));
                    case "account":
                        return Ok(await _bz.GetAccount());
                    case "version":
                        return Ok(await _bz.GetVersion());
                    default:
                        return StatusCode(404, $"Method {req.Method} not found");
                }
            }
            catch (Exception exception)
            {
                return StatusCode(400, new ExceptionResult(exception));
            }

            return Ok();
        }
    }
}