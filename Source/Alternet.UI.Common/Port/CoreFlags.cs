#pragma warning disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI.Port
{
    [Flags]
    internal enum CoreFlags : uint
    {
        None = 0x00000000,
        SnapsToDevicePixelsCache = 0x00000001,
        ClipToBoundsCache = 0x00000002,
        MeasureDirty = 0x00000004,
        ArrangeDirty = 0x00000008,
        MeasureInProgress = 0x00000010,
        ArrangeInProgress = 0x00000020,
        NeverMeasured = 0x00000040,
        NeverArranged = 0x00000080,
        MeasureDuringArrange = 0x00000100,
        IsCollapsed = 0x00000200,
        IsKeyboardFocusWithinCache = 0x00000400,
        IsKeyboardFocusWithinChanged = 0x00000800,
        IsMouseOverCache = 0x00001000,
        IsMouseOverChanged = 0x00002000,
        IsMouseCaptureWithinCache = 0x00004000,
        IsMouseCaptureWithinChanged = 0x00008000,
        IsStylusOverCache = 0x00010000,
        IsStylusOverChanged = 0x00020000,
        IsStylusCaptureWithinCache = 0x00040000,
        IsStylusCaptureWithinChanged = 0x00080000,
        HasAutomationPeer = 0x00100000,
        RenderingInvalidated = 0x00200000,
        IsVisibleCache = 0x00400000,
        AreTransformsClean = 0x00800000,
        IsOpacitySuppressed = 0x01000000,
        ExistsEventHandlersStore = 0x02000000,
        TouchesOverCache = 0x04000000,
        TouchesOverChanged = 0x08000000,
        TouchesCapturedWithinCache = 0x10000000,
        TouchesCapturedWithinChanged = 0x20000000,
        TouchLeaveCache = 0x40000000,
        TouchEnterCache = 0x80000000,
    }
}
