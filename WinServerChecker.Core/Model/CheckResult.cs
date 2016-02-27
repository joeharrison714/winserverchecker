using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinServerChecker.Core.Model
{
    public class CheckResult
    {
        public bool Passed { get; set; }
        public string Message { get; set; }
    }
}
