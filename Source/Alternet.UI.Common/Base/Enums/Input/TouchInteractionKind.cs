using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies constants that define which touch/mouse event occurred in the control.
    /// </summary>
    public enum TouchInteractionKind
    {
        /// <summary>
        /// 'CancelInteraction' event occurred.
        /// </summary>
        CancelInteraction,

        /// <summary>
        /// 'EndInteraction' event occurred.
        /// </summary>
        EndInteraction,

        /// <summary>
        /// 'DragInteraction' event occurred.
        /// </summary>
        DragInteraction,

        /// <summary>
        /// 'StartInteraction' event occurred.
        /// </summary>
        StartInteraction,

        /// <summary>
        /// 'EndHoverInteraction' event occurred.
        /// </summary>
        EndHoverInteraction,

        /// <summary>
        /// 'MoveHoverInteraction' event occurred.
        /// </summary>
        MoveHoverInteraction,

        /// <summary>
        /// 'StartHoverInteraction' event occurred.
        /// </summary>
        StartHoverInteraction,
    }
}