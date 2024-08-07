﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which create system dialogs.
    /// </summary>
    public interface IDialogFactoryHandler : IDisposable
    {
        /// <summary>
        /// Creates <see cref="ISelectDirectoryDialogHandler"/> interface provider.
        /// </summary>
        /// <param name="dialog">Owner.</param>
        /// <returns></returns>
        ISelectDirectoryDialogHandler CreateSelectDirectoryDialogHandler(SelectDirectoryDialog dialog);

        /// <summary>
        /// Creates <see cref="IOpenFileDialogHandler"/> interface provider.
        /// </summary>
        /// <param name="dialog">Owner.</param>
        /// <returns></returns>
        IOpenFileDialogHandler CreateOpenFileDialogHandler(OpenFileDialog dialog);

        /// <summary>
        /// Creates <see cref="ISaveFileDialogHandler"/> interface provider.
        /// </summary>
        /// <param name="dialog">Owner.</param>
        /// <returns></returns>
        ISaveFileDialogHandler CreateSaveFileDialogHandler(SaveFileDialog dialog);

        /// <summary>
        /// Creates <see cref="IFontDialogHandler"/> interface provider.
        /// </summary>
        /// <param name="dialog">Owner.</param>
        /// <returns></returns>
        IFontDialogHandler CreateFontDialogHandler(FontDialog dialog);

        /// <summary>
        /// Creates <see cref="IColorDialogHandler"/> interface provider.
        /// </summary>
        /// <param name="dialog">Owner.</param>
        /// <returns></returns>
        IColorDialogHandler CreateColorDialogHandler(ColorDialog dialog);

        /// <inheritdoc cref="MessageBox.ShowDefault(MessageBoxInfo)"/>
        DialogResult ShowMessageBox(MessageBoxInfo info);

        /// <inheritdoc cref="DialogFactory.GetTextFromUser"/>
        string? GetTextFromUser(
            string message,
            string caption,
            string defaultValue,
            Control? parent,
            int x,
            int y,
            bool centre);

        /// <inheritdoc cref="DialogFactory.GetNumberFromUser"/>
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
