using System;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for the <see cref="Control.QueryContinueDrag"/> event.
    /// </summary>
    public class QueryContinueDragEventArgs : EventArgs
    {
        /// <summary>
        ///  Initializes a new instance of the <see cref='QueryContinueDragEventArgs'/> class.
        /// </summary>
        public QueryContinueDragEventArgs(bool escapePressed, DragAction action)
        {
            EscapePressed = escapePressed;
            Action = action;
        }

        /// <summary>
        ///  Gets a value indicating whether the user pressed the ESC key.
        /// </summary>
        public bool EscapePressed { get; }

        /// <summary>
        ///  Gets or sets the status of a drag-and-drop operation.
        /// </summary>
        public DragAction Action { get; set; }
    }
}