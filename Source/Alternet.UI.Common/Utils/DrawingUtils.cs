﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods related to drawing.
    /// </summary>
    public static class DrawingUtils
    {
        /// <summary>
        /// Calculates distance between two points.
        /// </summary>
        /// <param name="p1">First point.</param>
        /// <param name="p2">Second point.</param>
        /// <returns><see cref="double"/> value with distance between point <paramref name="p1"/>
        /// and point <paramref name="p2"/>.</returns>
        public static double GetDistance(PointD p1, PointD p2)
        {
            return MathUtils.GetDistance(p1.X, p1.Y, p2.X, p2.Y);
        }

        /// <summary>
        /// Compares distance between two points and specified value.
        /// </summary>
        /// <param name="p1">First point.</param>
        /// <param name="p2">Second point.</param>
        /// <param name="value">Value to compare distance with.</param>
        /// <returns><c>true</c> if distance between two points is less
        /// than <paramref name="value"/>; <c>false</c> otherwise.</returns>
        public static bool DistanceIsLess(PointD p1, PointD p2, double value)
        {
            return (Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2)) < Math.Pow(value, 2);
        }

        /// <summary>
        /// Gets squared distance between two points.
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static double GetDistanceSquared(PointD p1, PointD p2) =>
            ((double)(p1.X - p2.X) * (double)(p1.X - p2.X))
            + ((double)(p1.Y - p2.Y) * (double)(p1.Y - p2.Y));

        /// <summary>
        /// Gets whether point is in circle.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static bool IsPointInCircle(PointD p, PointD center, double radius)
        {
            return GetDistanceSquared(p, center) <= radius * radius;
        }

        /// <summary>
        /// Gets point on circle.
        /// </summary>
        /// <param name="center">Circle center.</param>
        /// <param name="radius">Circle radius.</param>
        /// <param name="angle">Angle.</param>
        /// <returns></returns>
        public static PointD GetPointOnCircle(PointD center, double radius, double angle)
        {
            return new(
                center.X + (radius * Math.Cos(angle * MathUtils.DegToRad)),
                center.Y + (radius * Math.Sin(angle * MathUtils.DegToRad)));
        }

        /// <summary>
        /// Gets rectangle of the top border edge with the specified width.
        /// </summary>
        /// <param name="rect">Border rectangle.</param>
        /// <param name="width">Border side width.</param>
        public static RectD GetTopLineRect(this RectD rect, double width)
        {
            var point = rect.TopLeft;
            var size = new SizeD(rect.Width, width);
            return new RectD(point, size);
        }

        /// <summary>
        /// Gets rectangle of the horizontal center line of the rectangle.
        /// </summary>
        /// <param name="rect">Rectangle.</param>
        /// <returns></returns>
        public static RectD GetCenterLineHorz(this RectD rect)
        {
            var size = new SizeD(rect.Width, 1);
            var point = new PointD(rect.Left, (int)rect.Center.Y);
            return new RectD(point, size);
        }

        /// <summary>
        /// Gets rectangle of the vertical center line of the rectangle.
        /// </summary>
        /// <param name="rect">Rectangle.</param>
        /// <returns></returns>
        public static RectD GetCenterLineVert(this RectD rect)
        {
            var size = new SizeD(1, rect.Height);
            var point = new PointD((int)rect.Center.X, rect.Top);
            return new RectD(point, size);
        }

        /// <summary>
        /// Gets rectangle of the bottom border edge with the specified width.
        /// </summary>
        /// <param name="rect">Border rectangle.</param>
        /// <param name="width">Border side width.</param>
        public static RectD GetBottomLineRect(this RectD rect, double width)
        {
            var point = new PointD(rect.Left, rect.Bottom - width);
            var size = new SizeD(rect.Width, width);
            return new RectD(point, size);
        }

        /// <summary>
        /// Draws horizontal line using <see cref="Graphics.FillRectangle"/>.
        /// </summary>
        /// <param name="dc">Drawing context.</param>
        /// <param name="brush">Brush to draw line.</param>
        /// <param name="point">Starting point.</param>
        /// <param name="length">Line length.</param>
        /// <param name="width">Line width.</param>
        public static void DrawHorzLine(
            this Graphics dc,
            Brush brush,
            PointD point,
            double length,
            double width)
        {
            var rect = new RectD(point, new SizeD(length, width));
            dc.FillRectangle(brush, rect);
        }

        /// <summary>
        /// Draws vertical line using <see cref="Graphics.FillRectangle"/>.
        /// </summary>
        /// <param name="dc">Drawing context.</param>
        /// <param name="brush">Brush to draw line.</param>
        /// <param name="point">Starting point.</param>
        /// <param name="length">Line length.</param>
        /// <param name="width">Line width.</param>
        public static void DrawVertLine(
            this Graphics dc,
            Brush brush,
            PointD point,
            double length,
            double width)
        {
            var rect = new RectD(point, new SizeD(width, length));
            dc.FillRectangle(brush, rect);
        }

        /// <summary>
        /// Draws inner and outer border with the specified colors.
        /// </summary>
        /// <param name="canvas"><see cref="Graphics"/> where drawing is performed.</param>
        /// <param name="rect"><see cref="RectD"/> where drawing is performed.</param>
        /// <param name="innerColor">Inner border color.</param>
        /// <param name="outerColor">Outer border color.</param>
        /// <returns>
        /// Value of the <paramref name="rect"/> parameter deflated by number of the painted
        /// borders (0, 1 or 2).
        /// </returns>
        /// <remarks>
        /// If border color is <see cref="Color.Empty"/> it is not painted.
        /// </remarks>
        public static RectD DrawDoubleBorder(
            this Graphics canvas,
            RectD rect,
            Color innerColor,
            Color outerColor)
        {
            var result = rect;
            var hasOuterBorder = outerColor != Color.Empty;
            var hasInnerBorder = innerColor != Color.Empty;
            if (hasOuterBorder)
            {
                DrawingUtils.FillRectangleBorder(canvas, outerColor, result);
                result.Deflate();
            }

            if (hasInnerBorder)
            {
                DrawingUtils.FillRectangleBorder(canvas, innerColor, result);
                result.Deflate();
            }

            return result;
        }

        /// <summary>
        /// Draws rectangle border using <see cref="Graphics.FillRectangle"/>.
        /// </summary>
        /// <param name="dc">Drawing context.</param>
        /// <param name="brush">Brush to draw border.</param>
        /// <param name="rect">Border rectangle.</param>
        /// <param name="borderWidth">Border width.</param>
        public static void FillRectangleBorder(
            this Graphics dc,
            Brush brush,
            RectD rect,
            double borderWidth = 1)
        {
            dc.FillRectangle(brush, GetTopLineRect(rect, borderWidth));
            dc.FillRectangle(brush, GetBottomLineRect(rect, borderWidth));
            dc.FillRectangle(brush, GetLeftLineRect(rect, borderWidth));
            dc.FillRectangle(brush, GetRightLineRect(rect, borderWidth));
        }

        /// <summary>
        /// Draws rectangle border using <see cref="Graphics.FillRectangle"/>.
        /// </summary>
        /// <param name="dc">Drawing context.</param>
        /// <param name="brush">Brush to draw border.</param>
        /// <param name="rect">Border rectangle.</param>
        /// <param name="borderWidth">Border width.</param>
        public static void FillRectangleBorder(
            this Graphics dc,
            Brush brush,
            RectD rect,
            Thickness borderWidth)
        {
            if(borderWidth.Top > 0)
                dc.FillRectangle(brush, GetTopLineRect(rect, borderWidth.Top));
            if (borderWidth.Bottom > 0)
                dc.FillRectangle(brush, GetBottomLineRect(rect, borderWidth.Bottom));
            if (borderWidth.Left > 0)
                dc.FillRectangle(brush, GetLeftLineRect(rect, borderWidth.Left));
            if (borderWidth.Right > 0)
                dc.FillRectangle(brush, GetRightLineRect(rect, borderWidth.Right));
        }

        /// <summary>
        /// Draws rectangles border using <see cref="Graphics.FillRectangle"/>.
        /// </summary>
        /// <param name="dc">Drawing context.</param>
        /// <param name="brush">Brush to draw border.</param>
        /// <param name="rects">Border rectangles.</param>
        /// <param name="borders">Border width.</param>
        public static void FillRectanglesBorder(
            this Graphics dc,
            Brush brush,
            RectD[] rects,
            Thickness[]? borders = null)
        {
            for (int i = 0; i < rects.Length; i++)
            {
                FillRectangleBorder(dc, brush, rects[i], borders?[i] ?? 1);
            }
        }

        /// <summary>
        /// Gets rectangle of the left border edge with the specified width.
        /// </summary>
        /// <param name="rect">Border rectangle.</param>
        /// <param name="width">Border side width.</param>
        public static RectD GetLeftLineRect(this RectD rect, double width)
        {
            var point = rect.TopLeft;
            var size = new SizeD(width, rect.Height);
            return new RectD(point, size);
        }

        /// <summary>
        /// Gets rectangle of the right border edge with the specified width.
        /// </summary>
        /// <param name="rect">Border rectangle.</param>
        /// <param name="width">Border side width.</param>
        public static RectD GetRightLineRect(this RectD rect, double width)
        {
            var point = new PointD(rect.Right - width, rect.Top);
            var size = new SizeD(width, rect.Height);
            return new RectD(point, size);
        }
    }
}