using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinServerChecker.Core.Model;

namespace WinServerChecker.Core.Interfaces
{
    public interface ICheck
    {
        void Initialize(IConfigurationSection configSection);
        CheckResult Check();
    }
}
