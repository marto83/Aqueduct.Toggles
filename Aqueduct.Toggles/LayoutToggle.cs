using System;
using System.Collections.Generic;
using System.Linq;
using Aqueduct.Toggles.Configuration.Layouts;

namespace Aqueduct.Toggles
{
    public class LayoutToggle
    {
        public Guid Id { get; set; }
        public Guid? New { get; set; }
        public IList<LayoutToggleRendering> Renderings { get; private set; }

        public LayoutToggle()
        {
            Renderings = new List<LayoutToggleRendering>();
        }

        internal static LayoutToggle FromConfig(BaseFeatureToggleLayoutConfigurationElement element)
        {
            return new LayoutToggle()
            {
                Id = element.Id,
                New = element.New,
                Renderings = element.Renderings.Cast<FeatureToggleLayoutRenderingConfigurationElement>().Select(LayoutToggleRendering.FromConfing).ToList()
            };
        }
    }
}