using System;
using System.ComponentModel;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for the <see cref="ListView.ColumnClick"/> event.
    /// </summary>
    public class ListViewColumnEventArgs : CancelEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ListViewColumnEventArgs"/> class.
        /// </summary>
        public ListViewColumnEventArgs(long columnIndex)
        {
            ColumnIndex = columnIndex;
        }

        /// <summary>
        /// Gets the zero-based index of the <see cref="ListViewColumn"/>.
        /// </summary>
        public long ColumnIndex { get; }
    }
}