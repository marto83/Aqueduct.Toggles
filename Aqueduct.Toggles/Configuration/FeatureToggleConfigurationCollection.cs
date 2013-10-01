using System.Configuration;

namespace Aqueduct.Toggles.Configuration
{
    public class FeatureToggleConfigurationCollection : ConfigurationElementCollection
    {
        internal new FeatureToggleConfigurationElement this[string name]
        {
            get { return (FeatureToggleConfigurationElement)BaseGet(name); }
        }

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