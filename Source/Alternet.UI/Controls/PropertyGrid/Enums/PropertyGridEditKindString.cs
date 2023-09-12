using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates all possible <see cref="string"/> edit kinds in <see cref="PropertyGrid"/>.
    /// </summary>
    public enum PropertyGridEditKindString
    {
        /// <summary>
        /// Uses simple <see cref="TextBox"/>.
        /// </summary>
        Simple,

        /// <summary>
        /// Uses <see cref="TextBox"/> with dialog which allows to edit long string values.
        /// </summary>
        Long,

        /// <summary>
        /// Uses <see cref="TextBox"/> with ellipsis button. Use
        /// <see cref="PropertyGrid.ButtonClick"/> to handle button clicks.
        /// </summary>
        Ellipsis,

        /// <summary>
        /// Uses <see cref="TextBox"/> with <see cref="OpenFileDialog"/> dialog which allows
        /// to select filename. It is possible to specify file masks, default path and
        /// dialog title in the property attributes.
        /// </summary>
        FileName,

        /// <summary>
        /// Uses <see cref="TextBox"/> with <see cref="OpenFileDialog"/> dialog which allows
        /// to select image filename. It is possible to specify default path and
        /// dialog title in the property attributes. File mask is setup to
        /// supported image extensions.
        /// </summary>
        ImageFileName,

        /// <summary>
        /// Uses <see cref="TextBox"/> with <see cref="SelectDirectoryDialog"/> dialog which allows
        /// to select path. It is possible to specify file masks, default path and
        /// dialog title in the property attributes.
        /// </summary>
        Directory,
    }
}
