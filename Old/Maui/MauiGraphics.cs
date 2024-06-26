﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;
using Alternet.UI.Extensions;

using Microsoft.Maui.Graphics;

#pragma warning disable
/*
https://learn.microsoft.com/en-us/dotnet/maui/user-interface/graphics/draw?view=net-maui-8.0

canvas.FontColor = Colors.Blue;
canvas.FontSize = 18;

canvas.Font = Font.Default;


public interface IAttributedText
{
	string Text { get; }

	IReadOnlyList<IAttributedTextRun> Runs { get; }
}

public interface IAttributedTextRun
{
	int Start { get; }

	int Length { get; }

	ITextAttributes Attributes { get; }
}

public interface ITextAttributes : IReadOnlyDictionary<TextAttribute, string>, IEnumerable<KeyValuePair<TextAttribute, string>>, IEnumerable, IReadOnlyCollection<KeyValuePair<TextAttribute, string>>
{
}

public enum TextAttribute
{
	FontName,
	FontSize,
	Bold,
	Italic,
	Underline,
	Strikethrough,
	Subscript,
	Superscript,
	Color,
	Background,
	UnorderedList,
	Marker
}



    public enum TextFlow
    {
	    ClipBounds,
	    OverflowBounds
    }

    public enum HorizontalAlignment
    {
	    Left,
	    Center,
	    Right,
	    Justified
    }

    public enum VerticalAlignment
    {
	    Top,
	    Center,
	    Bottom
    }

    Microsoft.Maui.Graphics.Font

    IFontCollection

	public enum FontStyleType
	{
		Normal,
		Italic,
		Oblique
	}

	public interface IFont
	{
		string Name { get; }

		int Weight { get; }

		FontStyleType StyleType { get; }
	}

/// <summary>
/// Sets the font used when drawing text.
/// </summary>
public IFont Font { set; }

/// <summary>
/// Sets the size of the font used when drawing text.
/// </summary>
public float FontSize { set; }

/// <summary>
/// Draws a text string onto the canvas.
/// </summary>
/// <remarks>To draw attributed text, use <see cref="DrawText(IAttributedText, float, float, float, float)"/> instead.</remarks>
/// <param name="value">Text to be displayed.</param>
/// <param name="x">Starting <c>x</c> coordinate.</param>
/// <param name="y">Starting <c>y</c> coordinate.</param>
/// <param name="horizontalAlignment">Horizontal alignment options to align the string.</param>
public void DrawString(string value, float x, float y, HorizontalAlignment horizontalAlignment);

/// <summary>
/// Draws a text string within a bounding box onto the canvas.
/// </summary>
/// <param name="value">Text to be displayed.</param>
/// <param name="x">Starting <c>x</c> coordinate of the bounding box.</param>
/// <param name="y">Starting <c>y</c> coordinate of the bounding box.</param>
/// <param name="width">Width of the bounding box.</param>
/// <param name="height">Height of the bounding box.</param>
/// <param name="horizontalAlignment">Horizontal alignment options to align the
/// string within the bounding box.</param>
/// <param name="verticalAlignment">Vertical alignment options to align the
/// string within the bounding box.</param>
/// <param name="textFlow">Specifies whether text will be clipped in case it overflows
/// the bounding box. Default is <see cref="TextFlow.ClipBounds"/>.</param>
/// <param name="lineSpacingAdjustment">Spacing adjustment between lines. Default is 0.</param>
public void DrawString(
	string value,
	float x,
	float y,
	float width,
	float height,
	HorizontalAlignment horizontalAlignment,
	VerticalAlignment verticalAlignment,
	TextFlow textFlow = TextFlow.ClipBounds,
	float lineSpacingAdjustment = 0);

/// <summary>
/// Draws attributed text within a bounding box onto the canvas.
/// </summary>
/// <param name="value">Attributed text to be displayed.</param>
/// <param name="x">Starting <c>x</c> coordinate of the bounding box.</param>
/// <param name="y">Starting <c>y</c> coordinate of the bounding box.</param>
/// <param name="width">Width of the bounding box.</param>
/// <param name="height">Height of the bounding box.</param>
public void DrawText(
	IAttributedText value,
	float x,
	float y,
	float width,
	float height);

/// <summary>
/// Calculates the area a string would occupy if drawn on the canvas.
/// </summary>
/// <param name="value">String to calculate the size on.</param>
/// <param name="font">The string's font type.</param>
/// <param name="fontSize">The string's font size.</param>
/// <returns>The area the string would occupy on the canvas.</returns>
public SizeF GetStringSize(string value, IFont font, float fontSize);

/// <summary>
/// Calculates the area a string would occupy if drawn on the canvas.
/// </summary>
/// <param name="value">String to calculate the size on.</param>
/// <param name="font">The string's font type.</param>
/// <param name="fontSize">The string's font size.</param>
/// <param name="horizontalAlignment">Horizontal alignment options for the string.</param>
/// <param name="verticalAlignment">Vertical alignment options for the string.</param>
/// <returns>The area the string would occupy on the canvas.</returns>
public SizeF GetStringSize(
    string value,
    IFont font,
    float fontSize,
    HorizontalAlignment horizontalAlignment,
    VerticalAlignment verticalAlignment);

*/
#pragma warning enable

