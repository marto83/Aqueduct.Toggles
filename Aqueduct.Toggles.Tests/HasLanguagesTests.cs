using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Aqueduct.Toggles.Tests
{
    [TestFixture]
    public class LanguagesTests
    {
        [TestCase("featureenabled", false)]
        [TestCase("featuredisabled", true)]
        public void HasLanguages_Correct(string name, bool expected)
        {
            var feature = FeatureToggles.Configuration.GetFeature(name);
            Assert.AreEqual(expected, feature.LanguagesList.Any());
        }

        [Test]
        public void HasLanguages_GivenNoLanguages_SetsLanguagesListToEmpty()
        {
            var feature = FeatureToggles.Configuration.GetFeature("featureenabled");

            Assert.IsEmpty(feature.Languages);
        }

        [Test]
        public void HasLanguages_GivenLanguageGets_SetsLanguagesToCorrectValues()
        {
            var feature = FeatureToggles.Configuration.GetFeature("featuredisabled");

            var langArray = feature.LanguagesList;
            Assert.AreEqual(3, langArray.Count);
            Assert.AreEqual("en", langArray[0]);
            Assert.AreEqual("en-gb", langArray[1]);
            Assert.AreEqual("de-de", langArray[2]);
        }
    }
}
