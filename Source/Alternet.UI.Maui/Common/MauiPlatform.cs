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

        public static void Initialize()
        {
            if (initialized)
                return;
            NativeDrawing.Default = new SkiaDrawing();
            Default = new MauiPlatform();
            initialized = true;
        }

        /// <inheritdoc/>
        public override bool ShowExceptionWindow(
            Exception exception,
            string? additionalInfo = null,
            bool canContinue = true)
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
