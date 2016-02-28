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
    public class RequestHeaderAuthenticator : IAuthenticator
    {
        const string HeaderNameParamaterName = "headerName";
        const string HeaderValueParamaterName = "headerValue";

        private string _headerName;
        private string _headerValue;

        public void Initialize(NameValueCollection parameters)
        {
            _headerName = parameters[HeaderNameParamaterName];
            if (String.IsNullOrWhiteSpace(_headerName))
            {
                throw new ConfigurationErrorsException(string.Format("The {0} authenticator requires a '{1}' parameter in the config file", this.GetType().Name, HeaderNameParamaterName));
            }

            _headerValue = parameters[HeaderValueParamaterName];
            if (String.IsNullOrWhiteSpace(_headerValue))
            {
                throw new ConfigurationErrorsException(string.Format("The {0} authenticator requires a '{1}' parameter in the config file", this.GetType().Name, HeaderValueParamaterName));
            }
        }

        public bool Authenticate(HttpRequest request)
        {
            string[] tokens = request.Headers.GetValues(_headerName);

            if (tokens == null || tokens.Length == 0) return false;

            return (string.Equals(tokens[0], _headerValue, StringComparison.OrdinalIgnoreCase));
        }
    }
}
