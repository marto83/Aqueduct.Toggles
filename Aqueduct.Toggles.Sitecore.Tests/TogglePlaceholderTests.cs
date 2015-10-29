using System;
using FluentAssertions;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using NUnit.Framework;

namespace Aqueduct.Toggles.Sitecore.Tests
{

    [TestFixture]
    public class TogglePlaceholderTests
    {
        [Test]
        public void RenderBeginTag()
        {
            GetRenderedHtml(new TogglePlaceholder(), (placeholder, writer) => placeholder.RenderBeginTag(writer))
                .Should().Be(string.Empty);
        }

        [Test]
        public void RenderEndTag()
        {
            GetRenderedHtml(new TogglePlaceholder(), (placeholder, writer) => placeholder.RenderEndTag(writer))
                .Should().Be(string.Empty);
        }

        [Test]
        public void GivenFeatureEnabled_RendersChildrenFromEnabledPlaceholder()
        {
            GetHtmlForPlaceholder("featureenabled")
                .Should().Contain("enabledTemplate").And.NotContain("disabledTemplate");
        }

        [Test]
        public void GivenFeatureDisabled_RendersChildrenFromEnabledPlaceholder()
        {
            GetHtmlForPlaceholder("featuredisabled")
                .Should().Contain("disabledTemplate").And.NotContain("enabledTemplate");
        }

        #region Helpers
        private static string GetRenderedHtml<TControl>(TControl placeholder, Action<TControl, HtmlTextWriter> getHtml)
        {
            using (var stringWriter = new StringWriter())
            using (var htmlWriter = new HtmlTextWriter(stringWriter))
            {
                getHtml(placeholder, htmlWriter);

                return stringWriter.ToString();
            }
        }

        private static string GetHtmlForPlaceholder(string featureName)
        {
            var control = new TogglePlaceholder
            {
                FeatureName = featureName,
                EnabledTemplate = new FakeTemplate("enabledTemplate"),
                DisabledTemplate = new FakeTemplate("disabledTemplate")
            };
            control.Controls.Should().NotBeNull(); //This line needs to be here because WebForms is hell 

            var output = GetRenderedHtml(control, (placeholder, writer) => placeholder.RenderControl(writer));
            return output;
        }

        private class FakeTemplate : ITemplate
        {
            private readonly string _templateName;

            public FakeTemplate(string templateName)
            {
                _templateName = templateName;
            }

            public void InstantiateIn(Control container)
            {
                var literal = new Literal { Text = _templateName };
                container.Controls.Add(literal);
            }
        }
        #endregion
    }
}
