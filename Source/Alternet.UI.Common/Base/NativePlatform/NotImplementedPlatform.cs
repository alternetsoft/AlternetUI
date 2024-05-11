using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.Drawing.Printing;

namespace Alternet.UI
{
    internal class NotImplementedPlatform : NativePlatform
    {
        public override void BeginBusyCursor()
        {
            throw new NotImplementedException();
        }

        public override void Bell()
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

        public override IPropertyGridChoices CreateChoices()
        {
            throw new NotImplementedException();
        }

        public override IPageSettingsHandler CreatePageSettingsHandler()
        {
            throw new NotImplementedException();
        }

        public override IPageSetupDialogHandler CreatePageSetupDialogHandler()
        {
            throw new NotImplementedException();
        }

        public override IPrintDialogHandler CreatePrintDialogHandler()
        {
            throw new NotImplementedException();
        }

        public override IPrintDocumentHandler CreatePrintDocumentHandler()
        {
            throw new NotImplementedException();
        }

        public override IPrinterSettingsHandler CreatePrinterSettingsHandler()
        {
            throw new NotImplementedException();
        }

        public override IPrintPreviewDialogHandler CreatePrintPreviewDialogHandler()
        {
            throw new NotImplementedException();
        }

        public override object CreateTimer()
        {
            throw new NotImplementedException();
        }

        public override void EndBusyCursor()
        {
            throw new NotImplementedException();
        }

        public override void ExitMainLoop()
        {
            throw new NotImplementedException();
        }

        public override LangDirection GetLangDirection()
        {
            throw new NotImplementedException();
        }

        public override long? GetNumberFromUser(string message, string prompt, string caption, long value, long min, long max, Control? parent, PointI pos)
        {
            throw new NotImplementedException();
        }

        public override UIPlatformKind GetPlatformKind()
        {
            throw new NotImplementedException();
        }

        public override string? GetTextFromUser(string message, string caption, string defaultValue, Control? parent, int x, int y, bool centre)
        {
            throw new NotImplementedException();
        }

        public override bool IsBusyCursor()
        {
            throw new NotImplementedException();
        }

        public override IntPtr MemoryAlloc(int size)
        {
            throw new NotImplementedException();
        }

        public override IntPtr MemoryAllocLong(ulong size)
        {
            throw new NotImplementedException();
        }

        public override IntPtr MemoryCopy(IntPtr dest, IntPtr src, int count)
        {
            throw new NotImplementedException();
        }

        public override IntPtr MemoryCopyLong(IntPtr dest, IntPtr src, ulong count)
        {
            throw new NotImplementedException();
        }

        public override void MemoryFree(IntPtr ptr)
        {
            throw new NotImplementedException();
        }

        public override IntPtr MemoryMove(IntPtr dest, IntPtr src, int count)
        {
            throw new NotImplementedException();
        }

        public override IntPtr MemoryMoveLong(IntPtr dest, IntPtr src, ulong count)
        {
            throw new NotImplementedException();
        }

        public override IntPtr MemoryRealloc(IntPtr ptr, int newSize)
        {
            throw new NotImplementedException();
        }

        public override IntPtr MemoryReallocLong(IntPtr ptr, ulong newSize)
        {
            throw new NotImplementedException();
        }

        public override IntPtr MemorySet(IntPtr dest, byte fillByte, int count)
        {
            throw new NotImplementedException();
        }

        public override IntPtr MemorySetLong(IntPtr dest, byte fillByte, ulong count)
        {
            throw new NotImplementedException();
        }

        public override void MessageBeep(SystemSoundType soundType)
        {
            throw new NotImplementedException();
        }

        public override void ProcessPendingEvents()
        {
            throw new NotImplementedException();
        }

        public override bool ShowExceptionWindow(Exception exception, string? additionalInfo = null, bool canContinue = true)
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

        public override bool SystemSettingsAppearanceIsDark()
        {
            throw new NotImplementedException();
        }

        public override string SystemSettingsAppearanceName()
        {
            throw new NotImplementedException();
        }

        public override Color SystemSettingsGetColor(SystemSettingsColor index)
        {
            throw new NotImplementedException();
        }

        public override Font SystemSettingsGetFont(SystemSettingsFont systemFont)
        {
            throw new NotImplementedException();
        }

        public override int SystemSettingsGetMetric(SystemSettingsMetric index)
        {
            throw new NotImplementedException();
        }

        public override int SystemSettingsGetMetric(SystemSettingsMetric index, IControl? control)
        {
            throw new NotImplementedException();
        }

        public override bool SystemSettingsHasFeature(SystemSettingsFeature index)
        {
            throw new NotImplementedException();
        }

        public override bool SystemSettingsIsUsingDarkBackground()
        {
            throw new NotImplementedException();
        }

        public override bool TimerGetEnabled(Timer timer)
        {
            throw new NotImplementedException();
        }

        public override int TimerGetInterval(Timer timer)
        {
            throw new NotImplementedException();
        }

        public override void TimerSetEnabled(Timer timer, bool value)
        {
            throw new NotImplementedException();
        }

        public override void TimerSetInterval(Timer timer, int value)
        {
            throw new NotImplementedException();
        }

        public override void TimerSetTick(Timer timer, Action? value)
        {
            throw new NotImplementedException();
        }
    }
}
