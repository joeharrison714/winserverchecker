using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinServerChecker.Model
{
    public class StatusCheckResponse
    {
        public string Name { get; set; }
        public bool Passed { get; set; }
        public string Message { get; set; }
    }
}
