using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal partial class WxPlatform : NativePlatform
    {
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

        private class SafeNativeMethods
        {
            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
            internal static extern bool MessageBeep(int type);
        }
    }
}
