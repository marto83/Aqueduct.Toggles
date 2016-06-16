using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aqueduct.Toggles.Overrides;

namespace Aqueduct.Toggles.Sitecore
{
    public class SitecoreOverrideProvider : IOverrideProvider
    {
        public string Name => "Sitecore";
        private SitecoreOverrideSettings Settings { get; }

        public SitecoreOverrideProvider()
        {
            Settings = FeatureToggles.Configuration.SitecoreOverrideSettings;
        }

        public IEnumerable<Override> GetOverrides()
        {
            throw new NotImplementedException();
        }

        public void SetOverrides(IEnumerable<Override> overrides)
        {
            throw new NotImplementedException("You can only set overrides from the sitecore interface");
        }
    }
}
