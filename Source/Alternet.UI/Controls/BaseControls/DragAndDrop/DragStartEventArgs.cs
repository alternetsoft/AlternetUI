using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for the <see cref="Control.DragStart"/> event.
    /// </summary>
    public class DragStartEventArgs : EventArgs
    {
        private readonly Point mouseClientLocation;
        private readonly Point mouseDownLocation;

        /// <summary>
        /// Initializes a new instance of the <see cref="DragStartEventArgs"/> class.
        /// </summary>
        /// <param name="mouseClientLocation">The client coordinates of the mouse pointer
        /// in logical units (1/96th of an inch).</param>
        /// <param name="mouseDownLocation">Coordinates of the mouse pointer in the moment when
        /// <see cref="UIElement.MouseDown"/> event was fired.</param>
        public DragStartEventArgs(
            Point mouseClientLocation,
            Point mouseDownLocation)
        {
            this.mouseClientLocation = mouseClientLocation;
            this.mouseDownLocation = mouseDownLocation;
        }

        /// <summary>
        /// Gets or sets minimal mouse drag distance which starts drag operations.
        /// </summary>
        /// <remarks>
        /// Distance calculated between current mouse pointer cordinates
        /// and coordinates in the moment when
        /// <see cref="UIElement.MouseDown"/> event was fired. If distance is greater
        /// than <see cref="MinDragStartDistance"/>, drag operation can be started.
        /// </remarks>
        public static double MinDragStartDistance { get; set; } = 3;

        /// <summary>
        /// Gets the client coordinates of the mouse pointer in logical units (1/96th of an inch).
        /// </summary>
        public Point MouseClientLocation => mouseClientLocation;

        /// <summary>
        /// Gets coordinates of the mouse pointer in the moment when
        /// <see cref="UIElement.MouseDown"/> event was fired.
        /// </summary>
        /// <remarks>
        /// Coordinates of the mouse pointer are in logical units (1/96th of an inch).
        /// </remarks>
        public Point MouseDownLocation => mouseDownLocation;

        /// <summary>
        /// Gets whether distance between <see cref="MouseDownLocation"/> and
        /// <see cref="MouseClientLocation"/> is less than <see cref="MinDragStartDistance"/>.
        /// </summary>
        public bool DistanceIsLess =>
            MathUtils.DistanceIsLess(MouseDownLocation, MouseClientLocation, MinDragStartDistance);
    }
}
