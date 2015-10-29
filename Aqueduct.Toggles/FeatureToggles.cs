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

        public static IOverrideProvider GetOverrideProvider()
        {
            return Configuration.Provider;
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

    }
}
