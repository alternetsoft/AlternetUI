using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the method that handles the <see langword="Scroll" /> event of the
    /// <see cref="ScrollBar" /> and other controls.</summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">A <see cref="ScrollEventArgs" /> that contains the event data.</param>
    public delegate void ScrollEventHandler(object sender, ScrollEventArgs e);

    /// <summary>
    /// Provides data for the <see langword="Scroll" /> event.
    /// </summary>
    public class ScrollEventArgs : BaseEventArgs
    {
        private readonly ScrollEventType type;
        private readonly int oldValue = -1;
        private readonly ScrollOrientation scrollOrientation;
        private int newValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollEventArgs" /> class
        /// using the given values for the <see cref="ScrollEventArgs.Type" /> and
        /// <see cref="ScrollEventArgs.NewValue" /> properties.</summary>
        /// <param name="type">One of the <see cref="ScrollEventType" /> values.</param>
        /// <param name="newValue">The new value for the scroll bar.</param>
        public ScrollEventArgs(ScrollEventType type, int newValue)
        {
            this.type = type;
            this.newValue = newValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollEventArgs" /> class using
        /// the given values for the <see cref="ScrollEventArgs.Type" />,
        /// <see cref="ScrollEventArgs.NewValue" />, and
        /// <see cref="ScrollEventArgs.ScrollOrientation" /> properties.</summary>
        /// <param name="type">One of the <see cref="ScrollEventType" /> values.</param>
        /// <param name="newValue">The new value for the scroll bar.</param>
        /// <param name="scroll">One of the <see cref="ScrollOrientation" /> values.</param>
        public ScrollEventArgs(ScrollEventType type, int newValue, ScrollOrientation scroll)
        {
            this.type = type;
            this.newValue = newValue;
            scrollOrientation = scroll;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollEventArgs" /> class
        /// using the given values for the <see cref="ScrollEventArgs.Type" />,
        /// <see cref="ScrollEventArgs.OldValue" />,
        /// and <see cref="ScrollEventArgs.NewValue" /> properties.</summary>
        /// <param name="type">One of the <see cref="ScrollEventType" /> values.</param>
        /// <param name="oldValue">The old value for the scroll bar.</param>
        /// <param name="newValue">The new value for the scroll bar.</param>
        public ScrollEventArgs(ScrollEventType type, int oldValue, int newValue)
        {
            this.type = type;
            this.newValue = newValue;
            this.oldValue = oldValue;
        }

        /// <summary>Initializes a new instance of the <see cref="ScrollEventArgs" />
        /// class using the given values for the <see cref="ScrollEventArgs.Type" />,
        /// <see cref="ScrollEventArgs.OldValue" />, <see cref="ScrollEventArgs.NewValue" />,
        /// and <see cref="ScrollEventArgs.ScrollOrientation" /> properties.</summary>
        /// <param name="type">One of the <see cref="ScrollEventType" /> values.</param>
        /// <param name="oldValue">The old value for the scroll bar.</param>
        /// <param name="newValue">The new value for the scroll bar.</param>
        /// <param name="scroll">One of the <see cref="ScrollOrientation" /> values.</param>
        public ScrollEventArgs(
            ScrollEventType type,
            int oldValue,
            int newValue,
            ScrollOrientation scroll)
        {
            this.type = type;
            this.newValue = newValue;
            scrollOrientation = scroll;
            this.oldValue = oldValue;
        }

        internal ScrollEventArgs()
        {
        }

        /// <summary>
        /// Gets the scroll bar orientation that raised the <see langword="Scroll" /> event.
        /// </summary>
        /// <returns>One of the <see cref="ScrollOrientation" /> values.</returns>
        public ScrollOrientation ScrollOrientation => scrollOrientation;

        /// <summary>
        /// Gets the type of scroll event that occurred.
        /// </summary>
        /// <returns>One of the <see cref="ScrollEventType" /> values.</returns>
        public ScrollEventType Type => type;

        /// <summary>Gets or sets the new <see cref="ScrollBar.Value" /> of the scroll bar.</summary>
        /// <returns>The numeric value that the <see cref="ScrollBar.Value" /> property
        /// will be changed to.</returns>
        public int NewValue
        {
            get
            {
                return newValue;
            }

            set
            {
                newValue = value;
            }
        }

        /// <summary>Gets the old <see cref="ScrollBar.Value" /> of the scroll bar.</summary>
        /// <returns>The numeric value that the <see cref="ScrollBar.Value" /> property
        /// contained before it changed.</returns>
        public int OldValue => oldValue;
    }
}
