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
            var currentLanguage = Context.Language.Name;
            if (SitecoreFeatureToggles.ShouldReplaceLayout(itemId, templateId, currentLanguage))
            {
                page.ClearRenderings();

                var renderings = SitecoreFeatureToggles.GetLayoutReplacement(itemId, templateId, currentLanguage);
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

                var sublayoutReplacements = SitecoreFeatureToggles.GetAllRenderingReplacements(currentLanguage);
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

        private static RenderingReference GetRenderingByItemId(Guid itemId, string placeholder)
        {
            var renderingItem = (RenderingItem)Context.Database.GetItem(new ID(itemId));

            if (renderingItem == null) return null;

            var renderingReference = new RenderingReference(renderingItem);
            renderingReference.Settings.Caching = renderingItem.Caching;
            renderingReference.Settings.Conditions = renderingItem.Conditions;
            renderingReference.Settings.DataSource = renderingItem.DataSource;
            renderingReference.Settings.MultiVariateTest = renderingItem.MultiVariateTest;
            renderingReference.Settings.Parameters = renderingItem.Parameters;
            renderingReference.Settings.Placeholder = renderingItem.Placeholder;

            if (!string.IsNullOrWhiteSpace(placeholder))
                renderingReference.Placeholder = placeholder;

            return renderingReference;
        }
    }
}
