using ApiCommon;
using System;

namespace NativeApi.Api
{
    [Flags]
    [ManagedExternName("Alternet.UI.DragDropEffects")]
    [ManagedName("Alternet.UI.DragDropEffects")]
    public enum DragDropEffects
    {
        None = 0,
        Copy = 1 << 0,
        Move = 1 << 1,
        Link = 1 << 2,
    }
}