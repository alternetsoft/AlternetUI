#nullable disable

using System;
using System.ComponentModel;

namespace Alternet.UI
{
	/// <summary>Defines column-specific properties that apply to <see cref="Grid" /> controls. </summary>
	public class ColumnDefinition : DefinitionBase
	{
		/// <summary>Initializes a new instance of the <see cref="ColumnDefinition" /> class.</summary>
		public ColumnDefinition() : base(true)
		{
		}

		GridLength width = new GridLength(1, GridUnitType.Star);
		double minWidth;
		double maxWidth = double.PositiveInfinity;

		/// <summary>Gets the calculated width of a <see cref="ColumnDefinition" />, or sets the <see cref="GridLength" /> value of a column that is defined by the <see cref="ColumnDefinition" />.   </summary>
		/// <returns>The <see cref="GridLength" /> that represents the width of the Column. The default value is 1.0.</returns>
		public GridLength Width
		{
			get
			{
				return width;
			}
			set
			{
				if (!IsUserSizePropertyValueValid(value))
					throw new ArgumentException();

				var oldValue = width;
				width = value;
				OnUserSizePropertyChanged(oldValue, value);
			}
		}

		/// <summary>Gets or sets a value that represents the minimum width of a <see cref="ColumnDefinition" />.   </summary>
		/// <returns>A <see cref="double" /> that represents the minimum width. The default value is 0.</returns>
		[TypeConverter(typeof(LengthConverter))]
		public double MinWidth
		{
			get
			{
				return minWidth;
			}
			set
			{
				if (!IsUserMinSizePropertyValueValid(value))
					throw new ArgumentException();

				minWidth = value;
				OnUserMinSizePropertyChanged(value);
			}
		}

		/// <summary>Gets or sets a value that represents the maximum width of a <see cref="ColumnDefinition" />.   </summary>
		/// <returns>A <see cref="double" /> that represents the maximum width. The default value is <see cref="F:System.Double.PositiveInfinity" />.</returns>
		[TypeConverter(typeof(LengthConverter))]
		public double MaxWidth
		{
			get
			{
				return maxWidth;
			}

			set
			{
				if (!IsUserMaxSizePropertyValueValid(value))
					throw new ArgumentException();

				maxWidth = value;
				OnUserMaxSizePropertyChanged(value);
			}
		}

		/// <summary>Gets a value that represents the actual calculated width of a <see cref="ColumnDefinition" />. </summary>
		/// <returns>A <see cref="double" /> that represents the actual calculated width in device independent pixels. The default value is 0.</returns>
		public double ActualWidth
		{
			get
			{
				double result = 0;
				if (base.InParentLogicalTree)
				{
					result = ((Grid)base.LogicalParent).GetFinalColumnDefinitionWidth(base.Index);
				}
				return result;
			}
		}

		/// <summary>Gets a value that represents the offset value of this <see cref="ColumnDefinition" />. </summary>
		/// <returns>A <see cref="double" /> that represents the offset of the column. The default value is 0.</returns>
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