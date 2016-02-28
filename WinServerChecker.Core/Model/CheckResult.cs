using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinServerChecker.Model
{
    public class CheckResult
    {
        public CheckResult()
        {
            Data = new Dictionary<string, object>();
        }

        public bool Passed { get; set; }
        public string Message { get; set; }

        public Dictionary<string,object> Data { get; set; }
    }
}
