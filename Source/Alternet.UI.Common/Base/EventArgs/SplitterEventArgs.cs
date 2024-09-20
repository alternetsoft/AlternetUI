using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the method that will handle the splitter events.</summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">A <see cref="SplitterEventArgs" /> that contains the event data.</param>
    public delegate void SplitterEventHandler(object? sender, SplitterEventArgs e);

    /// <summary>
    /// Provides data for a splitter events.
    /// </summary>
    public class SplitterEventArgs : BaseEventArgs
    {
        private readonly Coord x;
        private readonly Coord y;
        private Coord splitX;
        private Coord splitY;

        /// <summary>
        /// Initializes an instance of the <see cref="SplitterEventArgs" /> class with the
        /// specified coordinates of the mouse pointer and the coordinates of the
        /// upper-left corner of the splitter.</summary>
        /// <param name="x">The x-coordinate of the mouse pointer.</param>
        /// <param name="y">The y-coordinate of the mouse pointer.</param>
        /// <param name="splitX">The x-coordinate of the upper-left corner of the splitter.</param>
        /// <param name="splitY">The y-coordinate of the upper-left corner of the splitter.</param>
        /// <remarks>
        /// Parameters are in client coordinates.
        /// </remarks>
        public SplitterEventArgs(Coord x, Coord y, Coord splitX, Coord splitY)
        {
            this.x = x;
            this.y = y;
            this.splitX = splitX;
            this.splitY = splitY;
        }

        /// <summary>
        /// Gets the x-coordinate of the mouse pointer.
        /// </summary>
        /// <returns>The x-coordinate of the mouse pointer.</returns>
        /// <remarks>
        /// Property value is in client coordinates.
        /// </remarks>
        public Coord X => x;

        /// <summary>
        /// Gets the y-coordinate of the mouse pointer.
        /// </summary>
        /// <returns>
        /// The y-coordinate of the mouse pointer.
        /// </returns>
        /// <remarks>
        /// Property value is in client coordinates.
        /// </remarks>
        public Coord Y => y;

        /// <summary>
        /// Gets or sets the x-coordinate of the upper-left corner of the splitter.
        /// </summary>
        /// <returns>The x-coordinate of the upper-left corner of the control.</returns>
        /// <remarks>
        /// Property value is in client coordinates.
        /// </remarks>
        public Coord SplitX
        {
            get
            {
                return splitX;
            }

            set
            {
                splitX = value;
            }
        }

        /// <summary>
        /// Gets or sets the y-coordinate of the upper-left corner of the splitter.
        /// </summary>
        /// <returns>The y-coordinate of the upper-left corner of the control.</returns>
        /// <remarks>
        /// Property value is in client coordinates.
        /// </remarks>
        public Coord SplitY
        {
            get
            {
                return splitY;
            }

            set
            {
                splitY = value;
            }
        }
    }
}
