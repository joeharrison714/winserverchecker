﻿using System;
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
    public class DiskSpaceCheck : ICheck
    {
        const string DriveLetterParamaterName = "driveLetter";
        const string MinPercentFreeSpaceParamaterName = "minPercentFreeSpace";

        private string _driveLetter;
        private int _mintPercentFreeSpace;

        public CheckResult Check()
        {
            var cr = new CheckResult();

            DriveInfo di = new DriveInfo(_driveLetter);

            long freeSpace = di.AvailableFreeSpace;
            long size = di.TotalSize;

            var percentFree = (freeSpace + 0.0) / (size + 0.0);

            percentFree = percentFree * 100;

            cr.Passed = (percentFree > _mintPercentFreeSpace);

            cr.Data.Add("size", size);
            cr.Data.Add("freeSpace", freeSpace);
            cr.Data.Add("percentFree", percentFree);
            cr.Data.Add("percentFreeDisplay", percentFree.ToString("N0") + "%");

            return cr;
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
