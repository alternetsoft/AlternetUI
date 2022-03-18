#nullable disable

using System;
using System.ComponentModel;

namespace Alternet.UI
{
    /// <summary>Defines row-specific properties that apply to <see cref="T:System.Windows.Controls.Grid" /> elements.</summary>
    public class RowDefinition : DefinitionBase
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.RowDefinition" /> class.</summary>
        public RowDefinition() : base(false)
        {
        }

        /// <summary>Gets the calculated height of a <see cref="T:System.Windows.Controls.RowDefinition" /> element, or sets the <see cref="T:System.Windows.GridLength" /> value of a row that is defined by the <see cref="T:System.Windows.Controls.RowDefinition" />.   </summary>
        /// <returns>The <see cref="T:System.Windows.GridLength" /> that represents the height of the row. The default value is 1.0.</returns>
        public GridLength Height
        {
            get
            {
                return base.UserSizeValueCache;
            }
            set
            {
                base.SetValue(RowDefinition.HeightProperty, value);
            }
        }

        /// <summary>Gets or sets a value that represents the minimum allowable height of a <see cref="T:System.Windows.Controls.RowDefinition" />.  </summary>
        /// <returns>A <see cref="T:System.Double" /> that represents the minimum allowable height. The default value is 0.</returns>
        [TypeConverter(typeof(LengthConverter))]
        public double MinHeight
        {
            get
            {
                return base.UserMinSizeValueCache;
            }
            set
            {
                base.SetValue(RowDefinition.MinHeightProperty, value);
            }
        }

        /// <summary>Gets or sets a value that represents the maximum height of a <see cref="T:System.Windows.Controls.RowDefinition" />.  </summary>
        /// <returns>A <see cref="T:System.Double" /> that represents the maximum height. </returns>
        [TypeConverter(typeof(LengthConverter))]
        public double MaxHeight
        {
            get
            {
                return base.UserMaxSizeValueCache;
            }
            set
            {
                base.SetValue(RowDefinition.MaxHeightProperty, value);
            }
        }

        /// <summary>Gets a value that represents the calculated height of the <see cref="T:System.Windows.Controls.RowDefinition" />.</summary>
        /// <returns>A <see cref="T:System.Double" /> that represents the calculated height in device independent pixels. The default value is 0.0.</returns>
        public double ActualHeight
        {
            get
            {
                double result = 0.0;
                if (base.InParentLogicalTree)
                {
                    result = ((Grid)base.Parent).GetFinalRowDefinitionHeight(base.Index);
                }
                return result;
            }
        }

        /// <summary>Gets a value that represents the offset value of this <see cref="T:System.Windows.Controls.RowDefinition" />.</summary>
        /// <returns>A <see cref="T:System.Double" /> that represents the offset of the row. The default value is 0.0.</returns>
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

        /// <summary>Identifies the <see cref="P:System.Windows.Controls.RowDefinition.Height" /> dependency property. </summary>
        /// <returns>The identifier for the <see cref="P:System.Windows.Controls.RowDefinition.Height" /> dependency property.</returns>
        [CommonDependencyProperty]
        public static readonly DependencyProperty HeightProperty = DependencyProperty.Register("Height", typeof(GridLength), typeof(RowDefinition), new FrameworkPropertyMetadata(new GridLength(1.0, GridUnitType.Star), new PropertyChangedCallback(DefinitionBase.OnUserSizePropertyChanged)), new ValidateValueCallback(DefinitionBase.IsUserSizePropertyValueValid));

        /// <summary>Identifies the <see cref="P:System.Windows.Controls.RowDefinition.MinHeight" /> dependency property. </summary>
        /// <returns>The identifier for the <see cref="P:System.Windows.Controls.RowDefinition.MinHeight" /> dependency property.</returns>
        [CommonDependencyProperty]
        [TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
        public static readonly DependencyProperty MinHeightProperty = DependencyProperty.Register("MinHeight", typeof(double), typeof(RowDefinition), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(DefinitionBase.OnUserMinSizePropertyChanged)), new ValidateValueCallback(DefinitionBase.IsUserMinSizePropertyValueValid));

        /// <summary>Identifies the <see cref="P:System.Windows.Controls.RowDefinition.MaxHeight" /> dependency property. </summary>
        /// <returns>The identifier for the <see cref="P:System.Windows.Controls.RowDefinition.MaxHeight" /> dependency property.</returns>
        [CommonDependencyProperty]
        [TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
        public static readonly DependencyProperty MaxHeightProperty = DependencyProperty.Register("MaxHeight", typeof(double), typeof(RowDefinition), new FrameworkPropertyMetadata(double.PositiveInfinity, new PropertyChangedCallback(DefinitionBase.OnUserMaxSizePropertyChanged)), new ValidateValueCallback(DefinitionBase.IsUserMaxSizePropertyValueValid));
    }
}