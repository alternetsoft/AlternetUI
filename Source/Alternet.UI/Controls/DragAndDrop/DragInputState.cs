using System;

namespace Alternet.UI
{
    /// <summary>
    /// The bits that are set identify the keys or mouse buttons that are pressed during the drag-and-drop operation.
    /// </summary>
    [Flags]
    public enum DragInputState
    {
        /// <summary>
        /// No keys or mouse buttons are pressed.
        /// </summary>
        None = 0,

        /// <summary>
        /// Left mouse button is pressed.
        /// </summary>
        LeftMouseButtonPressed = 1 << 0,

        /// <summary>
        /// Right mouse button is pressed.
        /// </summary>
        RightMouseButtonPressed = 1 << 1,

        /// <summary>
        /// Shift key is pressed.
        /// </summary>
        ShiftKeyPressed = 1 << 2,

        /// <summary>
        /// Control key is pressed.
        /// </summary>
        ControlKeyPressed = 1 << 3,

        /// <summary>
        /// Middle mouse button is pressed.
        /// </summary>
        MiddleMouseButtonPressed = 1 << 4,

        /// <summary>
        /// Alt key is pressed.
        /// </summary>
        AltKeyPressed = 1 << 5,

        /// <summary>
        /// Option key is pressed.
        /// </summary>
        OptionKeyPressed = 1 << 6,

        /// <summary>
        /// Command key is pressed.
        /// </summary>
        CommandKeyPressed = 1 << 7,
    }
}