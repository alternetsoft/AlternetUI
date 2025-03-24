using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates all possible <see cref="string"/> property editor kinds in the property grid control.
    /// </summary>
    public enum PropertyGridEditKindString
    {
        /// <summary>
        /// Uses simple <c>TextBox</c>.
        /// </summary>
        Simple,

        /// <summary>
        /// Uses <c>TextBox</c> with dialog which allows to edit long string values.
        /// </summary>
        Long,

        /// <summary>
        /// Uses <c>TextBox</c> with ellipsis button. Use
        /// <c>PropertyGrid.ButtonClick</c> to handle button clicks.
        /// </summary>
        Ellipsis,

        /// <summary>
        /// Uses <c>TextBox</c> with open file dialog which allows
        /// to select filename. It is possible to specify file masks, default path and
        /// dialog title in the property attributes.
        /// </summary>
        FileName,

        /// <summary>
        /// Uses <c>TextBox</c> with open file dialog which allows
        /// to select image filename. It is possible to specify default path and
        /// dialog title in the property attributes. File mask is setup to
        /// supported image extensions.
        /// </summary>
        ImageFileName,

        /// <summary>
        /// Uses <c>TextBox</c> with select directory dialog which allows
        /// to select path. It is possible to specify file masks, default path and
        /// dialog title in the property attributes.
        /// </summary>
        Directory,
    }
}
