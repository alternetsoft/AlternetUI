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
        /// <summary>
        /// Raises paint event for the control and for all its children at the specified point.
        /// This method is used for template painting. Controls are painted only if
        /// <see cref="AbstractControl.UserPaint"/> is <c>true</c>.
        /// </summary>
        public static void RaisePaintRecursive(
            AbstractControl? control,
            PaintEventArgs e,
            PointD origin)
        {
            var dc = e.Graphics;
            dc.PushAndTranslate(origin.X, origin.Y);
            try
            {
                RaisePaintRecursive(control, e);
            }
            finally
            {
                dc.Pop();
            }
        }

        /// <summary>
        /// Raises paint event for all children of the specified control and not the control itself.
        /// This method is used for template painting. Controls are painted only if
        /// <see cref="AbstractControl.UserPaint"/> is <c>true</c>.
        /// </summary>
        public static void RaisePaintForChildren(
            AbstractControl? control,
            PaintEventArgs e)
        {
            if (control is null || !control.HasChildren)
                return;

            // We need Children here, not AllChildrenInLayout
            var children = control.Children;

            foreach (var child in children)
            {
                if (!child.Visible)
                    continue;
                RaisePaintRecursive(child, e, child.Location);
            }
        }

        /// <summary>
        /// Raises paint event for the control and for all its children.
        /// This method is used for template painting. Controls are painted only if
        /// <see cref="AbstractControl.UserPaint"/> is <c>true</c>.
        /// </summary>
        public static void RaisePaintRecursive(
            AbstractControl? control,
            PaintEventArgs e)
        {
            if (control is null)
                return;

            if (control.UserPaint)
                control.RaisePaint(e);

            RaisePaintForChildren(control, e);
        }

        /// <summary>
        /// Gets template contents as <see cref="ImageSet"/>.
        /// </summary>
        /// <param name="template">Template control</param>
        /// <param name="backColor">Background color. Optional. If not specified, background color
        /// of the template control is used.</param>
        /// <returns></returns>
        public static ImageSet GetTemplateAsImageSet(TemplateControl template, Color? backColor = null)
        {
            ImageSet imageSet = new(GetTemplateAsImage(template, backColor));
            return imageSet;
        }

        /// <summary>
        /// Gets template contents as <see cref="Image"/>.
        /// </summary>
        /// <param name="template">Template control</param>
        /// <param name="backColor">Background color. Optional. If not specified, background color
        /// of the template control is used.</param>
        /// <returns></returns>
        public static Image GetTemplateAsImage(TemplateControl template, Color? backColor = null)
        {
            var result = (Image)GetTemplateAsSKBitmap(template, backColor);
            return result;
        }

        /// <summary>
        /// Gets template contents as <see cref="SKBitmap"/>.
        /// </summary>
        /// <param name="template">Template control</param>
        /// <param name="backColor">Background color. Optional. If not specified, background color
        /// of the template control is used.</param>
        /// <returns></returns>
        public static SKBitmap GetTemplateAsSKBitmap(TemplateControl template, Color? backColor = null)
        {
            try
            {
                backColor ??= template.BackgroundColor;

                GraphicsFactory.MeasureCanvasOverride
                    = SkiaUtils.CreateMeasureCanvas(template.ScaleFactor);

                template.SetSizeToContent();

                var canvas = SkiaUtils.CreateBitmapCanvas(
                    template.Bounds.Size,
                    template.ScaleFactor,
                    true);

                GraphicsFactory.MeasureCanvasOverride = canvas;

                if (backColor is not null)
                {
                    canvas.Canvas.Clear(backColor);
                }

                DrawControlTemplate(template, canvas);

                return canvas.Bitmap ?? new SKBitmap();
            }
            finally
            {
                GraphicsFactory.MeasureCanvasOverride = null;
            }
        }

        /// <summary>
        /// Draws control template on the specified canvas.
        /// </summary>
        /// <param name="control">Control template.</param>
        /// <param name="canvas">Canvas where to draw the template.</param>
        /// <param name="translate">Value on which to translate the top-left
        /// corner of the control.</param>
        public static void DrawControlTemplate(
            TemplateControl control,
            Graphics canvas,
            PointD? translate = null)
        {
            control.SetSizeToContent();

            RectD clipRect = (0, 0, control.Width, control.Height);

            PaintEventArgs e = new(canvas, clipRect);

            if (translate is not null)
            {
                canvas.PushAndTranslate(translate.Value.X, translate.Value.Y);
            }

            RaisePaintRecursive(control, e);

            if (translate is not null)
            {
                canvas.Pop();
            }
        }

        /// <summary>
        /// Creates template with text which has a middle part with bold font.
        /// </summary>
        /// <param name="prefix">First part of the text.</param>
        /// <param name="boldText">Middle part of the text with bold attribute.</param>
        /// <param name="suffix">Last part of the text.</param>
        /// <param name="fontAndColor">Default font and color attributes of the text.</param>
        /// <param name="hasBorder">Whether to draw default border around the text.</param>
        /// <returns></returns>
        /// <remarks>
        /// After template is created any part of it's text can be changed including
        /// it's font and color attributes using template properties.
        /// </remarks>
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

        /// <summary>
        /// Template control with text which has a middle part with bold font.
        /// </summary>
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

            /// <summary>
            /// Initializes a new instance of the <see cref="TemplateWithBoldText"/> class.
            /// </summary>
            /// <param name="prefix">First part of the text.</param>
            /// <param name="boldText">Middle part of the text with bold attribute.</param>
            /// <param name="suffix">Last part of the text.</param>
            /// <param name="hasBorder">Whether to draw default border around the text.</param>
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

                    SetChildrenUseParentBackColor(true, true);
                    SetChildrenUseParentForeColor(true, true);
                    SetChildrenUseParentFont(true, true);
                });
            }

            /// <summary>
            /// Gets control which contains first part of the text.
            /// </summary>
            public GenericLabel PrefixLabel => prefixLabel;

            /// <summary>
            /// Gets control which contains last part of the text.
            /// </summary>
            public GenericLabel SuffixLabel => suffixLabel;

            /// <summary>
            /// Gets control which contains middle part of the text.
            /// </summary>
            public GenericLabel BoldLabel => boldLabel;
        }
    }
}
