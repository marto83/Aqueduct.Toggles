using System;
using System.Collections.Generic;
using System.Web;
using Aqueduct.Toggles.Helpers;

namespace Aqueduct.Toggles.Overrides
{
    internal class CookieOverrideProvider : IOverrideProvider
    {
        private readonly HttpContextBase _context;
        internal const string CookieName = "Aqueduct.Toggles";

        public CookieOverrideProvider(HttpContextBase context)
        {
            _context = context;
        }

        public CookieOverrideProvider() : this(InitializeContext())
        {
        }

        private static HttpContextBase InitializeContext()
        {
            return HttpContext.Current != null ? new HttpContextWrapper(HttpContext.Current) : null;
        }

        public Dictionary<string, bool> GetOverrides()
        {
            var dictionary = new Dictionary<string, bool>();

            if (_context == null)
                return dictionary;

            var request = _context.Request;

            if (_context.Items[CookieName] != null)
            {
                return _context.Items[CookieName] as Dictionary<string, bool>;
            }

            var cookie = request.Cookies[CookieName];
            if (cookie != null)
            {
                try
                {
                    var decryptedValue = cookie.Value.Decrypt();
                    var features = decryptedValue.Deserialize<Dictionary<string, bool>>();
                    _context.Items[CookieName] = features;

                    return features;
                }
                catch (Exception)
                {
                    return dictionary;
                }
            }
            return dictionary;
        }

        public void SetOverrides(Dictionary<string, bool> overrides)
        {
            var cookie = new HttpCookie(CookieName, overrides.Serialize().Encrypt()) {HttpOnly = true};
            _context.Response.Cookies.Add(cookie);
        }
    }
}
