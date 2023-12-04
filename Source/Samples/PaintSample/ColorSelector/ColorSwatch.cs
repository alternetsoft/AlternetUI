using Alternet.Drawing;
using Alternet.UI;
using System;

namespace PaintSample
{
    internal class ColorSwatch : PictureBox
    {
        public ColorSwatch(Color swatchColor)
        {
            var normalBorder = Border.CreateDefault();
            normalBorder.UniformCornerRadius = 25;
            normalBorder.UniformRadiusIsPercent = true;
            normalBorder.Width = 0;
            normalBorder.Color = Color.Transparent;

            BorderSettings hotBorder = new(normalBorder)
            {
                Width = 0,
                Color = SystemColors.ControlText,
            };
            hotBorder.Color = Color.Transparent;
            this.SetBorder(hotBorder, GenericControlState.Hovered);
            this.SetBorder(hotBorder, GenericControlState.Pressed);
            this.SetBorder(normalBorder);

            SwatchColor = swatchColor;
            var colorHot = swatchColor.ChangeLightness(140);
            var colorPressed = swatchColor.ChangeLightness(80);

            this.SetBackground((SolidBrush)swatchColor);
            this.SetBackground((SolidBrush)colorHot, GenericControlState.Hovered);
            this.SetBackground((SolidBrush)colorPressed, GenericControlState.Pressed);
        }

        public Color SwatchColor { get; }
    }
}