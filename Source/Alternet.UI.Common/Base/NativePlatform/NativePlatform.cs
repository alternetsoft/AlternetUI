using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.Drawing.Printing;

namespace Alternet.UI
{
    /// <summary>
    /// Declares platform related methods.
    /// </summary>
    /// <remarks>
    /// Do not use <see cref="Default"/> property until native platform
    /// is initialized.
    /// </remarks>
    public abstract partial class NativePlatform : BaseObject
    {
        /// <summary>
        /// Gets default native drawing implementation.
        /// </summary>
        public static NativePlatform Default = new NotImplementedPlatform();

        /// <summary>
        /// Returns the value of a system metric, or -1 if the metric is not supported on
        /// the current system.
        /// </summary>
        /// <param name="index">System metric identifier.</param>
        /// <param name="control">Control for which metric is requested (optional).</param>
        /// <remarks>
        /// The value of <paramref name="control"/> determines if the metric returned is a global
        /// value or a control based value, in which case it might determine the widget, the
        /// display the window is on, or something similar. The window given should be as close
        /// to the metric as possible (e.g.a <see cref="Window"/> in case of
        /// the <see cref="SystemSettingsMetric.CaptionY"/> metric).
        /// </remarks>
        /// <remarks>
        /// Specifying the <paramref name="control"/> parameter is encouraged, because some
        /// metrics on some ports are not supported without one,or they might be capable of
        /// reporting better values if given one. If a control does not make sense for a metric,
        /// one should still be given, as for example it might determine which displays
        /// cursor width is requested with <see cref="SystemSettingsMetric.CursorX"/>.
        /// </remarks>
        public abstract int SystemSettingsGetMetric(SystemSettingsMetric index, IControl? control);

        public abstract void SetSystemOption(string name, int value);

        public abstract void ExitMainLoop();

        public abstract int SystemSettingsGetMetric(SystemSettingsMetric index);

        public abstract string SystemSettingsAppearanceName();

        public abstract bool SystemSettingsAppearanceIsDark();

        public abstract bool SystemSettingsIsUsingDarkBackground();

        public abstract bool SystemSettingsHasFeature(SystemSettingsFeature index);

        public abstract Color SystemSettingsGetColor(SystemSettingsColor index);

        public abstract Font SystemSettingsGetFont(SystemSettingsFont systemFont);

        public abstract bool ShowExceptionWindow(
            Exception exception,
            string? additionalInfo = null,
            bool canContinue = true);

        public abstract bool IsBusyCursor();

        public abstract void BeginBusyCursor();

        public abstract void EndBusyCursor();

        public abstract void ProcessPendingEvents();

        public abstract LangDirection GetLangDirection();

        public abstract IDataObject? ClipboardGetDataObject();

        public abstract void ClipboardSetDataObject(IDataObject value);

        public abstract DialogResult ShowMessageBox(MessageBoxInfo info);

        public abstract void StopSound();

        public abstract void Bell();

        public abstract void MessageBeep(SystemSoundType soundType);

        public abstract void TimerSetTick(Timer timer, Action? value);

        public abstract object CreateTimer();

        public abstract bool TimerGetEnabled(Timer timer);

        public abstract void TimerSetEnabled(Timer timer, bool value);

        public abstract int TimerGetInterval(Timer timer);

        public abstract void TimerSetInterval(Timer timer, int value);

        public abstract UIPlatformKind GetPlatformKind();

        public abstract IPropertyGridChoices CreateChoices();

        public abstract IPrintDocumentHandler CreatePrintDocumentHandler();

        public abstract IPrinterSettingsHandler CreatePrinterSettingsHandler();

        public abstract IPrintDialogHandler CreatePrintDialogHandler();

        public abstract IPageSettingsHandler CreatePageSettingsHandler();

        public abstract IValueValidatorText CreateValueValidatorText(ValueValidatorTextStyle style);

        public abstract IValueValidatorText CreateValueValidatorNum(
            ValueValidatorNumStyle numericType,
            int valueBase = 10);

        public abstract IPageSetupDialogHandler CreatePageSetupDialogHandler();

        public abstract void ValidatorSuppressBellOnError(bool value);

        public abstract void RegisterDefaultPreviewControls(PreviewFile preview);

        public abstract IPrintPreviewDialogHandler CreatePrintPreviewDialogHandler();

        public abstract IRichToolTipHandler CreateRichToolTipHandler(
            string title,
            string message,
            bool useGeneric);

        public abstract string? GetTextFromUser(
            string message,
            string caption,
            string defaultValue,
            Control? parent,
            int x,
            int y,
            bool centre);

        public abstract long? GetNumberFromUser(
            string message,
            string prompt,
            string caption,
            long value,
            long min,
            long max,
            Control? parent,
            PointI pos);
    }
}
