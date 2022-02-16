using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinServerChecker.Core.Interfaces;
using WinServerChecker.Core.Model;
using System.ServiceProcess;


namespace WinServerChecker.Core.Checks
{
    public class ServiceRunningCheck : ICheck
    {
        const string ProcessNameParamaterName = "ServiceName";

        private string _serviceName;

        public CheckResult Check()
        {
            var cr = new CheckResult();


            var sc = new ServiceController(_serviceName);

            cr.Passed = sc.Status == ServiceControllerStatus.Running;

            cr.Data.Add("status", sc.Status.ToString());

            return cr;
        }

        public void Initialize(IConfigurationSection configSection)
        {
            string theProcessName = configSection[ProcessNameParamaterName];

            if (String.IsNullOrWhiteSpace(theProcessName))
            {
                throw new Exception(string.Format("The {0} check requires a '{1}' parameter in the config file", this.GetType().Name, ProcessNameParamaterName));
            }

            _serviceName = theProcessName;
        }
    }
}
