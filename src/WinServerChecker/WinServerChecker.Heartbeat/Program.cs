using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using WinServerChecker.Core;
using WinServerChecker.Heartbeat.Configuration;

namespace WinServerChecker.Heartbeat
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true);

            var config = builder.Build();

            var configSection = config.GetSection(Checker.ConfigurationRootName);

            var heartbeatConfig = configSection.GetSection("HeartbeatEndpoint").Get<HeartbeatEndpointConfiguration>();

            Checker checker = new Checker(config);

            var wcResponse = checker.Process();

            string json = JsonConvert.SerializeObject(wcResponse, Formatting.Indented);

            Console.WriteLine(json);

            Console.WriteLine("===============");

            if (wcResponse.OverallPassed)
            {
                if (!string.IsNullOrWhiteSpace(heartbeatConfig.Url))
                {
                    HttpClient c = new HttpClient();
                    string resp = await c.GetStringAsync(heartbeatConfig.Url);

                    Console.WriteLine(resp);
                }
                else
                {
                    Console.WriteLine("No heartbeat URL was provided");
                }
            }

            using (StreamWriter writer = new StreamWriter("lastResult.json", false))
            {
                writer.Write(json);
            }
        }
    }
}
