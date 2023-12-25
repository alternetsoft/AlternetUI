using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the method that will handle the <see cref="Control.QueryContinueDrag" /> event.</summary>
    /// <param name="sender">The source of an event.</param>
    /// <param name="e">A <see cref="QueryContinueDragEventArgs" />
    /// that contains the event data.</param>
    public delegate void QueryContinueDragEventHandler(object? sender, QueryContinueDragEventArgs e);

    /// <summary>
    /// Provides data for the <see cref="Control.QueryContinueDrag" /> event.</summary>
    public class QueryContinueDragEventArgs : EventArgs
    {
        private readonly int keyState;

        private readonly bool escapePressed;

        private DragAction action;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryContinueDragEventArgs"/> class.</summary>
        /// <param name="keyState">The current state of the SHIFT, CTRL, and ALT keys.</param>
        /// <param name="escapePressed">
        /// <see langword="true" /> if the ESC key was pressed; otherwise, <see langword="false" />.
        /// </param>
        /// <param name="action">A <see cref="DragAction" /> value.</param>
        public QueryContinueDragEventArgs(int keyState, bool escapePressed, DragAction action)
        {
            this.keyState = keyState;
            this.escapePressed = escapePressed;
            this.action = action;
        }

        /// <summary>
        /// Gets the current state of the SHIFT, CTRL, and ALT keys.
        /// </summary>
        /// <returns>The current state of the SHIFT, CTRL, and ALT keys.</returns>
        public int KeyState => keyState;

        /// <summary>
        /// Gets whether the user pressed the ESC key.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if the ESC key was pressed;
        /// otherwise, <see langword="false" />.
        /// </returns>
        public bool EscapePressed => escapePressed;

        /// <summary>
        /// Gets or sets the status of a drag-and-drop operation.
        /// </summary>
        /// <returns>A <see cref="DragAction" /> value.</returns>
        public DragAction Action
        {
            get
            {
                return action;
            }

            set
            {
                action = value;
            }
        }
    }
}
