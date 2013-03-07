using System;
using System.Linq;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Layouts;
using Sitecore.Pipelines.RenderLayout;

namespace Aqueduct.Toggles.Sitecore
{
    public class RenderLayoutProcessor : global::Sitecore.Pipelines.RenderLayout.RenderLayoutProcessor
    {
        public override void Process(RenderLayoutArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            var item = Context.Item;
            if (item == null) return;

            var itemId = item.ID.Guid;
            var templateId = item.Template.ID.Guid;

            var page = Context.Page;
            if (FeatureToggles.ShouldReplaceLayout(itemId, templateId))
            {
                page.ClearRenderings();

                var renderings = FeatureToggles.GetLayoutReplacement(itemId, templateId);
                foreach (var sublayout in renderings.Sublayouts)
                {
                    var rendering = GetRenderingByItemId(sublayout.SublayoutId, sublayout.Placeholder);
                    if (rendering != null)
                        page.AddRendering(rendering);
                }
            }
            else
            {
                var originalRenderings = page.Renderings.ToList();

                var sublayoutReplacements = FeatureToggles.GetAllSublayoutReplacements();
                var foundReplacement = false;
                foreach (var replacement in sublayoutReplacements)
                {
                    for (var i = 0; i < originalRenderings.Count; i++)
                    {
                        var reference = originalRenderings[i];
                        if (reference.RenderingID.Guid == replacement.Original)
                        {
                            foundReplacement = true;
                            var newRendering = GetRenderingByItemId(replacement.New, reference.Placeholder);
                            originalRenderings[i] = newRendering;
                        }
                    }
                }

                if (foundReplacement)
                {
                    page.ClearRenderings();
                    foreach (var reference in originalRenderings)
                    {
                        page.AddRendering(reference);
                    }
                }
            }
        }

        private RenderingReference GetRenderingByItemId(Guid itemId, string placeholder)
        {
            var renderingItem = Context.Database.GetItem(new ID(itemId));
            if (renderingItem == null) return null;

            var rendering = new RenderingItem(renderingItem);
            var renderingReference = new RenderingReference(rendering) { Placeholder = placeholder };

            return renderingReference;
        }
    }
}
