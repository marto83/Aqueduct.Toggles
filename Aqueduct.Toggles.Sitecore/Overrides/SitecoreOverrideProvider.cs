using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Aqueduct.Toggles.Overrides;
using Aqueduct.Toggles.Sitecore.Overrides;
using Sitecore.Data.Items;
using Sitecore.StringExtensions;
using SC = Sitecore;

namespace Aqueduct.Toggles.Sitecore.Overrides
{
    public class SitecoreOverrideProvider : IOverrideProvider
    {
        public string Name => "Sitecore";

        private static readonly ReaderWriterLockSlim ReadWriteLock = new ReaderWriterLockSlim();
        private static Dictionary<string, bool> _sitecoreOverrideDictionary;

        static SitecoreOverrideProvider()
        {
            RefreshSitecoreOverrides();
        }

        public static void RefreshSitecoreOverrides(string databaseName = null)
        {
            ReadWriteLock.EnterWriteLock();
            try
            {
                _sitecoreOverrideDictionary = new Dictionary<string, bool>();

                if (HttpContext.Current != null)
                {
                    SC.Data.Database database;

                    if (databaseName.IsNullOrEmpty())
                    {
                        if (SC.Context.Database != null && SC.Context.Database.Name != "core")
                            database = SC.Context.Database;
                        else
                        {
                            database = SC.Data.Database.GetDatabase(FeatureToggles.Configuration.FeatureToggleConfigurationSection.SitecoreFeatureDatabaseDefault);
                        }
                    }
                    else
                    {
                        database = SC.Data.Database.GetDatabase(databaseName);
                    }

                    var featureCollection =
                        database?.GetItem(
                            FeatureToggles.Configuration.FeatureToggleConfigurationSection.SitecoreOverridesPath);

                    if (featureCollection != null)
                    {
                        foreach (Item child in featureCollection.Children)
                        {
                            _sitecoreOverrideDictionary.Add(child.Fields["Name"].ToString(),
                                child.Fields["Enabled"].ToString() == "1");
                        }
                    }
                }
            }
            finally
            {
                ReadWriteLock.ExitWriteLock();
            }
        }


        public Dictionary<string, bool> GetOverrides()
        {
            return _sitecoreOverrideDictionary;
        }

        public void SetOverrides(Dictionary<string, bool> overrides)
        {
            throw new NotImplementedException("You can only set overrides from the sitecore interface");
        }
    }
}
