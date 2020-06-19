using System;
using BluzelleCSharp;
using Microsoft.Extensions.Configuration;
using TestAPI.Interfaces;

namespace TestAPI.Services
{
    public class BlzApi : IBlzApi
    {
        public BlzApi(IConfiguration configuration)
        {
            var config = configuration.GetSection("Bluzelle");

            Api = new BluzelleApi(
                Environment.GetEnvironmentVariable("UUID") ?? config["Namespace"],
                Environment.GetEnvironmentVariable("MNEMONIC") ?? config["Mnemonic"],
                endpoint: Environment.GetEnvironmentVariable("ENDPOINT") ??
                          "http://dev.testnet.public.bluzelle.com:1317"
            );
        }

        public BluzelleApi Api { get; }
    }
}