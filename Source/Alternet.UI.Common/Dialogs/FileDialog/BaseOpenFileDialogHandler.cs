using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides a base implementation of <see cref="IOpenFileDialogHandler"/> interface.
    /// </summary>
    public class BaseOpenFileDialogHandler : BaseFileDialogHandler, IOpenFileDialogHandler
    {
        private bool multiSelect;
     
        /// <inheritdoc/>
        public virtual bool MultiSelect
        {
            get => multiSelect;
            set => multiSelect = value;
        }

        /// <inheritdoc/>
        public virtual bool FileMustExist { get; set; }

        /// <inheritdoc/>
        public virtual bool AllowMultipleSelection { get; set; }

        /// <inheritdoc/>
        public virtual string[] FileNames => Array.Empty<string>();
    }
}
