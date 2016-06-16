using System;
using System.Configuration;

namespace Aqueduct.Toggles.Configuration
{
    internal class FeatureToggleConfigurationSection : ConfigurationSection
    {
        public static Func<FeatureToggleConfigurationSection> Settings = () => settings;

        private static FeatureToggleConfigurationSection settings { get; } = ConfigurationManager.GetSection("featureToggles") as FeatureToggleConfigurationSection;

        [ConfigurationProperty("", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(FeatureToggleConfigurationCollection), AddItemName = "feature")]
        internal FeatureToggleConfigurationCollection Features => base[""] as FeatureToggleConfigurationCollection;
        
        [ConfigurationProperty("encryption")]
        public EncryptionConfigurationElement EncryptionElement => (EncryptionConfigurationElement)this["encryption"];

        [ConfigurationProperty("enableSitecoreOverrides", DefaultValue = false)]
        public bool EnableSitecoreOverrides => (bool) base["enableSitecoreOverrides"];

        [ConfigurationProperty("sitecoreOverridesPath")]
        public string SitecoreOverridesPath => (string) base["sitecoreOverridesPath"];

        [ConfigurationProperty("sitecoreOverridesTemplateName")]
        public string SitecoreOverridesTemplateName => (string) base["sitecoreOverridesTemplateName"];

        public IEncryptionConfiguration Encryption => EncryptionElement;
    }
}
