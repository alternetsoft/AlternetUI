using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.Drawing.Printing;

namespace Alternet.UI
{
    public partial class MauiPlatform : NativePlatform
    {
        private static bool initialized;

        public override IFontFactoryHandler FontFactory
        {
            get
            {
                return new MauiFontFactoryHandler();
            }
        }

        public static void Initialize()
        {
            if (initialized)
                return;
            NativeDrawing.Default = new SkiaDrawing();
            Default = new MauiPlatform();
            initialized = true;
        }

        /// <inheritdoc/>
        public override void BeginBusyCursor()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void EndBusyCursor()
        {
            throw new NotImplementedException();
        }

        public override bool IsBusyCursor()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override bool ShowExceptionWindow(
            Exception exception,
            string? additionalInfo = null,
            bool canContinue = true)
        {
            throw new NotImplementedException();
        }

        public override CustomControlPainter GetPainter()
        {
            throw new NotImplementedException();
        }

        public override Color GetClassDefaultAttributesBgColor(ControlTypeId controlType, ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal)
        {
            throw new NotImplementedException();
        }

        public override Color GetClassDefaultAttributesFgColor(ControlTypeId controlType, ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal)
        {
            throw new NotImplementedException();
        }

        public override Font? GetClassDefaultAttributesFont(ControlTypeId controlType, ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal)
        {
            throw new NotImplementedException();
        }

        public override IDataObject? ClipboardGetDataObject()
        {
            throw new NotImplementedException();
        }

        public override void ClipboardSetDataObject(IDataObject value)
        {
            throw new NotImplementedException();
        }

        public override DialogResult ShowMessageBox(MessageBoxInfo info)
        {
            throw new NotImplementedException();
        }

        public override void StopSound()
        {
            throw new NotImplementedException();
        }

        public override void Bell()
        {
            throw new NotImplementedException();
        }

        public override void MessageBeep(SystemSoundType soundType)
        {
            throw new NotImplementedException();
        }

        public override void TimerSetTick(Timer timer, Action? value)
        {
            throw new NotImplementedException();
        }

        public override object CreateTimer()
        {
            throw new NotImplementedException();
        }

        public override bool TimerGetEnabled(Timer timer)
        {
            throw new NotImplementedException();
        }

        public override void TimerSetEnabled(Timer timer, bool value)
        {
            throw new NotImplementedException();
        }

        public override int TimerGetInterval(Timer timer)
        {
            throw new NotImplementedException();
        }

        public override void TimerSetInterval(Timer timer, int value)
        {
            throw new NotImplementedException();
        }

        public override string? GetTextFromUser(string message, string caption, string defaultValue, Control? parent, int x, int y, bool centre)
        {
            throw new NotImplementedException();
        }

        public override long? GetNumberFromUser(string message, string prompt, string caption, long value, long min, long max, Control? parent, PointI pos)
        {
            throw new NotImplementedException();
        }
    }
}
