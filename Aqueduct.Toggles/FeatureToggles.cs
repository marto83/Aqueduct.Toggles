using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Aqueduct.Toggles.Configuration;
using Aqueduct.Toggles.Configuration.Layouts;
using Aqueduct.Toggles.Configuration.Sublayouts;

namespace Aqueduct.Toggles
{
    public static class FeatureToggles
    {
        private static readonly FeatureToggleConfigurationSection FeatureToggleConfiguration = ConfigurationManager.GetSection("featureToggles") as FeatureToggleConfigurationSection;

        static FeatureToggles()
        {
            if (FeatureToggleConfiguration == null) throw new ConfigurationErrorsException("Missing featureToggles section in config.");
        }

        public static bool IsEnabled(string name)
        {
            var feature = FeatureToggleConfiguration.Features[name];

            return feature != null && feature.Enabled;
        }

        public static IEnumerable<SublayoutReplacement> GetAllSublayoutReplacements()
        {
            var allFeatures = FeatureToggleConfiguration.Features.Cast<FeatureToggleConfigurationElement>().Where(x => x.Enabled);

            var allSublayouts = new List<SublayoutReplacement>();
            foreach (var feature in allFeatures)
            {
                var feature1 = feature;
                var sublayouts =
                    feature.Sublayouts.Cast<FeatureSublayoutsConfigurationElement>()
                           .Select(x => new SublayoutReplacement
                                            {
                                                Enabled = feature1.Enabled,
                                                New = x.New,
                                                Original = x.Original
                                            });

                allSublayouts.AddRange(sublayouts);
            }

            return allSublayouts;
        }

        public static bool ShouldReplaceLayout(Guid itemId, Guid templateId)
        {
            var replacement = GetLayoutReplacementElement(itemId, templateId);

            return replacement != null;
        }

        public static LayoutReplacement GetLayoutReplacement(Guid itemId, Guid templateId)
        {
            var replacement = GetLayoutReplacementElement(itemId, templateId);
            if (replacement == null) throw new ArgumentException(string.Format("Cannot find replacement layout for item ID: {0}, template ID: {1}", itemId, templateId));

            return new LayoutReplacement
                       {
                           LayoutId = replacement.Layout.New,
                           Sublayouts = replacement.Layout.Sublayouts
                                                   .Cast<FeatureToggleLayoutSublayoutsConfigurationElement>()
                                                   .Select(x => new LayoutReplacement.SublayoutReplacement
                                                                    {
                                                                        SublayoutId = x.SublayoutId,
                                                                        Placeholder = x.Placeholder
                                                                    })
                                                   .ToList()
                       };
        }

        private static FeatureToggleConfigurationElement GetLayoutReplacementElement(Guid itemId, Guid templateId)
        {
            return FeatureToggleConfiguration
                .Features
                .Cast<FeatureToggleConfigurationElement>()
                .FirstOrDefault(x => x.Layout != null && (x.Layout.ItemId == itemId || x.Layout.TemplateId == templateId));
        }

        public static List<FeatureToggleConfigurationElement> GetAllFeatures()
        {
            return FeatureToggleConfiguration.Features.Cast<FeatureToggleConfigurationElement>().ToList();
        }
    }
}
