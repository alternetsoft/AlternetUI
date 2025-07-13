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

            private Record record = new();

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
                SetImmutable(immutable);
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
                    return record.Minimum;
                }

                set
                {
                    if (record.Minimum == value || Immutable)
                        return;

                    if (record.Maximum < value)
                        record.Maximum = value;
                    if (value > record.Value)
                        Value = value;
                    record.Minimum = value;
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
                    return record.Maximum;
                }

                set
                {
                    if (record.Maximum == value || Immutable)
                        return;

                    if (record.Minimum > value)
                        record.Minimum = value;
                    if (value < record.Value)
                        Value = value;
                    record.Maximum = value;
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
                    return Math.Min(record.SmallChange, LargeChange);
                }

                set
                {
                    if (value < 1)
                        value = 1;

                    if (record.SmallChange == value || Immutable)
                        return;

                    record.SmallChange = value;
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
                    return record.Value;
                }

                set
                {
                    if (value < record.Minimum)
                        value = record.Minimum;
                    if (value > record.Maximum)
                        value = record.Maximum;

                    if (record.Value == value || Immutable)
                        return;

                    record.Value = value;

                    // Here should be null, it is used in ScrollBar.OnPositionPropertyChanged
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
                    return Math.Min(record.LargeChange, record.Maximum - record.Minimum + 1);
                }

                set
                {
                    if (value < 1)
                        value = 1;

                    if (record.LargeChange == value || Immutable)
                        return;

                    record.LargeChange = value;
                    RaisePropertyChanged(nameof(LargeChange));
                }
            }

            /// <summary>
            /// Assigns properties from the specified <see cref="ScrollBarInfo"/> value.
            /// </summary>
            /// <param name="value"><see cref="ScrollBarInfo"/> value to assign
            /// properties from.</param>
            public virtual void Assign(ScrollBarInfo value)
            {
                var newMaximum = (value.Range / SmallChange) + Minimum;
                var newValue = (value.Position / SmallChange) + Minimum;
                var newLargeChange = value.PageSize / SmallChange;
                newValue = MathUtils.ApplyMinMax(newValue, Minimum, Maximum);

                var oldRecord = record;

                SuspendPropertyChanged();

                try
                {
                    Maximum = newMaximum;
                    Value = newValue;
                    LargeChange = newLargeChange;
                }
                finally
                {
                    ResumePropertyChanged(oldRecord != record);
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

            /// <summary>
            /// Defines all fields of the container class.
            /// </summary>
            public struct Record : IEquatable<Record>
            {
                /// <inheritdoc cref="AltPositionInfo.Minimum"/>
                public int Minimum;

                /// <inheritdoc cref="AltPositionInfo.Maximum"/>
                public int Maximum = 100;

                /// <inheritdoc cref="AltPositionInfo.SmallChange"/>
                public int SmallChange = 1;

                /// <inheritdoc cref="AltPositionInfo.LargeChange"/>
                public int LargeChange = 10;

                /// <inheritdoc cref="AltPositionInfo.Value"/>
                public int Value;

                /// <summary>
                /// Initializes a new instance of the <see cref="Record"/> struct.
                /// </summary>
                public Record()
                {
                }

                /// <summary>
                /// Tests whether two <see cref='Record'/> structs are not equal.
                /// </summary>
                public static bool operator !=(Record left, Record right)
                {
                    return !(left == right);
                }

                /// <summary>
                /// Tests whether two <see cref='Record'/> structs are equal.
                /// </summary>
                public static bool operator ==(Record left, Record right)
                {
                    return
                        left.Minimum == right.Minimum &&
                        left.Maximum == right.Maximum &&
                        left.SmallChange == right.SmallChange &&
                        left.LargeChange == right.LargeChange &&
                        left.Value == right.Value;
                }

                /// <inheritdoc/>
                public readonly override bool Equals(object? obj)
                {
                    return obj is Record record && Equals(record);
                }

                /// <inheritdoc/>
                public readonly bool Equals(Record other)
                {
                    return this == other;
                }

                /// <inheritdoc/>
                public readonly override int GetHashCode()
                {
                    return (Minimum, Maximum, SmallChange, LargeChange, Value).GetHashCode();
                }
            }
        }
    }
}