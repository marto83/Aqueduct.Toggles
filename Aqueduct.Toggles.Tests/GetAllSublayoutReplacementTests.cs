using System;
using System.Linq;
using NUnit.Framework;

namespace Aqueduct.Toggles.Tests
{
    [TestFixture]
    public class GetAllSublayoutReplacementTests
    {
        [Test]
        public void GetsSublayoutsCorrectly_IgnoringDisabledFeaturesAndOnesForOtherLanguages()
        {
            var sublayouts = FeatureToggles.GetAllRenderingReplacements("current").ToArray();

            Assert.AreEqual(3, sublayouts.Length);
            Assert.IsTrue(sublayouts[0].Enabled);
            Assert.AreEqual(new Guid("{AD909EB2-A8E8-484C-BA23-E0CC137142A1}"), sublayouts[0].Original);
            Assert.AreEqual(new Guid("{E8A4B6F9-E787-45A1-AB8A-3883405C4436}"), sublayouts[0].New);
            Assert.AreEqual(new Guid("{E1D37C73-44D6-4F62-B8AD-FE16CBC6C0E9}"), sublayouts[1].Original);
            Assert.AreEqual(new Guid("{95197DC8-C3E9-4BF0-AAEF-327DCECF4436}"), sublayouts[1].New);
            Assert.AreEqual(new Guid("{390A83A2-06FA-42BD-80AA-988C545439C8}"), sublayouts[2].Original);
            Assert.AreEqual(new Guid("{79B92D87-D3A1-41D9-8A6B-2E4FF38A3827}"), sublayouts[2].New);
        }
    }
}