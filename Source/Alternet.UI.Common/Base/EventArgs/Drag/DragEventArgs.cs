using System;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for the <see cref="AbstractControl.DragDrop"/>, <see cref="AbstractControl.DragEnter"/>,
    /// or <see cref="AbstractControl.DragOver"/> events.
    /// </summary>
    public class DragEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DragEventArgs"/> class.
        /// </summary>
        /// <param name="data">The data associated with this event.</param>
        /// <param name="mouseClientLocation">The client coordinates of the mouse pointer
        /// in device-independent units.</param>
        /// <param name="effect">One of the <see cref="DragDropEffects"/> values.</param>
        public DragEventArgs(
            IDataObject data,
            PointD mouseClientLocation,
            DragDropEffects effect)
        {
            Data = data;
            MouseClientLocation = mouseClientLocation;
            Effect = effect;
        }

        /// <summary>
        /// Gets the <see cref="IDataObject"/> that contains the data associated
        /// with this event.
        /// </summary>
        public IDataObject Data { get; set; }

        /// <summary>
        /// Gets the client coordinates of the mouse pointer in device-independent units.
        /// </summary>
        public PointD MouseClientLocation { get; set; }

        /// <summary>
        /// Gets or sets the target drop effect in a drag-and-drop operation.
        /// </summary>
        public DragDropEffects Effect { get; set; }
    }
}