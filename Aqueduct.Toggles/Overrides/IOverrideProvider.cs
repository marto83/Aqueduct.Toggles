using System.Collections.Generic;

namespace Aqueduct.Toggles.Overrides
{
    public interface IOverrideProvider
    {
        string Name { get; }
        IEnumerable<Override> GetOverrides();
        void SetOverrides(IEnumerable<Override> overrides);
    }
}