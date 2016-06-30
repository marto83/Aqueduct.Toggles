using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web.SessionState;
using Aqueduct.Toggles.Configuration;
using Aqueduct.Toggles.Overrides;

namespace Aqueduct.Toggles
{
    public static class FeatureToggles
    {
        internal static List<IOverrideProvider> Providers = new List<IOverrideProvider>();

        public static readonly FeatureConfiguration Configuration = new FeatureConfiguration();
        private static object _lock = new object();
        private static List<Action<FeatureConfiguration>> _postConfigLoadedActions = new List<Action<FeatureConfiguration>>();

        public static void PostConfigLoaded(Action<FeatureConfiguration> postConfigLoadedAction)
        {
            lock (_lock)
            {
                if (postConfigLoadedAction != null)
                    _postConfigLoadedActions.Add(postConfigLoadedAction);
            }
        }

        static FeatureToggles()
        {
            ExecutePostLoadedActions();
        }

        private static void ExecutePostLoadedActions()
        {
            lock (_lock)
            {
                foreach (var action in _postConfigLoadedActions)
                {
                    try
                    {
                        action.Invoke(Configuration);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Error executing post loaded action. " + ex.Message);
                    }
                }

                _postConfigLoadedActions.Clear();
            }
        }

        public static void SetOverrideProvider(IOverrideProvider provider)
        {
            Providers.Add(provider);
        }

        public static void ResetProviders()
        {
            Providers.Clear();
        }

        public static IEnumerable<IOverrideProvider> GetOverrideProviders()
        {
            return Providers.ToList();
        }

        public static bool IsEnabled(string name)
        {
            return GetFeatureFlagFromOverrideProviders(name)?[name] ?? Configuration.IsEnabled(name);
        }

        public static string GetCssClassesForFeatures(string currentLanguage)
        {
            var enabled = Configuration.EnabledFeatures.Select(x =>
            {
                var featureEnabled = x.EnabledForLanguage(currentLanguage);
                return featureEnabled ? $"feat-{x.Name}" : $"no-feat-{x.Name}";
            })
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

        private static Dictionary<string, bool> GetFeatureFlagFromOverrideProviders(string name)
        {
            //return first matching provider that has the key
            foreach (var provider in Providers)
            {
                var providerOverride = provider.GetOverrides();
                if (providerOverride?.ContainsKey(name) ?? false)
                {
                    var feature = Configuration.GetFeature(name);
                    if (feature != null)
                    {
                        feature.FeatureOverride = provider.Name;
                    }
                    return new Dictionary<string, bool> { { name, providerOverride[name] } };
                }
            }
            return null;

        }
    }
}
