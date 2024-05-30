using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public interface IDialogFactoryHandler : IDisposable
    {
        ISelectDirectoryDialogHandler CreateSelectDirectoryDialogHandler(SelectDirectoryDialog dialog);

        IOpenFileDialogHandler CreateOpenFileDialogHandler(OpenFileDialog dialog);

        ISaveFileDialogHandler CreateSaveFileDialogHandler(SaveFileDialog dialog);

        IFontDialogHandler CreateFontDialogHandler(FontDialog dialog);

        IColorDialogHandler CreateColorDialogHandler(ColorDialog dialog);

        DialogResult ShowMessageBox(MessageBoxInfo info);

        string? GetTextFromUser(
            string message,
            string caption,
            string defaultValue,
            Control? parent,
            int x,
            int y,
            bool centre);

        long? GetNumberFromUser(
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
