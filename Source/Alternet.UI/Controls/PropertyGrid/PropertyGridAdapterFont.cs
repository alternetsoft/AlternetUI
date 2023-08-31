using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Helper class for using <see cref="Font"/> properties in the <see cref="PropertyGrid"/>.
    /// </summary>
    public class PropertyGridAdapterFont : PropertyGridAdapterGeneric
    {
        private static IPropertyGridChoices? fontNameChoices;
        private double sizeInPoints = Font.Default.SizeInPoints;
        private string name = Font.Default.Name;
        private FontStyle style = FontStyle.Regular;

        /// <summary>
        /// Gets all installed <see cref="Font"/> names as <see cref="IPropertyGridChoices"/>
        /// </summary>
        public static IPropertyGridChoices FontNameChoices
        {
            get
            {
                if (fontNameChoices != null)
                    return fontNameChoices;
                fontNameChoices = PropertyGrid.CreateChoices();
                string[] names = FontFamily.FamiliesNamesAscending;
                fontNameChoices.AddRange(names);
                return fontNameChoices;
            }
        }

        /// <summary>
        /// Returns <see cref="PropertyGridAdapterGeneric.Value"/> as <see cref="Font"/>.
        /// </summary>
        public Font? Font
        {
            get => Value as Font;
            set => Value = value;
        }

        /// <inheritdoc cref="Font.SizeInPoints"/>
        public double SizeInPoints
        {
            get
            {
                if (Font == null)
                    return sizeInPoints;
                return Font.SizeInPoints;
            }

            set
            {
                if (SizeInPoints == value)
                    return;
                sizeInPoints = value;
                OnInstancePropertyChanged();
            }
        }

        /// <inheritdoc cref="Font.Name"/>
        public string Name
        {
            get
            {
                if (Font == null)
                    return name;
                return Font.Name;
            }

            set
            {
                if (Name == value)
                    return;
                name = value;
                OnInstancePropertyChanged();
            }
        }

        /// <inheritdoc cref="Font.IsBold"/>
        public bool IsBold
        {
            get => (Style & FontStyle.Bold) != 0;
            set
            {
                Style = Font.ChangeFontStyle(Style, FontStyle.Bold, value);
            }
        }

        /// <inheritdoc cref="Font.IsItalic"/>
        public bool IsItalic
        {
            get => (Style & FontStyle.Italic) != 0;
            set
            {
                Style = Font.ChangeFontStyle(Style, FontStyle.Italic, value);
            }
        }

        /// <inheritdoc cref="Font.IsStrikethrough"/>
        public bool IsStrikethrough
        {
            get => (Style & FontStyle.Strikethrough) != 0;
            set
            {
                Style = Font.ChangeFontStyle(Style, FontStyle.Strikethrough, value);
            }
        }

        /// <inheritdoc cref="Font.IsUnderlined"/>
        public bool IsUnderlined
        {
            get => (Style & FontStyle.Underlined) != 0;
            set
            {
                Style = Font.ChangeFontStyle(Style, FontStyle.Underlined, value);
            }
        }

        /// <inheritdoc cref="Font.Style"/>
        public FontStyle Style
        {
            get
            {
                if (Font == null)
                    return style;
                return Font.Style;
            }

            set
            {
                if (Style == value)
                    return;
                style = value;
                OnInstancePropertyChanged();
            }
        }

        private void OnInstancePropertyChanged()
        {
            Font = new(Name, SizeInPoints, Style);
        }
    }
}