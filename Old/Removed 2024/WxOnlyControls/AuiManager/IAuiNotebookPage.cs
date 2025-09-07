using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Provides access to the <see cref="AuiNotebook"/> page properties and methods.
    /// </summary>
    internal interface IAuiNotebookPage
    {
        /// <summary>
        /// Gets the zero-based index of the page within the <see cref="AuiNotebook"/> control,
        /// or <see langword="null"/> if the item is not associated with
        /// a <see cref="AuiNotebook"/> control.
        /// </summary>
        int? Index { get; }
    }
}
