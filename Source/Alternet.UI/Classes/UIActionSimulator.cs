using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// <see cref="UIActionSimulator"/> is a class used to simulate user
    /// interface actions such as a mouse click or a key press.
    /// Common usage for this class would be to provide
    /// playback and record (aka macro recording)
    /// functionality for users, or to drive unit tests by simulating user sessions.
    /// This class currently doesn't work when using Wayland with Linux.
    /// </summary>
    public class UIActionSimulator : DisposableObject<IntPtr>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIActionSimulator"/> class.
        /// </summary>
        public UIActionSimulator()
            : base(Native.WxOtherFactory.UIActionSimulatorCreate(), true)
        {
        }

        /// <summary>
        /// Key modifiers supported in the <see cref="UIActionSimulator"/>.
        /// </summary>
        [Flags]
        public enum KeyModifier
        {
            /// <summary>
            /// None of the key modifiers is pressed.
            /// </summary>
            None = 0x0000,

            /// <summary>
            /// Alt key is pressed.
            /// </summary>
            Alt = 0x0001,

            /// <summary>
            /// Control key or Apple/Command key under OS X is pressed.
            /// </summary>
            Control = 0x0002,

            /// <summary>
            /// AltGr key is pressed.
            /// </summary>
            AltGr = Alt | Control,

            /// <summary>
            /// Shift key is pressed.
            /// </summary>
            Shift = 0x0004,

            /// <summary>
            /// Meta/Windows/Apple key is pressed.
            /// </summary>
            Meta = 0x0008,

            /// <summary>
            /// Meta/Windows/Apple key is pressed.
            /// </summary>
            Windows = Meta,

            /// <summary>
            /// Raw Control key is pressed under OS X.
            /// </summary>
            RawControlOnMac = 0x0010,

            /// <summary>
            /// Raw Control key is pressed.
            /// </summary>
            RawControl = Control,

            /// <summary>
            /// Key used for command accelerators is pressed.
            /// </summary>
            ModCmd = Control,
        }

        /// <summary>
        /// Mouse buttons supported in the <see cref="UIActionSimulator"/>.
        /// </summary>
        public enum MouseButton
        {
            /// <summary>
            /// Left mouse button.
            /// </summary>
            Left = 1,

            /// <summary>
            /// Middle mouse button.
            /// </summary>
            Middle = 2,

            /// <summary>
            /// Right mouse button.
            /// </summary>
            Right = 3,
        }

        /// <summary>
        /// Presses and release a key.
        /// </summary>
        /// <param name="keyCode">Key to operate on.</param>
        /// <param name="modifiers">A combination of key modifier flags to be
        /// pressed with the given keycode.</param>
        /// <returns></returns>
        public virtual bool SendChar(
            WxWidgetsKeyCode keyCode,
            KeyModifier modifiers = KeyModifier.None)
        {
            var result = Native.WxOtherFactory.UIActionSimulatorChar(Handle, (int)keyCode, (int)modifiers);
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
            WxWidgetsKeyCode keyCode,
            KeyModifier modifiers = KeyModifier.None)
        {
            var result = Native.WxOtherFactory.UIActionSimulatorKeyDown(Handle, (int)keyCode, (int)modifiers);
            ExecuteCommand();
            return result;
        }

        /// <summary>
        /// Sends key. This methods calls <see cref="SendKeyDown"/> and <see cref="SendKeyUp"/>.
        /// </summary>
        /// <param name="keyCode"></param>
        /// <param name="modifiers"></param>
        public virtual bool SendKey(
            WxWidgetsKeyCode keyCode,
            KeyModifier modifiers = KeyModifier.None)
        {
            var result1 = SendKeyDown(keyCode, modifiers);
            var result2 = SendKeyUp(keyCode, modifiers);
            return result1 & result2;
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
            WxWidgetsKeyCode keyCode,
            KeyModifier modifiers = KeyModifier.None)
        {
            var result = Native.WxOtherFactory.UIActionSimulatorKeyUp(Handle, (int)keyCode, (int)modifiers);
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
            var result = Native.WxOtherFactory.UIActionSimulatorMouseClick(Handle, (int)button);
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
            var result = Native.WxOtherFactory.UIActionSimulatorMouseDblClick(Handle, (int)button);
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
            var result = Native.WxOtherFactory.UIActionSimulatorMouseDown(Handle, (int)button);
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
            long x1,
            long y1,
            long x2,
            long y2,
            MouseButton button = MouseButton.Left)
        {
            var result = Native.WxOtherFactory.UIActionSimulatorMouseDragDrop(
                Handle,
                x1,
                y1,
                x2,
                y2,
                (int)button);
            ExecuteCommand();
            return result;
        }

        /// <summary>
        /// Moves the mouse to the top-left corner of the control.
        /// </summary>
        /// <param name="control">Control to which mouse will be moved.</param>
        /// <param name="offset">Additional offset for the mouse movement.
        /// Value is in dips (1/96 inch)</param>
        /// <returns></returns>
        public virtual bool SendMouseMove(Control control, PointD? offset = default)
        {
            var screenLocationDip = control.ClientToScreen((0, 0));
            if(offset is not null)
                screenLocationDip.Offset(offset.Value.X, offset.Value.Y);
            var screenLocationPixel = control.PixelFromDip(screenLocationDip);
            var result = SendMouseMove(screenLocationPixel);
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
            var result = Native.WxOtherFactory.UIActionSimulatorMouseUp(Handle, (int)button);
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
        /// <see cref="ComboBox"/>, <see cref="ListBox"/> and similar controls.
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
        /// Same as <see cref="SendChar"/> but checks for the <paramref name="condition"/>
        /// before sending action. This method sets <paramref name="condition"/> to
        /// the result of the send action operation.
        /// </summary>
        public bool SendCharIf(
            ref bool condition,
            WxWidgetsKeyCode keyCode,
            KeyModifier modifiers = KeyModifier.None)
        {
            if (!condition)
                return false;
            return SendChar(keyCode, modifiers);
        }

        /// <summary>
        /// Same as <see cref="SendKeyDown"/> but checks for the <paramref name="condition"/>
        /// before sending action. This method sets <paramref name="condition"/> to
        /// the result of the send action operation.
        /// </summary>
        public bool SendKeyDownIf(
            ref bool condition,
            WxWidgetsKeyCode keyCode,
            KeyModifier modifiers = KeyModifier.None)
        {
            if (!condition)
                return false;
            return SendKeyDown(keyCode, modifiers);
        }

        /// <summary>
        /// Same as <see cref="SendKey"/> but checks for the <paramref name="condition"/>
        /// before sending action. This method sets <paramref name="condition"/> to
        /// the result of the send action operation.
        /// </summary>
        public bool SendKeyIf(
            ref bool condition,
            WxWidgetsKeyCode keyCode,
            KeyModifier modifiers = KeyModifier.None)
        {
            if (!condition)
                return false;
            return SendKey(keyCode, modifiers);
        }

        /// <summary>
        /// Same as <see cref="SendKeyUp"/> but checks for the <paramref name="condition"/>
        /// before sending action. This method sets <paramref name="condition"/> to
        /// the result of the send action operation.
        /// </summary>
        public bool SendKeyUpIf(
            ref bool condition,
            WxWidgetsKeyCode keyCode,
            KeyModifier modifiers = KeyModifier.None)
        {
            if (!condition)
                return false;
            return SendKeyUp(keyCode, modifiers);
        }

        /// <summary>
        /// Same as <see cref="SendMouseClick"/> but checks for the <paramref name="condition"/>
        /// before sending action. This method sets <paramref name="condition"/> to
        /// the result of the send action operation.
        /// </summary>
        public bool SendMouseClickIf(
            ref bool condition,
            MouseButton button = MouseButton.Left)
        {
            if (!condition)
                return false;
            return SendMouseClick(button);
        }

        /// <summary>
        /// Same as <see cref="SendMouseDblClick"/> but checks for the <paramref name="condition"/>
        /// before sending action. This method sets <paramref name="condition"/> to
        /// the result of the send action operation.
        /// </summary>
        public bool SendMouseDblClickIf(
            ref bool condition,
            MouseButton button = MouseButton.Left)
        {
            if (!condition)
                return false;
            return SendMouseDblClick(button);
        }

        /// <summary>
        /// Same as <see cref="SendMouseDown"/> but checks for the <paramref name="condition"/>
        /// before sending action. This method sets <paramref name="condition"/> to
        /// the result of the send action operation.
        /// </summary>
        public bool SendMouseDownIf(
            ref bool condition,
            MouseButton button = MouseButton.Left)
        {
            if (!condition)
                return false;
            return SendMouseDown(button);
        }

        /// <summary>
        /// Same as <see cref="SendMouseDragDrop"/> but checks for the <paramref name="condition"/>
        /// before sending action. This method sets <paramref name="condition"/> to
        /// the result of the send action operation.
        /// </summary>
        public bool SendMouseDragDropIf(
            ref bool condition,
            long x1,
            long y1,
            long x2,
            long y2,
            MouseButton button = MouseButton.Left)
        {
            if (!condition)
                return false;
            return SendMouseDragDrop(x1, y1, x2, y2, button);
        }

        /// <summary>
        /// Same as <see cref="SendMouseMove(Control,PointD?)"/> but checks for
        /// the <paramref name="condition"/>
        /// before sending action. This method sets <paramref name="condition"/> to
        /// the result of the send action operation.
        /// </summary>
        public bool SendMouseMoveIf(
            ref bool condition,
            Control control,
            PointD? offset = default)
        {
            if (!condition)
                return false;
            return SendMouseMove(control, offset);
        }

        /// <summary>
        /// Same as <see cref="SendMouseMove(PointI)"/> but checks for the <paramref name="condition"/>
        /// before sending action. This method sets <paramref name="condition"/> to
        /// the result of the send action operation.
        /// </summary>
        public bool SendMouseMoveIf(
            ref bool condition,
            PointI point)
        {
            if (!condition)
                return false;
            return SendMouseMove(point);
        }

        /// <summary>
        /// Same as <see cref="SendMouseUp"/> but checks for the <paramref name="condition"/>
        /// before sending action. This method sets <paramref name="condition"/> to
        /// the result of the send action operation.
        /// </summary>
        public bool SendMouseUpIf(
            ref bool condition,
            MouseButton button = MouseButton.Left)
        {
            if (!condition)
                return false;
            return SendMouseUp(button);
        }

        /// <summary>
        /// Same as <see cref="SendSelect"/> but checks for the <paramref name="condition"/>
        /// before sending action. This method sets <paramref name="condition"/> to
        /// the result of the send action operation.
        /// </summary>
        public bool SendSelectIf(
            ref bool condition,
            string text)
        {
            if (!condition)
                return false;
            return SendSelect(text);
        }

        /// <summary>
        /// Same as <see cref="SendText"/> but checks for the <paramref name="condition"/>
        /// before sending action. This method sets <paramref name="condition"/> to
        /// the result of the send action operation.
        /// </summary>
        public bool SendTextIf(
            ref bool condition,
            string text)
        {
            if (!condition)
                return false;
            return SendText(text);
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
    }
}