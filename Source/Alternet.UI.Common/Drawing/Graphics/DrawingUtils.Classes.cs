using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    public static partial class DrawingUtils
    {
        /// <summary>
        /// Represents the parameters required to fill a rectangle and optionally draw its border.
        /// This structure is used in
        /// <see cref="FillBorderRectangle(Graphics, ref DrawBorderParams)"/> and other methods.
        /// </summary>
        /// <remarks>This structure encapsulates all the necessary information for rendering
        /// a filled rectangle with an optional border. It includes the target graphics context,
        /// the rectangle dimensions, the fill brush, border settings, and an optional
        /// control that influences the border rendering.</remarks>
        public struct DrawBorderParams
        {
            /// <summary>
            /// Gets or sets the rectangle to fill and draw border.
            /// </summary>
            public RectD Rect;

            /// <summary>
            /// Gets or sets the brush to fill the rectangle.
            /// </summary>
            public Brush? Brush;

            /// <summary>
            /// Gets or sets the border settings.
            /// </summary>
            public BorderSettings? Border;

            /// <summary>
            /// gets or sets Whether border is painted.
            /// </summary>
            public bool HasBorder = true;

            /// <summary>
            /// Control in which border is painted. Optional.
            /// </summary>
            public AbstractControl? Control;

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
            /// Initializes a new instance of the <see cref="DrawBorderParams"/> struct.
            /// </summary>
            public DrawBorderParams(
                RectD rect,
                Brush? brush,
                BorderSettings? border,
                bool hasBorder = true,
                AbstractControl? control = null)
            {
                Rect = rect;
                Brush = brush;
                Border = border;
                HasBorder = hasBorder;
                Control = control;
            }

            /// <summary>
            /// Gets a value indicating whether the inner border is visible.
            /// </summary>
            /// <remarks>The inner border is only visible if the outer border is present and both the
            /// inner border visibility and presence are set to true.</remarks>
            public readonly bool InnerBorderVisible
            {
                get
                {
                    if (!HasBorder)
                        return false;
                    if (Border == null)
                        return false;
                    if (!Border.InnerBorderVisible)
                        return false;
                    if (!Border.HasInnerBorders)
                        return false;

                    return true;
                }
            }

            /// <summary>
            /// Gets the margin thickness between the inner edge of the border and the content of the control.
            /// </summary>
            /// <remarks>If the control does not have a border, this property returns <see cref="Thickness.Empty"/>.
            /// The value is determined by the associated border's inner margin settings.</remarks>
            public readonly Thickness InnerBorderMargin => Border?.InnerBorderMargin ?? Thickness.Empty;

            /// <summary>
            /// Gets the collection of inner border settings associated with the current border.
            /// Before accessing this property, ensure that the inner border is visible by checking
            /// the <see cref="InnerBorderVisible"/> property.
            /// </summary>
            /// <remarks>If the current border is not defined, this property returns an empty
            /// collection. This allows callers to safely enumerate the result without checking for null
            /// values.</remarks>
            public readonly IEnumerable<BorderSettings> InnerBorders => Border?.InnerBorders ?? Enumerable.Empty<BorderSettings>();

            /// <summary>
            /// Gets the effective border color, or null if the border is hidden.
            /// </summary>
            public readonly Color? GetEffectiveBorderColor()
            {
                if (!HasBorder)
                    return null;

                var borderColor = Border?.Color ?? ColorUtils.GetDefaultBorderColor(Control);

                if (borderColor.IsEmptyOrTransparent)
                    return null;

                return borderColor;
            }

            /// <summary>
            /// Gets the effective border pen, or null if the border is hidden.
            /// </summary>
            /// <returns></returns>
            public readonly Pen? GetEffectiveBorderPen()
            {
                var pen = Border?.GetPen();

                if (pen is not null)
                    return pen;

                var color = GetEffectiveBorderColor();
                if (color == null)
                    return null;
                return color.AsPen;
            }
        }
    }
}
