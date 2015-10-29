using System.Configuration;

namespace Aqueduct.Toggles.Configuration.Layouts
{
    internal class BaseFeatureToggleLayoutConfigurationCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new BaseFeatureToggleLayoutConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((BaseFeatureToggleLayoutConfigurationElement)element).Id;
        }
    }
}
