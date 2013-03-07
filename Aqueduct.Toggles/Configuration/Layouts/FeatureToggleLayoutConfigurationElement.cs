using System;
using System.Configuration;

namespace Aqueduct.Toggles.Configuration.Layouts
{
    internal class FeatureToggleLayoutConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("item")]
        internal Guid ItemId
        {
            get { return (Guid)this["item"]; }
        }

        [ConfigurationProperty("template")]
        internal Guid TemplateId
        {
            get { return (Guid)this["template"]; }
        }

        [ConfigurationProperty("new", IsRequired = true)]
        internal Guid New
        {
            get { return (Guid)this["new"]; }
        }

        [ConfigurationProperty("", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(FeatureToggleLayoutSublayoutsConfigurationCollection), AddItemName = "sublayout")]
        internal FeatureToggleLayoutSublayoutsConfigurationCollection Sublayouts
        {
            get { return base[""] as FeatureToggleLayoutSublayoutsConfigurationCollection; }
        }
    }
}
