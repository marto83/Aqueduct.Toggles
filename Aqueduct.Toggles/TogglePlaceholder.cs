using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Aqueduct.Toggles
{
    [ParseChildren(ChildrenAsProperties = true)]
    public class TogglePlaceholder : CompositeControl
    {
        public string FeatureName { get; set; }

        [Browsable(false)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [TemplateContainer(typeof(TogglePlaceholder))]
        public ITemplate EnabledTemplate { get; set; }

        [Browsable(false)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [TemplateContainer(typeof(TogglePlaceholder))]
        public ITemplate DisabledTemplate { get; set; }

        protected override void CreateChildControls()
        {
            if (FeatureToggles.IsEnabled(FeatureName))
                EnabledTemplate.InstantiateIn(this);
            else
                DisabledTemplate.InstantiateIn(this);

            base.CreateChildControls();
        }

        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            writer.Write("");
        }

        public override void RenderEndTag(HtmlTextWriter writer)
        {
            writer.Write("");
        }
    }
}