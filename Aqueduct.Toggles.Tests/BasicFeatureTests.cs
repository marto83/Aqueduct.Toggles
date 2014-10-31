using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using NUnit.Framework;

namespace Aqueduct.Toggles.Tests
{
    [TestFixture]
    public class BasicFeatureTests
    {
        [TestCase("featureenabled", true)]
        [TestCase("featuredisabled", false)]
        [TestCase("featuremissing", false)]
        public void ReadsBasicFeaturesFromConfigCorrectly(string feature, bool expected)
        {
            Assert.AreEqual(expected, FeatureToggles.IsEnabled(feature));
        }

        [Test]
        public void GetsCssClassStringCorrectly()
        {
            Assert.AreEqual("feat-featureenabled feat-featurewithsublayouts feat-enabledforcurrentlanguage feat-featurewithlayoutbytemplateid feat-featurewithlayoutbyitemid feat-featurewithlayoutdefault feat-featurewithmultiplelayouts", FeatureToggles.GetCssClassesForFeatures("current"));
        }

        [Test]
        public void GetAllFeatures_GivenFeaturesInConfig_ReturnsElevenFeatureToggles()
        {
            var features = FeatureToggles.GetAllFeatures();

            Assert.AreEqual(11, features.Count);
        }

        [Test]
        public void GetAllFeatures_GivenFeatureEnabledConfig_ReturnsEnabledFeature()
        {
            var feature = FeatureToggles.GetAllFeatures().FirstOrDefault(x => x.Name == "featureenabled");

            Assert.IsNotNull(feature);
            Assert.AreEqual("featureenabled", feature.Name);
            Assert.AreEqual(true, feature.Enabled);
        }

        [Test]
        public void GetAllFeatures_GivenFeatureEnabledConfigButOverriddenByTheUser_ReturnsDisabledFeature()
        {
            FeatureToggles.GetUserOverrides = () => new Dictionary<string, bool>() { { "featureenabled", false } };
            var enabled = FeatureToggles.IsEnabled("featureenabled");
            Assert.False(enabled);
        }

        [TearDown]
        public void TearDown()
        {
            FeatureToggles.GetUserOverrides = () => new Dictionary<string, bool>();
        }
    }
}