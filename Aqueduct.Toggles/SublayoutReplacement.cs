using System;

namespace Aqueduct.Toggles
{
    public class SublayoutReplacement
    {
        public Guid Original { get; set; }
        public Guid New { get; set; }
        public bool Enabled { get; set; }
    }
}
