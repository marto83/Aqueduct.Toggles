using System;
using System.Configuration;

namespace Aqueduct.Toggles.Configuration.Sublayouts
{
    internal class FeatureSublayoutsConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        internal string Name
        {
            get { return (string)this["name"]; }
        }

        [ConfigurationProperty("original", IsRequired = true)]
        internal Guid Original
        {
            get { return (Guid)this["original"]; }
        }

        [ConfigurationProperty("new", IsRequired = true)]
        internal Guid New
        {
            get { return (Guid)this["new"]; }
        }
    }
}