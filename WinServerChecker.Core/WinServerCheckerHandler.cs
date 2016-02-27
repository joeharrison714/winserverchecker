using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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

            _checks = new ConcurrentDictionary<string, ICheck>();

            foreach(ProviderSettings check in wcSection.Checks)
            {
                Type checkType = Type.GetType(check.Type, true);
                var nvConfig = check.Parameters;

                var theCheck = (ICheck)Activator.CreateInstance(checkType, true);
                theCheck.Initialize(nvConfig);

                _checks.TryAdd(check.Name, theCheck);
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            Init();

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
                }
                catch (Exception ex)
                {
                    scr.Passed = false;
                    scr.Message = ex.Message;
                }

                if (!scr.Passed) overall = false;

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
