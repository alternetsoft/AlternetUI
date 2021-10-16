using System;

namespace Alternet.UI
{
	/// <summary>Defines column-specific properties that apply to <see cref="T:System.Windows.Controls.Grid" /> elements. </summary>
	public class ColumnDefinition : DefinitionBase
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.ColumnDefinition" /> class.</summary>
		public ColumnDefinition() : base(true)
		{
		}

		/// <summary>Gets the calculated width of a <see cref="T:System.Windows.Controls.ColumnDefinition" /> element, or sets the <see cref="T:System.Windows.GridLength" /> value of a column that is defined by the <see cref="T:System.Windows.Controls.ColumnDefinition" />.   </summary>
		/// <returns>The <see cref="T:System.Windows.GridLength" /> that represents the width of the Column. The default value is 1.0.</returns>
		public GridLength Width
		{
			get
			{
				return base.UserSizeValueCache;
			}
			set
			{
				base.SetValue(ColumnDefinition.WidthProperty, value);
			}
		}

		/// <summary>Gets or sets a value that represents the minimum width of a <see cref="T:System.Windows.Controls.ColumnDefinition" />.   </summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the minimum width. The default value is 0.</returns>
		[TypeConverter(typeof(LengthConverter))]
		public double MinWidth
		{
			get
			{
				return base.UserMinSizeValueCache;
			}
			set
			{
				base.SetValue(ColumnDefinition.MinWidthProperty, value);
			}
		}

		/// <summary>Gets or sets a value that represents the maximum width of a <see cref="T:System.Windows.Controls.ColumnDefinition" />.   </summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the maximum width. The default value is <see cref="F:System.Double.PositiveInfinity" />.</returns>
		[TypeConverter(typeof(LengthConverter))]
		public double MaxWidth
		{
			get
			{
				return base.UserMaxSizeValueCache;
			}
			set
			{
				base.SetValue(ColumnDefinition.MaxWidthProperty, value);
			}
		}

		/// <summary>Gets a value that represents the actual calculated width of a <see cref="T:System.Windows.Controls.ColumnDefinition" />. </summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the actual calculated width in device independent pixels. The default value is 0.0.</returns>
		public double ActualWidth
		{
			get
			{
				double result = 0.0;
				if (base.InParentLogicalTree)
				{
					result = ((Grid)base.Parent).GetFinalColumnDefinitionWidth(base.Index);
				}
				return result;
			}
		}

		/// <summary>Gets a value that represents the offset value of this <see cref="T:System.Windows.Controls.ColumnDefinition" />. </summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the offset of the column. The default value is 0.0.</returns>
		public double Offset
		{
			get
			{
				double result = 0.0;
				if (base.Index != 0)
				{
					result = base.FinalOffset;
				}
				return result;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ColumnDefinition.Width" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ColumnDefinition.Width" /> dependency property.</returns>
		[CommonDependencyProperty]
		public static readonly DependencyProperty WidthProperty = DependencyProperty.Register("Width", typeof(GridLength), typeof(ColumnDefinition), new FrameworkPropertyMetadata(new GridLength(1.0, GridUnitType.Star), new PropertyChangedCallback(DefinitionBase.OnUserSizePropertyChanged)), new ValidateValueCallback(DefinitionBase.IsUserSizePropertyValueValid));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ColumnDefinition.MinWidth" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ColumnDefinition.MinWidth" /> dependency property.</returns>
		[CommonDependencyProperty]
		[TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		public static readonly DependencyProperty MinWidthProperty = DependencyProperty.Register("MinWidth", typeof(double), typeof(ColumnDefinition), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(DefinitionBase.OnUserMinSizePropertyChanged)), new ValidateValueCallback(DefinitionBase.IsUserMinSizePropertyValueValid));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ColumnDefinition.MaxWidth" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ColumnDefinition.MaxWidth" /> dependency property.</returns>
		[CommonDependencyProperty]
		[TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		public static readonly DependencyProperty MaxWidthProperty = DependencyProperty.Register("MaxWidth", typeof(double), typeof(ColumnDefinition), new FrameworkPropertyMetadata(double.PositiveInfinity, new PropertyChangedCallback(DefinitionBase.OnUserMaxSizePropertyChanged)), new ValidateValueCallback(DefinitionBase.IsUserMaxSizePropertyValueValid));
	}
}