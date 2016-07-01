using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aqueduct.Toggles.Sitecore.Overrides;
using Sitecore.Data;
using Sitecore.Data.Events;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Events;

namespace Aqueduct.Toggles.Sitecore.EventHandlers
{
    public class FeatureEventHandler
    {
        //Delivery DBs
        public void OnPublishEndRemoteHandler(object sender, EventArgs args)
        {
            var remoteEventArgs = args as PublishEndRemoteEventArgs;
            if (remoteEventArgs != null)
            {
                var database = Database.GetDatabase(remoteEventArgs.TargetDatabaseName);
                if (database != null)
                {
                    var rootItem = database.GetItem(new ID(remoteEventArgs.RootItemId));
                    if ((remoteEventArgs.Deep &&
                        FeatureToggles.Configuration.FeatureToggleConfigurationSection.SitecoreOverridesPath.Contains(
                            rootItem.Paths.Path)) ||
                        rootItem.Paths.Path.Contains(
                            FeatureToggles.Configuration.FeatureToggleConfigurationSection.SitecoreOverridesPath))
                    {
                        SitecoreOverrideProvider.RefreshSitecoreOverrides(remoteEventArgs.TargetDatabaseName);
                    }
                }
            }

        }

        //Master DB
        public void OnItemSavedHandler(object sender, EventArgs args)
        {
            var item = Event.ExtractParameter(args, 0) as Item;

            if (item != null && item.Paths.Path.Contains(FeatureToggles.Configuration.FeatureToggleConfigurationSection.SitecoreOverridesPath)
                && item.Database.Name.Equals("master"))
            {
                SitecoreOverrideProvider.RefreshSitecoreOverrides(item.Database.Name);
            }
        }

    }
}
