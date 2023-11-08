using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates flags used in some <see cref="RichTextBox"/> methods.
    /// </summary>
    [Flags]
    public enum RichTextSetStyleFlags
    {
        /// <summary>
        /// No style flag.
        /// </summary>
        None = 0,

        /// <summary>
        /// Specifies that this operation should be undoable.
        /// </summary>
        WithUndo = 0x01,

        /// <summary>
        /// Specifies that the style should not be applied if the
        /// combined style at this point is already the style in question.
        /// </summary>
        Optimize = 0x02,

        /// <summary>
        /// Specifies that the style should only be applied to paragraphs,
        /// and not the content. This allows content styling to be
        /// preserved independently from that of e.g. a named paragraph style.
        /// </summary>
        ParagraphsOnly = 0x04,

        /// <summary>
        /// Specifies that the style should only be applied to characters,
        /// and not the paragraph. This allows content styling to be
        /// preserved independently from that of e.g. a named paragraph style.
        /// </summary>
        CharactersOnly = 0x08,

        /// <summary>
        /// For RichTextBox.SetListStyle only:
        /// specifies starting from the given number, otherwise
        /// deduces number from existing attributes
        /// </summary>
        Renumber = 0x10,

        /// <summary>
        /// For RichTextBox.SetListStyle only: specifies the list level for all paragraphs, otherwise
        /// the current indentation will be used
        /// </summary>
        SpecifyLevel = 0x20,

        /// <summary>
        /// Resets the existing style before applying the new style
        /// </summary>
        Reset = 0x40,

        /// <summary>
        /// Removes the given style instead of applying it
        /// </summary>
        Remove = 0x80,
    }
}