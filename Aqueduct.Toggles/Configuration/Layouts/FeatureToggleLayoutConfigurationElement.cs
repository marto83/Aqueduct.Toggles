using System;
using System.Configuration;

namespace Aqueduct.Toggles.Configuration.Layouts
{
    internal class BaseFeatureToggleLayoutConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("id", IsKey = true, IsRequired = true)]
        internal Guid Id
        {
            get { return (Guid)this["id"]; }
        }

        [ConfigurationProperty("newLayoutId")]
        internal Guid? New
        {
            get { return (Guid?)this["newLayoutId"]; }
        }

        [ConfigurationProperty("", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(FeatureToggleLayoutRenderingsConfigurationCollection), AddItemName = "rendering")]
        internal FeatureToggleLayoutRenderingsConfigurationCollection Renderings
        {
            get { return base[""] as FeatureToggleLayoutRenderingsConfigurationCollection; }
        }
    }
}
