using System;

namespace Alternet.UI
{
    [Flags]
    public enum BoundsSpecified
    {
        None = 0x00000000,
        X = 0x00000001,
        Y = 0x00000002,
        Location = 0x00000003,
        Width = 0x00000004,
        Height = 0x00000008,
        Size = 0x0000000c,
        All = 0x0000000f
    }
}
