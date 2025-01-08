using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the method that will handle the measure item events.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">A <see cref="MeasureItemEventArgs" /> that contains the event data.</param>
    public delegate void MeasureItemEventHandler(object? sender, MeasureItemEventArgs e);

    /// <summary>
    /// Provides data for the measure item events.
    /// </summary>
    public class MeasureItemEventArgs : BaseEventArgs
    {
        private Graphics graphics;
        private Coord itemHeight;
        private Coord itemWidth;
        private int index;

        /// <summary>
        /// Initializes a new instance of the <see cref="MeasureItemEventArgs" /> class
        /// providing a parameter for the item height.
        /// </summary>
        /// <param name="graphics">The <see cref="Graphics" /> object being written to.</param>
        /// <param name="index">The index of the item for which you need the height or width.</param>
        /// <param name="itemHeight">The height of the item to measure relative to
        /// the <paramref name="graphics" /> object.</param>
        public MeasureItemEventArgs(Graphics graphics, int index, int itemHeight)
        {
            this.graphics = graphics;
            this.index = index;
            this.itemHeight = itemHeight;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MeasureItemEventArgs" /> class.</summary>
        /// <param name="graphics">The <see cref="Graphics" /> object being written to.</param>
        /// <param name="index">The index of the item for which you need the height or width.</param>
        public MeasureItemEventArgs(Graphics graphics, int index)
        {
            this.graphics = graphics;
            this.index = index;
        }

        /// <summary>
        /// Gets the <see cref="Graphics" /> object to measure against.
        /// </summary>
        /// <returns>
        /// The <see cref="Graphics" /> object to use to determine the scale of the item
        /// you are drawing.
        /// </returns>
        public virtual Graphics Graphics
        {
            get
            {
                return graphics;
            }

            set
            {
                graphics = value;
            }
        }

        /// <summary>
        /// Gets or sets the index of the item for which the height and width is needed.
        /// </summary>
        /// <returns>
        /// The index of the item to be measured.
        /// </returns>
        public virtual int Index
        {
            get
            {
                return index;
            }

            set
            {
                index = value;
            }
        }

        /// <summary>
        /// Gets or sets the height of the item specified by the
        /// <see cref="MeasureItemEventArgs.Index"/>.
        /// </summary>
        /// <returns>The height of the item measured.</returns>
        public virtual Coord ItemHeight
        {
            get
            {
                return itemHeight;
            }

            set
            {
                itemHeight = value;
            }
        }

        /// <summary>
        /// Gets or sets the width of the item specified by the
        /// <see cref="MeasureItemEventArgs.Index" />.
        /// </summary>
        /// <returns>The width of the item measured.</returns>
        public virtual Coord ItemWidth
        {
            get
            {
                return itemWidth;
            }

            set
            {
                itemWidth = value;
            }
        }
    }
}
