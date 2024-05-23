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
        /// Gets default native platform implementation.
        /// </summary>
        public static NativePlatform Default = new NotImplementedPlatform();

        public abstract IFontFactoryHandler FontFactory { get; }

        public abstract CustomControlPainter GetPainter();

        public abstract Color GetClassDefaultAttributesBgColor(
            ControlTypeId controlType,
            ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal);

        public abstract Color GetClassDefaultAttributesFgColor(
            ControlTypeId controlType,
            ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal);

        public abstract Font? GetClassDefaultAttributesFont(
            ControlTypeId controlType,
            ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal);

        public abstract bool ShowExceptionWindow(
            Exception exception,
            string? additionalInfo = null,
            bool canContinue = true);

        public abstract bool IsBusyCursor();

        public abstract void BeginBusyCursor();

        public abstract void EndBusyCursor();

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
