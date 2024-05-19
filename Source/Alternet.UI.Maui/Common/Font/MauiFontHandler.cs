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
    public class MauiFontHandler : PlessFontHandler
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
    }
}
