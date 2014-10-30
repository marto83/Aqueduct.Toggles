using System;
using System.Linq;
using NUnit.Framework;

namespace Aqueduct.Toggles.Tests
{
    [TestFixture]
    public class GetLayoutReplacementTests
    {
        [Test]
        public void GetLayoutReplacement_GivenNonExistingElement_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => FeatureToggles.GetLayoutReplacement(Guid.NewGuid(), Guid.NewGuid(), "current"));
        }

        [Test]
        public void GetLayoutReplacement_GivenExistingElement_ReturnsCorrectLayoutReplacement()
        {
            var replacement = FeatureToggles.GetLayoutReplacement(new Guid("{9E316C3C-9494-4C99-8AF6-653560D20F76}"),
                                                                  Guid.NewGuid(), "current");

            Assert.IsNotNull(replacement);
            Assert.AreEqual(new Guid("{0C993911-CCAB-4303-8D6F-9811E0BB0847}"), replacement.LayoutId);
            var sublayouts = replacement.Sublayouts.ToArray();
            Assert.AreEqual(3, replacement.Sublayouts.Count);
            Assert.AreEqual("topnav", sublayouts[0].Placeholder);
            Assert.AreEqual(string.Empty, sublayouts[1].Placeholder);
            Assert.AreEqual(new Guid("{BBDBC750-D502-4B1B-A5B4-77A4AB947DE8}"), sublayouts[0].SublayoutId);
            Assert.AreEqual(new Guid("{039BF107-3806-464E-B137-CF46A139D1F8}"), sublayouts[1].SublayoutId);
        }

        [Test]
        public void GetLayoutReplacement_GivenElementWithoutNewLayoutId_ReturnsNullForLayoutId()
        {
            var replacement = FeatureToggles.GetLayoutReplacement(new Guid("{BC8A19E9-C908-4228-B860-0D895C3885B3}"),
                                                                  Guid.NewGuid(), "current");

            Assert.IsNotNull(replacement);
            Assert.IsNull(replacement.LayoutId);
        }
    }
}
