using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinServerChecker.Interfaces;
using WinServerChecker.Model;

namespace WinServerChecker.Checks
{
    public class FileExistsCheck : ICheck
    {
        private string path;

        public void Initialize(NameValueCollection parameters)
        {
            string thePath = parameters["path"];

            if (String.IsNullOrWhiteSpace(thePath))
            {
                throw new ConfigurationErrorsException(string.Format("The {0} check requires a 'path' parameter in the config file", this.GetType().Name));
            }

            path = thePath;
        }

        public CheckResult Check()
        {
            return new CheckResult()
            {
                Passed = File.Exists(path)
            };
        }
    }
}
