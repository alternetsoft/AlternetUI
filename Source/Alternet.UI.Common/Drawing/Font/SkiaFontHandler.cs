using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// <see cref="IFontHandler"/> implementation which is not platform-dependent.
    /// </summary>
    public struct SkiaFontHandler : IFontHandler
    {
        private string name = string.Empty;
        private FontStyle style = FontStyle.Regular;
        private Coord sizeInPoints = 12;
        private FontWeight weight = FontWeight.Normal;
        private FontEncoding encoding = FontEncoding.Default;
        private string? serialized;
        private bool? isFixedFont;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkiaFontHandler"/> class.
        /// </summary>
        /// <param name="name">Font name.</param>
        /// <param name="sizeInPoints">Font size in points.</param>
        public SkiaFontHandler(string name, Coord sizeInPoints)
        {
            SetName(name);
            this.SizeInPoints = sizeInPoints;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkiaFontHandler"/> class.
        /// </summary>
        public SkiaFontHandler()
        {
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
        public string GetName()
        {
            return name;
        }
        
        /// <summary>
        /// Sets font name.
        /// </summary>
        /// <param name="value"></param>
        public void SetName(string value)
        {
            if (name == value)
                return;
            name = value;
            Changed();
        }

        /// <summary>
        /// Gets or sets font style.
        /// </summary>
        public FontStyle Style
        {
            get
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
            get
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
        public FontEncoding GetEncoding(Font font)
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
        public int GetNumericWeight(Font font)
        {
            return Font.GetNumericWeightOf(weight);
        }

        /// <inheritdoc/>
        public int GetPixelSize(Font font)
        {
            var result = GraphicsUnitConverter.Convert(
                GraphicsUnit.Point,
                GraphicsUnit.Pixel,
                Display.Primary.DPI.Height,
                sizeInPoints);
            return (int)result;
        }

        /// <inheritdoc/>
        public bool GetItalic()
        {
            return style.HasFlag(FontStyle.Italic);
        }

        /// <inheritdoc/>
        public bool GetStrikethrough()
        {
            return style.HasFlag(FontStyle.Strikeout);
        }

        /// <inheritdoc/>
        public bool GetUnderlined()
        {
            return style.HasFlag(FontStyle.Underline);
        }

        /// <inheritdoc/>
        public FontWeight GetWeight()
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
        public bool IsUsingSizeInPixels(Font font)
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
        public void Update(Font font, IFontHandler.FontParams prm)
        {
            Font.CoerceFontParams(prm);
            if (prm.GenericFamily is null)
                name = prm.FamilyName ?? FontFamily.GetName(GenericFontFamily.Default);
            else
                name = FontFamily.GetName(prm.GenericFamily ?? GenericFontFamily.Default);
            style = prm.Style;
            sizeInPoints = prm.Size;
            if (style.HasFlag(FontStyle.Bold))
                weight = FontWeight.Bold;
            else
                weight = FontWeight.Normal;

            Changed();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
        }
    }
}
