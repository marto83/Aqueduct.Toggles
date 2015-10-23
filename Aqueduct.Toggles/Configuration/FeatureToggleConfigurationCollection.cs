using System.Configuration;

namespace Aqueduct.Toggles.Configuration
{
    internal class FeatureToggleConfigurationCollection : ConfigurationElementCollection
    {
        internal new FeatureToggleConfigurationElement this[string name] => (FeatureToggleConfigurationElement)BaseGet(name);

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