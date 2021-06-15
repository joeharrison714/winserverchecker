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
    public class UserAgentAuthenticator : IAuthenticator
    {
        const string ContainsParamaterName = "contains";

        string _contains;

        public void Initialize(IConfigurationSection configSection)
        {
            _contains = configSection[ContainsParamaterName];
            if (String.IsNullOrWhiteSpace(_contains))
            {
                throw new Exception(string.Format("The {0} authenticator requires a '{1}' parameter in the config file", this.GetType().Name, ContainsParamaterName));
            }
        }

        public bool Authenticate(HttpRequest request)
        {
            string theUserAgent = request.Headers["User-Agent"];

            return theUserAgent.ToLower().Contains(_contains.ToLower());
        }
    }
}
