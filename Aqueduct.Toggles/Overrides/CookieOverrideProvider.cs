using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;
using Aqueduct.Toggles.Helpers;

namespace Aqueduct.Toggles.Overrides
{
    internal class CookieOverrideProvider : IOverrideProvider
    {
        public string Name => "Cookie";

        private readonly HttpContextBase _context;
        internal const string CookieName = "Aqueduct.Toggles";

        public CookieOverrideProvider(HttpContextBase context)
        {
            _context = context;
        }

        public CookieOverrideProvider()
        {
        }

        public IEnumerable<Override> GetOverrides()
        {
            var context = GetCurrentContext();

            if (context == null)
                return Enumerable.Empty<Override>();

            var request = context.Request;

            if (context.Items[CookieName] != null)
            {
                return context.Items[CookieName] as IEnumerable<Override>;
            }

            var cookie = request.Cookies[CookieName];
            if (cookie != null)
            {
                try
                {
                    var decryptedValue = cookie.Value.Decrypt();
                    var features = decryptedValue.Deserialize<List<Override>>();
                    foreach (var feature in features)
                    {
                        feature.ProviderName = Name;
                    }
                    context.Items[CookieName] = features;

                    return features;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error reading cookie value. " + ex.Message);

                    //Expire the cookie
                    cookie.Expires = DateTime.Now.AddYears(-1);
                    context.Request.Cookies.Add(cookie);
                }
            }
            return Enumerable.Empty<Override>();
        }

        private HttpContextBase GetCurrentContext()
        {
            var context = _context;
            if (context == null && HttpContext.Current != null)
                context = new HttpContextWrapper(HttpContext.Current);
            return context;
        }

        public void SetOverrides(IEnumerable<Override> overrides)
        {
            Contract.Assert(overrides != null);
            if (overrides.Any() == false) return;

            var context = GetCurrentContext();

            var cookie = new HttpCookie(CookieName, overrides.Serialize().Encrypt()) { HttpOnly = true };
            context.Response.Cookies.Add(cookie);
        }
    }
}
