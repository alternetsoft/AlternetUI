using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

using SkiaSharp;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties related to the control templates.
    /// </summary>
    public static class TemplateUtils
    {
        public static RichToolTip CreateAndShowTemplateToolTip(
            Control tooltipParent,
            PointD location,
            TemplateControl template,
            Color? backColor = null)
        {
            var toolTip = CreateTemplateToolTip(
                tooltipParent,
                location,
                template,
                backColor);
            RichToolTip.Default = toolTip;
            toolTip.ShowAtLocation(tooltipParent, location, false);
            return toolTip;
        }

        public static RichToolTip CreateTemplateToolTip(
            Control tooltipParent,
            PointD location,
            TemplateControl template,
            Color? backColor)
        {
            backColor ??= template.BackgroundColor;

            ImageSet imageSet = GetTemplateAsImageSet(template, backColor);
            RichToolTip toolTip = new();
            toolTip.SetTipKind(RichToolTipKind.None);
            if (backColor is not null)
                toolTip.SetBackgroundColor(backColor);
            toolTip.SetIcon(imageSet);
            return toolTip;
        }

        public static ImageSet GetTemplateAsImageSet(TemplateControl template, Color? backColor)
        {
            ImageSet imageSet = new(GetTemplateAsImage(template, backColor));
            return imageSet;
        }

        public static Image GetTemplateAsImage(TemplateControl template, Color? backColor)
        {
            var result = (Image)GetTemplateAsSKBitmap(template, backColor);
            return result;
        }

        public static SKBitmap GetTemplateAsSKBitmap(TemplateControl template, Color? backColor)
        {
            try
            {
                backColor ??= template.BackgroundColor;

                GraphicsFactory.MeasureCanvasOverride = SkiaUtils.CreateMeasureCanvas(1);

                template.SetSizeToContent(WindowSizeToContentMode.WidthAndHeight);

                var bounds = template.Bounds;
                SKBitmap bitmap = new((int)bounds.Width, (int)bounds.Height, false);
                var canvas = new SkiaGraphics(bitmap);

                if (backColor is not null)
                {
                    canvas.Canvas.Clear(backColor);
                }

                DrawControlTemplate(template, canvas);

                return bitmap;
            }
            finally
            {
                GraphicsFactory.MeasureCanvasOverride = null;
            }
        }

        public static void DrawControlTemplate(
            TemplateControl control,
            Graphics canvas,
            PointD? translate = null)
        {
            RectD clipRect = (0, 0, control.Width, control.Height);

            PaintEventArgs e = new(canvas, clipRect);

            if (translate is not null)
            {
                TransformMatrix transform = new();
                transform.Translate(translate.Value.X, translate.Value.Y);
                canvas.PushTransform(transform);
            }

            control.RaisePaintRecursive(e);

            if (translate is not null)
            {
                canvas.Pop();
            }
        }

        public static TemplateWithBoldText CreateTemplateWithBoldText(
            string prefix,
            string boldText,
            string suffix,
            IReadOnlyFontAndColor? fontAndColor = null,
            bool hasBorder = false)
        {
            var result = new TemplateWithBoldText(prefix, boldText, suffix, hasBorder);

            if(fontAndColor is not null)
            {
                result.BackgroundColor = fontAndColor.BackgroundColor;
                result.ForegroundColor = fontAndColor.ForegroundColor;
                result.Font = fontAndColor.Font;
            }

            return result;
        }

        public class TemplateWithBoldText : TemplateControl
        {
            private readonly Border border = new()
            {
                Layout = LayoutStyle.Horizontal,
                HasBorder = false,
            };

            private readonly GenericLabel prefixLabel = new();
            private readonly GenericLabel suffixLabel = new();

            private readonly GenericLabel boldLabel = new()
            {
                IsBold = true,
            };

            public TemplateWithBoldText(string prefix, string boldText, string suffix, bool hasBorder)
            {
                HasBorder = hasBorder;

                prefixLabel.Text = prefix;
                boldLabel.Text = boldText;
                suffixLabel.Text = suffix;

                DoInsideLayout(() =>
                {
                    border.Parent = this;
                    prefixLabel.Parent = border;
                    boldLabel.Parent = border;
                    suffixLabel.Parent = border;
                });

                SetChildrenUseParentBackColor(true, true);
                SetChildrenUseParentForeColor(true, true);
                SetChildrenUseParentFont(true, true);
            }

            public GenericLabel PrefixLabel => prefixLabel;

            public GenericLabel SuffixLabel => suffixLabel;

            public GenericLabel BoldLabel => boldLabel;
        }
    }
}
