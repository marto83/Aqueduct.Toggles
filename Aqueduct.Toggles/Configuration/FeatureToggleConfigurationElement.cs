using System.Configuration;
using System.Xml;
using Aqueduct.Toggles.Configuration.Layouts;
using Aqueduct.Toggles.Configuration.Renderings;

namespace Aqueduct.Toggles.Configuration
{
    public class FeatureToggleHelpConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("description", IsRequired = false)]
        public CDataElement Description => (CDataElement)this["description"];

        [ConfigurationProperty("releaseDate", IsRequired = false)]
        public CDataElement ReleaseDate => (CDataElement)this["releaseDate"];

        [ConfigurationProperty("issueTrackingReference", IsRequired = false)]
        public CDataElement IssueTrackingReference => (CDataElement)this["issueTrackingReference"];


        [ConfigurationProperty("requirements", IsRequired = false)]
        public CDataElement Requirements => (CDataElement)this["requirements"];
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
        public string Name => (string)this["name"];

        [ConfigurationProperty("enabled", IsRequired = true)]
        public bool Enabled => (bool)this["enabled"];

        [ConfigurationProperty("help")]
        public FeatureToggleHelpConfigurationElement Help => (FeatureToggleHelpConfigurationElement)this["help"];

        [ConfigurationProperty("languages")]
        internal string Languages => (string)this["languages"];

        [ConfigurationProperty("renderings", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(FeatureRenderingConfigurationCollection), AddItemName = "rendering")]
        internal FeatureRenderingConfigurationCollection Renderings => base["renderings"] as FeatureRenderingConfigurationCollection;

        [ConfigurationProperty("items", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(BaseFeatureToggleLayoutConfigurationCollection), AddItemName = "item")]
        internal BaseFeatureToggleLayoutConfigurationCollection Items => base["items"] as BaseFeatureToggleLayoutConfigurationCollection;

        [ConfigurationProperty("templates", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(BaseFeatureToggleLayoutConfigurationCollection), AddItemName = "template")]
        internal BaseFeatureToggleLayoutConfigurationCollection Templates => base["templates"] as BaseFeatureToggleLayoutConfigurationCollection;
    }
}