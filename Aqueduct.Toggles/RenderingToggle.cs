using System;
using Aqueduct.Toggles.Configuration.Renderings;

namespace Aqueduct.Toggles
{
    public class RenderingToggle
    {
        public string Name { get; set; }
        public Guid Original { get; set; }
        public Guid New { get; set; }

        internal static RenderingToggle FromConfig(FeatureRenderingConfigurationElement element)
        {
            return new RenderingToggle
            {
                Name = element.Name,
                Original = element.Original,
                New = element.New
            };
        }
    }
}