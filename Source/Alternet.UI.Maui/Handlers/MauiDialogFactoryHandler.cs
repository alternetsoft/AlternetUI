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
    internal partial class MauiDialogFactoryHandler : DisposableObject, IDialogFactoryHandler
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
            AbstractControl? parent)
        {
            /*
                var page = MauiApplicationHandler.GetParentPage(this);
                if (page is null)
                    return DialogResult.Cancel;

                int line = Position.Y + 1;
                int maxLine = Lines.Count;

                page.Dispatcher.DispatchAsync(Fn);

                Task<string> Fn()
                {
                    var lineNumberStr = String.Format(
                        Alternet.UI.Localization.CommonStrings.Default.LineNumberTemplate,
                        line,
                        maxLine);

                    var task = page.DisplayPromptAsync(
                                        Alternet.UI.Localization.CommonStrings.Default.WindowTitleGoToLine,
                                        lineNumberStr,
                                        Alternet.UI.Localization.CommonStrings.Default.ButtonOk,
                                        Alternet.UI.Localization.CommonStrings.Default.ButtonCancel,
                                        placeholder: null,
                                        maxLength: -1,
                                        Microsoft.Maui.Keyboard.Numeric,
                                        initialValue: line.ToString());

                    task.ContinueWith((s) =>
                    {
                        var myResult = s.Result;

                        if (!int.TryParse(myResult, out var intResult))
                            return;

                        intResult = Math.Max(intResult, 0);
                        intResult = Math.Min(intResult, maxLine - 1);

                        GoToLine(intResult - 1);
                    });

                    return task;
                }

            */
            return null;
        }

        public void GetNumberFromUserAsync(LongFromUserParams prm)
        {
        }

        public void GetTextFromUserAsync(TextFromUserParams prm)
        {
            var page = MauiApplicationHandler.GetParentPage(prm.Parent);
            if (page is null)
            {
                prm.RaiseActions(null);
                return;
            }

            page.Dispatcher.DispatchAsync(Fn);

            Task<string> Fn()
            {
                var task = page.DisplayPromptAsync(
                                    prm.SafeTitle,
                                    prm.Message,
                                    prm.SafeAcceptButtonText,
                                    prm.SafeCancelButtonText,
                                    placeholder: null,
                                    prm.MaxLength,
                                    Microsoft.Maui.Keyboard.Numeric,
                                    initialValue: prm.SafeDefaultValueAsString);

                task.ContinueWith((s) =>
                {
                    var myResult = s.Result;
                    prm.RaiseActions(myResult);
                });

                return task;
            }
        }

        public DialogResult ShowMessageBox(MessageBoxInfo info)
        {
            throw new NotImplementedException();
        }
    }
}
