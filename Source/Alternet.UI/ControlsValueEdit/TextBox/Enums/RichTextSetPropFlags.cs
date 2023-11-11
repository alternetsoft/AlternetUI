using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates properties assign flags for <see cref="RichTextBox"/>.
    /// </summary>
    [Flags]
    public enum RichTextSetPropFlags
    {
        /// <summary>
        /// No flags are specified.
        /// </summary>
        None = 0x00,

        /// <summary>
        /// Specifies that this operation should be undoable.
        /// </summary>
        WithUndo = 0x01,

        /// <summary>
        /// Specifies that the properties should only be applied to paragraphs,
        /// and not the content.
        /// </summary>
        ParagraphsOnly = 0x02,

        /// <summary>
        /// Specifies that the properties should only be applied to characters,
        /// and not the paragraph.
        /// </summary>
        CharactersOnly = 0x04,

        /// <summary>
        /// Resets the existing properties before applying the new properties.
        /// </summary>
        Reset = 0x08,

        /// <summary>
        /// Removes the given properties instead of applying them.
        /// </summary>
        Remove = 0x10,
    }
}
