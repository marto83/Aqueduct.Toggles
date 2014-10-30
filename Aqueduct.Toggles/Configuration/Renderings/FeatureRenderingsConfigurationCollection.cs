using System.Configuration;

namespace Aqueduct.Toggles.Configuration.Renderings
{
    internal class FeatureRenderingConfigurationCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new FeatureRenderingConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FeatureRenderingConfigurationElement)element).Name;
        }
    }
}