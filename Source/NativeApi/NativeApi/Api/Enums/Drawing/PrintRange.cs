#pragma warning disable
using ApiCommon;

namespace NativeApi.Api
{
    [ManagedExternName("Alternet.Drawing.Printing.PrintRange")]
    [ManagedName("Alternet.Drawing.Printing.PrintRange")]
    public enum PrintRange
    {
        AllPages,
        Selection,
        SomePages
    }
}