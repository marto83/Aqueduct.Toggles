using System.Configuration;
using Aqueduct.Toggles.Configuration.Layouts;
using Aqueduct.Toggles.Configuration.Sublayouts;

namespace Aqueduct.Toggles.Configuration
{
    public class FeatureToggleConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        internal string Name
        {
            get { return (string)this["name"]; }
        }

        [ConfigurationProperty("enabled", IsRequired = true)]
        internal bool Enabled
        {
            get { return (bool)this["enabled"]; }
        }

        [ConfigurationProperty("sublayouts", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(FeatureSublayoutsConfigurationCollection), AddItemName = "sublayout")]
        internal FeatureSublayoutsConfigurationCollection Sublayouts
        {
            get { return base["sublayouts"] as FeatureSublayoutsConfigurationCollection; }
        }

        [ConfigurationProperty("layout")]
        internal FeatureToggleLayoutConfigurationElement Layout
        {
            get { return base["layout"] as FeatureToggleLayoutConfigurationElement; }
        }
    }
}