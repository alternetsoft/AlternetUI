using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides a base implementation of <see cref="ISelectDirectoryDialogHandler"/> interface.
    /// </summary>
    public class BaseSelectDirectoryDialogHandler : DisposableObject, ISelectDirectoryDialogHandler
    {
        private string? title;
        private string? initialDirectory;
        private string? directoryName;

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

        /// <inheritdoc/>
        public virtual string? GetDirectoryName()
        {
            return directoryName;
        }

        /// <inheritdoc/>
        public virtual void SetDirectoryName(string? value)
        {
            directoryName = value;
        }
    }
}
