using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinServerChecker.Core.Model;

namespace WinServerChecker.Core.Interfaces
{
    public interface IStatusCheck
    {
        void Initialize(NameValueCollection parameters);
        CheckResult Check();
    }
}
