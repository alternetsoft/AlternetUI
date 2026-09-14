using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Defines a font handler for SkiaSharp fonts.
    /// </summary>
    public struct SkiaFontHandler
    {
        private FontStyle style = FontStyle.Regular;
        private Coord sizeInPoints = 12;
        private FontWeight weight = FontWeight.Normal;
        private FontEncoding encoding = FontEncoding.Default;
        private string? serialized;
        private bool? isFixedFont;
        private FontFamily fontFamily;

        /// <summary>
        /// Gets or sets font family.
        /// </summary>
        /// <param name="family">Font family.</param>
        /// <param name="sizeInPoints">Font size in points.</param>
        public SkiaFontHandler(FontFamily family, Coord sizeInPoints)
        {
            this.fontFamily = family;
            this.SizeInPoints = sizeInPoints;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkiaFontHandler"/> class.
        /// </summary>
        public SkiaFontHandler()
        {
            this.fontFamily = Font.Default.FontFamily;
            this.SizeInPoints = Font.Default.SizeInPoints;
        }

        /// <inheritdoc/>
        public bool IsFixedWidth(Font font)
        {
            return isFixedFont ??= font.SkiaFont.Typeface.IsFixedPitch;
        }

        /// <summary>
        /// Called when font properties are changed.
        /// </summary>
        public void Changed()
        {
            serialized = null;
            isFixedFont = null;
        }

        /// <inheritdoc/>
        public string GetDescription()
        {
            return Serialize();
        }

        /// <inheritdoc/>
        public readonly string GetName()
        {
            return fontFamily.Name;
        }
        
        /// <summary>
        /// Gets or sets font style.
        /// </summary>
        public FontStyle Style
        {
            readonly get
            {
                return style;
            }

            set
            {
                if (style == value)
                    return;
                style = value;
                Changed();
            }
        }

        /// <inheritdoc/>
        public Coord SizeInPoints
        {
            readonly get
            {
                return sizeInPoints;
            }

            set
            {
                if (sizeInPoints == value)
                    return;
                sizeInPoints = value;
                Changed();
            }
        }

        /// <inheritdoc/>
        public readonly FontEncoding GetEncoding(Font font)
        {
            return encoding;
        }

        /// <summary>
        /// Sets font encoding.
        /// </summary>
        /// <param name="value"></param>
        public void SetEncoding(FontEncoding value)
        {
            encoding = value;
            Changed();
        }

        /// <inheritdoc/>
        public readonly int GetNumericWeight(Font font)
        {
            return Font.GetNumericWeightOf(weight);
        }

        /// <inheritdoc/>
        public readonly int GetPixelSize(Font font)
        {
            var result = GraphicsUnitConverter.Convert(
                GraphicsUnit.Point,
                GraphicsUnit.Pixel,
                Display.Primary.DPI.Height,
                sizeInPoints);
            return (int)result;
        }

        /// <inheritdoc/>
        public readonly bool GetItalic()
        {
            return style.HasFlag(FontStyle.Italic);
        }

        /// <inheritdoc/>
        public readonly bool GetStrikethrough()
        {
            return style.HasFlag(FontStyle.Strikeout);
        }

        /// <inheritdoc/>
        public readonly bool GetUnderlined()
        {
            return style.HasFlag(FontStyle.Underline);
        }

        /// <inheritdoc/>
        public readonly FontWeight GetWeight()
        {
            return weight;
        }

        /// <summary>
        /// Sets font weight as numeric value.
        /// </summary>
        /// <param name="value"></param>
        public void SetNumericWeight(int value)
        {
            var newWeight = Font.GetWeightClosestToNumericValue(value);
            SetWeight(newWeight);
        }

        /// <summary>
        /// Sets font weight.
        /// </summary>
        /// <param name="value"></param>
        public void SetWeight(FontWeight value)
        {
            if (weight == value)
                return;
            weight = value;
            style = Font.ChangeFontStyle(style, FontStyle.Bold, Font.GetIsBold(weight));
            Changed();
        }

        /// <inheritdoc/>
        public readonly bool IsUsingSizeInPixels(Font font)
        {
            return false;
        }

        /// <inheritdoc/>
        public bool Equals(Font font)
        {
            var thisSerialized = Serialize();
            var otherSerialized = font.Serialize();
            return thisSerialized == otherSerialized;
        }

        /// <inheritdoc/>
        public string Serialize()
        {
            return serialized ??= Font.ToUserString(this);
        }

        /// <inheritdoc/>
        public void Update(Font font, Font.FontParams prm)
        {
            Font.CoerceFontParams(prm);
            fontFamily = prm.Family ?? Font.Default.FontFamily;
            style = prm.Style;
            sizeInPoints = prm.Size;
            if (style.HasFlag(FontStyle.Bold))
                weight = FontWeight.Bold;
            else
                weight = FontWeight.Normal;

            Changed();
        }

        /// <inheritdoc/>
        public readonly void Dispose()
        {
        }
    }
}
