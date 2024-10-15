using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for the <see cref="AbstractControl.DragStart"/> event.
    /// </summary>
    public class DragStartEventArgs : BaseCancelEventArgs
    {
        /// <summary>
        /// Gets or set time period limit used in <see cref="TimeIsGreater"/> property.
        /// </summary>
        public static long DefaultTimePeriodLimit = 10;

        private readonly MouseEventArgs mouseDownArgs;
        private readonly MouseEventArgs mouseMoveArgs;
        private readonly PointD mouseClientLocation;
        private readonly PointD mouseDownLocation;

        /// <summary>
        /// Initializes a new instance of the <see cref="DragStartEventArgs"/> class.
        /// </summary>
        /// <param name="mouseClientLocation">The client coordinates of the mouse pointer
        /// in device-independent units.</param>
        /// <param name="mouseDownLocation">Coordinates of the mouse pointer in the moment when
        /// <see cref="AbstractControl.MouseDown"/> event was fired.</param>
        /// <param name="mouseDownArgs"></param>
        /// <param name="mouseMoveArgs"></param>
        public DragStartEventArgs(
            PointD mouseClientLocation,
            PointD mouseDownLocation,
            MouseEventArgs mouseDownArgs,
            MouseEventArgs mouseMoveArgs)
        {
            this.mouseDownArgs = mouseDownArgs;
            this.mouseMoveArgs = mouseMoveArgs;
            this.mouseClientLocation = mouseClientLocation;
            this.mouseDownLocation = mouseDownLocation;
        }

        /// <summary>
        /// Gets or sets minimal mouse drag distance which starts drag operations.
        /// </summary>
        /// <remarks>
        /// Distance calculated between current mouse pointer cordinates
        /// and coordinates in the moment when
        /// <see cref="AbstractControl.MouseDown"/> event was fired. If distance is greater
        /// than <see cref="MinDragStartDistance"/>, drag operation can be started.
        /// </remarks>
        public static Coord MinDragStartDistance { get; set; } = 7;

        /// <summary>
        /// Gets the client coordinates of the mouse pointer in device-independent units.
        /// </summary>
        public PointD MouseClientLocation => mouseClientLocation;

        /// <summary>
        /// Gets coordinates of the mouse pointer in the moment when
        /// <see cref="AbstractControl.MouseDown"/> event was fired.
        /// </summary>
        /// <remarks>
        /// Coordinates of the mouse pointer are in device-independent units.
        /// </remarks>
        public PointD MouseDownLocation => mouseDownLocation;

        /// <summary>
        /// Gets or sets whether drag operation was actually started.
        /// </summary>
        /// <remarks>
        /// Set this property <c>true</c> if you want to start drag operation.
        /// See <see cref="AbstractControl.DragStart"/> for the details.
        /// </remarks>
        public bool DragStarted { get; set; }

        /// <summary>
        /// Gets whether distance between <see cref="MouseDownLocation"/> and
        /// <see cref="MouseClientLocation"/> is less than <see cref="MinDragStartDistance"/>.
        /// </summary>
        public bool DistanceIsLess
        {
            get
            {
                var result = DrawingUtils.DistanceIsLess(
                    MouseDownLocation,
                    MouseClientLocation,
                    MinDragStartDistance);
                return result;
            }
        }

        /// <summary>
        /// Gets whether <see cref="TimePeriod"/> is greater than <see cref="DefaultTimePeriodLimit"/>.
        /// </summary>
        public bool TimeIsGreater
        {
            get
            {
                return TimePeriod > DefaultTimePeriodLimit;
            }
        }

        /// <summary>
        /// Gets the time when mouse down event was received.
        /// </summary>
        public long TimestampStart => mouseDownArgs.Timestamp;

        /// <summary>
        /// Gets the time when mouse move event was received.
        /// </summary>
        public long TimestampEnd => mouseMoveArgs.Timestamp;

        /// <summary>
        /// Gets time period between <see cref="TimestampEnd"/> and <see cref="TimestampStart"/>.
        /// </summary>
        public long TimePeriod => Math.Abs(TimestampEnd - TimestampStart);
    }
}
