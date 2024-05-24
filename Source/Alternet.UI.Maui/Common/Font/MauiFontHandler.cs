using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui;
using Microsoft.Maui.Graphics;

using SkiaSharp;

namespace Alternet.Drawing
{
    public class MauiFontHandler
		: PlessFontHandler, Microsoft.Maui.Graphics.IFont
    {
        private SKFont? skiaFont;

        public MauiFontHandler()
            : this(Microsoft.Maui.Graphics.Font.Default)
        {
        }

        public MauiFontHandler(Microsoft.Maui.Graphics.Font font)
        {
			Name = font.Name;
			if (font.StyleType == FontStyleType.Italic)
				Style = FontStyle.Italic;
			SetNumericWeight(font.Weight);
        }

        public SKFont? SkiaFont
		{
			get => skiaFont;
			set => skiaFont = value;
		}

        public TextDecorations TextDecorations
		{
			get
			{
				TextDecorations result = TextDecorations.None;
				if (GetStrikethrough())
					result |= TextDecorations.Strikethrough;
				if(GetUnderlined())
                    result |= TextDecorations.Underline;
				return result;
            }
		}

		string IFont.Name => Name;

        int IFont.Weight => GetNumericWeight();

		FontStyleType IFont.StyleType => GetStyleType();

        public FontStyleType GetStyleType()
        {
            if (GetItalic())
                return FontStyleType.Italic;
            else
                return FontStyleType.Normal;
        }

        public override void Update(IFontHandler.FontParams prm)
        {
            base.Update(prm);
			skiaFont = null;
        }
    }
}
