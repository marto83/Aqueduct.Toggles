using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aqueduct.Toggles.Overrides;
using Aqueduct.Toggles.Sitecore.Overrides;
using Sitecore.Data.Items;
using SC = Sitecore;

namespace Aqueduct.Toggles.Sitecore.Overrides
{
    public class SitecoreOverrideProvider : IOverrideProvider
    {
        public string Name => "Sitecore";
   
        public Dictionary<string, bool> GetOverrides()
        {
            Dictionary<string, bool> featureDictionary = new Dictionary<string, bool>();

            if (SC.Context.Database != null && SC.Context.Database.Name != "core")
            {
                var featureCollection = SC.Context.Database.GetItem(FeatureToggles.Configuration.FeatureToggleConfigurationSection.SitecoreOverridesPath);

                if (featureCollection != null)
                {
                    foreach (Item child in featureCollection.Children)
                    {
                        featureDictionary.Add(child.Fields["Name"].ToString(), child.Fields["Enabled"].ToString() == "1");
                    }
                }
            }
            return featureDictionary;
        }

        public void SetOverrides(Dictionary<string,bool> overrides)
        {
            throw new NotImplementedException("You can only set overrides from the sitecore interface");
        }
    }
}
