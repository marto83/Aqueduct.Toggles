using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aqueduct.Toggles.Overrides;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace Aqueduct.Toggles.Tests
{
    [TestFixture]
    public class FeatureTogglesTests
    {
        [Test]
        public void GetCurrentOverrides_GivenOverrideProvider_ReturnsOverrides()
        {
            var provider = Substitute.For<IOverrideProvider>();
            provider.GetOverrides().Returns(new Dictionary<string, bool> {{"test1", true}});
            FeatureToggles.SetOverrideProvider(provider);

            var overrides = FeatureToggles.GetCurrentOverrides();

            overrides.Should().HaveCount(1);
            overrides.Should().ContainKey("test1");
            overrides["test1"].Should().BeTrue();
        }
    }
}
