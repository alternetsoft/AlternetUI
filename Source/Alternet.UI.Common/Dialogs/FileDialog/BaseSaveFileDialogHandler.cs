using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides a base implementation of <see cref="ISaveFileDialogHandler"/> interface.
    /// </summary>
    public class BaseSaveFileDialogHandler : BaseFileDialogHandler, ISaveFileDialogHandler
    {
        /// <inheritdoc/>
        public virtual bool OverwritePrompt { get; set; }
    }
}
