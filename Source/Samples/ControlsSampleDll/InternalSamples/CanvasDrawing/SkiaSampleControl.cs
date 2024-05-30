﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public class SkiaSampleControl : UserControl
    {
        public SkiaSampleControl()
        {
            Font = Control.DefaultFont
                .Scaled(2).GetWithStyle(FontStyle.Underline | FontStyle.Bold | FontStyle.Strikeout);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var dc = e.Graphics;

            dc.FillRectangle(Color.LightGoldenrodYellow.AsBrush, e.ClipRectangle);

            var font = Font ?? Control.DefaultFont;

            dc.DrawText(
                $"hello text: {font.SizeInPoints}",
                (0, 0),
                font,
                Color.Black,
                Color.LightGreen);

            dc.DrawText(
                $"; Hello text: {font.SizeInPoints}",
                (150, 0),
                font,
                Color.Black,
                Color.LightGreen);

            font = font.Scaled(2);

            dc.DrawText(
                $"hello text 2: {font.SizeInPoints}",
                (50, 150),
                font,
                Color.Black,
                Color.LightGreen);

            dc.SetPixel(0, 0, Color.Red);
            dc.SetPixel(50, 150, Color.Red);
        }
    }
}