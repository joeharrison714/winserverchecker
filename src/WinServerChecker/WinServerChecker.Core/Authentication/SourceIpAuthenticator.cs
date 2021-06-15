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
    public class SourceIpAuthenticator : IAuthenticator
    {
        const string IpsParamaterName = "ips";

        string _ips;

        private static HashSet<string> _ipList;
        private static bool _init;
        private static object _lock = new object();

        public void Initialize(IConfigurationSection _configSection)
        {
            if (!_init)
            {
                lock (_lock)
                {
                    _ips = _configSection[IpsParamaterName];
                    if (String.IsNullOrWhiteSpace(_ips))
                    {
                        throw new Exception(string.Format("The {0} authenticator requires a '{1}' parameter in the config file", this.GetType().Name, IpsParamaterName));
                    }

                    string[] parts = _ips.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);

                    _ipList = new HashSet<string>(parts);


                    _init = true;
                }
            }
        }

        public bool Authenticate(HttpRequest request)
        {
            string sourceip = request.HttpContext.Connection.RemoteIpAddress.ToString();

            return _ipList.Contains(sourceip);
        }


    }
}
