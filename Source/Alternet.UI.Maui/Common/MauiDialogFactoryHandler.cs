using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.UI.Localization;

using Microsoft.Maui.Dispatching;

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

        /// <param name="message">A (possibly) multiline dialog message above the single line
        /// <paramref name="prompt"/>.</param>
        /// <param name="prompt">Single line dialog prompt.</param>
        /// <param name="caption">Dialog title.</param>
        /// <param name="value">Default value. Optional. Default is 0.</param>
        /// <param name="min">A positive minimal value. Optional. Default is 0.</param>
        /// <param name="max">A positive maximal value. Optional. Default is 100.</param>
        /// <param name="parent">Dialog parent.</param>
        public long? GetNumberFromUser(
            string message,
            string prompt,
            string caption,
            long value,
            long min,
            long max,
            Control? parent)
        {
            /*
            var page = MauiApplicationHandler.GetParentPage(parent);

            if (page is null)
                return null;

            Task<string> ShowDialog()
            {
                var result = page.DisplayPromptAsync(
                                    caption,
                                    prompt,
                                    CommonStrings.Default.ButtonOk,
                                    CommonStrings.Default.ButtonCancel,
                                    placeholder: message,
                                    maxLength: -1,
                                    Microsoft.Maui.Keyboard.Numeric,
                                    initialValue: value.ToString());
                return result;
            }

            string? result = await ShowDialog();

            if (long.TryParse(result, out var longResult))
                return longResult;
            */
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
