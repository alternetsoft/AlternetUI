using System;

using Alternet.UI;

using SkiaSharp;

namespace Alternet.Drawing
{
    /// <summary>
    /// Defines a brush with a hatch style and a color.
    /// </summary>
    /// <remarks>
    /// A hatch pattern is made from the lines of a color defined by the <see cref="Color"/> property.
    /// The <see cref="HatchStyle"/> property defines what type of pattern the brush has and can
    /// be any value from the <see cref="BrushHatchStyle"/> enumeration.
    /// </remarks>
    public partial class HatchBrush : Brush
    {
        private Color? backgroundColor;
        private Color? color;
        private BrushHatchStyle hatchStyle;
        private int tileSize = 8;
        private float strokeWidth = 1.0f;
        private SKShaderTileMode tileModeX = SKShaderTileMode.Repeat;
        private SKShaderTileMode tileModeY = SKShaderTileMode.Repeat;

        /// <summary>
        /// Initializes a new instance of the <see cref="HatchBrush"/> class with the specified
        /// <see cref="BrushHatchStyle"/> enumeration, and the color.
        /// </summary>
        /// <param name="hatchStyle">One of the <see cref="BrushHatchStyle"/> values that
        /// represents the pattern drawn by this <see cref="HatchBrush"/>.</param>
        /// <param name="color">The <see cref="Drawing.Color"/> structure that represents the
        /// color of lines drawn by this <see cref="HatchBrush"/>.</param>
        public HatchBrush(BrushHatchStyle hatchStyle, Color color)
            : base(immutable: false)
        {
            HatchStyle = hatchStyle;
            Color = color;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HatchBrush"/> class with the specified
        /// <see cref="BrushHatchStyle"/> enumeration, and the color.
        /// </summary>
        /// <param name="hatchStyle">One of the <see cref="BrushHatchStyle"/> values that
        /// represents the pattern drawn by this <see cref="HatchBrush"/>.</param>
        /// <param name="foreColor">The <see cref="Drawing.Color"/> structure that represents the
        /// color of lines drawn by this <see cref="HatchBrush"/>.</param>
        /// <param name="backColor">The <see cref="Drawing.Color"/> structure that represents the
        /// background color of this <see cref="HatchBrush"/>.</param>
        public HatchBrush(BrushHatchStyle hatchStyle, Color foreColor, Color backColor)
            : base(immutable: false)
        {
            HatchStyle = hatchStyle;
            Color = foreColor;
            BackgroundColor = backColor;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HatchBrush"/> class with the specified
        /// <see cref="HatchStyle"/> enumeration, and the color.
        /// </summary>
        /// <param name="hatchStyle">One of the <see cref="HatchStyle"/> values that
        /// represents the pattern drawn by this <see cref="HatchBrush"/>.</param>
        /// <param name="foreColor">The <see cref="Drawing.Color"/> structure that represents the
        /// color of lines drawn by this <see cref="HatchBrush"/>.</param>
        /// <param name="backColor">The <see cref="Drawing.Color"/> structure that represents the
        /// background color of this <see cref="HatchBrush"/>.</param>
        public HatchBrush(HatchStyle hatchStyle, Color foreColor, Color backColor)
            : this((BrushHatchStyle)hatchStyle, foreColor, backColor)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HatchBrush"/> class with the specified
        /// <see cref="HatchStyle"/> enumeration, and the color.
        /// </summary>
        /// <param name="hatchStyle">One of the <see cref="HatchStyle"/> values that
        /// represents the pattern drawn by this <see cref="HatchBrush"/>.</param>
        /// <param name="color">The <see cref="Drawing.Color"/> structure that represents the
        /// color of lines drawn by this <see cref="HatchBrush"/>.</param>
        public HatchBrush(HatchStyle hatchStyle, Color color)
            : this((BrushHatchStyle)hatchStyle, color)
        {
        }

        /// <summary>
        /// Gets the color of hatch lines drawn by this <see cref="HatchBrush"/> object.
        /// </summary>
        /// <value>A <see cref="Drawing.Color"/> structure that represents the color for this
        /// <see cref="HatchBrush"/>.</value>
        public virtual Color Color
        {
            get
            {
                return color ??= Color.Black;
            }

            set
            {
                if (color == value)
                    return;
                color = value;
                UpdateRequired();
            }
        }

        /// <summary>
        /// Gets or sets the horizontal tiling mode used when repeating
        /// the hatch pattern along the X axis.
        /// </summary>
        /// <remarks>
        /// This corresponds to <see cref="SKShaderTileMode"/> in SkiaSharp.
        /// Typical values are <see cref="SKShaderTileMode.Repeat"/> (default),
        /// <see cref="SKShaderTileMode.Clamp"/>, or <see cref="SKShaderTileMode.Mirror"/>.
        /// </remarks>
        public virtual SKShaderTileMode TileModeX
        {
            get => tileModeX;
            set
            {
                if (tileModeX == value) return;
                tileModeX = value;
                UpdateRequired();
            }
        }

        /// <summary>
        /// Gets or sets the vertical tiling mode used when repeating
        /// the hatch pattern along the Y axis.
        /// </summary>
        /// <remarks>
        /// This corresponds to <see cref="SKShaderTileMode"/> in SkiaSharp.
        /// Typical values are <see cref="SKShaderTileMode.Repeat"/> (default),
        /// <see cref="SKShaderTileMode.Clamp"/>, or <see cref="SKShaderTileMode.Mirror"/>.
        /// </remarks>
        public virtual SKShaderTileMode TileModeY
        {
            get => tileModeY;
            set
            {
                if (tileModeY == value) return;
                tileModeY = value;
                UpdateRequired();
            }
        }

        /// <summary>
        /// Gets or sets the width of the hatch lines.
        /// Default is 1.0f.
        /// </summary>
        public virtual float StrokeWidth
        {
            get
            {
                return strokeWidth;
            }

            set
            {
                if (strokeWidth == value)
                    return;
                strokeWidth = value;
                UpdateRequired();
            }
        }

        /// <summary>
        /// Gets or sets size of the hatch pattern tile.
        /// This is used when creating a hatch pattern to determine the width and height of the pattern bitmap.
        /// Default is 8.
        /// </summary>
        public virtual int TileSize
        {
            get
            {
                return tileSize;
            }

            set
            {
                if (tileSize == value)
                    return;
                tileSize = value;
                UpdateRequired();
            }
        }

        /// <summary>
        /// Gets or sets the background color of this <see cref="HatchBrush"/> object.
        /// Default is transparent.
        /// </summary>
        public virtual Color BackgroundColor
        {
            get
            {
                return backgroundColor ??= Color.Transparent;
            }

            set
            {
                if (backgroundColor == value)
                    return;
                backgroundColor = value;
                UpdateRequired();
            }
        }

        /// <summary>
        /// Gets the hatch style of this <see cref="HatchBrush"/> object.
        /// </summary>
        /// <value>One of the <see cref="BrushHatchStyle"/> values that represents the pattern of
        /// this <see cref="HatchBrush"/>.</value>
        public virtual BrushHatchStyle HatchStyle
        {
            get
            {
                return hatchStyle;
            }

            set
            {
                if (hatchStyle == value)
                    return;
                hatchStyle = value;
                UpdateRequired();
            }
        }

        /// <inheritdoc/>
        public override BrushType BrushType => BrushType.Hatch;

        /// <inheritdoc/>
        public override Color AsColor => this.Color;

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"HatchBrush ({HatchStyle}, {Color}, {BackgroundColor})";
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
            => (HatchStyle, Color, BackgroundColor, TileSize, StrokeWidth, TileModeX, TileModeY).GetHashCode();

        /// <summary>
        /// Sets background color of the hatch pattern.
        /// </summary>
        /// <param name="color">The background color to set.</param>
        /// <returns>The current <see cref="HatchBrush"/> instance.</returns>
        public HatchBrush SetBackgroundColor(Color color)
        {
            BackgroundColor = color;
            return this;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        public override bool Equals(object? other)
        {
            var o = other as HatchBrush;
            if (o == null)
                return false;

            return Color == o.Color && HatchStyle == o.HatchStyle && BackgroundColor == o.BackgroundColor
                && TileSize == o.TileSize && StrokeWidth == o.StrokeWidth
                && TileModeX == o.TileModeX && TileModeY == o.TileModeY;
        }

        /// <inheritdoc/>
        protected override SKPaint CreateSkiaPaint()
        {
            var result = base.CreateSkiaPaint();
            result.Shader = CreateSkiaShader();
            return result;
        }

        /// <summary>
        /// Creates <see cref="SKShader"/> for this hatch brush.
        /// </summary>
        /// <returns>The <see cref="SKShader"/> instance for this hatch brush.</returns>
        protected virtual SKShader CreateSkiaShader()
        {
            var info = new SKImageInfo(tileSize, tileSize);

            using var surface = SKSurface.Create(info);
            var canvas = surface.Canvas;

            canvas.Clear(BackgroundColor.SkiaColor);

            using var linePaint = new SKPaint
            {
                Color = this.Color.SkiaColor,
                StrokeWidth = this.StrokeWidth,
                IsAntialias = true,
            };

            switch (HatchStyle)
            {
                case BrushHatchStyle.Horizontal:
                    canvas.DrawLine(0, tileSize / 2, tileSize, tileSize / 2, linePaint);
                    break;

                case BrushHatchStyle.Vertical:
                    canvas.DrawLine(tileSize / 2, 0, tileSize / 2, tileSize, linePaint);
                    break;

                case BrushHatchStyle.DiagonalCross:
                    canvas.DrawLine(0, tileSize, tileSize, 0, linePaint);
                    canvas.DrawLine(0, 0, tileSize, tileSize, linePaint);
                    break;

                case BrushHatchStyle.Cross:
                    canvas.DrawLine(0, 0, tileSize, 0, linePaint); // top edge
                    canvas.DrawLine(0, 0, 0, tileSize, linePaint); // left edge
                    break;                
                case BrushHatchStyle.ForwardDiagonal:
                    canvas.DrawLine(0, tileSize, tileSize, 0, linePaint);
                    break;

                case BrushHatchStyle.BackwardDiagonal:
                    canvas.DrawLine(0, 0, tileSize, tileSize, linePaint);
                    break;
                default:
                    canvas.DrawLine(0, tileSize, tileSize, 0, linePaint);
                    break;
            }

            var tile = surface.Snapshot();
            var shader = SKShader.CreateImage(
                tile,
                TileModeX,
                TileModeY);
            return shader;
        }
    }
}

