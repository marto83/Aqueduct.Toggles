using System.Configuration;

namespace Aqueduct.Toggles.Configuration.Layouts
{
    internal class FeatureToggleLayoutRenderingsConfigurationCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new FeatureToggleLayoutRenderingConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FeatureToggleLayoutRenderingConfigurationElement)element).Name;
        }
    }
}
