using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal class MauiDialogFactoryHandler : DisposableObject, IDialogFactoryHandler
    {
        public IColorDialogHandler CreateColorDialogHandler(ColorDialog dialog)
        {
            throw new NotImplementedException();
        }

        public IFontDialogHandler CreateFontDialogHandler(FontDialog dialog)
        {
            throw new NotImplementedException();
        }

        public IOpenFileDialogHandler CreateOpenFileDialogHandler(OpenFileDialog dialog)
        {
            throw new NotImplementedException();
        }

        public ISaveFileDialogHandler CreateSaveFileDialogHandler(SaveFileDialog dialog)
        {
            throw new NotImplementedException();
        }

        public ISelectDirectoryDialogHandler CreateSelectDirectoryDialogHandler(
            SelectDirectoryDialog dialog)
        {
            throw new NotImplementedException();
        }

        public long? GetNumberFromUser(
            string message,
            string prompt,
            string caption,
            long value,
            long min,
            long max,
            Control? parent)
        {
            return null;
        }

        public string? GetTextFromUser(
            string message,
            string caption,
            string defaultValue,
            Control? parent)
        {
            return null;
        }

        public DialogResult ShowMessageBox(MessageBoxInfo info)
        {
            throw new NotImplementedException();
        }
    }
}
