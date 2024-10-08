#nullable disable

using System;
using System.ComponentModel;

namespace Alternet.UI
{
    /// <summary>
    /// Defines row-specific properties that apply to <see cref="Grid"/> controls.
    /// </summary>
    public class RowDefinition : GridDefinitionBase
    {
        private GridLength height = GridLength.Star;
        private Coord minHeight;
        private Coord maxHeight = Coord.PositiveInfinity;

        /// <summary>Initializes a new instance of the <see cref="RowDefinition" /> class.</summary>
        public RowDefinition()
            : base(false)
        {
        }

        /// <summary>Gets the calculated height of a <see cref="RowDefinition" />,
        /// or sets the <see cref="GridLength" /> value of a row that is defined
        /// by the <see cref="RowDefinition" />.
        /// </summary>
        /// <returns>The <see cref="GridLength" /> that represents the height of the row.
        /// The default value is 1.0.</returns>
        public GridLength Height
        {
            get
            {
                return height;
            }

            set
            {
                if (!IsUserSizePropertyValueValid(value))
                    throw new ArgumentException(nameof(Height));

                var oldValue = height;
                height = value;
                OnUserSizePropertyChanged(oldValue, value);
            }
        }

        /// <summary>
        /// Gets or sets a value that represents the minimum allowable height
        /// of a <see cref="RowDefinition" />.
        /// </summary>
        /// <returns>A value that represents the minimum allowable
        /// height. The default value is 0.</returns>
        [TypeConverter(typeof(LengthConverter))]
        public Coord MinHeight
        {
            get
            {
                return minHeight;
            }

            set
            {
                if (!IsUserMinSizePropertyValueValid(value))
                    throw new ArgumentException(nameof(MinHeight));

                minHeight = value;
                OnUserMinSizePropertyChanged(value);
            }
        }

        /// <summary>
        /// Gets or sets a value that represents the
        /// maximum height of a <see cref="RowDefinition" />.
        /// </summary>
        /// <returns>A value that represents
        /// the maximum height. </returns>
        [TypeConverter(typeof(LengthConverter))]
        public Coord MaxHeight
        {
            get
            {
                return maxHeight;
            }

            set
            {
                if (!IsUserMaxSizePropertyValueValid(value))
                    throw new ArgumentException(nameof(MaxHeight));

                maxHeight = value;
                OnUserMaxSizePropertyChanged(value);
            }
        }

        /// <summary>Gets a value that represents the calculated height
        /// of the <see cref="RowDefinition" />.</summary>
        /// <returns>A value that represents the
        /// calculated height in device independent pixels. The default value is 0.</returns>
        public Coord ActualHeight
        {
            get
            {
                Coord result = 0;
                if (InParentLogicalTree)
                {
                    result = ((Grid)LogicalParent).GetFinalRowDefinitionHeight(Index);
                }

                return result;
            }
        }

        /// <summary>Gets a value that represents the offset value of this
        /// <see cref="RowDefinition" />.</summary>
        /// <returns>A value that represents the offset of the row.
        /// The default value is 0.</returns>
        public Coord Offset
        {
            get
            {
                Coord result = 0;
                if (Index != 0)
                {
                    result = FinalOffset;
                }

                return result;
            }
        }

        /// <summary>
        /// Creates <see cref="RowDefinition"/> instance with <see cref="Height"/> equal to
        /// <see cref="GridLength.Auto"/>.
        /// </summary>
        public static RowDefinition CreateAuto()
        {
            return new()
            {
                Height = GridLength.Auto,
            };
        }
    }
}