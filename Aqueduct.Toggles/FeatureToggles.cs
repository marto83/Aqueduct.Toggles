using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Aqueduct.Toggles.Configuration;

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
            Configuration.GetOverrides = GetUserOverrides;
        }

        public static Func<Dictionary<string, bool>> GetUserOverrides
        {
            get { return Configuration.GetOverrides; }
            set { Configuration.GetOverrides = value; }
        }

        public static bool IsEnabled(string name)
        {
            return Configuration.IsEnabled(name);
        }

        public static string GetCssClassesForFeatures(string currentLanguage)
        {
            var enabled = Configuration.EnabledFeatures.Where(x => x.EnabledForLanguage(currentLanguage))
                                                    .Select(x => string.Concat("feat-", x.Name))
                                                    .ToArray();
            return string.Join(" ", enabled);
        }

        public static IList<Feature> GetAllFeatures()
        {
            return Configuration.AllFeatures.ToList();
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
            if (replacement == null) throw new ArgumentException(string.Format("Cannot find replacement layout for item ID: {0}, template ID: {1}", itemId, templateId));

            return new LayoutReplacement
                       {
                           LayoutId = replacement.New,
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
