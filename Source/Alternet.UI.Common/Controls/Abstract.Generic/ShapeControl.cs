using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control that can display shapes.
    /// </summary>
    public partial class ShapeControl : GenericControl
    {
        private readonly ShapeDrawable drawable;
        private bool isFilled = true;
        private bool isStroked = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShapeControl"/> class.
        /// </summary>
        public ShapeControl()
        {
            drawable = CreateDrawable();
            drawable.ShapeType = DrawingShapeType.Rectangle;
        }

        /// <summary>
        /// Gets the <see cref="ShapeDrawable"/> that is used to draw shapes on the control.
        /// </summary>
        public ShapeDrawable Drawable => drawable;

        /// <summary>
        /// Gets or sets the shape to be displayed on the control.
        /// </summary>
        public DrawingShapeType ShapeType
        {
            get => drawable.ShapeType;
            set
            {
                if (ShapeType == value)
                    return;
                drawable.ShapeType = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the shape should be filled. Default is <c>true</c>.
        /// </summary>
        public bool IsFilled
        {
            get => isFilled;
            set
            {
                if (isFilled == value) return;
                isFilled = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the shape border should be drawn. Default is <c>true</c>.
        /// </summary>
        public bool IsStroked
        {
            get => isStroked;
            set
            {
                if (isStroked == value) return;
                isStroked = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the fill brush for the shape.
        /// </summary>
        public Brush? Fill
        {
            get => drawable.Brush;
            set
            {
                if (Fill == value)
                    return;
                drawable.Brush = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the stroke pen for the shape.
        /// </summary>
        public Pen? Stroke
        {
            get => drawable.Pen;
            set
            {
                if (Stroke == value)
                    return;
                drawable.Pen = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets effective stroke pen for the shape. If <see cref="Stroke"/> is null, returns a pen constructed
        /// from the <see cref="AbstractControl.ForeColor"/>. If <see cref="IsStroked"/> is false, returns null.
        /// </summary>
        /// <returns></returns>
        public virtual Pen? GetEffectiveStroke()
        {
            if (IsStroked)
            {
                return Stroke ?? ForeColor.AsPen;
            }

            return null;
        }

        /// <summary>
        /// Gets effective fill brush for the shape. If <see cref="Fill"/> is null, returns a brush constructed
        /// from the <see cref="AbstractControl.BackColor"/>. If <see cref="IsFilled"/> is false, returns null.
        /// </summary>
        /// <returns></returns>
        public virtual Brush? GetEffectiveFill()
        {
            if (IsFilled)
                return Fill ?? BackColor.AsBrush;
            return null;
        }

        /// <inheritdoc/>
        public override void DefaultPaint(PaintEventArgs e)
        {
            if (!drawable.Visible || drawable.ShapeType == DrawingShapeType.None)
                return;

            drawable.Bounds = e.ClipRectangle;

            if (drawable.Bounds.SizeIsEmpty)
                return;

            drawable.Pen = GetEffectiveStroke();
            drawable.Brush = GetEffectiveFill();

            drawable.Draw(this, e.Graphics);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ShapeDrawable"/> class that is used to draw shapes on the control.
        /// Override this method to provide a custom implementation of the <see cref="ShapeDrawable"/> class.
        /// </summary>
        /// <returns>A new instance of the <see cref="ShapeDrawable"/> class.</returns>
        protected virtual ShapeDrawable CreateDrawable()
        {
            return new ShapeDrawable();
        }
    }
}
