using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinServerChecker.Core.Model
{
    public class StatusCheckResponse
    {
        public StatusCheckResponse()
        {
            Data = new Dictionary<string, object>();
        }
        public string Name { get; set; }
        public bool Passed { get; set; }
        public string Message { get; set; }

        public Dictionary<string, object> Data { get; set; }
    }
}
