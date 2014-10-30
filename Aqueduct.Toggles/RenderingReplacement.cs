using System;

namespace Aqueduct.Toggles
{
    public class RenderingReplacement
    {
        public Guid Original { get; set; }
        public Guid New { get; set; }
        public bool Enabled { get; set; }
    }
}
