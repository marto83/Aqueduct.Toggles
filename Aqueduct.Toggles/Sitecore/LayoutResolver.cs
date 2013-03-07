using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.HttpRequest;

namespace Aqueduct.Toggles.Sitecore
{
    public class LayoutResolver : global::Sitecore.Pipelines.HttpRequest.LayoutResolver
    {
        public override void Process(HttpRequestArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            var item = Context.Item;
            if (item == null) return;

            var itemId = item.ID.Guid;
            var templateId = item.Template.ID.Guid;
            if (FeatureToggles.ShouldReplaceLayout(itemId, templateId))
            {
                var replacement = FeatureToggles.GetLayoutReplacement(itemId, templateId);

                LayoutItem pageEditLayout = Context.Database.GetItem(new ID(replacement.LayoutId));
                Context.Page.FilePath = pageEditLayout.FilePath;
            }
        }
    }
}
