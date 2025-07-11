using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    [IsCsLocalized(true)]
    public class SkiaSampleControl : UserControl
    {
        private static Font? sampleFont;

        public static string S1 = "He|l lo";
        public static string S2 = "; hello ";

        public SkiaSampleControl()
        {
            Font = SampleFont;
        }

        public static Font SampleFont
        {
            get
            {
                return sampleFont ??=
                    AbstractControl.DefaultFont.Scaled(2)
                    .WithStyle(FontStyle.Underline | FontStyle.Bold | FontStyle.Strikeout);
            }

            set
            {
                sampleFont = value;
            }
        }

        public override void DefaultPaint(PaintEventArgs e)
        {
            var dc = e.Graphics;
            var rect = e.ClipRectangle;

            dc.FillRectangle(Color.LightGoldenrodYellow.AsBrush, rect);

            var svgImage = KnownSvgImages.ImgAngleDown;
            var image = svgImage.ImageWithColor(64, Color.Red);
                
            image ??= Bitmap.Empty;

            var font = Font ?? AbstractControl.DefaultFont;

            dc.DrawText(
                $"{S1}",
                (5, 0),
                font,
                Color.Black,
                Color.LightGreen);

            dc.DrawText(
                $"{S2}",
                (160, 0),
                font,
                Color.Navy,
                Color.LightGreen);

            font = font.Base;

            dc.DrawText(
                $"{S2} {font.SizeInPoints}",
                (50, 150),
                font,
                Color.Red,
                Color.LightGreen);

            dc.DrawRectangle(Color.Red.AsPen, rect);

            dc.DrawImage(image, (50, 150));
        }
    }
}
