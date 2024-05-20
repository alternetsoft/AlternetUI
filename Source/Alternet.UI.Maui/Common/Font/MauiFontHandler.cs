using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui;
using Microsoft.Maui.Graphics;

using SkiaSharp;

#pragma warning disable
/*
	public struct Font : IFont, IEquatable<IFont>
	{
		public static Font Default
			=> new Font(null);

		public static Font DefaultBold
			=> new Font(null, FontWeights.Bold);

		public Font(string name, int weight = FontWeights.Normal, FontStyleType styleType = FontStyleType.Normal)

		public string Name { get; private set; }
		public int Weight { get; private set; }
		public FontStyleType StyleType { get; private set; }

		public bool IsDefault
			=> string.IsNullOrEmpty(Name);
	}

*/
#pragma warning enable

namespace Alternet.Drawing
{
    public class MauiFontHandler
		: PlessFontHandler, Microsoft.Maui.Graphics.IFont
    {
        private Microsoft.Maui.Graphics.Font font;
        private SKFont? skiaFont;

        public MauiFontHandler()
            : this(Microsoft.Maui.Graphics.Font.Default)
        {
        }

        public MauiFontHandler(Microsoft.Maui.Graphics.Font font)
        {
            this.font = font;
			Name = font.Name;
			if (font.StyleType == FontStyleType.Italic)
				Style = FontStyle.Italic;
			SetNumericWeight(font.Weight);
        }

        public Microsoft.Maui.Graphics.Font Font
		{
			get => font;
			set => font = value;
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
			var oldFontName = Name;
			var oldFontWeight = GetNumericWeight();
			var oldFontStyle = GetStyleType();

            base.Update(prm);

            var newFontName = Name;
            var newFontWeight = GetNumericWeight();
            var newFontStyle = GetStyleType();

			if(oldFontName != newFontName || oldFontWeight != newFontWeight
				|| oldFontStyle != newFontStyle)
			{
				font = new(newFontName, newFontWeight, newFontStyle);
            }
        }
    }
}
