using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// A <see cref="ScrollBar"/> is a control that represents a horizontal or vertical scrollbar.
    /// </summary>
    /// <remarks>
    /// A scrollbar has the following main attributes: range, thumb size,
    /// page size, and position.The range is the total number of units
    /// associated with the view represented by the scrollbar.For a table
    /// with 15 columns, the range would be 15. The thumb size is the number of units
    /// that are currently visible.For the table example, the window might be sized
    /// so that only 5 columns are currently visible, in which case the application
    /// would set the thumb size to 5. When the thumb size becomes the same as or
    /// greater than the range, the scrollbar will be automatically hidden on
    /// most platforms.The page size is the number of units that the scrollbar
    /// should scroll by, when 'paging' through the data.This value is normally the
    /// same as the thumb size length, because it is natural to assume that the visible
    /// window size defines a page.The scrollbar position is the current thumb position.
    /// Most applications will find it convenient to provide a function called
    /// AdjustScrollbars() which can be called initially, from an OnSize event
    /// handler, and whenever the application data changes in size.It will adjust
    /// the view, object and page size according to the size of the window and the size of the data.
    /// </remarks>
    public class ScrollBar : Control
    {
        private int minimum;
        private int maximum = 100;
        private int smallChange = 1;
        private int largeChange = 10;
        private int value;

        /// <summary>
        /// Occurs when the scroll box has been moved by either a mouse or keyboard action.
        /// </summary>
        public event ScrollEventHandler? Scroll;

        /// <summary>
        /// Occurs when the <see cref="Value" /> property is changed, either
        /// by a <see cref="Scroll" /> event or programmatically.
        /// </summary>
        [Category("Action")]
        public event EventHandler? ValueChanged;

        /// <summary>
        /// Gets or sets a value to be added to or subtracted from the
        /// <see cref="Value" /> property when the scroll box is moved a large distance.
        /// </summary>
        /// <returns>A numeric value. The default value is 10.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The assigned value is less than 0.
        /// </exception>
        [Category("Behavior")]
        [DefaultValue(10)]
        [RefreshProperties(RefreshProperties.Repaint)]
        public int LargeChange
        {
            get
            {
                return Math.Min(largeChange, maximum - minimum + 1);
            }

            set
            {
                if (largeChange != value)
                {
                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException(
                            nameof(LargeChange),
                            SR.GetString(
                                "InvalidLowBoundArgumentEx",
                                nameof(LargeChange),
                                value.ToString(CultureInfo.CurrentCulture),
                                0.ToString(CultureInfo.CurrentCulture)));
                    }

                    largeChange = value;
                    UpdateScrollInfo();
                }
            }
        }

        /// <summary>
        /// Gets or sets the upper limit of values of the scrollable range.
        /// </summary>
        /// <returns>
        /// A numeric value. The default value is 100.
        /// </returns>
        [Category("Behavior")]
        [DefaultValue(100)]
        [RefreshProperties(RefreshProperties.Repaint)]
        public int Maximum
        {
            get
            {
                return maximum;
            }

            set
            {
                if (maximum != value)
                {
                    if (minimum > value)
                    {
                        minimum = value;
                    }

                    if (value < this.value)
                    {
                        Value = value;
                    }

                    maximum = value;
                    UpdateScrollInfo();
                }
            }
        }

        /// <summary>
        /// Gets or sets the lower limit of values of the scrollable range.
        /// </summary>
        /// <returns>
        /// A numeric value. The default value is 0.
        /// </returns>
        [Category("Behavior")]
        [DefaultValue(0)]
        [RefreshProperties(RefreshProperties.Repaint)]
        public int Minimum
        {
            get
            {
                return minimum;
            }

            set
            {
                if (minimum != value)
                {
                    if (maximum < value)
                    {
                        maximum = value;
                    }

                    if (value > this.value)
                    {
                        Value = value;
                    }

                    minimum = value;
                    UpdateScrollInfo();
                }
            }
        }

        /// <summary>
        /// Gets or sets the value to be added to or subtracted from
        /// the <see cref="Value" /> property when the scroll thumb is moved
        /// a small distance.
        /// </summary>
        /// <returns>A numeric value. The default value is 1.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The assigned value is less than 0.
        /// </exception>
        [Category("Behavior")]
        [DefaultValue(1)]
        [SRDescription("ScrollBarSmallChangeDescr")]
        public int SmallChange
        {
            get
            {
                return Math.Min(smallChange, LargeChange);
            }

            set
            {
                if (smallChange != value)
                {
                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException(
                            nameof(SmallChange),
                            SR.GetString(
                                "InvalidLowBoundArgumentEx",
                                nameof(SmallChange),
                                value.ToString(CultureInfo.CurrentCulture),
                                0.ToString(CultureInfo.CurrentCulture)));
                    }

                    smallChange = value;
                    UpdateScrollInfo();
                }
            }
        }

        /// <summary>
        /// Gets or sets a numeric value that represents the current position of the
        /// scroll thumb on the scroll bar control.
        /// </summary>
        /// <returns>A numeric value that is within the <see cref="Minimum" /> and
        /// <see cref="Maximum" /> range. The default value is 0.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The assigned value is less than the <see cref="Minimum" /> property value
        /// or is greater than the <see cref="Maximum" /> property value.
        /// </exception>
        [Category("Behavior")]
        [DefaultValue(0)]
        [Bindable(true)]
        public int Value
        {
            get
            {
                return value;
            }

            set
            {
                if (this.value != value)
                {
                    if (value < minimum || value > maximum)
                    {
                        throw new ArgumentOutOfRangeException(
                            nameof(Value),
                            SR.GetString(
                                "InvalidBoundArgument",
                                nameof(Value),
                                value.ToString(CultureInfo.CurrentCulture),
                                "'minimum'",
                                "'maximum'"));
                    }

                    this.value = value;

                    UpdateScrollInfo();

                    OnValueChanged(EventArgs.Empty);
                }
            }
        }

        internal new Native.ScrollBar NativeControl => (Native.ScrollBar)base.NativeControl;

        /// <summary>
        /// Sets the scrollbar properties.
        /// </summary>
        /// <param name="position">The position of the scrollbar in scroll units.</param>
        /// <param name="thumbSize">The size of the thumb, or visible portion of the
        /// scrollbar, in scroll units.</param>
        /// <param name="range">The maximum position of the scrollbar.</param>
        /// <param name="pageSize">The size of the page size in scroll units. This is the
        /// number of units the scrollbar will scroll when it is paged up or down.
        /// Often it is the same as the thumb size.</param>
        /// <param name="refresh"><c>true</c> to redraw the scrollbar, <c>false</c> otherwise.</param>
        /// <remarks>
        /// Let's say you wish to display 50 lines of text, using the same font.
        /// The window is sized so that you can only see 16 lines at a time. You would use:
        /// scrollbar->SetScrollbar(0, 16, 50, 15);
        /// The page size is 1 less than the thumb size so that the last line of the previous
        /// page will be visible on the next page, to help orient the user. Note that with the
        /// window at this size, the thumb position can never go above 50 minus 16, or 34.
        /// You can determine how many lines are currently visible by dividing the
        /// current view size by the character height in pixels. When defining your own
        /// scrollbar behaviour, you will always need to recalculate the scrollbar settings
        /// when the window size changes. You could therefore put your scrollbar calculations
        /// and SetScrollbar() call into a function named AdjustScrollbars, which can
        /// be called initially and also from a size event handler function.
        /// </remarks>
        internal void SetScrollbar(
            int position,
            int thumbSize,
            int range,
            int pageSize,
            bool refresh = true)
        {
            NativeControl.SetScrollbar(
                position,
                thumbSize,
                range,
                pageSize,
                refresh);
        }

        /// <summary>
        /// Raises the <see cref="ValueChanged" /> event.</summary>
        /// <param name="e">An <see cref="EventArgs" /> that contains the event data.</param>
        protected virtual void OnValueChanged(EventArgs e)
        {
            ValueChanged?.Invoke(this, e);
        }

        /// <inheritdoc/>
        protected override ControlHandler CreateHandler()
        {
            return new ScrollBarHandler();
        }

        private void UpdateScrollInfo()
        {

        }

        private void RaiseScroll()
        {
            var eventType = (ScrollEventType)NativeControl.EventTypeID;
            var pos = NativeControl.EventNewPos;
            var oldPos = NativeControl.ThumbPosition;
            var orientation = NativeControl.IsVertical ? ScrollOrientation.VerticalScroll
                : ScrollOrientation.HorizontalScroll;
            Scroll?.Invoke(this, new ScrollEventArgs(eventType, oldPos, pos, orientation));
        }

        internal class ScrollBarHandler : NativeControlHandler<ScrollBar, Native.ScrollBar>
        {
            internal override Native.Control CreateNativeControl()
            {
                var result = new Native.ScrollBar();
                return result;
            }

            protected override void OnDetach()
            {
                base.OnDetach();
                NativeControl.Scroll -= NativeControl_Scroll;
            }

            protected override void OnAttach()
            {
                base.OnAttach();
                Control.UpdateScrollInfo();
                NativeControl.Scroll += NativeControl_Scroll;
            }

            private void NativeControl_Scroll(object? sender, EventArgs e)
            {
                Control.RaiseScroll();
            }
        }
    }
}
