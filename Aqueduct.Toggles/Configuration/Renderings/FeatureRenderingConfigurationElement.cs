using System;
using System.Configuration;

namespace Aqueduct.Toggles.Configuration.Renderings
{
    public class FeatureRenderingConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        internal string Name
        {
            get { return (string)this["name"]; }
        }

        [ConfigurationProperty("originalId", IsRequired = true)]
        internal Guid Original
        {
            get { return (Guid)this["originalId"]; }
        }

        [ConfigurationProperty("newId", IsRequired = true)]
        internal Guid New
        {
            get { return (Guid)this["newId"]; }
        }
    }
}