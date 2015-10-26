using System.Configuration;

namespace Aqueduct.Toggles.Configuration
{
    internal class FeatureToggleConfigurationCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new FeatureToggleConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FeatureToggleConfigurationElement)element).Name;
        }
    }
}