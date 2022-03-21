#nullable disable

using System;
using System.ComponentModel;

namespace Alternet.UI
{
    /// <summary>
    /// Defines row-specific properties that apply to <see cref="Grid"/> controls.
    /// </summary>
    public class RowDefinition : DefinitionBase
	{
		/// <summary>Initializes a new instance of the <see cref="RowDefinition" /> class.</summary>
		public RowDefinition() : base(false)
		{
		}

		GridLength height = new GridLength(1, GridUnitType.Star);
		double minHeight;
		double maxHeight = double.PositiveInfinity;


		/// <summary>Gets the calculated height of a <see cref="RowDefinition" />, or sets the <see cref="GridLength" /> value of a row that is defined by the <see cref="RowDefinition" />.   </summary>
		/// <returns>The <see cref="GridLength" /> that represents the height of the row. The default value is 1.0.</returns>
		public GridLength Height
		{
			get
			{
				return height;
			}
			set
			{
				if (!IsUserSizePropertyValueValid(value))
					throw new ArgumentException();

				var oldValue = height;
				height = value;
				OnUserSizePropertyChanged(oldValue, value);
			}
		}

		/// <summary>Gets or sets a value that represents the minimum allowable height of a <see cref="RowDefinition" />.  </summary>
		/// <returns>A <see cref="double" /> that represents the minimum allowable height. The default value is 0.</returns>
		[TypeConverter(typeof(LengthConverter))]
		public double MinHeight
		{
			get
			{
				return minHeight;
			}
			set
			{
				if (!IsUserMinSizePropertyValueValid(value))
					throw new ArgumentException();

				minHeight = value;
				OnUserMinSizePropertyChanged(value);
			}
		}

		/// <summary>Gets or sets a value that represents the maximum height of a <see cref="RowDefinition" />.  </summary>
		/// <returns>A <see cref="double" /> that represents the maximum height. </returns>
		[TypeConverter(typeof(LengthConverter))]
		public double MaxHeight
		{
			get
			{
				return maxHeight;
			}
			set
			{
				if (!IsUserMaxSizePropertyValueValid(value))
					throw new ArgumentException();

				maxHeight = value;
				OnUserMaxSizePropertyChanged(value);
			}
		}

		/// <summary>Gets a value that represents the calculated height of the <see cref="RowDefinition" />.</summary>
		/// <returns>A <see cref="double" /> that represents the calculated height in device independent pixels. The default value is 0.</returns>
		public double ActualHeight
		{
			get
			{
				double result = 0;
				if (base.InParentLogicalTree)
				{
					result = ((Grid)base.Parent).GetFinalRowDefinitionHeight(base.Index);
				}
				return result;
			}
		}

		/// <summary>Gets a value that represents the offset value of this <see cref="RowDefinition" />.</summary>
		/// <returns>A <see cref="double" /> that represents the offset of the row. The default value is 0.</returns>
		public double Offset
		{
			get
			{
				double result = 0;
				if (base.Index != 0)
				{
					result = base.FinalOffset;
				}
				return result;
			}
		}
	}
}