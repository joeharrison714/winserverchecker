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
    public class CPUCheck : ICheck
    {
        private PerformanceCounter theCPUCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");

        public CPUCheck()
        {
            theCPUCounter.NextValue();
        }

        public CheckResult Check()
        {
            var cpu = this.theCPUCounter.NextValue();

            var cr = new CheckResult()
            {
                Passed = true
            };

            cr.Data.Add("cpu", cpu);
            return cr;
        }

        public void Initialize(IConfigurationSection configSection)
        {
        }
    }
}
