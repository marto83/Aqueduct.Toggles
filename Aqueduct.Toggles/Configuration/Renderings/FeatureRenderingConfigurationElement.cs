using System;
using System.Configuration;

namespace Aqueduct.Toggles.Configuration.Renderings
{
    public class FeatureRenderingConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        internal string Name => (string)this["name"];

        [ConfigurationProperty("originalId", IsRequired = true)]
        internal Guid Original => (Guid)this["originalId"];

        [ConfigurationProperty("newId", IsRequired = true)]
        internal Guid New => (Guid)this["newId"];
    }
}