using System.Collections.Generic;
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
                return sizeInPoints;
            }

            set
            {
                sizeInPoints = value;
                Save();
            }
        }

        /// <inheritdoc cref="Font.Name"/>
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
                Save();
            }
        }

        /// <summary>
        /// Gets or sets font name as index in <see cref="FontNameChoices"/>.
        /// </summary>
        public int? NameAsIndex
        {
            get
            {
                return FontNameChoices.GetValueFromLabel(Name);
            }

            set
            {
                if (value == null)
                    Name = Font.Default.Name;
                else
                {
                    var newName = FontNameChoices.GetLabelFromValue((int)value);
                    if (newName == null)
                        Name = Font.Default.Name;
                    else
                        Name = newName;
                }
            }
        }

        /// <inheritdoc cref="Font.IsBold"/>
        public bool Bold
        {
            get => (Style & FontStyle.Bold) != 0;
            set
            {
                Style = Font.ChangeFontStyle(Style, FontStyle.Bold, value);
            }
        }

        /// <inheritdoc cref="Font.IsItalic"/>
        public bool Italic
        {
            get => (Style & FontStyle.Italic) != 0;
            set
            {
                Style = Font.ChangeFontStyle(Style, FontStyle.Italic, value);
            }
        }

        /// <inheritdoc cref="Font.IsStrikethrough"/>
        public bool Strikethrough
        {
            get => (Style & FontStyle.Strikeout) != 0;
            set
            {
                Style = Font.ChangeFontStyle(Style, FontStyle.Strikeout, value);
            }
        }

        /// <inheritdoc cref="Font.IsUnderlined"/>
        public bool Underlined
        {
            get => (Style & FontStyle.Underline) != 0;
            set
            {
                Style = Font.ChangeFontStyle(Style, FontStyle.Underline, value);
            }
        }

        /// <inheritdoc cref="Font.Style"/>
        public FontStyle Style
        {
            get
            {
                return style;
            }

            set
            {
                style = value;
                Save();
            }
        }

        /// <inheritdoc/>
        public override IEnumerable<IPropertyGridItem> CreateProps(IPropertyGrid propGrid)
        {
            var choices = PropertyGridAdapterFont.FontNameChoices;

            var item = propGrid.CreateChoicesItem(
                nameof(Name),
                null,
                choices,
                NameAsIndex,
                null);
            item.Instance = this;
            item.PropInfo = AssemblyUtils.GetPropInfo(this, nameof(NameAsIndex));

            IPropertyGridItem[] list =
            {
                item,
                propGrid.CreateProperty(this, nameof(SizeInPoints))!,
                propGrid.CreateProperty(this, nameof(Bold))!,
                propGrid.CreateProperty(this, nameof(Italic))!,
                propGrid.CreateProperty(this, nameof(Strikethrough))!,
                propGrid.CreateProperty(this, nameof(Underlined))!,
            };

            return list;
        }

        /// <inheritdoc/>
        protected override void Save()
        {
            Font = new(name, sizeInPoints, style);
        }

        /// <inheritdoc/>
        protected override void Load()
        {
            if (Font == null)
                return;
            sizeInPoints = Font.SizeInPoints;
            name = Font.Name;
            style = Font.Style;
        }
    }
}