using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Defines visual style of the <see cref="StdTreeView"/> controls.
    /// </summary>
    [Flags]
    public enum TreeViewCreateStyle
    {
        /// <summary>
        /// Use this style to show "+" and "-" buttons to the left of parent items.
        /// </summary>
        /// <remarks>
        /// If not specified no buttons are to be drawn.
        /// </remarks>
        HasButtons = 0x0001,

        /// <summary>
        /// Use this style to hide vertical level connectors.
        /// </summary>
        NoLines = 0x0004,

        /// <summary>
        /// Use this style to show lines between root nodes.
        /// </summary>
        /// <remarks>
        /// Only applicable if <see cref="HideRoot"/> is set and <see cref="NoLines"/> is not set.
        /// </remarks>
        LinesAtRoot = 0x0008,

        /// <summary>
        /// Selects alternative style of +/- buttons and shows rotating ("twisting") arrows
        /// instead.
        /// </summary>
        /// <remarks>
        ///  Currently this style is only implemented under Microsoft Windows and is ignored under
        ///  the other platforms.
        /// </remarks>
        TwistButtons = 0x0010,

        /// <summary>
        /// Use this style to allow a range of items to be selected. If a second range is
        /// selected, the current range, if any, is deselected.
        /// </summary>
        /// <remarks>
        /// If this style is not specified (default), only one item may be selected at a time.
        /// Selecting another item causes the current selection, if any, to be deselected.
        /// </remarks>
        Multiple = 0x0020,

        /// <summary>
        /// Use this style to cause row heights to be just big enough to fit the content.
        /// </summary>
        /// <remarks>
        /// If not set, all rows use the largest row height. The default is that this flag
        /// is unset. Not supported on all platforms.
        /// </remarks>
        VariableRowHeight = 0x0080,

        /// <summary>
        /// Use this style if you wish the user to be able to edit labels in the tree control.
        /// </summary>
        EditLabels = 0x0200,

        /// <summary>
        /// Use this style to draw a contrasting border between displayed rows.
        /// </summary>
        RowLines = 0x0400,

        /// <summary>
        /// Use this style to suppress the display of the root node, effectively
        /// causing the first-level nodes to appear as a series of root nodes.
        /// </summary>
        HideRoot = 0x0800,

        /// <summary>
        /// Use this style to have the background colour and the selection highlight
        /// extend over the entire horizontal row of the tree control window.
        /// </summary>
        /// <remarks>
        /// This flag is ignored under Windows unless you specify <see cref="NoLines"/> as well.
        /// </remarks>
        FullRowHighlight = 0x2000,
    }
}