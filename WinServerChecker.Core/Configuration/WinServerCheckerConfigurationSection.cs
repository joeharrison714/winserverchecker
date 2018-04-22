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

            _properties.Add(_propAuthenticatorsOperator);
            _properties.Add(_propChecks);
            _properties.Add(_propAuthenticators);
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                return _properties;
            }
        }

        public static readonly ConfigurationProperty _propAuthenticatorsOperator =
            new ConfigurationProperty(
            "authenticatorsOperator",
            typeof(string),
            null,
            null,
            null,
            ConfigurationPropertyOptions.None);

        private static readonly ConfigurationProperty _propChecks =
            new ConfigurationProperty(
            "checks",
            typeof(ProviderSettingsCollection),
            null,
            ConfigurationPropertyOptions.None
           );

        private static readonly ConfigurationProperty _propAuthenticators =
            new ConfigurationProperty(
            "authenticators",
            typeof(ProviderSettingsCollection),
            null,
            ConfigurationPropertyOptions.None
           );

        public string AuthenticatorsOperator
        {
            get
            {
                return (string)base[_propAuthenticatorsOperator];
            }
            set
            {
                base[_propAuthenticatorsOperator] = value;
            }
        }

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

        public ProviderSettingsCollection Authenticators
        {
            get
            {
                return (ProviderSettingsCollection)base[_propAuthenticators];
            }
            set
            {
                base[_propAuthenticators] = value;
            }
        }
    }
}
