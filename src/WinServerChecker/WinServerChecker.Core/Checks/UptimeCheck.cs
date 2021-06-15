using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinServerChecker.Core.Interfaces;
using WinServerChecker.Core.Model;

namespace WinServerChecker.Core.Checks
{
    public class UptimeCheck : ICheck
    {
        private PerformanceCounter uptime = new PerformanceCounter("System", "System Up Time");

        public UptimeCheck()
        {
            uptime.NextValue();
        }

        public CheckResult Check()
        {
            var cr = new CheckResult()
            {
                Passed = true
            };

            cr.Data.Add("uptime", TimeSpan.FromSeconds(uptime.NextValue()).ToString());

            return cr;
        }

        public void Initialize(IConfigurationSection configSection)
        {
        }
    }
}
