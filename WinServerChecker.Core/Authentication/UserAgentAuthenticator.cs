using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WinServerChecker.Interfaces;

namespace WinServerChecker.Authentication
{
    public class UserAgentAuthenticator : IAuthenticator
    {
        const string ContainsParamaterName = "contains";

        string _contains;

        public void Initialize(NameValueCollection parameters)
        {
            _contains = parameters[ContainsParamaterName];
            if (String.IsNullOrWhiteSpace(_contains))
            {
                throw new ConfigurationErrorsException(string.Format("The {0} authenticator requires a '{1}' parameter in the config file", this.GetType().Name, ContainsParamaterName));
            }
        }

        public bool Authenticate(HttpRequest request)
        {
            string theUserAgent = request.UserAgent;

            return theUserAgent.ToLower().Contains(_contains.ToLower());
        }
    }
}
