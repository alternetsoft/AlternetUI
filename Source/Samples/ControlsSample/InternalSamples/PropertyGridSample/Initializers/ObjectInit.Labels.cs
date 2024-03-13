using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace PropertyGridSample
{
    internal partial class ObjectInit
    {
        public static void InitGenericLabel(object control)
        {
            if (control is not GenericLabel label)
                return;
            label.Text = "GenericLabel";
            label.HorizontalAlignment = HorizontalAlignment.Left;
            label.ForegroundColor = Color.Sienna;
            label.MnemonicCharIndex = 3;
            label.ImageVisible = true;
            label.TextAlignment = GenericAlignment.Center;
            label.Image = DefaultImage;
            label.DisabledImage = DefaultImage.ToGrayScale();
            label.SuggestedSize = (300, 300);

            label.Borders ??= new();
            var border = BorderSettings.Default.Clone();
            border.UniformCornerRadius = 15;
            border.UniformRadiusIsPercent = true;
            var doubleBorder = border.Clone();
            doubleBorder.Width = 2;

            label.Borders.SetAll(border);
            label.Borders.SetObject(doubleBorder, GenericControlState.Hovered);

            var colors = new FontAndColor(Color.Black, null, Control.DefaultFont.AsBold);

            label.StateObjects ??= new();
            label.StateObjects.Colors ??= new();
            label.StateObjects.Colors.SetObject(colors, GenericControlState.Hovered);
            SetBackgrounds(label);
        }

        public static void InitLabel(object control)
        {
            if (control is not Label label)
                return;
            label.Text = "La&bel";
            label.HorizontalAlignment = HorizontalAlignment.Left;
        }

        public static void InitLinkLabel(object control)
        {
            if (control is not LinkLabel label)
                return;
            var s = "https://www.google.com";
            label.Text = "LinkLabel";
            label.Url = s;
        }
    }
}