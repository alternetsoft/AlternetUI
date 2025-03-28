using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies the kind of buttons used in a TreeView control.
    /// </summary>
    public enum TreeViewButtonsKind
    {
        /// <summary>
        /// No buttons.
        /// </summary>
        Null,

        /// <summary>
        /// Empty buttons.
        /// </summary>
        Empty,

        /// <summary>
        /// Angle-shaped buttons.
        /// </summary>
        Angle,

        /// <summary>
        /// Arrow-shaped buttons.
        /// </summary>
        Arrow,

        /// <summary>
        /// Triangle arrow-shaped buttons.
        /// </summary>
        TriangleArrow,

        /// <summary>
        /// Plus and minus buttons.
        /// </summary>
        PlusMinus,

        /// <summary>
        /// Plus and minus buttons inside a square.
        /// </summary>
        PlusMinusSquare,
    }
}
