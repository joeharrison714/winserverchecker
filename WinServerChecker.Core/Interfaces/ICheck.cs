using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinServerChecker.Model;

namespace WinServerChecker.Interfaces
{
    public interface ICheck
    {
        void Initialize(NameValueCollection parameters);
        CheckResult Check();
    }
}
