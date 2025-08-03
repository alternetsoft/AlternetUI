using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies the columns that can be displayed in a file list box.
    /// </summary>
    public enum FileListBoxColumn
    {
        /// <summary>
        /// The file name column.
        /// </summary>
        Name,

        /// <summary>
        /// The date and time the file was last modified.
        /// </summary>
        DateModified,

        /// <summary>
        /// The file size column.
        /// </summary>
        Size,
    }
}
