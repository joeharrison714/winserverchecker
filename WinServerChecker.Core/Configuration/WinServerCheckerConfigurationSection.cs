using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinServerChecker.Configuration
{
    public class WinServerCheckerConfigurationSection : ConfigurationSection
    {
        public const String ConfigurationSectionName = "winServerChecker";

        private static ConfigurationPropertyCollection _properties;

        static WinServerCheckerConfigurationSection()
        {
            _properties = new ConfigurationPropertyCollection();

            //_properties.Add(_propEndSessionTimeoutSeconds);
            _properties.Add(_propChecks);
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                return _properties;
            }
        }

        //public static readonly ConfigurationProperty _propEndSessionTimeoutSeconds =
        //    new ConfigurationProperty(
        //    "endSessionTimeoutSeconds",
        //    typeof(int),
        //    null,
        //    null,
        //    new System.Configuration.IntegerValidator(1, 1440),
        //    ConfigurationPropertyOptions.None);

        private static readonly ConfigurationProperty _propChecks =
            new ConfigurationProperty(
            "checks",
            typeof(ProviderSettingsCollection),
            null,
            ConfigurationPropertyOptions.None
           );

        //public int EndSessionTimeoutSeconds
        //{
        //    get
        //    {
        //        return (int)base[_propEndSessionTimeoutSeconds];
        //    }
        //    set
        //    {
        //        base[_propEndSessionTimeoutSeconds] = value;
        //    }
        //}

        public ProviderSettingsCollection Checks
        {
            get
            {
                return (ProviderSettingsCollection)base[_propChecks];
            }
            set
            {
                base[_propChecks] = value;
            }
        }
    }
}
