﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.Drawing.Printing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    internal partial class WxPlatform : NativePlatform
    {
        internal const string DialogCancelGuid = "5DB20A10B5974CD4885CFCF346AF0F81";

        private static bool initialized;

        /// <inheritdoc/>
        public override IFontFactoryHandler FontFactory
        {
            get => WxFontFactoryHandler.Default;
        }

        public static void Initialize()
        {
            if (initialized)
                return;
            NativeDrawing.Default = new WxDrawing();
            Default = new WxPlatform();
            initialized = true;
        }

        /// <inheritdoc/>
        public override CustomControlPainter GetPainter()
        {
            return NativeControlPainter.Default;
        }

        /// <inheritdoc/>
        public override Color GetClassDefaultAttributesBgColor(
            ControlTypeId controlType,
            ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal)
        {
            return Native.Control.GetClassDefaultAttributesBgColor(
                (int)controlType,
                (int)renderSize);
        }

        /// <inheritdoc/>
        public override Color GetClassDefaultAttributesFgColor(
            ControlTypeId controlType,
            ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal)
        {
            return Native.Control.GetClassDefaultAttributesFgColor(
                (int)controlType,
                (int)renderSize);
        }

        /// <inheritdoc/>
        public override Font? GetClassDefaultAttributesFont(
            ControlTypeId controlType,
            ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal)
        {
            var font = Native.Control.GetClassDefaultAttributesFont(
                (int)controlType,
                (int)renderSize);
            return Font.FromInternal(font);
        }

        public override bool TimerGetEnabled(Timer timer)
        {
            return ((Native.Timer)timer.NativeTimer).Enabled;
        }

        public override void TimerSetEnabled(Timer timer, bool value)
        {
            ((Native.Timer)timer.NativeTimer).Enabled = value;
        }

        public override int TimerGetInterval(Timer timer)
        {
            return ((Native.Timer)timer.NativeTimer).Interval;
        }

        public override void TimerSetInterval(Timer timer, int value)
        {
            ((Native.Timer)timer.NativeTimer).Interval = value;
        }

        public override object CreateTimer()
        {
            return new Native.Timer();
        }

        public override void TimerSetTick(Timer timer, Action? value)
        {
            ((Native.Timer)timer.NativeTimer).Tick = value;
        }

        public override void StopSound()
        {
            Native.WxOtherFactory.SoundStop();
        }

        public override void MessageBeep(SystemSoundType soundType)
        {
            if (BaseApplication.IsWindowsOS)
                SafeNativeMethods.MessageBeep((int)soundType);
            else
                Bell();
        }

        public override void Bell()
        {
            Native.WxOtherFactory.Bell();
        }

        public override DialogResult ShowMessageBox(MessageBoxInfo info)
        {
            var nativeOwner = info.Owner == null ? null :
                ((WindowHandler)info.Owner.Handler).NativeControl;
            return (DialogResult)Native.MessageBox.Show(
                nativeOwner,
                info.Text?.ToString() ?? string.Empty,
                info.Caption ?? string.Empty,
                (Native.MessageBoxButtons)info.Buttons,
                (Native.MessageBoxIcon)info.Icon,
                (Native.MessageBoxDefaultButton)info.DefaultButton);
        }

        public override IDataObject? ClipboardGetDataObject()
        {
            var unmanagedDataObject =
                Application.Current.NativeClipboard.GetDataObject();
            if (unmanagedDataObject == null)
                return null;

            return new UnmanagedDataObjectAdapter(unmanagedDataObject);
        }

        public override void ClipboardSetDataObject(IDataObject value)
        {
            Application.Current.NativeClipboard.SetDataObject(
                UnmanagedDataObjectService.GetUnmanagedDataObject(value));
        }

        public override bool IsBusyCursor() => Native.WxOtherFactory.IsBusyCursor();

        public override void BeginBusyCursor() => Native.WxOtherFactory.BeginBusyCursor();

        public override void EndBusyCursor() => Native.WxOtherFactory.EndBusyCursor();

        public override bool ShowExceptionWindow(
            Exception exception,
            string? additionalInfo = null,
            bool canContinue = true)
        {
            using var errorWindow =
                new ThreadExceptionWindow(exception, additionalInfo, canContinue);
            if (Application.IsRunning)
            {
                return errorWindow.ShowModal() == ModalResult.Accepted;
            }
            else
            {
                errorWindow.CanContinue = false;
                Application.Current.Run(errorWindow);
                return false;
            }
        }

        /// <summary>
        /// Pop up a dialog box with title set to <paramref name="caption"/>,
        /// <paramref name="message"/>, and a <paramref name="defaultValue"/>.
        /// The user may type in text and press OK to return this text, or press Cancel
        /// to return the empty string.
        /// </summary>
        /// <param name="message">Dialog message.</param>
        /// <param name="caption">Dialog title.</param>
        /// <param name="defaultValue">Default value. Optional.</param>
        /// <param name="parent">Parent control. Optional.</param>
        /// <param name="x">X-position on the screen. Optional. By default is -1.</param>
        /// <param name="y">Y-position on the screen. Optional. By default is -1.</param>
        /// <param name="centre">If <c>true</c>, the message text (which may include new line
        /// characters) is centred; if <c>false</c>, the message is left-justified.</param>
        public override string? GetTextFromUser(
            string message,
            string caption,
            string defaultValue,
            Control? parent,
            int x,
            int y,
            bool centre)
        {
            var handle = WxApplicationHandler.WxWidget(parent);
            var result = Native.WxOtherFactory.GetTextFromUser(
                message,
                caption,
                defaultValue,
                handle,
                x,
                y,
                centre);
            if (result == DialogCancelGuid)
                return null;
            return result;
        }

        /// <summary>
        /// Shows a dialog asking the user for numeric input.
        /// </summary>
        /// <remarks>
        /// The dialogs title is set to <paramref name="caption"/>, it contains a (possibly) multiline
        /// <paramref name="message"/> above the single line prompt and the zone for entering
        /// the number. Dialog is centered on its parent unless an explicit position is given
        /// in <paramref name="pos"/>.
        /// </remarks>
        /// <remarks>
        /// If the user cancels the dialog, the function returns <c>null</c>.
        /// </remarks>
        /// <remarks>
        /// The number entered must be in the range <paramref name="min"/> to <paramref name="max"/>
        /// (both of which should be positive) and
        /// value is the initial value of it. If the user enters an invalid value, it is forced to fall
        /// into the specified range.
        /// </remarks>
        /// <param name="message">A (possibly) multiline dialog message above the single line
        /// <paramref name="prompt"/>.</param>
        /// <param name="prompt">Single line dialog prompt.</param>
        /// <param name="caption">Dialog title.</param>
        /// <param name="value">Default value. Optional. Default is 0.</param>
        /// <param name="min">A positive minimal value. Optional. Default is 0.</param>
        /// <param name="max">A positive maximal value. Optional. Default is 100.</param>
        /// <param name="parent">Dialog parent.</param>
        /// <param name="pos"></param>
        public override long? GetNumberFromUser(
            string message,
            string prompt,
            string caption,
            long value,
            long min,
            long max,
            Control? parent,
            PointI pos)
        {
            var handle = WxApplicationHandler.WxWidget(parent);
            var result = Native.WxOtherFactory.GetNumberFromUser(
                message,
                prompt,
                caption,
                value,
                min,
                max,
                handle,
                pos);
            if (result < 0)
                return null;
            return result;
        }

        private class SafeNativeMethods
        {
            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
            internal static extern bool MessageBeep(int type);
        }
    }
}
