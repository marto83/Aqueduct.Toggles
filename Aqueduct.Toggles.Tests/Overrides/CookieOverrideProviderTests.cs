using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Aqueduct.Toggles.Helpers;
using Aqueduct.Toggles.Overrides;
using FluentAssertions;
using Moq;
using NSubstitute;
using NUnit.Framework;

namespace Aqueduct.Toggles.Tests.Overrides
{
    [TestFixture]
    public class CookieOverrideProviderTests
    {
        private CookieOverrideProvider _provider;
        private HttpContextBase _context;
        private List<Override> _validOverrides;

        [SetUp]
        public void Setup()
        {
            _context = Substitute.For<HttpContextBase>();
            _provider = new CookieOverrideProvider(_context);
            _validOverrides = new List<Override> {new Override("test", true)};
        }

        [Test]
        public void GetOverrides_GivenOverridesInContextItems_ReturnsOverrides()
        {
            var items = new Dictionary<string, object>
                        {
                            {CookieOverrideProvider.CookieName, _validOverrides}
                        };
            _context.Items.Returns(items);

            var overrides = _provider.GetOverrides();

            ValidateOverrides(overrides);
        }

        [Test]
        public void GetOverrides_GivenBadCookie_ReturnsEmptyOverrides()
        {
            var request = Substitute.For<HttpRequestBase>();
            request.Cookies.Returns(new HttpCookieCollection
                                    {
                                        new HttpCookie(CookieOverrideProvider.CookieName, "whoops")
                                    });
            _context.Request.Returns(request);

            var overrides = _provider.GetOverrides();

            ValidateEmptyOverrides(overrides);
        }

        [Test]
        public void GetOverrides_GivenGoodCookie_ReturnsOverrides()
        {
            var request = Substitute.For<HttpRequestBase>();
            request.Cookies.Returns(new HttpCookieCollection
                                    {
                                        new HttpCookie(CookieOverrideProvider.CookieName, _validOverrides.Serialize().Encrypt())
                                    });
            _context.Request.Returns(request);

            var overrides = _provider.GetOverrides();

            ValidateOverrides(overrides);
        }

        [Test]
        public void GetOverrides_GivenNoCookie_ReturnsEmptyOverrides()
        {
            var request = Substitute.For<HttpRequestBase>();
            request.Cookies.Returns(new HttpCookieCollection());
            _context.Request.Returns(request);

            var overrides = _provider.GetOverrides();

            ValidateEmptyOverrides(overrides);
        }

        [Test]
        public void SetOverrides_SetsCookie()
        {
            var response = Substitute.For<HttpResponseBase>();
            var cookies = new HttpCookieCollection();
            response.Cookies.Returns(cookies);
            _context.Response.Returns(response);

            _provider.SetOverrides(_validOverrides);

            cookies.Should().HaveCount(1);
            var cookie = cookies[CookieOverrideProvider.CookieName];
            cookie.Should().NotBeNull();
            var overrides = cookie.Value.Decrypt().Deserialize<List<Override>>();
            ValidateOverrides(overrides);
        }


        private static void ValidateOverrides(IEnumerable<Override> overrides)
        {
            var @override = overrides.FirstOrDefault(x => x.Name == "test");
            @override.Should().NotBeNull();
            @override.Enabled.Should().BeTrue();
        }

        private static void ValidateEmptyOverrides(IEnumerable<Override> overrides)
        {
            overrides.Should().NotBeNull();
            overrides.Should().BeEmpty();
        }
    }
}
