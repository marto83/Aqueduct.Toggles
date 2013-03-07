using System;
using System.Linq;
using NUnit.Framework;

namespace Aqueduct.Toggles.Tests
{
    [TestFixture]
    public class FeatureToggleTests
    {
        [TestCase("featureenabled", true)]
        [TestCase("featuredisabled", false)]
        [TestCase("featuremissing", false)]
        public void ReadsConfigCorrectly(string feature, bool expected)
        {
            Assert.AreEqual(expected, FeatureToggles.IsEnabled(feature));
        }

        [Test]
        public void GetsSublayoutsCorrectly()
        {
            var sublayouts = FeatureToggles.GetAllSublayoutReplacements().ToArray();

            Assert.AreEqual(1, sublayouts.Length);
            Assert.IsTrue(sublayouts[0].Enabled);
            Assert.AreEqual(new Guid("{AD909EB2-A8E8-484C-BA23-E0CC137142A1}"), sublayouts[0].Original);
            Assert.AreEqual(new Guid("{E8A4B6F9-E787-45A1-AB8A-3883405C4436}"), sublayouts[0].New);
        }

        [TestCase("{78FB424D-565B-4543-91AE-F7C0DC2D8018}", "{03AFA791-2A92-46E8-8A10-47EC6502B633}", true, Description = "Find by template ID")]
        [TestCase("{9E316C3C-9494-4C99-8AF6-653560D20F76}", "{BE1FD360-516C-48CA-A041-81C9C6B87301}", true, Description = "Find by item ID")]
        [TestCase("{CC5932DE-0177-495D-B0C7-652D603AB2F8}", "{8B8C7658-45EA-4241-9E89-C85589544DE6}", false, Description = "Replacement doesn't exist")]
        public void ShouldReplaceLayout_CorrectlyIdentifiesWhetherToReplaceLayout(string itemId, string templateId, bool expected)
        {
            var replace = FeatureToggles.ShouldReplaceLayout(new Guid(itemId), new Guid(templateId));

            Assert.AreEqual(expected, replace);
        }

        [Test]
        public void GetLayoutReplacement_GivenNonExistingElement_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => FeatureToggles.GetLayoutReplacement(Guid.NewGuid(), Guid.NewGuid()));
        }

        [Test]
        public void GetLayoutReplacement_GivenExistingElement_ReturnsCorrectLayoutReplacement()
        {
            var replacement = FeatureToggles.GetLayoutReplacement(new Guid("{9E316C3C-9494-4C99-8AF6-653560D20F76}"),
                                                                  Guid.NewGuid());

            Assert.IsNotNull(replacement);
            Assert.AreEqual(new Guid("{0C993911-CCAB-4303-8D6F-9811E0BB0847}"), replacement.LayoutId);
            var sublayouts = replacement.Sublayouts.ToArray();
            Assert.AreEqual(3, replacement.Sublayouts.Count);
            Assert.AreEqual("topnav", sublayouts[0].Placeholder);
            Assert.AreEqual("main", sublayouts[1].Placeholder);
            Assert.AreEqual(new Guid("{BBDBC750-D502-4B1B-A5B4-77A4AB947DE8}"), sublayouts[0].SublayoutId);
            Assert.AreEqual(new Guid("{039BF107-3806-464E-B137-CF46A139D1F8}"), sublayouts[1].SublayoutId);
        }
    }
}
