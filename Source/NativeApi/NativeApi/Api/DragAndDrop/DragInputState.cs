using System;

namespace NativeApi.Api
{
    [Flags]
    public enum DragInputState
    {
        None = 0,
        LeftMouseButtonPressed = 1 << 0,
        RightMouseButtonPressed = 1 << 1,
        ShiftKeyPressed = 1 << 2,
        ControlKeyPressed = 1 << 3,
        MiddleMouseButtonPressed = 1 << 4,
        AltKeyPressed = 1 << 5,
        OptionKeyPressed = 1 << 6,
        CommandKeyPressed = 1 << 7,
    }
}