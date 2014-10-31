using System;
using Aqueduct.Toggles.Configuration.Layouts;

namespace Aqueduct.Toggles
{
    public class LayoutToggleRendering
    {
        public string Name { get; set; }
        public string PlaceHolder { get; set; }
        public Guid SublayoutId { get; set; }

        internal static LayoutToggleRendering FromConfing(FeatureToggleLayoutRenderingConfigurationElement element)
        {
            return new LayoutToggleRendering
            {
                Name = element.Name,
                PlaceHolder = element.Placeholder,
                SublayoutId = element.SublayoutId
            };
        }
    }
}