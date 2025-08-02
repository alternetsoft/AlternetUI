using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal class WxActionSimulatorHandler : DisposableObject<IntPtr>, IActionSimulatorHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIActionSimulator"/> class.
        /// </summary>
        public WxActionSimulatorHandler()
            : base(Native.WxOtherFactory.UIActionSimulatorCreate(), true)
        {
        }

        /// <summary>
        /// Presses and release a key.
        /// </summary>
        /// <param name="keyCode">Key to operate on.</param>
        /// <param name="modifiers">A combination of key modifier flags to be
        /// pressed with the given keycode.</param>
        /// <returns></returns>
        public virtual bool SendChar(
            Key keyCode,
            RawModifierKeys modifiers = RawModifierKeys.None)
        {
            var result = Native.WxOtherFactory.UIActionSimulatorChar(
                Handle,
                KeyCodeToIndex(keyCode),
                KeyModifierToIndex(modifiers));
            ExecuteCommand();
            return result;
        }

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
        public virtual bool SendKeyDown(
            Key keyCode,
            RawModifierKeys modifiers = RawModifierKeys.None)
        {
            var result = Native.WxOtherFactory.UIActionSimulatorKeyDown(
                Handle,
                KeyCodeToIndex(keyCode),
                KeyModifierToIndex(modifiers));
            ExecuteCommand();
            return result;
        }

        /// <summary>
        /// Releases a key.
        /// </summary>
        /// <param name="keyCode">Key to operate on.</param>
        /// <param name="modifiers">A combination of key modifier flags to be
        /// pressed with the given keycode.</param>
        /// <returns></returns>
        /// <returns></returns>
        public virtual bool SendKeyUp(
            Key keyCode,
            RawModifierKeys modifiers = RawModifierKeys.None)
        {
            var result = Native.WxOtherFactory.UIActionSimulatorKeyUp(
                Handle,
                KeyCodeToIndex(keyCode),
                KeyModifierToIndex(modifiers));
            ExecuteCommand();
            return result;
        }

        /// <summary>
        /// Clicks a mouse button.
        /// </summary>
        /// <param name="button">Mouse button to press</param>
        /// <returns></returns>
        public virtual bool SendMouseClick(MouseButton button = MouseButton.Left)
        {
            var result = Native.WxOtherFactory.UIActionSimulatorMouseClick(
                Handle,
                MouseButtonToIndex(button));
            ExecuteCommand();
            return result;
        }

        /// <summary>
        /// Double-clicks a mouse button.
        /// </summary>
        /// <param name="button">Mouse button to press</param>
        /// <returns></returns>
        public virtual bool SendMouseDblClick(MouseButton button = MouseButton.Left)
        {
            var result = Native.WxOtherFactory.UIActionSimulatorMouseDblClick(
                Handle,
                MouseButtonToIndex(button));
            ExecuteCommand();
            return result;
        }

        /// <summary>
        /// Presses a mouse button.
        /// </summary>
        /// <param name="button">Mouse button to press</param>
        /// <returns></returns>
        public virtual bool SendMouseDown(MouseButton button = MouseButton.Left)
        {
            var result = Native.WxOtherFactory.UIActionSimulatorMouseDown(
                Handle,
                MouseButtonToIndex(button));
            ExecuteCommand();
            return result;
        }

        /// <summary>
        /// Performs a drag and drop operation.
        /// </summary>
        /// <param name="x1">x start coordinate, in screen coordinates.</param>
        /// <param name="y1">y start coordinate, in screen coordinates.</param>
        /// <param name="x2">x destination coordinate, in screen coordinates.</param>
        /// <param name="y2">y destination coordinate, in screen coordinates.</param>
        /// <param name="button">Mouse button to press</param>
        /// <returns></returns>
        public virtual bool SendMouseDragDrop(
            int x1,
            int y1,
            int x2,
            int y2,
            MouseButton button = MouseButton.Left)
        {
            var result = Native.WxOtherFactory.UIActionSimulatorMouseDragDrop(
                Handle,
                x1,
                y1,
                x2,
                y2,
                MouseButtonToIndex(button));
            ExecuteCommand();
            return result;
        }

        /// <summary>
        /// Moves the mouse to the specified coordinates.
        /// </summary>
        /// <param name="point">Point to move to, in screen coordinates (pixels).</param>
        /// <returns></returns>
        public virtual bool SendMouseMove(PointI point)
        {
            var result = Native.WxOtherFactory.UIActionSimulatorMouseMove(Handle, point);
            ExecuteCommand();
            return result;
        }

        /// <summary>
        /// Releases a mouse button.
        /// </summary>
        /// <param name="button">Mouse button to press</param>
        /// <returns></returns>
        public virtual bool SendMouseUp(MouseButton button = MouseButton.Left)
        {
            var result = Native.WxOtherFactory.UIActionSimulatorMouseUp(
                Handle,
                MouseButtonToIndex(button));
            ExecuteCommand();
            return result;
        }

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
        public virtual bool SendSelect(string text)
        {
            var result = Native.WxOtherFactory.UIActionSimulatorSelect(Handle, text);
            ExecuteCommand();
            return result;
        }

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
        public virtual bool SendText(string text)
        {
            var result = Native.WxOtherFactory.UIActionSimulatorText(Handle, text);
            ExecuteCommand();
            return result;
        }

        /// <summary>
        /// Executes 'Send*' command.
        /// </summary>
        internal void ExecuteCommand()
        {
            Native.WxOtherFactory.UIActionSimulatorYield();
        }

        /// <inheritdoc/>
        protected override void DisposeUnmanaged()
        {
            Native.WxOtherFactory.UIActionSimulatorDelete(Handle);
        }

        private static int KeyModifierToIndex(RawModifierKeys modifiers)
        {
            return (int)modifiers;
        }

        private static int KeyCodeToIndex(Key keyCode)
        {
            var wxKey = WxKeyboardHandler.KeyAndWxMapping.DestToSource.Convert(keyCode);

            return (int)wxKey;
        }

        private static int MouseButtonToIndex(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return 1;
                case MouseButton.Middle:
                    return 2;
                case MouseButton.Right:
                    return 3;
                default:
                    throw new Exception("Only left, right or middle mouse buttons are supported.");
            }
        }
    }
}