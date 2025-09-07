#pragma warning disable
using System.ComponentModel;

using ApiCommon;

namespace NativeApi.Api
{
    [ManagedExternName("Alternet.Drawing.Printing.Duplex")]
    [ManagedName("Alternet.Drawing.Printing.Duplex")]
    public enum Duplex
    {
        Simplex,
        Vertical,
        Horizontal
    }
}