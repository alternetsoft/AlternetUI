using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Maui.Controls;

namespace Alternet.Maui
{
    /// <summary>
    /// Provides a base collection view with extended functionality.
    /// </summary>
    public partial class BaseCollectionView : CollectionView
    {
        /// <summary>
        /// Identifies the FontFamily bindable property.
        /// </summary>
        /// <remarks>This field is used to reference the FontFamily property when adding bindings or
        /// setting property values in XAML or code. It is typically used by framework infrastructure and advanced
        /// scenarios.</remarks>
        public static readonly BindableProperty FontFamilyProperty =
            BindableProperty.Create(
                nameof(FontFamily),
                typeof(string),
                typeof(BaseCollectionView),
                Label.FontFamilyProperty.DefaultValue);

        /// <summary>
        /// Identifies the FontSize bindable property.
        /// </summary>
        /// <remarks>This field is used to reference the FontSize property when adding bindings or setting
        /// property values in styles. The default value is the platform-specific default font size for
        /// labels.</remarks>
        public static readonly BindableProperty FontSizeProperty =
            BindableProperty.Create(
                nameof(FontSize),
                typeof(double),
                typeof(BaseCollectionView),
                Label.FontSizeProperty.DefaultValue);

        /// <summary>
        /// Gets or sets the name of the font family to use for text rendering.
        /// </summary>
        public string? FontFamily
        {
            get => (string?)GetValue(FontFamilyProperty);
            set => SetValue(FontFamilyProperty, value);
        }

        /// <summary>
        /// Gets or sets the font size used to display text.
        /// </summary>
        public double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }
    }
}
