using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WinServerChecker.Authentication;
using WinServerChecker.Configuration;
using WinServerChecker.Formatters;
using WinServerChecker.Interfaces;
using WinServerChecker.Model;

namespace WinServerChecker
{
    public class WinServerCheckerHandler : IHttpHandler
    {
        private static bool _init = false;
        private static object _lock = new object();
        private static ConcurrentDictionary<string, ICheck> _checks;
        private static ConcurrentDictionary<string, IAuthenticator> _authenticators;
        private static bool _authenticatorsOperatorAny;

        public bool IsReusable
        {
            get
            {
                return true;
            }
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
            var section = ConfigurationManager.GetSection(WinServerCheckerConfigurationSection.ConfigurationSectionName);

            if (section == null)
            {
                throw new ConfigurationErrorsException(String.Format("The config section is missing: {0}", WinServerCheckerConfigurationSection.ConfigurationSectionName));
            }

            var wcSection = (WinServerCheckerConfigurationSection)section;

            _authenticatorsOperatorAny = false;
            if (string.Equals(wcSection.AuthenticatorsOperator, "any", StringComparison.OrdinalIgnoreCase))
            {
                _authenticatorsOperatorAny = true;
            }

            _checks = new ConcurrentDictionary<string, ICheck>();

            foreach(ProviderSettings check in wcSection.Checks)
            {
                Type checkType = Type.GetType(check.Type, true);
                var nvConfig = check.Parameters;

                var theCheck = (ICheck)Activator.CreateInstance(checkType, true);
                theCheck.Initialize(nvConfig);

                _checks.TryAdd(check.Name, theCheck);
            }


            _authenticators = new ConcurrentDictionary<string, IAuthenticator>();
            if (wcSection.Authenticators != null && wcSection.Authenticators.Count > 0)
            {
                foreach (ProviderSettings authenticator in wcSection.Authenticators)
                {
                    Type checkType = Type.GetType(authenticator.Type, true);
                    var nvConfig = authenticator.Parameters;

                    var theAuth = (IAuthenticator)Activator.CreateInstance(checkType, true);
                    theAuth.Initialize(nvConfig);

                    _authenticators.TryAdd(authenticator.Name, theAuth);
                }
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            Init();

            if (_authenticators.Count > 0)
            {
                bool anyAuthenticated = false;
                bool allAuthenticated = true;
                foreach (var auth in _authenticators)
                {
                    bool thisAuth = auth.Value.Authenticate(context.Request);
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
                    context.Response.StatusCode = 403;
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("Access denied");
                    return;
                }
            }

            bool overall = true;

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

                if (!scr.Passed) overall = false;
                if (scr.Data == null) scr.Data = new Dictionary<string, object>();

                wcResponse.Checks.Add(scr);
            }

            IFormatter formatter = new JsonFormatter();
            string content = formatter.Format(wcResponse);

            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.Response.ContentType = formatter.ContentType;

            if (!overall)
            {
                context.Response.StatusCode = 500;
            }

            context.Response.Write(content);

        }
    }
}
