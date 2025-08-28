using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Implements background and border painting.
    /// Additionally it can paint image inside the rectangle.
    /// </summary>
    public class RectangleDrawable : ImageDrawable
    {
        /// <summary>
        /// Gets or sets brush to fill the rectangle.
        /// </summary>
        public Brush? Brush;

        /// <summary>
        /// Gets or sets border settings.
        /// </summary>
        public BorderSettings? Border;

        /// <summary>
        /// Gets or sets whether border is painted. Default is <c>true</c>.
        /// </summary>
        public bool HasBorder = true;

        /// <summary>
        /// Gets or sets whether image is painted. Default is <c>false</c>.
        /// </summary>
        public bool HasImage = false;

        /// <summary>
        /// Gets or sets a value indicating whether border and/or background should be painted
        /// using rounded corners.
        /// </summary>
        public bool UseRoundCorners;

        /// <summary>
        /// Gets or sets the corner radius.
        /// This value is used when <see cref="UseRoundCorners"/>
        /// is set to <see langword="true"/>.
        /// </summary>
        public Coord CornerRadius;

        /// <summary>
        /// Indicates whether the corner radius is specified as a percentage of the element's size.
        /// </summary>
        public bool CornerRadiusIsPercent;

        /// <summary>
        /// Gets or sets a value indicating whether the corner settings specified in
        /// <see cref="Border"/> should be overridden.
        /// </summary>
        public bool OverrideBorderCornerSettings;

        /// <summary>
        /// Returns this object if it is visible; otherwise returns <c>null</c>.
        /// </summary>
        public RectangleDrawable? OnlyVisible
        {
            get
            {
                if (Visible)
                    return this;
                return null;
            }
        }

        /// <summary>
        /// Performs default background and border painting.
        /// </summary>
        /// <param name="control">Control in which this object is painted.</param>
        /// <param name="dc">Drawing context.</param>
        public virtual void DefaultDrawBackAndBorder(AbstractControl control, Graphics dc)
        {
            if (!Visible)
                return;

            var prm = new DrawingUtils.DrawBorderParams(
                Bounds,
                Brush,
                Border,
                HasBorder,
                control);

            prm.UseRoundCorners = UseRoundCorners;
            prm.CornerRadius = CornerRadius;
            prm.CornerRadiusIsPercent = CornerRadiusIsPercent;
            prm.OverrideBorderCornerSettings = OverrideBorderCornerSettings;

            DrawingUtils.FillBorderRectangle(dc, ref prm);
        }

        /// <inheritdoc/>
        protected override void OnDraw(AbstractControl control, Graphics dc)
        {
            if (!Visible || Bounds.SizeIsEmpty)
                return;
            DefaultDrawBackAndBorder(control, dc);
            if(HasImage)
                DefaultDrawImage(control, dc);
        }
    }
}