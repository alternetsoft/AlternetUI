using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Contains methods and properties which allow to paint on the drawing context.
    /// </summary>
    public interface IGraphics
    {
        /// <inheritdoc cref="Graphics.IsOk"/>
        bool IsOk { get; }

        /// <inheritdoc cref="Graphics.Name"/>
        string? Name { get; set; }

        /// <inheritdoc cref="Graphics.Transform"/>
        TransformMatrix Transform { get; set; }

        /// <inheritdoc cref="Graphics.InterpolationMode"/>
        InterpolationMode InterpolationMode { get; set; }

        /// <inheritdoc cref="Graphics.NativeObject"/>
        object NativeObject { get; }

        /// <inheritdoc cref="Graphics.RoundedRectangle"/>
        void RoundedRectangle(
            Pen pen,
            Brush brush,
            RectD rectangle,
            Coord cornerRadius);

        /// <inheritdoc cref="Graphics.GetTextExtent(string, Font)"/>
        SizeD GetTextExtent(string text, Font font);

        /// <inheritdoc cref="Graphics.Rectangle"/>
        void Rectangle(Pen pen, Brush brush, RectD rectangle);

        /// <inheritdoc cref="Graphics.Ellipse"/>
        void Ellipse(Pen pen, Brush brush, RectD rectangle);

        /// <inheritdoc cref="Graphics.Path"/>
        void Path(Pen pen, Brush brush, GraphicsPath path);

        /// <inheritdoc cref="Graphics.Pie"/>
        void Pie(
            Pen pen,
            Brush brush,
            PointD center,
            Coord radius,
            Coord startAngle,
            Coord sweepAngle);

        /// <inheritdoc cref="Graphics.Circle"/>
        void Circle(Pen pen, Brush brush, PointD center, Coord radius);

        /// <inheritdoc cref="Graphics.Polygon"/>
        void Polygon(Pen pen, Brush brush, ReadOnlySpan<PointD> points, FillMode fillMode);

        /// <inheritdoc cref="Graphics.FillRectangle(Brush, RectD)"/>
        void FillRectangle(Brush brush, RectD rectangle);

        /// <inheritdoc cref="Graphics.DrawArc"/>
        void DrawArc(
            Pen pen,
            PointD center,
            Coord radius,
            Coord startAngle,
            Coord sweepAngle);

        /// <inheritdoc cref="Graphics.FillPie"/>
        void FillPie(
            Brush brush,
            PointD center,
            Coord radius,
            Coord startAngle,
            Coord sweepAngle);

        /// <inheritdoc cref="Graphics.DrawPie"/>
        void DrawPie(
            Pen pen,
            PointD center,
            Coord radius,
            Coord startAngle,
            Coord sweepAngle);

        /// <inheritdoc cref="Graphics.DrawBezier"/>
        void DrawBezier(
            Pen pen,
            PointD startPoint,
            PointD controlPoint1,
            PointD controlPoint2,
            PointD endPoint);

        /// <inheritdoc cref="Graphics.DrawBeziers"/>
        void DrawBeziers(Pen pen, ReadOnlySpan<PointD> points);

        /// <inheritdoc cref="Graphics.DrawCircle"/>
        void DrawCircle(Pen pen, PointD center, Coord radius);

        /// <inheritdoc cref="Graphics.FillCircle"/>
        void FillCircle(Brush brush, PointD center, Coord radius);

        /// <inheritdoc cref="Graphics.DrawRoundedRectangle"/>
        void DrawRoundedRectangle(Pen pen, RectD rect, Coord cornerRadius);

        /// <inheritdoc cref="Graphics.FillRoundedRectangle"/>
        void FillRoundedRectangle(Brush brush, RectD rect, Coord cornerRadius);

        /// <inheritdoc cref="Graphics.DrawPolygon"/>
        void DrawPolygon(Pen pen, ReadOnlySpan<PointD> points);

        /// <inheritdoc cref="Graphics.FillPolygon"/>
        void FillPolygon(
            Brush brush,
            ReadOnlySpan<PointD> points,
            FillMode fillMode = FillMode.Alternate);

        /// <inheritdoc cref="Graphics.DrawRectangles"/>
        void DrawRectangles(Pen pen, ReadOnlySpan<RectD> rects);

        /// <inheritdoc cref="Graphics.FillRectangles"/>
        void FillRectangles(Brush brush, ReadOnlySpan<RectD> rects);

        /// <inheritdoc cref="Graphics.FillEllipse"/>
        void FillEllipse(Brush brush, RectD bounds);

        /// <inheritdoc cref="Graphics.DrawRectangle"/>
        void DrawRectangle(Pen pen, RectD rectangle);

        /// <inheritdoc cref="Graphics.DrawPath"/>
        void DrawPath(Pen pen, GraphicsPath path);

        /// <inheritdoc cref="Graphics.FillPath"/>
        void FillPath(Brush brush, GraphicsPath path);

        /// <inheritdoc cref="Graphics.DrawLine(Pen, PointD, PointD)"/>
        void DrawLine(Pen pen, PointD a, PointD b);

        /// <inheritdoc cref="Graphics.DrawLine(Pen, Coord, Coord, Coord, Coord)"/>
        void DrawLine(Pen pen, Coord x1, Coord y1, Coord x2, Coord y2);

        /// <inheritdoc cref="Graphics.DrawEllipse"/>
        void DrawEllipse(Pen pen, RectD bounds);

        /// <inheritdoc cref="Graphics.DrawImageUnscaled"/>
        void DrawImageUnscaled(Image image, PointD origin);

        /// <inheritdoc cref="Graphics.DrawImage(Image, PointD)"/>
        void DrawImage(Image image, PointD origin);

        /// <inheritdoc cref="Graphics.DrawImage(Image, RectD)"/>
        void DrawImage(Image image, RectD destinationRect);

        /// <inheritdoc cref="Graphics.DrawText(string, Font, Brush, PointD)"/>
        void DrawText(string text, Font font, Brush brush, PointD origin);

        /// <inheritdoc cref="Graphics.DrawText(string, PointD)"/>
        void DrawText(string text, PointD origin);

        /// <inheritdoc cref="Graphics.DrawText(string, Font, Brush, RectD)"/>
        void DrawText(string text, Font font, Brush brush, RectD bounds);

        /// <inheritdoc cref="Graphics.DrawWave(RectD, Color)"/>
        void DrawWave(RectD rect, Color color);

        /// <inheritdoc cref="Graphics.MeasureText(string, Font)"/>
        SizeD MeasureText(string text, Font font);

        /// <inheritdoc cref="Graphics.PopTransform"/>
        void PopTransform();

        /// <inheritdoc cref="Graphics.PushTransform(TransformMatrix)"/>
        void PushTransform(TransformMatrix transform);

        /// <inheritdoc cref="Graphics.PushTransform()"/>
        void PushTransform();

        /// <inheritdoc cref="Graphics.Save()"/>
        void Save();

        /// <inheritdoc cref="Graphics.Restore()"/>
        void Restore();

        /// <inheritdoc cref="Graphics. DrawText(string,PointD,Font,Color,Color)"/>
        void DrawText(
            string text,
            PointD location,
            Font font,
            Color foreColor,
            Color backColor);

        /// <inheritdoc cref="Graphics.DrawLabel(string, Font, Color, Color, Image?, RectD, HVAlignment?, int)"/>
        RectD DrawLabel(
            string text,
            Font font,
            Color foreColor,
            Color backColor,
            Image? image,
            RectD rect,
            HVAlignment? alignment = null,
            int indexAccel = -1);

        /// <inheritdoc cref="Graphics.GetDPI"/>
        SizeI GetDPI();
    }
}
