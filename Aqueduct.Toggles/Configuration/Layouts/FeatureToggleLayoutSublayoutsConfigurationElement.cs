using System;
using System.Configuration;

namespace Aqueduct.Toggles.Configuration.Layouts
{
    internal class FeatureToggleLayoutSublayoutsConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        internal string Name
        {
            get { return (string)this["name"]; }
        }

        [ConfigurationProperty("placeholder", IsRequired = true)]
        internal string Placeholder
        {
            get { return (string)this["placeholder"]; }
        }

        [ConfigurationProperty("id", IsRequired = true)]
        internal Guid SublayoutId
        {
            get { return (Guid)this["id"]; }
        }
    }
}
