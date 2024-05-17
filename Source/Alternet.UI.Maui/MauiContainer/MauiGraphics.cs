using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

using Microsoft.Maui.Graphics;

#pragma warning disable
/*
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
/// <param name="horizontalAlignment">Horizontal alignment options to align the string within the bounding box.</param>
/// <param name="verticalAlignment">Vertical alignment options to align the string within the bounding box.</param>
/// <param name="textFlow">Specifies whether text will be clipped in case it overflows the bounding box. Default is <see cref="TextFlow.ClipBounds"/>.</param>
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
public SizeF GetStringSize(string value, IFont font, float fontSize, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment);

*/
#pragma warning enable

namespace Alternet.Drawing
{
    public class MauiGraphics : NotImplementedGraphics
    {
        private ICanvas canvas;

        public MauiGraphics(ICanvas canvas)
        {
            this.canvas = canvas;
        }

        public ICanvas Canvas
        {
            get => canvas;
            set => canvas = value;
        }

        public override SizeD GetTextExtent(
            string text,
            Font font,
            out double descent,
            out double externalLeading,
            IControl? control = null)
        {
            descent = 0;
            externalLeading = 0;
            return SizeD.Empty;
        }

        public override SizeD GetTextExtent(
            string text,
            Font font,
            IControl? control)
        {
            return SizeD.Empty;
        }

        public override SizeD GetTextExtent(string text, Font font)
        {
            return SizeD.Empty;
        }

        public override void DrawText(string text, Font font, Brush brush, PointD origin)
        {
            DrawText(text, font, brush, origin, TextFormat.Default);
        }

        public override void DrawText(string text, PointD origin)
        {
            DrawText(text, Font.Default, Brush.Default, origin);
        }

        public override void DrawText(
            string text,
            Font font,
            Brush brush,
            PointD origin,
            TextFormat format)
        {
        }

        public override void DrawText(string text, Font font, Brush brush, RectD bounds)
        {
            DrawText(text, font, brush, bounds, TextFormat.Default);
        }

        public override void DrawText(
            string text,
            Font font,
            Brush brush,
            RectD bounds,
            TextFormat format)
        {
        }

        public override SizeD MeasureText(string text, Font font)
        {
            return SizeD.Empty;
        }

        public override SizeD MeasureText(string text, Font font, double maximumWidth)
        {
            return SizeD.Empty;
        }

        public override SizeD MeasureText(
            string text,
            Font font,
            double maximumWidth,
            TextFormat format)
        {
            return SizeD.Empty;
        }

        public override void DrawText(
            string text,
            PointD location,
            Font font,
            Color foreColor,
            Color backColor)
        {
        }
    }
}
