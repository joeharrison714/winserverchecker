using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WinServerChecker.WebApi.Configuration
{
    public class WinServerCheckerConfiguration
    {
        public WinServerCheckerConfiguration()
        {
            Checks = new List<CheckConfiguration>();
        }
        public List<CheckConfiguration> Checks{ get; set; }
    }

    public class CheckConfiguration
    {
        public string Name { get; set; }
        public string Type{ get; set; }
    }
}
