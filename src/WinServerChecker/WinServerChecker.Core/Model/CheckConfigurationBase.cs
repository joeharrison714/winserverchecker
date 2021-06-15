using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinServerChecker.Core.Model
{
    public abstract class CheckConfigurationBase
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
