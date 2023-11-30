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

            BorderSettings hotBorder = new(normalBorder);
            hotBorder.Width = 0;
            hotBorder.Color = SystemColors.ControlText;
            hotBorder.Color = Color.Transparent;
            this.SetBorder(hotBorder, GenericControlState.Hovered);
            this.SetBorder(hotBorder, GenericControlState.Pressed);
            this.SetBorder(normalBorder);

            SwatchColor = swatchColor;
            var size = Alternet.UI.Toolbar.GetDefaultImageSize(this) * 2;

            /*var image = swatchColor.AsImage(size);

            Image = image;

            var imageHot = image.ChangeLightness(150);
            var imagePressed = image.ChangeLightness(50);*/

            var colorHot = swatchColor.ChangeLightness(140);
            var colorPressed = swatchColor.ChangeLightness(80);

            //this.SetImage(imageHot, GenericControlState.Hovered);
            //this.SetImage(imagePressed, GenericControlState.Pressed);

            this.SetBackground((SolidBrush)swatchColor);
            this.SetBackground((SolidBrush)colorHot, GenericControlState.Hovered);
            this.SetBackground((SolidBrush)colorPressed, GenericControlState.Pressed);
        }

        public Color SwatchColor { get; }
    }
}