using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Aqueduct.Toggles.Configuration;

namespace Aqueduct.Toggles
{
    public class FeatureConfiguration
    {
        private IList<Feature> _features = new List<Feature>();

        protected virtual FeatureConfiguration LoadFromConfiguration()
        {
            var config = ConfigurationManager.GetSection("featureToggles") as FeatureToggleConfigurationSection;
            var configuration = new FeatureConfiguration();
            configuration._features = config.Features.Cast<FeatureToggleConfigurationElement>().Select(Feature.FromConfig).ToList();
            return configuration;
        }
        
        public IEnumerable<Feature> AllFeatures => _features;

        public IEnumerable<Feature> EnabledFeatures => _features.Where(IsEnabled);

        public bool IsEnabled(string name)
        {
            return
            IsEnabled(GetFeature(name));
        }

        public bool IsEnabled(Feature feature)
        {
            if (feature == null)
                return false;

            return feature.Enabled;
        }

        public Feature GetFeature(string name)
        {
            return _features.FirstOrDefault(x => x.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}