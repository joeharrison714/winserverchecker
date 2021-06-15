using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinServerChecker.Core.Interfaces;
using WinServerChecker.Core.Model;

namespace WinServerChecker.Core.Checks
{
    public class ComputerNameCheck : ICheck
    {
        public CheckResult Check()
        {
            var cr = new CheckResult()
            {
                Passed = true
            };

            cr.Data.Add("computerName", Environment.MachineName);

            return cr;
        }

        public void Initialize(IConfigurationSection configSection)
        {
        }
    }
}
