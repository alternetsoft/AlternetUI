using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        /// Gets or sets whether to show debug corners when control is painted.
        /// </summary>
        public static bool ShowDebugCorners = false;

        /// <summary>
        /// Calls <see cref="RaisePaintRecursive(AbstractControl?, Graphics, PointD)"/>
        /// inside changed clip rectangle.
        /// </summary>
        /// <param name="control">Control template.</param>
        /// <param name="dc">Canvas where to draw the template.</param>
        /// <param name="origin">Value on which to translate the top-left
        /// corner of the control.</param>
        /// <param name="isClipped">Whether to clip painting.</param>
        public static void RaisePaintClipped(
            AbstractControl? control,
            Graphics dc,
            PointD origin,
            bool isClipped = true)
        {
            if (control is null)
                return;

            dc.DoInsideClipped(
                (origin, control.Size),
                () =>
                {
                    RaisePaintRecursive(control, dc, origin);
                },
                isClipped);
        }

        /// <summary>
        /// Raises paint event for the control and for all its children at the specified point.
        /// This method is used for template painting. Controls are painted only if
        /// <see cref="AbstractControl.UserPaint"/> is <c>true</c>.
        /// </summary>
        public static void RaisePaintRecursive(
            AbstractControl? control,
            Graphics dc,
            PointD origin)
        {
            if(origin != PointD.Empty)
                dc.PushAndTranslate(origin.X, origin.Y);
            try
            {
                RaisePaintRecursive(control, dc);
            }
            finally
            {
                if (origin != PointD.Empty)
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
            Graphics dc)
        {
            if (control is null || !control.HasChildren)
                return;

            // We need Children here, not AllChildrenInLayout
            var children = control.Children;

            foreach (var child in children)
            {
                if (!child.Visible)
                    continue;
                RaisePaintRecursive(child, dc, child.Location);
            }
        }

        /// <summary>
        /// Raises paint event for all generic children
        /// of the specified control.
        /// </summary>
        /// <remarks>
        /// Only controls which are inherited from <see cref="GenericControl"/>
        /// are painted by this method.
        /// </remarks>
        public static void RaisePaintForGenericChildren(
            AbstractControl? control,
            Func<Graphics> dc)
        {
            if (control is null || !control.HasChildren)
                return;

            // We need Children here, not AllChildrenInLayout
            var children = control.Children;

            foreach (var child in children)
            {
                if (!child.Visible)
                    continue;
                if (child is not GenericControl)
                    continue;

                try
                {
                    var scaleFactor = child.ScaleFactor;

                    GraphicsFactory.MeasureCanvasOverride
                        = SkiaUtils.CreateMeasureCanvas(scaleFactor);

                    var canvas = SkiaUtils.CreateBitmapCanvas(
                        child.Bounds.Size,
                        scaleFactor,
                        true);
                    canvas.UseUnscaledDrawImage = true;

                    GraphicsFactory.MeasureCanvasOverride = canvas;

                    RaisePaintRecursive(child, canvas, PointD.Empty);

                    var skBitmap = canvas.Bitmap ?? new SKBitmap();
                    var bitmap = (Image)skBitmap;
                    dc().DrawImage(bitmap, child.Location);
                }
                finally
                {
                    GraphicsFactory.MeasureCanvasOverride = null;
                }
            }
        }

        /// <summary>
        /// Raises paint event for the control and for all its children.
        /// This method is used for template painting. Controls are painted only if
        /// <see cref="AbstractControl.UserPaint"/> is <c>true</c>.
        /// </summary>
        public static void RaisePaintRecursive(
            AbstractControl? control,
            Graphics dc)
        {
            if (control is null)
                return;

            if (control.UserPaint)
            {
                RectD r = (PointD.Empty, control.Size);

                PaintEventArgs e = new(() => dc, r);
                control.RaisePaint(e);
            }

            RaisePaintForChildren(control, dc);
        }

        /// <summary>
        /// Gets template contents as <see cref="ImageSet"/>.
        /// </summary>
        /// <param name="template">Template control</param>
        /// <param name="backColor">Background color. Optional.
        /// If not specified, background color
        /// of the template control is used.</param>
        /// <returns></returns>
        public static ImageSet GetTemplateAsImageSet(
            TemplateControl template,
            Color? backColor = null)
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
        public static SKBitmap GetTemplateAsSKBitmap(
            TemplateControl template,
            Color? backColor = null)
        {
            try
            {
                backColor ??= template.BackgroundColor;

                GraphicsFactory.MeasureCanvasOverride
                    = SkiaUtils.CreateMeasureCanvas(template.ScaleFactor);

                template.SetSizeToContent();

                var canvas = SkiaUtils.CreateBitmapCanvas(
                    template.Bounds.Size.Ceiling(),
                    template.ScaleFactor,
                    true);
                canvas.UseUnscaledDrawImage = true;

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
        public static TemplateControls.BoldText<GenericLabel> CreateTemplateWithBoldText(
            string prefix,
            string boldText,
            string suffix,
            IReadOnlyFontAndColor? fontAndColor = null,
            bool hasBorder = false)
        {
            return CreateTemplateWithBoldText<GenericLabel>(
                        prefix,
                        boldText,
                        suffix,
                        fontAndColor,
                        hasBorder);
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
        /// <typeparam name="TLabel">Type of the label controls.</typeparam>
        public static TemplateControls.BoldText<TLabel> CreateTemplateWithBoldText<TLabel>(
            string prefix,
            string boldText,
            string suffix,
            IReadOnlyFontAndColor? fontAndColor = null,
            bool hasBorder = false)
            where TLabel : AbstractControl, new()
        {
            var result = new TemplateControls.BoldText<TLabel>(prefix, boldText, suffix, hasBorder);
            if(fontAndColor is not null)
                result.AsFontAndColor = fontAndColor;
            return result;
        }

        /// <summary>
        /// Draws control template on the specified canvas.
        /// </summary>
        /// <param name="control">Control template.</param>
        /// <param name="canvas">Canvas where to draw the template.</param>
        /// <param name="translate">Value on which to translate the top-left
        /// corner of the control.</param>
        internal static void DrawControlTemplate(
            TemplateControl control,
            Graphics canvas,
            PointD? translate = null)
        {
            var pt = translate ?? PointD.Empty;
            control.SetSizeToContent();
            RaisePaintRecursive(control, canvas, pt);
            DefaultPaintDebug(canvas, (pt, control.Size));
        }

        [Conditional("DEBUG")]
        private static void DefaultPaintDebug(Graphics canvas, RectD rect)
        {
            if (ShowDebugCorners)
                BorderSettings.DrawDesignCorners(canvas, rect);
        }
    }
}
