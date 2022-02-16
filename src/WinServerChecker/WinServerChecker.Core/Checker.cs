using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinServerChecker.Core.Exceptions;
using WinServerChecker.Core.Interfaces;
using WinServerChecker.Core.Model;

namespace WinServerChecker.Core
{
    public class Checker
    {
        private readonly IConfiguration _configuration;

        private static bool _init = false;
        private static object _lock = new object();
        private static ConcurrentDictionary<string, ICheck> _checks;
        private static ConcurrentDictionary<string, IAuthenticator> _authenticators;
        private static bool _authenticatorsOperatorAny;

        public const string ConfigurationRootName = "WinServerChecker";

        public Checker(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        private void Init()
        {
            if (!_init)
            {
                lock (_lock)
                {
                    if (!_init)
                    {
                        DoInit();
                        _init = true;
                    }
                }
            }
        }

        private void DoInit()
        {
            //var configSection = _configuration.GetSection("WinServerChecker").Get<WinServerCheckerConfiguration>();

            var configSection = _configuration.GetSection(ConfigurationRootName);

            var ao = _configuration["AuthenticatorsOperator"];
            _authenticatorsOperatorAny = false;
            if (string.Equals(ao, "any", StringComparison.OrdinalIgnoreCase))
            {
                _authenticatorsOperatorAny = true;
            }

            _checks = new ConcurrentDictionary<string, ICheck>();

            foreach (var check in configSection.GetSection("Checks").GetChildren())
            {
                var cn = check["Name"];
                var ts = check["Type"];

                Type checkType = Type.GetType(ts, true);

                ICheck theCheck = (ICheck)Activator.CreateInstance(checkType);
                theCheck.Initialize(check);

                _checks.TryAdd(cn, theCheck);
            }

            _authenticators = new ConcurrentDictionary<string, IAuthenticator>();
            if (configSection.GetSection("Authenticators") != null)
            {
                foreach (var authenticator in configSection.GetSection("Authenticators").GetChildren())
                {
                    var cn = authenticator["Name"];
                    var ts = authenticator["Type"];

                    Type checkType = Type.GetType(ts, true);

                    var theAuth = (IAuthenticator)Activator.CreateInstance(checkType, true);
                    theAuth.Initialize(authenticator);

                    _authenticators.TryAdd(cn, theAuth);
                }
            }
        }

        public WinServerCheckerResponse Process(HttpRequest request = null){
            Init();

            if (request != null && _authenticators.Count > 0)
            {
                bool anyAuthenticated = false;
                bool allAuthenticated = true;
                foreach (var auth in _authenticators)
                {
                    bool thisAuth = auth.Value.Authenticate(request);
                    if (thisAuth)
                    {
                        anyAuthenticated = true;
                    }
                    else
                    {
                        allAuthenticated = false;
                    }
                }

                bool isAuthenticated = false;

                if (_authenticatorsOperatorAny && anyAuthenticated)
                {
                    isAuthenticated = true;
                }
                else if (allAuthenticated)
                {
                    isAuthenticated = true;
                }

                if (!isAuthenticated)
                {
                    throw new AuthenticationFailedException();
                }
            }

            

            WinServerCheckerResponse wcResponse = new WinServerCheckerResponse();
            wcResponse.Date = DateTime.UtcNow;
            foreach (var check in _checks)
            {
                StatusCheckResponse scr = new StatusCheckResponse();
                scr.Name = check.Key;

                try
                {
                    var thisResult = check.Value.Check();

                    scr.Passed = thisResult.Passed;
                    scr.Message = thisResult.Message;
                    scr.Data = thisResult.Data;
                }
                catch (Exception ex)
                {
                    scr.Passed = false;
                    scr.Message = ex.Message;
                }

                
                if (scr.Data == null) scr.Data = new Dictionary<string, object>();

                wcResponse.Checks.Add(scr);
            }

            return wcResponse;
        }
    }
}
