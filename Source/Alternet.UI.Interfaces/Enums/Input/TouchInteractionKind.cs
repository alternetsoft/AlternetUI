using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies constants that define which touch/mouse event occured in the control.
    /// </summary>
    public enum TouchInteractionKind
    {
        /// <summary>
        /// 'CancelInteraction' event occured.
        /// </summary>
        CancelInteraction,

        /// <summary>
        /// 'EndInteraction' event occured.
        /// </summary>
        EndInteraction,

        /// <summary>
        /// 'DragInteraction' event occured.
        /// </summary>
        DragInteraction,

        /// <summary>
        /// 'StartInteraction' event occured.
        /// </summary>
        StartInteraction,

        /// <summary>
        /// 'EndHoverInteraction' event occured.
        /// </summary>
        EndHoverInteraction,

        /// <summary>
        /// 'MoveHoverInteraction' event occured.
        /// </summary>
        MoveHoverInteraction,

        /// <summary>
        /// 'StartHoverInteraction' event occured.
        /// </summary>
        StartHoverInteraction,
    }
}