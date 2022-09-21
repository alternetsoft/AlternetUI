using Alternet.Drawing;
using System;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for the <see cref="Control.DragDrop"/>, <see cref="Control.DragEnter"/>, or <see cref="Control.DragOver"/> events.
    /// </summary>
    public class DragEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DragEventArgs"/> class.
        /// </summary>
        /// <param name="data">The data associated with this event.</param>
        /// <param name="mouseClientLocation">The client coordinates of the mouse pointer in logical units (1/96th of an inch).</param>
        /// <param name="effect">One of the <see cref="DragDropEffects"/> values.</param>
        public DragEventArgs(
            IDataObject data,
            Point mouseClientLocation,
            DragDropEffects effect)
        {
            Data = data;
            MouseClientLocation = mouseClientLocation;
            Effect = effect;
        }

        /// <summary>
        /// Gets the <see cref="IDataObject"/> that contains the data associated with this event.
        /// </summary>
        public IDataObject Data { get; }

        /// <summary>
        /// Gets the client coordinates of the mouse pointer in logical units (1/96th of an inch).
        /// </summary>
        public Point MouseClientLocation { get; }

        /// <summary>
        /// Gets or sets the target drop effect in a drag-and-drop operation.
        /// </summary>
        public DragDropEffects Effect { get; set; }
    }
}