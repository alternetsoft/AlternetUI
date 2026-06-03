using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Implements shapes painting. Use <see cref="ShapeType"/> property to specify the shape type.
    /// </summary>
    public partial class ShapeDrawable : BaseDrawable
    {
        /// <summary>
        /// Gets or sets the type of shape to be drawn.
        /// </summary>
        public DrawingShapeType ShapeType { get; set; } = DrawingShapeType.None;

        /// <summary>
        /// Gets or sets the corner radius for rounded rectangles.
        /// </summary>
        public Coord CornerRadius { get; set; } = 5f;

        /// <summary>
        /// Gets or sets the brush used to fill the shape.
        /// </summary>
        public Brush? Brush { get; set; }

        /// <summary>
        /// Gets or sets the pen used to draw the shape outline.
        /// </summary>
        public Pen? Pen { get; set; }

        /// <summary>
        /// Gets or sets the start angle for arc shapes, in degrees.
        /// </summary>
        public Coord StartAngle { get; set; } = 0f;

        /// <summary>
        /// Gets or sets the sweep angle for arc shapes, in degrees.
        /// </summary>
        public Coord SweepAngle { get; set; } = 90f;

        /// <inheritdoc/>
        protected override void OnDraw(AbstractControl control, Graphics dc)
        {
            if (!Visible || Bounds.SizeIsEmpty)
                return;

            switch (ShapeType)
            {
                case DrawingShapeType.Ellipse:
                    dc.FillOrDrawEllipse(Pen, Brush, Bounds);
                    break;
                case DrawingShapeType.Rectangle:
                    dc.FillOrDrawRectangle(Pen, Brush, Bounds);
                    break;
                case DrawingShapeType.RoundedRectangle:
                    dc.FillOrDrawRoundedRectangle(Pen, Brush, Bounds, CornerRadius);
                    break;
                case DrawingShapeType.Pie:
                    dc.FillOrDrawPie(Pen, Brush, Bounds.Center, Bounds.CircleRadius, StartAngle, SweepAngle);
                    break;
                case DrawingShapeType.Circle:
                    dc.FillOrDrawCircle(Pen, Brush, Bounds.Center, Bounds.CircleRadius);
                    break;
                case DrawingShapeType.Arc:
                    if(Pen != null)
                        dc.DrawArc(Pen, Bounds.Center, Bounds.CircleRadius, StartAngle, SweepAngle);
                    break;
            }
        }
    }
}
