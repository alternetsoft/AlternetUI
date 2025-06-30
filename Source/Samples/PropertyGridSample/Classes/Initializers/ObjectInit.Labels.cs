using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace PropertyGridSample
{
    public partial class ObjectInit
    {
        public static void InitGenericLabel(object control)
        {
            if (control is not GenericLabel label)
                return;
            label.ParentBackColor = true;
            label.Text = "GenericLabel";
            label.HorizontalAlignment = HorizontalAlignment.Left;
            label.ForegroundColor = Color.Sienna;
            label.MnemonicCharIndex = 3;
            label.ImageVisible = true;
            label.TextAlignment = HVAlignment.Center;
            label.Image = DefaultImage;
            label.DisabledImage = DefaultImage.ToGrayScale();
            label.SuggestedSize = (300, 300);

            SetBackgrounds(label);

            label.Borders ??= new();
            var border = BorderSettings.Default.Clone();
            border.UniformCornerRadius = 15;
            border.UniformRadiusIsPercent = true;
            var doubleBorder = border.Clone();
            doubleBorder.Width = 5;

            label.Borders.SetAll(border);
            label.Borders.SetObject(doubleBorder, VisualControlState.Hovered);

            var colors = new FontAndColor(Color.Black, null, AbstractControl.DefaultFont.AsBold);

            label.StateObjects ??= new();
            label.StateObjects.Colors ??= new();
            label.StateObjects.Colors.SetObject(colors, VisualControlState.Hovered);

            label.VisualStateChanged += Label_VisualStateChanged;

            static void Label_VisualStateChanged(object? sender, EventArgs e)
            {
                App.LogReplace(
                    $"Label.VisualState = {(sender as AbstractControl)?.VisualState}",
                    "Label.VisualState");
            }

            /*
                            var border = (c as Border)!;
                            border.ParentBackColor = false;
                            border.ParentForeColor = false;
                            border.SuggestedSize = defaultListSize;
                            SetBackgrounds(border);

                            border.Layout = LayoutStyle.Vertical;
                            Button button = new();
                            button.Text = "Click me";
                            button.Parent = border;
                            button.Click += Button_Click;

                            static void Button_Click(object? sender, EventArgs e)
                            {
                                App.Log("Button in Border clicked.");
                            }

            */
        }

        public static void InitLabelAndButton(object control)
        {
            if (control is not LabelAndButton label)
                return;
            label.Text = "This is text";
            label.HorizontalAlignment = HorizontalAlignment.Left;
        }

        public static void InitGenericTextControl(object control)
        {
            if (control is not GenericTextControl label)
                return;
            label.Text = "This is text";
            label.HorizontalAlignment = HorizontalAlignment.Left;
        }

        public static void InitLabel(object control)
        {
            if (control is not Label label)
                return;
            label.Text = LoremIpsum.Replace("\n",StringUtils.OneSpace).Trim();
            label.HorizontalAlignment = HorizontalAlignment.Left;
            label.MaxTextWidth = 200;
        }

        public static void InitLinkLabel(object control)
        {
            if (control is not LinkLabel label)
                return;
            var s = "https://www.google.com";
            label.Text = "LinkLabel";
            label.Url = s;
            label.IsUnderline = true;
        }
    }
}