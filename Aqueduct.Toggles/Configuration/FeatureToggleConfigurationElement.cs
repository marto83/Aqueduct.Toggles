using System.Configuration;
using Aqueduct.Toggles.Configuration.Layouts;
using Aqueduct.Toggles.Configuration.Renderings;

namespace Aqueduct.Toggles.Configuration
{
    public class FeatureToggleConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name => (string)this["name"];

        [ConfigurationProperty("enabled", IsRequired = true)]
        public bool Enabled => (bool)this["enabled"];

        [ConfigurationProperty("help")]
        public FeatureToggleHelpConfigurationElement Help => (FeatureToggleHelpConfigurationElement)this["help"];

        [ConfigurationProperty("languages")]
        internal string Languages => (string)this["languages"];

        [ConfigurationProperty("renderings", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(FeatureRenderingConfigurationCollection), AddItemName = "rendering")]
        internal FeatureRenderingConfigurationCollection Renderings => base["renderings"] as FeatureRenderingConfigurationCollection;

        [ConfigurationProperty("items", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(BaseFeatureToggleLayoutConfigurationCollection), AddItemName = "item")]
        internal BaseFeatureToggleLayoutConfigurationCollection Items => base["items"] as BaseFeatureToggleLayoutConfigurationCollection;

        [ConfigurationProperty("templates", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(BaseFeatureToggleLayoutConfigurationCollection), AddItemName = "template")]
        internal BaseFeatureToggleLayoutConfigurationCollection Templates => base["templates"] as BaseFeatureToggleLayoutConfigurationCollection;
    }
}