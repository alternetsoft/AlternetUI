using System;
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

        public static void Initialize()
        {
            if (initialized)
                return;
            NativeDrawing.Default = new WxDrawing();
            NativeControl.Default = new WxPlatformControl();
            NativeWindow.Default = new WxPlatformWindow();
            Default = new WxPlatform();
            initialized = true;
        }

        public override IPropertyGridChoices CreateChoices()
        {
            return new PropertyGridChoices();
        }

        public override bool TimerGetEnabled(Timer timer)
        {
            return ((Native.Timer)timer.NativeTimer).Enabled;
        }

        public override void TimerSetEnabled(Timer timer, bool value)
        {
            ((Native.Timer)timer.NativeTimer).Enabled = value;
        }

        public override UIPlatformKind GetPlatformKind()
        {
            return UIPlatformKind.WxWidgets;
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

        public override LangDirection GetLangDirection()
        {
            return (LangDirection?)Application.Current?.nativeApplication.GetLayoutDirection()
                ?? LangDirection.LeftToRight;
        }

        public override void ProcessPendingEvents()
        {
            Application.Current?.nativeApplication.ProcessPendingEvents();
        }

        public override bool IsBusyCursor() => Native.WxOtherFactory.IsBusyCursor();

        public override void BeginBusyCursor() => Native.WxOtherFactory.BeginBusyCursor();

        public override void EndBusyCursor() => Native.WxOtherFactory.EndBusyCursor();

        public override void ExitMainLoop()
        {
            Application.Current.NativeApplication.ExitMainLoop();
        }

        public override int SystemSettingsGetMetric(SystemSettingsMetric index, IControl? control)
        {
            return Native.WxOtherFactory.SystemSettingsGetMetric(
                (int)index,
                WxPlatformControl.WxWidget(control));
        }

        public override int SystemSettingsGetMetric(SystemSettingsMetric index)
        {
            return Native.WxOtherFactory.SystemSettingsGetMetric((int)index, default);
        }

        public override string SystemSettingsAppearanceName()
        {
            return Native.WxOtherFactory.SystemAppearanceGetName();
        }

        public override bool SystemSettingsAppearanceIsDark()
        {
            return Native.WxOtherFactory.SystemAppearanceIsDark();
        }

        public override bool SystemSettingsIsUsingDarkBackground()
        {
            return Native.WxOtherFactory.SystemAppearanceIsUsingDarkBackground();
        }

        public override bool SystemSettingsHasFeature(SystemSettingsFeature index)
        {
            return Native.WxOtherFactory.SystemSettingsHasFeature((int)index);
        }

        public override Color SystemSettingsGetColor(SystemSettingsColor index)
        {
            return Native.WxOtherFactory.SystemSettingsGetColor((int)index);
        }

        public override Font SystemSettingsGetFont(SystemSettingsFont systemFont)
        {
            var fnt = Native.WxOtherFactory.SystemSettingsGetFont((int)systemFont);
            return new Font(fnt);
        }

        public override bool ShowExceptionWindow(
            Exception exception,
            string? additionalInfo = null,
            bool canContinue = true)
        {
            return ThreadExceptionWindow.Show(exception, additionalInfo, canContinue);
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
            var handle = WxPlatformControl.WxWidget(parent);
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
            var handle = WxPlatformControl.WxWidget(parent);
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

        public override IPrinterSettingsHandler CreatePrinterSettingsHandler()
        {
            return new UI.Native.PrinterSettings();
        }

        private class SafeNativeMethods
        {
            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
            internal static extern bool MessageBeep(int type);
        }
    }
}
