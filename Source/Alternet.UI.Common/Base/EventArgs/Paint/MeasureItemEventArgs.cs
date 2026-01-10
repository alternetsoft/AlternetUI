using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
#pragma warning disable
        public MeasureItemEventArgs(Graphics graphics, int index, Coord itemHeight)
#pragma warning restore
        {
            Reset(graphics, index, itemHeight);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MeasureItemEventArgs" /> class.</summary>
        /// <param name="graphics">The <see cref="Graphics" /> object being written to.</param>
        /// <param name="index">The index of the item for which you need the height or width.</param>
#pragma warning disable
        public MeasureItemEventArgs(Graphics graphics, int index = 0)
#pragma warning restore
        {
            Reset(graphics, index);
        }

        /// <summary>
        /// Gets or sets the <see cref="Graphics" /> object to measure against.
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
        /// Gets or sets the size of the item.
        /// This is an alias for the <see cref="ItemWidth"/> and <see cref="ItemHeight"/> properties.
        /// </summary>
        public SizeD ItemSize
        {
            get
            {
                return new SizeD(itemWidth, itemHeight);
            }

            set
            {
                itemWidth = value.Width;
                itemHeight = value.Height;
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

        /// <summary>
        /// Ensures that the specified <see cref="MeasureItemEventArgs"/> instance is initialized and associated with
        /// the given <see cref="Graphics"/> context and item index.
        /// </summary>
        /// <param name="e">A reference to the <see cref="MeasureItemEventArgs"/> instance to initialize or reset.
        /// If <see langword="null"/>, a new instance will be created and assigned.</param>
        /// <param name="graphics">The <see cref="Graphics"/> context to associate with the event arguments.
        /// Cannot be <see langword="null"/>.</param>
        /// <param name="index">The index of the item to be measured. The default value is 0.</param>
        public static void EnsureCreated([NotNull] ref MeasureItemEventArgs? e, Graphics graphics, int index = 0)
        {
            if (e == null)
                e = new MeasureItemEventArgs(graphics, index);
            else
                e.Reset(graphics, index);
        }

        /// <summary>
        /// Ensures that the specified <see cref="MeasureItemEventArgs"/> instance is initialized with the provided
        /// graphics context, item index, and item height.
        /// </summary>
        /// <remarks>If <paramref name="e"/> is already initialized, its state is reset with the new
        /// parameters. Otherwise, a new <see cref="MeasureItemEventArgs"/> is created and assigned to <paramref
        /// name="e"/>.</remarks>
        /// <param name="e">A reference to the <see cref="MeasureItemEventArgs"/> instance to initialize.
        /// If <see langword="null"/>, a new instance is created and assigned.</param>
        /// <param name="graphics">The <see cref="Graphics"/> context used for measuring the item.
        /// Cannot be <see langword="null"/>.</param>
        /// <param name="index">The zero-based index of the item to measure.</param>
        /// <param name="itemHeight">The height of the item to be measured.</param>
        public static void EnsureCreated([NotNull] ref MeasureItemEventArgs? e, Graphics graphics, int index, Coord itemHeight)
        {
            if (e == null)
                e = new MeasureItemEventArgs(graphics, index, itemHeight);
            else
                e.Reset(graphics, index, itemHeight);
        }

        /// <summary>
        /// Assigns the specified graphics context, item index, and item height to the current instance for subsequent
        /// drawing operations.
        /// </summary>
        /// <param name="graphics">The graphics context to use for rendering. Cannot be null.</param>
        /// <param name="index">The zero-based index of the item to assign. Must be greater than or equal to 0.</param>
        /// <param name="itemHeight">The height, in pixels, of the item to assign. Must be greater than 0.</param>
        public virtual void Reset(Graphics graphics, int index, Coord itemHeight)
        {
            this.graphics = graphics;
            this.index = index;
            this.itemHeight = itemHeight;
            this.itemWidth = 0;
        }

        /// <summary>
        /// Assigns the specified graphics context and item index to the current instance for subsequent
        /// drawing operations.
        /// </summary>
        /// <param name="graphics">The graphics context to use for rendering. Cannot be null.</param>
        /// <param name="index">The zero-based index of the item to assign. Must be greater than or equal to 0.</param>
        public virtual void Reset(Graphics graphics, int index = 0)
        {
            this.graphics = graphics;
            this.index = index;
            this.itemHeight = 0;
            this.itemWidth = 0;
        }
    }
}
