using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Aqueduct.Toggles.Configuration;
using Aqueduct.Toggles.Overrides;

namespace Aqueduct.Toggles
{
    public static class FeatureToggles
    {
        private static readonly FeatureToggleConfigurationSection FeatureToggleConfiguration = ConfigurationManager.GetSection("featureToggles") as FeatureToggleConfigurationSection;
        internal static readonly FeatureConfiguration Configuration = new FeatureConfiguration();

        static FeatureToggles()
        {
            if (FeatureToggleConfiguration == null) throw new ConfigurationErrorsException("Missing featureToggles section in config.");

            Configuration.LoadFromConfiguration(FeatureToggleConfiguration);
            SetOverrideProvider(new CookieOverrideProvider());
        }

        public static void SetOverrideProvider(IOverrideProvider provider)
        {
            Configuration.SetOverrideProvider(provider);
        }

        public static bool IsEnabled(string name)
        {
            return Configuration.IsEnabled(name);
        }

        public static string GetCssClassesForFeatures(string currentLanguage)
        {
            var enabled = Configuration.EnabledFeatures.Where(x => x.EnabledForLanguage(currentLanguage))
                                                    .Select(x => $"feat-{x.Name}")
                                                    .ToArray();
            return string.Join(" ", enabled);
        }

        public static IEnumerable<Feature> GetAllFeatures()
        {
            return Configuration.AllFeatures.ToList();
        }

        public static IEnumerable<Feature> GetAllEnabledFeatures()
        {
            return GetAllFeatures().Where(x => IsEnabled(x.Name));
        }

        internal static IEnumerable<RenderingReplacement> GetAllRenderingReplacements(string currentLanguage)
        {
            var allFeatures = Configuration.EnabledFeatures.Where(x => x.EnabledForLanguage(currentLanguage));

            var allSublayouts = new List<RenderingReplacement>();
            foreach (var feature in allFeatures)
            {
                var currentFeature = feature;
                var sublayouts =
                    feature.Renderings.Select(x => new RenderingReplacement
                                            {
                                                Enabled = currentFeature.Enabled,
                                                New = x.New,
                                                Original = x.Original
                                            });

                allSublayouts.AddRange(sublayouts);
            }

            return allSublayouts;
        }

        internal static bool ShouldReplaceLayout(Guid itemId, Guid templateId, string currentLanguage)
        {
            var replacement = GetLayoutReplacementElement(itemId, templateId, currentLanguage);

            return replacement != null;
        }

        internal static LayoutReplacement GetLayoutReplacement(Guid itemId, Guid templateId, string currentLanguage)
        {
            var replacement = GetLayoutReplacementElement(itemId, templateId, currentLanguage);
            if (replacement == null) throw new ArgumentException($"Cannot find replacement layout for item ID: {itemId}, template ID: {templateId}");

            return new LayoutReplacement
                       {
                           NewLayoutId = replacement.New,
                           LayoutId = replacement.New ?? replacement.Id,
                           Sublayouts = replacement.Renderings
                                                   .Select(x => new LayoutReplacement.SublayoutReplacement
                                                                    {
                                                                        SublayoutId = x.SublayoutId,
                                                                        Placeholder = x.PlaceHolder
                                                                    })
                                                   .ToList()
                       };
        }

        private static LayoutToggle GetLayoutReplacementElement(Guid itemId, Guid templateId, string currentLanguage)
        {
            return GetElement(itemId, currentLanguage, x => x.Items) ??
                   GetElement(templateId, currentLanguage, x => x.Templates);
        }

        private static LayoutToggle GetElement(Guid itemId, string currentLanguage, Func<Feature, IList<LayoutToggle>> expression)
        {
            return Configuration
                .EnabledFeatures
                .Where(x => x.EnabledForLanguage(currentLanguage))
                .SelectMany(expression.Invoke)
                .Where(x => x != null)
                .FirstOrDefault(x => x.Id == itemId);
        }
    }
}
