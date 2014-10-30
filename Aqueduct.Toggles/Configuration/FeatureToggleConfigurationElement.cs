using System.Configuration;
using Aqueduct.Toggles.Configuration.Layouts;
using Aqueduct.Toggles.Configuration.Renderings;

namespace Aqueduct.Toggles.Configuration
{
    public class FeatureToggleConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)this["name"]; }
        }

        [ConfigurationProperty("enabled", IsRequired = true)]
        public bool Enabled
        {
            get { return (bool)this["enabled"]; }
        }

        [ConfigurationProperty("languages")]
        internal string Languages
        {
            get { return (string)this["languages"]; }
        }

        [ConfigurationProperty("renderings", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(FeatureRenderingConfigurationCollection), AddItemName = "rendering")]
        internal FeatureRenderingConfigurationCollection Renderings
        {
            get { return base["renderings"] as FeatureRenderingConfigurationCollection; }
        }

        [ConfigurationProperty("items", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(BaseFeatureToggleLayoutConfigurationCollection), AddItemName = "item")]
        internal BaseFeatureToggleLayoutConfigurationCollection Items
        {
            get { return base["items"] as BaseFeatureToggleLayoutConfigurationCollection; }
        }

        [ConfigurationProperty("templates", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(BaseFeatureToggleLayoutConfigurationCollection), AddItemName = "template")]
        internal BaseFeatureToggleLayoutConfigurationCollection Templates
        {
            get { return base["templates"] as BaseFeatureToggleLayoutConfigurationCollection; }
        }
    }
}