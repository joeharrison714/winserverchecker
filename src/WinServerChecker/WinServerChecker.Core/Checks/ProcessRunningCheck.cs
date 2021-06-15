using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinServerChecker.Core.Interfaces;
using WinServerChecker.Core.Model;

namespace WinServerChecker.Core.Checks
{
    public class ProcessRunningCheck : ICheck
    {
        const string ProcessNameParamaterName = "ProcessName";
        const string MinParamaterName = "MinInstances";
        const string MaxParamaterName = "MaxInstances";

        private string _processName;
        private int? _min=1;
        private int? _max;

        public void Initialize(IConfigurationSection configSection)
        {
            string theProcessName = configSection[ProcessNameParamaterName];

            if (String.IsNullOrWhiteSpace(theProcessName))
            {
                throw new Exception(string.Format("The {0} check requires a '{1}' parameter in the config file", this.GetType().Name, ProcessNameParamaterName));
            }

            _processName = theProcessName;

            string min = configSection[MinParamaterName];
            if (!string.IsNullOrWhiteSpace(min))
            {
                _min = int.Parse(min);
            }

            string max = configSection[MaxParamaterName];
            if (!string.IsNullOrWhiteSpace(max))
            {
                _max = int.Parse(max);
            }
        }

        public CheckResult Check()
        {
            var cr = new CheckResult();

            var processes = Process.GetProcessesByName(_processName);

            bool allPassed = true;

            if (_min.HasValue && processes.Count() < _min.Value)
                allPassed = false;

            if (_max.HasValue && processes.Count() > _max.Value)
                allPassed = false;

            cr.Passed = allPassed;

            cr.Data.Add("processCount", processes.Count());
            cr.Data.Add("processName", _processName);

            return cr;
        }
    }
}
