using System.Configuration;

namespace Aqueduct.Toggles.Configuration.Sublayouts
{
    internal class FeatureSublayoutsConfigurationCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new FeatureSublayoutsConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FeatureSublayoutsConfigurationElement)element).Name;
        }
    }
}