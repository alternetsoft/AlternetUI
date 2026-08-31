using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    public partial class Graphics
    {
        /// <summary>
        /// Represents the parameters required to draw a line with two colors or brushes.
        /// </summary>
        public struct DualColorLineParams
        {
            /// <summary>
            /// Gets or sets the starting point of the line.
            /// </summary>
            public PointD StartPoint;

            /// <summary>
            /// Gets or sets the length of the line.
            /// </summary>
            public Coord Length;

            /// <summary>
            /// Gets or sets the width of the line.
            /// </summary>
            public Coord Width;

            /// <summary>
            /// Gets or sets the color of the first segment of the line.
            /// </summary>
            public Color? FirstColor;
            
            /// <summary>
            /// Gets or sets the color of the second segment of the line.
            /// </summary>
            public Color? SecondColor;

            /// <summary>
            /// Gets or sets the brush of the first segment of the line.
            /// </summary>
            public Brush? FirstBrush;

            /// <summary>
            /// Gets or sets the brush of the second segment of the line.
            /// </summary>
            public Brush? SecondBrush;

            /// <summary>
            /// Gets or sets the size of the first segment of the line.
            /// </summary>
            public Coord FirstSize;             

            /// <summary>
            /// Gets or sets the size of the second segment of the line.
            /// </summary>
            public Coord SecondSize;

            /// <summary>
            /// Gets or sets a value indicating whether the line is vertical or horizontal.
            /// </summary>
            public bool IsVertical;
        }

        /// <summary>
        /// Represents the parameters required to create a measure canvas.
        /// </summary>
        /// <remarks>This structure is used to encapsulate the configuration options or data necessary
        /// for initializing a measure canvas. The specific parameters should
        /// be defined within this structure to
        /// ensure clarity and maintainability.</remarks>
        public struct CanvasCreateParams : IEquatable<CanvasCreateParams>
        {
            private Coord scaleFactor;
            private ControlRenderingFlags controlRenderingFlags;
            private int? hashCode;

            /// <summary>
            /// Initializes a new instance of the <see cref="CanvasCreateParams"/> class.
            /// </summary>
            public CanvasCreateParams()
                : this(null)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="CanvasCreateParams"/>
            /// class with the specified scale factor.
            /// </summary>
            /// <param name="scaleFactor">The scale factor to be applied to the canvas measurements.
            /// If <see langword="null"/>, a default scale factor will be used.</param>
            public CanvasCreateParams(Coord? scaleFactor)
            {
                this.scaleFactor = GraphicsFactory.ScaleFactorOrDefault(scaleFactor);
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="CanvasCreateParams"/>
            /// class with the specified scale factor and control rendering flags.
            /// </summary>
            /// <param name="scaleFactor">An optional scaling factor to be applied to the canvas.
            /// If <see langword="null"/>, no scaling is applied.</param>
            /// <param name="controlRenderingFlags">Flags that specify how controls should
            /// be rendered on the canvas.</param>
            public CanvasCreateParams(Coord? scaleFactor, ControlRenderingFlags controlRenderingFlags)
                : this(scaleFactor)
            {
                this.controlRenderingFlags = controlRenderingFlags;
            }

            /// <summary>
            /// Specifies the rendering options for a control.
            /// </summary>
            /// <remarks>This field defines the flags that determine how a control is rendered.
            /// The value is typically a combination of flags from an enumeration,
            /// allowing for fine-grained control over rendering behavior.</remarks>
            public ControlRenderingFlags ControlRenderingFlags
            {
                readonly get => controlRenderingFlags;

                set
                {
                    controlRenderingFlags = value;
                    hashCode = null;
                }
            }

            /// <summary>
            /// Represents a scaling factor for a coordinate system.
            /// </summary>
            /// <remarks>This field can be used to adjust the scale of a coordinate system or
            /// transform. Ensure that the value is appropriately set to avoid
            /// unintended transformations.</remarks>
            public Coord ScaleFactor
            {
                readonly get => scaleFactor;

                set
                {
                    scaleFactor = value;
                    hashCode = null;
                }
            }

            /// <summary>
            /// Determines whether two specified CanvasCreateParams instances are equal.
            /// </summary>
            /// <param name="left">The first CanvasCreateParams instance to compare.</param>
            /// <param name="right">The second CanvasCreateParams instance to compare.</param>
            /// <returns>true if the two CanvasCreateParams instances are equal; otherwise, false.</returns>
            public static bool operator ==(CanvasCreateParams left, CanvasCreateParams right)
            {
                return left.Equals(right);
            }

            /// <summary>
            /// Determines whether two CanvasCreateParams instances are not equal.
            /// </summary>
            /// <param name="left">The first CanvasCreateParams instance to compare.</param>
            /// <param name="right">The second CanvasCreateParams instance to compare.</param>
            /// <returns>true if the specified instances are not equal; otherwise, false.</returns>
            public static bool operator !=(CanvasCreateParams left, CanvasCreateParams right)
            {
                return !(left == right);
            }

            /// <summary>
            /// Gets the graphics backend type used for rendering.
            /// </summary>
            public readonly GraphicsBackendType GraphicsBackendType
            {
                get
                {
                    if (ControlRenderingFlags.HasFlag(ControlRenderingFlags.UseSkiaSharp))
                        return GraphicsBackendType.SkiaSharp;
                    else
                        return GraphicsBackendType.WxWidgets;
                }
            }

            /// <inheritdoc/>
            public override int GetHashCode()
            {
                return hashCode ??= (ScaleFactor, controlRenderingFlags).GetHashCode();
            }

            /// <inheritdoc/>
            public readonly override string ToString()
            {
                return $"ScaleFactor: {ScaleFactor}, ControlRenderingFlags: {ControlRenderingFlags}";
            }

            /// <inheritdoc/>
            public readonly override bool Equals(object? obj)
            {
                return obj is CanvasCreateParams other && Equals(other);
            }

            /// <summary>
            /// Determines whether the current instance is equal to another instance
            /// of <see cref="CanvasCreateParams"/>.
            /// </summary>
            /// <param name="other">The <see cref="CanvasCreateParams"/> instance to compare
            /// with the current instance.</param>
            /// <returns><see langword="true"/> if the current instance is equal
            /// to the <paramref name="other"/> instance; otherwise, <see langword="false"/>.</returns>
            public readonly bool Equals(CanvasCreateParams other)
            {
                return ScaleFactor == other.ScaleFactor &&
                    controlRenderingFlags == other.controlRenderingFlags;
            }
        }
    }
}
