using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinServerChecker.Core.Interfaces;
using WinServerChecker.Core.Model;

namespace WinServerChecker.Core.Checks
{
    public class FileExistsCheck : ICheck
    {
        private string path;

        public void Initialize(IConfigurationSection configSection)
        {
            string thePath = configSection["path"];

            if (String.IsNullOrWhiteSpace(thePath))
            {
                throw new Exception(string.Format("The {0} check requires a 'path' parameter in the config file", this.GetType().Name));
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