namespace Alternet.Drawing
{
    public class MauiGraphics : NotImplementedGraphics
    {
        private ICanvas canvas;
        private RectF dirtyRect;
        private MauiContainer container;

        public MauiGraphics(MauiContainer container, ICanvas canvas, RectF dirtyRect)
        {
            this.canvas = canvas;
            this.dirtyRect = dirtyRect;
            this.container = container;
        }

        public MauiContainer Container
        {
            get => container;
            set => container = value;
        }

        public RectF DirtyRect
        {
            get => dirtyRect;
            set => dirtyRect = value;
        }

        public ICanvas Canvas
        {
            get => canvas;
            set => canvas = value;
        }

        /// <inheritdoc/>
        public override SizeD GetTextExtent(
            string text,
            Font font,
            out double? descent,
            out double? externalLeading,
            IControl? control = null)
        {
            descent = null;
            externalLeading = null;
            return GetTextExtent(text, font, control);
        }

        /// <inheritdoc/>
        public override SizeD GetTextExtent(
            string text,
            Font font,
            IControl? control)
        {
            return GetTextExtent(text, font);
        }

        // Used in editor
        /// <inheritdoc/>
        public override SizeD GetTextExtent(string text, Font font)
        {
            var size = canvas.GetStringSize(text, font.ToMaui(), (float)font.SizeInPoints);
            return (size.Width, size.Height);
        }

        public void FontToCanvas(Font font)
        {
            canvas.Font = font.ToMaui();
            canvas.FontSize = (float)font.SizeInPoints;
        }

        /*==========================================*/

        // Used in editor
        /// <inheritdoc/>
        public override void DrawText(
            string text,
            PointD location,
            Font font,
            Color foreColor,
            Color backColor)
        {
            var scale = canvas.DisplayScale;

            FontToCanvas(font);
            canvas.FontColor = foreColor.ToMaui();

            var locationX = (float)location.X;
            var locationY = (float)location.Y;
            var fontSize = (float)font.SizeInPoints;

            var platformFont = font.ToMaui();

            var size = canvas.GetStringSize(text, platformFont, fontSize + 5);

            if (backColor.IsOk)
            {
                canvas.FillColor = backColor.ToMaui();
                canvas.FillRectangle(locationX, locationY, size.Width, size.Height);
            }

            canvas.DrawString(
                text,
                locationX,
                locationY,
                int.MaxValue,
                int.MaxValue,
                Microsoft.Maui.Graphics.HorizontalAlignment.Left,
                Microsoft.Maui.Graphics.VerticalAlignment.Top,
                TextFlow.OverflowBounds,
                0);
        }

        /// <inheritdoc/>
        public override void DrawText(
            string text,
            Font font,
            Brush brush,
            PointD origin)
        {
        }

        /// <inheritdoc/>
        public override void DrawText(
            string text,
            Font font,
            Brush brush,
            RectD bounds,
            TextFormat format)
        {
        }

        // Used in editor
        /// <inheritdoc/>
        public override void SetPixel(double x, double y, Color color)
        {
        }

        // Used in editor
        /// <inheritdoc/>
        public override void PushTransform(TransformMatrix transform)
        {
        }

        // Used in editor
        /// <inheritdoc/>
        public override void FillRectangle(Brush brush, RectD rectangle)
        {
        }

        // Used in editor
        /// <inheritdoc/>
        public override void Pop()
        {
        }

        // Used in editor
        /// <inheritdoc/>
        public override void DrawBeziers(Pen pen, PointD[] points)
        {
        }

        // Used in editor
        /// <inheritdoc/>
        public override void DrawRoundedRectangle(Pen pen, RectD rect, double cornerRadius)
        {
        }

        // Used in editor
        /// <inheritdoc/>
        public override void FillRoundedRectangle(Brush brush, RectD rect, double cornerRadius)
        {
        }

        // Used in editor
        /// <inheritdoc/>
        public override void DrawPolygon(Pen pen, PointD[] points)
        {
        }

        // Used in editor
        /// <inheritdoc/>
        public override void DrawRectangle(Pen pen, RectD rectangle)
        {
        }

        // Used in editor
        /// <inheritdoc/>
        public override void DrawImage(Image image, PointD origin, bool useMask = false)
        {
        }

        // Used in editor
        /// <inheritdoc/>
        public override void DrawLine(Pen pen, PointD a, PointD b)
        {
        }
    }
}
