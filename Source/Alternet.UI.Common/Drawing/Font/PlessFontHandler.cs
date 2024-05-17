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
    public class PlessFontHandler : DisposableObject, IPlessFontHandler
    {
        private string name = string.Empty;
        private FontStyle style;
        private double sizeInPoints;
        private FontWeight weight;
        private FontEncoding encoding = FontEncoding.Default;
        private bool isFixedWidth;
        private string? serialized;

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
            Alternet.Drawing.Font.CoerceFontParams(ref prm);

            Changed();

            throw new NotImplementedException();

            /*
            public GenericFontFamily? GenericFamily;
            public string? FamilyName;
            public double Size;
            public FontStyle Style = FontStyle.Regular;
            public GraphicsUnit Unit = GraphicsUnit.Point;
            public byte GdiCharSet = 1;
            */

            /*
            Initialize(
               prm.GenericFamily ?? 0,
               prm.FamilyName,
               prm.Size,
               prm.Style);
            */
        }
    }
}
