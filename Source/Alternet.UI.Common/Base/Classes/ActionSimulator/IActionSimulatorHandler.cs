using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides access to the user interface action simulator.
    /// </summary>
    public interface IActionSimulatorHandler : IDisposable
    {
        /// <summary>
        /// Presses and release a key.
        /// </summary>
        /// <param name="keyCode">Key to operate on.</param>
        /// <param name="modifiers">A combination of key modifier flags to be
        /// pressed with the given keycode.</param>
        /// <returns></returns>
        bool SendChar(
            Key keyCode,
            RawModifierKeys modifiers = RawModifierKeys.None);

        /// <summary>
        /// Presses a key.
        /// </summary>
        /// <param name="keyCode">Key to operate on.</param>
        /// <param name="modifiers">A combination of key modifier flags to be
        /// pressed with the given keycode.</param>
        /// <returns></returns>
        /// <remarks>
        /// If you are using modifiers then it needs to
        /// be paired with an identical <see cref="SendKeyUp"/> or the modifiers will
        /// not be released (Windows and macOS).
        /// </remarks>
        bool SendKeyDown(
            Key keyCode,
            RawModifierKeys modifiers = RawModifierKeys.None);

        /// <summary>
        /// Releases a key.
        /// </summary>
        /// <param name="keyCode">Key to operate on.</param>
        /// <param name="modifiers">A combination of key modifier flags to be
        /// pressed with the given keycode.</param>
        /// <returns></returns>
        /// <returns></returns>
        bool SendKeyUp(
            Key keyCode,
            RawModifierKeys modifiers = RawModifierKeys.None);

        /// <summary>
        /// Clicks a mouse button.
        /// </summary>
        /// <param name="button">Mouse button to press</param>
        /// <returns></returns>
        bool SendMouseClick(MouseButton button = MouseButton.Left);

        /// <summary>
        /// Double-clicks a mouse button.
        /// </summary>
        /// <param name="button">Mouse button to press</param>
        /// <returns></returns>
        bool SendMouseDblClick(MouseButton button = MouseButton.Left);

        /// <summary>
        /// Presses a mouse button.
        /// </summary>
        /// <param name="button">Mouse button to press</param>
        /// <returns></returns>
        bool SendMouseDown(MouseButton button = MouseButton.Left);

        /// <summary>
        /// Performs a drag and drop operation.
        /// </summary>
        /// <param name="x1">x start coordinate, in screen coordinates.</param>
        /// <param name="y1">y start coordinate, in screen coordinates.</param>
        /// <param name="x2">x destination coordinate, in screen coordinates.</param>
        /// <param name="y2">y destination coordinate, in screen coordinates.</param>
        /// <param name="button">Mouse button to press</param>
        /// <returns></returns>
        bool SendMouseDragDrop(
            int x1,
            int y1,
            int x2,
            int y2,
            MouseButton button = MouseButton.Left);

        /// <summary>
        /// Moves the mouse to the specified coordinates.
        /// </summary>
        /// <param name="point">Point to move to, in screen coordinates (pixels).</param>
        /// <returns></returns>
        bool SendMouseMove(PointI point);

        /// <summary>
        /// Releases a mouse button.
        /// </summary>
        /// <param name="button">Mouse button to press</param>
        /// <returns></returns>
        bool SendMouseUp(MouseButton button = MouseButton.Left);

        /// <summary>
        /// Simulates selection of an item with the given text.
        /// </summary>
        /// <param name="text">The text of the item to select.</param>
        /// <returns><c>true</c> if the item text was successfully selected or <c>false</c> if
        /// the currently focused window is not one of the controls allowing
        /// item selection or if the item with the given text was not found in it.</returns>
        /// <remarks>
        /// This method selects an item in the currently focused
        /// <see cref="ComboBox"/>, <see cref="StdListBox"/> and similar controls.
        /// It does it by simulating keyboard events, so the behaviour
        /// should be the same as if the item was really selected by the user.
        /// </remarks>
        bool SendSelect(string text);

        /// <summary>
        /// Emulates typing in the keys representing the given string.
        /// </summary>
        /// <param name="text">The string, containing only US ASCII characters, to type.</param>
        /// <returns></returns>
        /// <remarks>
        /// Currently only the ASCII letters are universally supported.
        /// Digits and punctuation characters can be used with the
        /// standard QWERTY (US) keyboard layout but may not work with other layouts.
        /// </remarks>
        bool SendText(string text);
    }
}
