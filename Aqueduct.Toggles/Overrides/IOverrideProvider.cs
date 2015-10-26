using System.Collections.Generic;

namespace Aqueduct.Toggles.Overrides
{
    public interface IOverrideProvider
    {
        Dictionary<string, bool> GetOverrides();
        void SetOverrides(Dictionary<string, bool> overrides);
    }
}