using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinServerChecker.Interfaces;
using WinServerChecker.Model;

namespace WinServerChecker.Checks
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

        public void Initialize(NameValueCollection parameters)
        {
        }
    }
}
