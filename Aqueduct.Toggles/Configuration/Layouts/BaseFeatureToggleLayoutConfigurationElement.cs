using System;
using System.Configuration;

namespace Aqueduct.Toggles.Configuration.Layouts
{
    internal class BaseFeatureToggleLayoutConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("id", IsKey = true, IsRequired = true)]
        internal Guid Id => (Guid)this["id"];

        [ConfigurationProperty("newLayoutId")]
        internal Guid? New => (Guid?)this["newLayoutId"];

        [ConfigurationProperty("", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(FeatureToggleLayoutRenderingsConfigurationCollection), AddItemName = "rendering")]
        internal FeatureToggleLayoutRenderingsConfigurationCollection Renderings => base[""] as FeatureToggleLayoutRenderingsConfigurationCollection;
    }
}
