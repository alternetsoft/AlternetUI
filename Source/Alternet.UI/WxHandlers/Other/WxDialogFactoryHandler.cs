using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal class WxDialogFactoryHandler : DisposableObject, IDialogFactoryHandler
    {
        internal const string DialogCancelGuid = "5DB20A10B5974CD4885CFCF346AF0F81";

        /// <inheritdoc/>
        public IOpenFileDialogHandler CreateOpenFileDialogHandler(OpenFileDialog dialog)
        {
            return new UI.Native.FileDialog()
            {
                Mode = Native.FileDialogMode.Open,
            };
        }

        /// <inheritdoc/>
        public ISaveFileDialogHandler CreateSaveFileDialogHandler(SaveFileDialog dialog)
        {
            return new UI.Native.FileDialog()
            {
                Mode = Native.FileDialogMode.Save,
            };
        }

        /// <inheritdoc/>
        public IColorDialogHandler CreateColorDialogHandler(ColorDialog dialog)
        {
            return new UI.Native.ColorDialog();
        }

        /// <inheritdoc/>
        public ISelectDirectoryDialogHandler CreateSelectDirectoryDialogHandler(
            SelectDirectoryDialog dialog)
        {
            return new UI.Native.SelectDirectoryDialog();
        }

        /// <inheritdoc/>
        public IFontDialogHandler CreateFontDialogHandler(FontDialog dialog)
        {
            return new UI.Native.FontDialog();
        }

        public DialogResult ShowMessageBox(MessageBoxInfo info)
        {
            var nativeOwner = UI.Native.NativeObject.GetNativeWindow(info.Owner);
            return Native.MessageBox.Show(
                nativeOwner,
                info.Text?.ToString() ?? string.Empty,
                info.Caption ?? string.Empty,
                info.Buttons,
                info.Icon,
                info.DefaultButton);
        }

        /// <summary>
        /// Popups a dialog box with title, message and text input bot.
        /// The user may type in text and press OK to return this text, or press Cancel
        /// to return the empty string.
        /// </summary>
        /// <param name="prm">Dialog parameters.</param>
        public void GetTextFromUserAsync(TextFromUserParams prm)
        {
            WindowTextInput.GetTextFromUserAsync(prm);
        }

        /// <summary>
        /// Shows a dialog asking the user for numeric input.
        /// </summary>
        public void GetNumberFromUserAsync(LongFromUserParams prm)
        {
            WindowTextInput.GetLongFromUserAsync(prm);
        }

        /// <summary>
        /// Shows a dialog asking the user for numeric input.
        /// </summary>
        internal void NativeGetNumberFromUserAsync(LongFromUserParams prm)
        {
            var handle = WxApplicationHandler.WxWidget(prm.Parent);

            long minValue = MathUtils.ValueOrMin(prm.MinValue);
            long maxValue;
            long value = MathUtils.ValueOrMin(prm.DefaultValue);

            if (prm.MaxValue is null || prm.MaxValue < 0)
                maxValue = long.MaxValue;
            else
                maxValue = prm.MaxValue.Value;

            var result = Native.WxOtherFactory.GetNumberFromUser(
                string.Empty,
                prm.SafeMessage,
                prm.SafeTitle,
                value,
                minValue,
                maxValue,
                handle,
                (-1, -1));
            if (result < 0)
                prm.RaiseActions(null);
            else
                prm.RaiseActions(result);
        }

        internal void NativeGetTextFromUserAsync(TextFromUserParams prm)
        {
            var handle = WxApplicationHandler.WxWidget(prm.Parent);
            var result = Native.WxOtherFactory.GetTextFromUser(
                prm.SafeMessage,
                prm.SafeTitle,
                prm.SafeDefaultValueAsString,
                handle,
                -1,
                -1,
                true);
            if (result == DialogCancelGuid)
                prm.RaiseActions(null);
            else
                prm.RaiseActions(result);
        }
    }
}
