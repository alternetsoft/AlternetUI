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

        public abstract bool ShowExceptionWindow(
            Exception exception,
            string? additionalInfo = null,
            bool canContinue = true);

        public abstract IDataObject? ClipboardGetDataObject();

        public abstract void ClipboardSetDataObject(IDataObject value);

        public abstract DialogResult ShowMessageBox(MessageBoxInfo info);

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
