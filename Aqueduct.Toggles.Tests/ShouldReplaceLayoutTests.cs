using System;
using NUnit.Framework;

namespace Aqueduct.Toggles.Tests
{
    [TestFixture]
    public class ShouldReplaceLayoutTests
    {
        [TestCase("{78FB424D-565B-4543-91AE-F7C0DC2D8018}", "{03AFA791-2A92-46E8-8A10-47EC6502B633}", "current", true, Description = "Find by template ID")]
        [TestCase("{9E316C3C-9494-4C99-8AF6-653560D20F76}", "{BE1FD360-516C-48CA-A041-81C9C6B87301}", "current", true, Description = "Find by item ID")]
        [TestCase("{CC5932DE-0177-495D-B0C7-652D603AB2F8}", "{8B8C7658-45EA-4241-9E89-C85589544DE6}", "current", false, Description = "Replacement doesn't exist")]
        [TestCase("{78FB424D-565B-4543-91AE-F7C0DC2D8018}", "{03AFA791-2A92-46E8-8A10-47EC6502B633}", "fake", false, Description = "Language Wrong")]
        public void ShouldReplaceLayout_CorrectlyIdentifiesWhetherToReplaceLayout(string itemId, string templateId, string currentLanguage, bool expected)
        {
            var replace = FeatureToggles.ShouldReplaceLayout(new Guid(itemId), new Guid(templateId), currentLanguage);

            Assert.AreEqual(expected, replace);
        }

        [Test]
        public void ShouldReplaceLayout_GivenFeatureWithMultipleLayoutsToReplace_ReturnsTrue()
        {
            var replace = FeatureToggles.ShouldReplaceLayout(new Guid("{EBAEDE0D-592A-48B3-B4BF-3E40E93B05E5}"), Guid.NewGuid(), "current");

            Assert.IsTrue(replace);
        }

        [Test]
        public void ShouldReplaceLayout_GivenTemplateId_InDisabledFeature_ReturnsFalse()
        {
            var replace = FeatureToggles.ShouldReplaceLayout(Guid.NewGuid(), new Guid("{30B677BC-C59D-460A-91E6-C9F9298EBC5A}"), "current");

            Assert.IsFalse(replace);
        }

        [Test]
        public void ShouldReplaceLayout_GivenItemId_InDisabledFeature_ReturnsFalse()
        {
            var replace = FeatureToggles.ShouldReplaceLayout(new Guid("{70BF5667-5ECA-440C-BFCE-B6ED24B7EE15}"), Guid.NewGuid(), "current");

            Assert.IsFalse(replace);
        }
    }
}