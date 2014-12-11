using System.Configuration;
using System.Xml;
using Aqueduct.Toggles.Configuration.Layouts;
using Aqueduct.Toggles.Configuration.Renderings;

namespace Aqueduct.Toggles.Configuration
{
    public class FeatureToggleHelpConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("description", IsRequired = false)]
        public CDataElement Description
        {
            get { return (CDataElement)this["description"]; }
        }

        [ConfigurationProperty("requirements", IsRequired = false)]
        public CDataElement Requirements
        {
            get { return (CDataElement)this["requirements"]; }
        }

    }

    public class CDataElement : ConfigurationElement
    {
        protected override void DeserializeElement(XmlReader reader, bool s)
        {
            Value = reader.ReadElementContentAs(typeof(string), null) as string;
        }

        public string Value { get; private set; }
    }

    public class FeatureToggleConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)this["name"]; }
        }

        [ConfigurationProperty("enabled", IsRequired = true)]
        public bool Enabled
        {
            get { return (bool)this["enabled"]; }
        }

        [ConfigurationProperty("help")]
        public FeatureToggleHelpConfigurationElement Help
        {
            get { return (FeatureToggleHelpConfigurationElement)this["help"]; }
        }

        [ConfigurationProperty("languages")]
        internal string Languages
        {
            get { return (string)this["languages"]; }
        }

        [ConfigurationProperty("renderings", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(FeatureRenderingConfigurationCollection), AddItemName = "rendering")]
        internal FeatureRenderingConfigurationCollection Renderings
        {
            get { return base["renderings"] as FeatureRenderingConfigurationCollection; }
        }

        [ConfigurationProperty("items", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(BaseFeatureToggleLayoutConfigurationCollection), AddItemName = "item")]
        internal BaseFeatureToggleLayoutConfigurationCollection Items
        {
            get { return base["items"] as BaseFeatureToggleLayoutConfigurationCollection; }
        }

        [ConfigurationProperty("templates", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(BaseFeatureToggleLayoutConfigurationCollection), AddItemName = "template")]
        internal BaseFeatureToggleLayoutConfigurationCollection Templates
        {
            get { return base["templates"] as BaseFeatureToggleLayoutConfigurationCollection; }
        }
    }
}