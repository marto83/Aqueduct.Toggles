using System;
using System.Collections.Generic;
using System.Linq;
using Aqueduct.Toggles.Configuration;

namespace Aqueduct.Toggles
{
    internal class FeatureConfiguration
    {
        private IList<Feature> _features = new List<Feature>();

        internal void LoadFromConfiguration(FeatureToggleConfigurationSection config)
        {
            _features = config.Features.Cast<FeatureToggleConfigurationElement>().Select(Feature.FromConfig).ToList();
        }

        public IEnumerable<Feature> AllFeatures => _features;

        public IEnumerable<Feature> EnabledFeatures => _features.Where(IsEnabled);

        public bool IsEnabled(string name)
        {
            return IsEnabled(GetFeature(name));
        }

        public bool IsEnabled(Feature feature)
        {
            if (feature == null)
                return false;

            var overrides = GetOverrides();
            if (overrides.ContainsKey(feature.Name))
            {
                return overrides[feature.Name];
            }
            return feature.Enabled;
        }

        public Feature GetFeature(string name)
        {
            return _features.FirstOrDefault(x => x.Name == name);
        }

        public Func<Dictionary<string, bool>> GetOverrides = () => new Dictionary<string, bool>();
    }
}