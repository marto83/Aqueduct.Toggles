using System.Configuration;

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
}