using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Platformless <see cref="IFontHandler"/> implementation.
    /// </summary>
    public class PlessFontHandler : DisposableObject, IFontHandler
    {
        private string name = string.Empty;
        private FontStyle style = FontStyle.Regular;
        private double sizeInPoints = 12;
        private FontWeight weight = FontWeight.Normal;
        private FontEncoding encoding = FontEncoding.Default;
        private bool isFixedWidth = false;
        private string? serialized;

        public PlessFontHandler(string name, double sizeInPoints)
        {
            this.Name = name;
            this.SizeInPoints = sizeInPoints;
        }

        public PlessFontHandler()
        {
        }

        public virtual string Description
        {
            get
            {
                return Serialize();
            }
        }

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

        public virtual double SizeInPoints
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

        public virtual FontEncoding GetEncoding()
        {
            return encoding;
        }

        public virtual void SetEncoding(FontEncoding value)
        {
            encoding = value;
            Changed();
        }

        public virtual int GetNumericWeight()
        {
            return Font.GetNumericWeightOf(weight);
        }

        public virtual int GetPixelSize()
        {
            var result = GraphicsUnitConverter.Convert(
                GraphicsUnit.Point,
                GraphicsUnit.Pixel,
                Display.Primary.DPI.Height,
                sizeInPoints);
            return (int)result;
        }

        public virtual bool GetItalic()
        {
            return style.HasFlag(FontStyle.Italic);
        }

        public virtual bool GetStrikethrough()
        {
            return style.HasFlag(FontStyle.Strikeout);
        }

        public virtual bool GetUnderlined()
        {
            return style.HasFlag(FontStyle.Underline);
        }

        public virtual FontWeight GetWeight()
        {
            return weight;
        }

        public virtual void SetNumericWeight(int value)
        {
            var newWeight = Font.GetWeightClosestToNumericValue(value);
            SetWeight(newWeight);
        }

        public virtual void SetWeight(FontWeight value)
        {
            if (weight == value)
                return;
            weight = value;
            style = Font.ChangeFontStyle(style, FontStyle.Bold, Font.GetIsBold(weight));
            Changed();
        }

        public virtual bool IsFixedWidth()
        {
            return isFixedWidth;
        }

        public virtual void SetIsFixedWidth(bool value)
        {
            isFixedWidth = value;
            Changed();
        }

        public virtual bool IsUsingSizeInPixels()
        {
            return false;
        }

        public virtual bool Equals(Font font)
        {
            var thisSerialized = Serialize();
            var otherSerialized = font.Handler.Serialize();
            return thisSerialized == otherSerialized;
        }

        public virtual string Serialize()
        {
            return serialized ??= Font.ToUserString(this);
        }

        public virtual void Changed()
        {
            serialized = null;
        }

        public virtual void Update(IFontHandler.FontParams prm)
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
