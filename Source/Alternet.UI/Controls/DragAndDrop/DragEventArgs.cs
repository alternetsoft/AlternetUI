using System;
using System.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for the <see cref="Window.DragDrop"/>, <see cref="Window.DragEnter"/>, or <see cref="Window.DragOver"/> events.
    /// </summary>
    public class DragEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DragEventArgs"/> class.
        /// </summary>
        /// <param name="data">The data associated with this event.</param>
        /// <param name="inputState">The current state of the SHIFT, CTRL, and ALT keys, as well as the state of the mouse buttons.</param>
        /// <param name="mouseClientLocation">The client coordinates of the mouse pointer in logical units (1/96th of an inch).</param>
        /// <param name="allowedEffect">One of the <see cref="DragDropEffects"/> values.</param>
        /// <param name="effect">One of the <see cref="DragDropEffects"/> values.</param>
        public DragEventArgs(
            IDataObject data,
            DragInputState inputState,
            PointF mouseClientLocation,
            DragDropEffects allowedEffect,
            DragDropEffects effect)
        {
            Data = data;
            InputState = inputState;
            MouseClientLocation = mouseClientLocation;
            AllowedEffect = allowedEffect;
            Effect = effect;
        }

        /// <summary>
        /// Gets the <see cref="IDataObject"/> that contains the data associated with this event.
        /// </summary>
        public IDataObject Data { get; }

        /// <summary>
        /// Gets the current state of the SHIFT, CTRL, and ALT keys, as well as the state of the mouse buttons.
        /// </summary>
        public DragInputState InputState { get; }

        /// <summary>
        /// Gets the client coordinates of the mouse pointer in logical units (1/96th of an inch).
        /// </summary>
        public PointF MouseClientLocation { get; }

        /// <summary>
        /// Gets which drag-and-drop operations are allowed by the originator (or source) of the drag event.
        /// </summary>
        public DragDropEffects AllowedEffect { get; }

        /// <summary>
        /// Gets or sets the target drop effect in a drag-and-drop operation.
        /// </summary>
        public DragDropEffects Effect { get; set; }
    }
}