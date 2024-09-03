using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    public partial class ScrollBar
    {
        /// <summary>
        /// Contains information about the scrollbar position and range in the alternative format.
        /// </summary>
        public class AltPositionInfo : ImmutableObject
        {
            private static AltPositionInfo? def;

            private int minimum;
            private int maximum = 100;
            private int smallChange = 1;
            private int largeChange = 10;
            private int val;

            /// <summary>
            /// Initializes a new instance of the <see cref="AltPositionInfo"/> struct.
            /// </summary>
            public AltPositionInfo()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="AltPositionInfo"/> class.
            /// </summary>
            /// <param name="immutable">Whether this object is immutable.</param>
            public AltPositionInfo(bool immutable)
            {
                Immutable = immutable;
            }

            /// <summary>
            /// Gets default <see cref="AltPositionInfo"/> object.
            /// Returned value is immutable and its properties are read only.
            /// </summary>
            public static AltPositionInfo Default
            {
                get
                {
                    return def ??= new(true);
                }
            }

            /// <summary>
            /// Gets or sets the lower limit of values of the scrollable range.
            /// </summary>
            /// <returns>
            /// A numeric value. The default value is 0.
            /// </returns>
            public virtual int Minimum
            {
                get
                {
                    return minimum;
                }

                set
                {
                    if (minimum == value || Immutable)
                        return;

                    if (maximum < value)
                        maximum = value;
                    if (value > this.val)
                        Value = value;
                    minimum = value;
                    RaisePropertyChanged(nameof(Minimum));
                }
            }

            /// <summary>
            /// Gets or sets the upper limit of values of the scrollable range.
            /// </summary>
            /// <returns>
            /// A numeric value. The default value is 100.
            /// </returns>
            public virtual int Maximum
            {
                get
                {
                    return maximum;
                }

                set
                {
                    if (maximum == value || Immutable)
                        return;

                    if (minimum > value)
                        minimum = value;
                    if (value < this.val)
                        Value = value;
                    maximum = value;
                    RaisePropertyChanged(nameof(Maximum));
                }
            }

            /// <summary>
            /// Gets or sets the value to be added to or subtracted from
            /// the <see cref="Value" /> property when the scroll thumb is moved
            /// a small distance.
            /// </summary>
            /// <returns>A numeric value. The default value is 1.</returns>
            public virtual int SmallChange
            {
                get
                {
                    return Math.Min(smallChange, LargeChange);
                }

                set
                {
                    if (smallChange == value || Immutable)
                        return;

                    if (value < 0)
                    {
                        LogUtils.LogInvalidBoundArgumentUInt(nameof(SmallChange), value);
                        return;
                    }

                    smallChange = value;
                    RaisePropertyChanged(nameof(SmallChange));
                }
            }

            /// <summary>
            /// Gets or sets a numeric value that represents the current position of the
            /// scroll thumb on the scroll bar control.
            /// </summary>
            /// <returns>A numeric value that is within the <see cref="Minimum" /> and
            /// <see cref="Maximum" /> range. The default value is 0.</returns>
            public virtual int Value
            {
                get
                {
                    return val;
                }

                set
                {
                    if (val == value || Immutable)
                        return;

                    if (value < minimum || value > maximum)
                    {
                        LogUtils.LogInvalidBoundArgument(
                            nameof(Value),
                            value,
                            Minimum,
                            Maximum);
                        return;
                    }

                    this.val = value;

                    RaisePropertyChanged(null);
                }
            }

            /// <summary>
            /// Gets or sets a value to be added to or subtracted from the
            /// <see cref="Value" /> property when the scroll box is moved a large distance.
            /// </summary>
            /// <returns>A numeric value. The default value is 10.</returns>
            public virtual int LargeChange
            {
                get
                {
                    return Math.Min(largeChange, maximum - minimum + 1);
                }

                set
                {
                    if (largeChange == value || Immutable)
                        return;

                    if (value < 0)
                    {
                        LogUtils.LogInvalidBoundArgumentUInt(nameof(LargeChange), value);
                        return;
                    }

                    largeChange = value;
                    RaisePropertyChanged(nameof(LargeChange));
                }
            }

            /// <summary>
            /// Converts this object to <see cref="ScrollBarInfo"/>.
            /// </summary>
            /// <returns></returns>
            public virtual ScrollBarInfo AsPositionInfo()
            {
                var range = (Maximum - Minimum) * SmallChange;
                var pageSize = LargeChange * SmallChange;
                var position = (Value - Minimum) * SmallChange;
                var result = new ScrollBarInfo(position, range, pageSize);
                return result;
            }
        }
    }
}