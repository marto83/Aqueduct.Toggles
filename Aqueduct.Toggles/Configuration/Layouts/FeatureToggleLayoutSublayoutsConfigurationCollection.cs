using System.Configuration;

namespace Aqueduct.Toggles.Configuration.Layouts
{
    internal class FeatureToggleLayoutSublayoutsConfigurationCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new FeatureToggleLayoutSublayoutsConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FeatureToggleLayoutSublayoutsConfigurationElement)element).Name;
        }
    }
}
