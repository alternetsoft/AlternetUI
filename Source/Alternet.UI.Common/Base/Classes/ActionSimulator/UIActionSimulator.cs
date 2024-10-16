﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// <remarks>
    /// Only left, right or middle mouse buttons are supported in this class.
    /// </remarks>
    public class UIActionSimulator : HandledObject<IActionSimulatorHandler>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIActionSimulator"/> class.
        /// </summary>
        public UIActionSimulator()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UIActionSimulator"/> class.
        /// </summary>
        public UIActionSimulator(IActionSimulatorHandler handler)
        {
            Handler = handler;
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
            ModifierKeys modifiers = ModifierKeys.None)
        {
            var result = Handler.SendChar(keyCode, modifiers.ToRawModifierKeys());
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
            ModifierKeys modifiers = ModifierKeys.None)
        {
            var result = Handler.SendKeyDown(keyCode, modifiers.ToRawModifierKeys());
            return result;
        }

        /// <summary>
        /// Sends key. This methods calls <see cref="SendKeyDown"/> and <see cref="SendKeyUp"/>.
        /// </summary>
        /// <param name="keyCode"></param>
        /// <param name="modifiers"></param>
        public virtual bool SendKey(
            Key keyCode,
            ModifierKeys modifiers = ModifierKeys.None)
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
            Key keyCode,
            ModifierKeys modifiers = ModifierKeys.None)
        {
            var result = Handler.SendKeyUp(keyCode, modifiers.ToRawModifierKeys());
            return result;
        }

        /// <summary>
        /// Clicks a mouse button.
        /// </summary>
        /// <param name="button">Mouse button to press</param>
        /// <returns></returns>
        public virtual bool SendMouseClick(MouseButton button = MouseButton.Left)
        {
            var result = Handler.SendMouseClick(button);
            return result;
        }

        /// <summary>
        /// Double-clicks a mouse button.
        /// </summary>
        /// <param name="button">Mouse button to press</param>
        /// <returns></returns>
        public virtual bool SendMouseDblClick(MouseButton button = MouseButton.Left)
        {
            var result = Handler.SendMouseDblClick(button);
            return result;
        }

        /// <summary>
        /// Presses a mouse button.
        /// </summary>
        /// <param name="button">Mouse button to press</param>
        /// <returns></returns>
        public virtual bool SendMouseDown(MouseButton button = MouseButton.Left)
        {
            var result = Handler.SendMouseDown(button);
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
            var result = Handler.SendMouseDragDrop(
                x1,
                y1,
                x2,
                y2,
                button);
            return result;
        }

        /// <summary>
        /// Moves the mouse to the top-left corner of the control.
        /// </summary>
        /// <param name="control">Control to which mouse will be moved.</param>
        /// <param name="offset">Additional offset for the mouse movement.
        /// Value is in dips (1/96 inch)</param>
        /// <returns></returns>
        public virtual bool SendMouseMove(AbstractControl control, PointD? offset = default)
        {
            var screenLocationDip = control.ClientToScreen((0, 0));
            if(offset is not null)
                screenLocationDip.Offset(offset.Value.X, offset.Value.Y);
            var screenLocationPixel = control.PixelFromDip(screenLocationDip);
            var result = SendMouseMove(screenLocationPixel);
            return result;
        }

        /// <summary>
        /// Moves the mouse to the specified coordinates.
        /// </summary>
        /// <param name="point">Point to move to, in screen coordinates (pixels).</param>
        /// <returns></returns>
        public virtual bool SendMouseMove(PointI point)
        {
            var result = Handler.SendMouseMove(point);
            return result;
        }

        /// <summary>
        /// Releases a mouse button.
        /// </summary>
        /// <param name="button">Mouse button to press</param>
        /// <returns></returns>
        public virtual bool SendMouseUp(MouseButton button = MouseButton.Left)
        {
            var result = Handler.SendMouseUp(button);
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
            var result = Handler.SendSelect(text);
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
            var result = Handler.SendText(text);
            return result;
        }

        /// <summary>
        /// Same as <see cref="SendChar"/> but checks for the <paramref name="condition"/>
        /// before sending action. This method sets <paramref name="condition"/> to
        /// the result of the send action operation.
        /// </summary>
        public bool SendCharIf(
            ref bool condition,
            Key keyCode,
            ModifierKeys modifiers = ModifierKeys.None)
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
            Key keyCode,
            ModifierKeys modifiers = ModifierKeys.None)
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
            Key keyCode,
            ModifierKeys modifiers = ModifierKeys.None)
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
            Key keyCode,
            ModifierKeys modifiers = ModifierKeys.None)
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
            int x1,
            int y1,
            int x2,
            int y2,
            MouseButton button = MouseButton.Left)
        {
            if (!condition)
                return false;
            return SendMouseDragDrop(x1, y1, x2, y2, button);
        }

        /// <summary>
        /// Same as <see cref="SendMouseMove(AbstractControl,PointD?)"/> but checks for
        /// the <paramref name="condition"/>
        /// before sending action. This method sets <paramref name="condition"/> to
        /// the result of the send action operation.
        /// </summary>
        public bool SendMouseMoveIf(
            ref bool condition,
            AbstractControl control,
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
        public bool SendSelectIf(ref bool condition, string text)
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
        public bool SendTextIf(ref bool condition, string text)
        {
            if (!condition)
                return false;
            return SendText(text);
        }

        /// <inheritdoc/>
        protected override IActionSimulatorHandler CreateHandler()
        {
            return App.Handler.CreateActionSimulatorHandler();
        }
    }
}