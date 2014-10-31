using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Pipelines.Response.BuildPageDefinition;
using Sitecore.Mvc.Presentation;

namespace Aqueduct.Toggles.Sitecore
{
    public class ProcessXmlBasedLayoutDefinition :
        global::Sitecore.Mvc.Pipelines.Response.BuildPageDefinition.ProcessXmlBasedLayoutDefinition
    {
        public override void Process(BuildPageDefinitionArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
      
            var item = Context.Item;
            if (item == null) return;

            var itemId = item.ID.Guid;
            var templateId = item.Template.ID.Guid;

            var currentLanguage = Context.Language.Name;
            var renderingBuilder = new RenderingBuilder();
            PageDefinition pageDefinition = args.Result;

            if (pageDefinition == null)
                return;

            if (FeatureToggles.ShouldReplaceLayout(itemId, templateId, currentLanguage))
            {
                //resolve all the renderings from the featuretoggle
                var layoutReplacement = FeatureToggles.GetLayoutReplacement(itemId, templateId, currentLanguage);
                var layoutRendering = renderingBuilder.GetRenederingById(layoutReplacement.LayoutId);
                layoutRendering.RenderingType = "Layout";
                pageDefinition.Renderings.Add(layoutRendering);

                foreach (var sublayout in layoutReplacement.Sublayouts)
                {
                    var rendering = renderingBuilder.GetRenederingById(sublayout.SublayoutId);
                    if (rendering != null)
                        pageDefinition.Renderings.Add(rendering);
                }
            }
            else
            {
                base.Process(args);
                var renderingReplacements = FeatureToggles.GetAllRenderingReplacements(currentLanguage);
                //loop through and replace the one I need to replace
                foreach (var replacement in renderingReplacements)
                {
                    ProcessRenderings(pageDefinition.Renderings, replacement, renderingBuilder);
                }
            }

        }

        private void ProcessRenderings(List<Rendering> renderings, RenderingReplacement replacement,
            RenderingBuilder renderingBuilder)
        {
            for (var i = 0; i < renderings.Count; i++)
            {
                var rendering = renderings[i];
                if (rendering.RenderingItem.ID.Guid == replacement.Original)
                {
                    var newRendering = renderingBuilder.GetRenederingById(replacement.New);
                    newRendering.Placeholder = rendering.Placeholder;
                    newRendering.DeviceId = rendering.DeviceId;
                    newRendering.LayoutId = rendering.LayoutId;
                    renderings[i] = newRendering;
                }

                if(rendering.ChildRenderings.Any())
                    ProcessRenderings(rendering.ChildRenderings, replacement, renderingBuilder);
            }
        }

        private class RenderingBuilder : XmlBasedRenderingParser
        {
            public Rendering GetRenederingById(Guid id)
            {
                var rendering = new Rendering() {RenderingItemPath = id.ToString()};
                rendering.UniqueId = Guid.NewGuid();
                base.AddRenderingItemProperties(rendering);
                return rendering;
            }
        }
    }
}