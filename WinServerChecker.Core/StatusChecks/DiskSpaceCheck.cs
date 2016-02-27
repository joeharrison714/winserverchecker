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

namespace WinServerChecker.Core.StatusChecks
{
    public class DiskSpaceCheck : IStatusCheck
    {
        const string DriveLetterParamaterName = "driveLetter";
        const string MinPercentFreeSpaceParamaterName = "minPercentFreeSpace";

        private string _driveLetter;
        private int _mintPercentFreeSpace;

        public CheckResult Check()
        {
            DriveInfo di = new DriveInfo(_driveLetter);

            long freeSpace = di.AvailableFreeSpace;
            long size = di.TotalSize;

            var percentFree = (freeSpace + 0.0) / (size + 0.0);

            percentFree = percentFree * 100;

            bool passed = (percentFree > _mintPercentFreeSpace);
            string message = string.Format("{0} drive has {1}% free space", _driveLetter, percentFree.ToString("N0"));

            return new CheckResult()
            {
                Message = message,
                Passed = passed
            };
        }

        public void Initialize(NameValueCollection parameters)
        {
            string theDriveLetter = parameters[DriveLetterParamaterName];

            if (String.IsNullOrWhiteSpace(theDriveLetter))
            {
                throw new ConfigurationErrorsException(string.Format("The {0} check requires a '{1}' parameter in the config file", this.GetType().Name, DriveLetterParamaterName));
            }

            _driveLetter = theDriveLetter;


            string thePercent = parameters[MinPercentFreeSpaceParamaterName];

            if (String.IsNullOrWhiteSpace(theDriveLetter))
            {
                throw new ConfigurationErrorsException(string.Format("The {0} check requires a '{1}' parameter in the config file", this.GetType().Name, MinPercentFreeSpaceParamaterName));
            }

            int thePercentI;
            if (!int.TryParse(thePercent, out thePercentI))
            {
                throw new ConfigurationErrorsException(string.Format("The {0} check requires the '{1}' parameter to be an int", this.GetType().Name, MinPercentFreeSpaceParamaterName));
            }

            _mintPercentFreeSpace = thePercentI;
        }
    }
}
