using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinServerChecker.Interfaces
{
    public interface IAuthenticator
    {
        void Initialize(NameValueCollection parameters);
        bool Authenticate(System.Web.HttpRequest request);
    }
}
