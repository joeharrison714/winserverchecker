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
    public class UptimeCheck : ICheck
    {
        private DateTime _date;

        public UptimeCheck()
        {
            _date = DateTime.UtcNow;
        }

        public CheckResult Check()
        {
            return new CheckResult()
            {
                Passed = true,
                Message = _date.ToString("s")
            };
        }

        public void Initialize(NameValueCollection parameters)
        {
        }
    }
}
