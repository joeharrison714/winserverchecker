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
    public class SourceIpAuthenticator : IAuthenticator
    {
        const string IpsParamaterName = "ips";

        string _ips;

        private static HashSet<string> _ipList;
        private static bool _init;
        private static object _lock = new object();

        public void Initialize(NameValueCollection parameters)
        {
            if (!_init)
            {
                lock (_lock)
                {
                    _ips = parameters[IpsParamaterName];
                    if (String.IsNullOrWhiteSpace(_ips))
                    {
                        throw new ConfigurationErrorsException(string.Format("The {0} authenticator requires a '{1}' parameter in the config file", this.GetType().Name, IpsParamaterName));
                    }

                    string[] parts = _ips.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);

                    _ipList = new HashSet<string>(parts);


                    _init = true;
                }
            }
        }

        public bool Authenticate(HttpRequest request)
        {
            string sourceip = request.UserHostAddress;

            return _ipList.Contains(sourceip);
        }

        
    }
}
