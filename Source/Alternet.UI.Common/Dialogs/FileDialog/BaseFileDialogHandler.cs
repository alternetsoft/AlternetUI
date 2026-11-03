using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides a base implementation of <see cref="IFileDialogHandler"/> interface.
    /// </summary>
    public class BaseFileDialogHandler : DisposableObject, IFileDialogHandler
    {
        private string? title;
        private string? filter;
        private string? initialDirectory;

        /// <inheritdoc/>
        public virtual int SelectedFilterIndex { get; set; }

        /// <inheritdoc/>
        public virtual bool NoShortcutFollow { get; set; }

        /// <inheritdoc/>
        public virtual bool ChangeDir { get; set; }

        /// <inheritdoc/>
        public virtual bool PreviewFiles { get; set; }

        /// <inheritdoc/>
        public virtual bool ShowHiddenFiles { get; set; }

        /// <inheritdoc/>
        public virtual bool ShowHelp { get; set; }

        /// <inheritdoc/>
        public virtual string? GetInitialDirectory()
        {
            return initialDirectory;
        }

        /// <inheritdoc/>
        public virtual void SetInitialDirectory(string? value)
        {
            initialDirectory = value;
        }

        /// <inheritdoc/>
        public virtual string? GetFilter()
        {
            return filter;
        }

        /// <inheritdoc/>
        public virtual void SetFilter(string? value)
        {
            filter = value;
        }

        /// <inheritdoc/>
        public virtual string? GetFileName()
        {
            return null;
        }

        /// <inheritdoc/>
        public virtual void SetFileName(string? value)
        {
        }

        /// <inheritdoc/>
        public virtual string? GetTitle()
        {
            return title;
        }

        /// <inheritdoc/>
        public virtual void SetTitle(string? value)
        {
            title = value;
        }

        /// <inheritdoc/>
        public virtual void ShowAsync(Window? owner, Action<bool>? onClose)
        {
            onClose?.Invoke(false);
        }
    }
}
