using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinServerChecker.Core.Interfaces;

namespace WinServerChecker.Core.Authentication
{
    public class RequestHeaderAuthenticator : IAuthenticator
    {
        const string HeaderNameParamaterName = "headerName";
        const string HeaderValueParamaterName = "headerValue";

        private string _headerName;
        private string _headerValue;

        public void Initialize(IConfigurationSection configSection)
        {
            _headerName = configSection[HeaderNameParamaterName];
            if (String.IsNullOrWhiteSpace(_headerName))
            {
                throw new Exception(string.Format("The {0} authenticator requires a '{1}' parameter in the config file", this.GetType().Name, HeaderNameParamaterName));
            }

            _headerValue = configSection[HeaderValueParamaterName];
            if (String.IsNullOrWhiteSpace(_headerValue))
            {
                throw new Exception(string.Format("The {0} authenticator requires a '{1}' parameter in the config file", this.GetType().Name, HeaderValueParamaterName));
            }
        }

        public bool Authenticate(HttpRequest request)
        {
            string[] tokens = request.Headers[_headerName];

            if (tokens == null || tokens.Length == 0) return false;

            return (string.Equals(tokens[0], _headerValue, StringComparison.OrdinalIgnoreCase));
        }
    }
}
