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
    public class PlessFontHandler : DisposableObject, IFontHandler
    {
        private string name = string.Empty;
        private FontStyle style = FontStyle.Regular;
        private Coord sizeInPoints = 12;
        private FontWeight weight = FontWeight.Normal;
        private FontEncoding encoding = FontEncoding.Default;
        private string? serialized;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlessFontHandler"/> class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sizeInPoints"></param>
        public PlessFontHandler(string name, Coord sizeInPoints)
        {
            this.Name = name;
            this.SizeInPoints = sizeInPoints;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlessFontHandler"/> class.
        /// </summary>
        public PlessFontHandler()
        {
        }

        /// <inheritdoc/>
        public virtual string Description
        {
            get
            {
                return Serialize();
            }
        }

        /// <inheritdoc/>
        public virtual string Name
        {
            get
            {
                return name;
            }

            set
            {
                if (name == value)
                    return;
                name = value;
                Changed();
            }
        }

        /// <summary>
        /// Gets or sets font style.
        /// </summary>
        public virtual FontStyle Style
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
        public virtual Coord SizeInPoints
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
        public virtual FontEncoding GetEncoding(Font font)
        {
            return encoding;
        }

        /// <summary>
        /// Sets font encoding.
        /// </summary>
        /// <param name="value"></param>
        public virtual void SetEncoding(FontEncoding value)
        {
            encoding = value;
            Changed();
        }

        /// <inheritdoc/>
        public virtual int GetNumericWeight(Font font)
        {
            return Font.GetNumericWeightOf(weight);
        }

        /// <inheritdoc/>
        public virtual int GetPixelSize(Font font)
        {
            var result = GraphicsUnitConverter.Convert(
                GraphicsUnit.Point,
                GraphicsUnit.Pixel,
                Display.Primary.DPI.Height,
                sizeInPoints);
            return (int)result;
        }

        /// <inheritdoc/>
        public virtual bool GetItalic()
        {
            return style.HasFlag(FontStyle.Italic);
        }

        /// <inheritdoc/>
        public virtual bool GetStrikethrough()
        {
            return style.HasFlag(FontStyle.Strikeout);
        }

        /// <inheritdoc/>
        public virtual bool GetUnderlined()
        {
            return style.HasFlag(FontStyle.Underline);
        }

        /// <inheritdoc/>
        public virtual FontWeight GetWeight()
        {
            return weight;
        }

        /// <summary>
        /// Sets font weight as numeric value.
        /// </summary>
        /// <param name="value"></param>
        public virtual void SetNumericWeight(int value)
        {
            var newWeight = Font.GetWeightClosestToNumericValue(value);
            SetWeight(newWeight);
        }

        /// <summary>
        /// Sets font weight.
        /// </summary>
        /// <param name="value"></param>
        public virtual void SetWeight(FontWeight value)
        {
            if (weight == value)
                return;
            weight = value;
            style = Font.ChangeFontStyle(style, FontStyle.Bold, Font.GetIsBold(weight));
            Changed();
        }

        /// <inheritdoc/>
        public virtual bool IsFixedWidth(Font font)
        {
            return false;
        }

        /// <inheritdoc/>
        public virtual bool IsUsingSizeInPixels(Font font)
        {
            return false;
        }

        /// <inheritdoc/>
        public virtual bool Equals(Font font)
        {
            var thisSerialized = Serialize();
            var otherSerialized = font.Handler.Serialize();
            return thisSerialized == otherSerialized;
        }

        /// <inheritdoc/>
        public virtual string Serialize()
        {
            return serialized ??= Font.ToUserString(this);
        }

        /// <summary>
        /// Called when font properties are changed.
        /// </summary>
        public virtual void Changed()
        {
            serialized = null;
        }

        /// <inheritdoc/>
        public virtual void Update(Font font, IFontHandler.FontParams prm)
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
    }
}
