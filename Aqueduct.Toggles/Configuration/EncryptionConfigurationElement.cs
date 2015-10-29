using System.Configuration;

namespace Aqueduct.Toggles.Configuration
{
    public class EncryptionConfigurationElement : ConfigurationElement, IEncryptionConfiguration
    {
        [ConfigurationProperty("key", IsRequired = true)]
        public string Key => (string) this["key"];
    }
}