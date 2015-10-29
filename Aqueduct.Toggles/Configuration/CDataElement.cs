using System.Configuration;
using System.Xml;

namespace Aqueduct.Toggles.Configuration
{
    public class CDataElement : ConfigurationElement
    {
        protected override void DeserializeElement(XmlReader reader, bool s)
        {
            Value = reader.ReadElementContentAs(typeof(string), null) as string;
        }

        public string Value { get; private set; }
    }
}