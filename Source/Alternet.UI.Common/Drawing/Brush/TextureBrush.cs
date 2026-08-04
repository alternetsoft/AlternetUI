using System;

using Alternet.UI;

using SkiaSharp;

namespace Alternet.Drawing
{
    /// <summary>
    /// Defines a brush of a single color. Brushes are used to fill graphics shapes, such
    /// as rectangles, ellipses, pies, polygons, and paths.
    /// </summary>
    public class TextureBrush : Brush
    {
        private Image image;
        private WrapMode wrapMode = WrapMode.Tile;
        private SKMatrix localMatrix = SKMatrix.CreateIdentity();
        private SKShaderTileMode tileModeX = SKShaderTileMode.Repeat;
        private SKShaderTileMode tileModeY = SKShaderTileMode.Repeat;
        private bool useWrapMode = true;

        /// <summary>
        /// Initializes a new <see cref="TextureBrush"/> object of the specified color.
        /// </summary>
        /// <param name="image">An <see cref="Image"/> that represents the texture of
        /// this brush.</param>
        public TextureBrush(Image image)
            : base(false)
        {
            this.image = image;
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
        public SKShaderTileMode TileModeX
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
        public SKShaderTileMode TileModeY
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
        /// Gets or sets a value indication what kind of wrap mode to use.
        /// Legacy applications may use <see cref="WrapMode"/> to specify the tiling mode.
        /// New applications should use <see cref="TileModeX"/> and <see cref="TileModeY"/> instead
        /// as they provide more flexibility and better control over the tiling behavior.
        /// Default is true, which means that <see cref="WrapMode"/> is used to determine the tiling mode.
        /// </summary>
        public virtual bool UseWrapMode
        {
            get => useWrapMode;
            set
            {
                if (useWrapMode == value) return;
                useWrapMode = value;
                UpdateRequired();
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="WrapMode"/> for this <see cref="TextureBrush"/>.
        /// </summary>
        public virtual WrapMode WrapMode
        {
            get => wrapMode;
            set
            {
                if (wrapMode == value) return;
                wrapMode = value;
                UpdateRequired();
            }
        }

        /// <summary>
        /// Gets or sets <see cref="SKMatrix"/> for this brush.
        /// </summary>
        public virtual SKMatrix LocalMatrix
        {
            get
            {
                return localMatrix;
            }

            set
            {
                if (localMatrix == value)
                    return;
                localMatrix = value;
                UpdateRequired();
            }
        }

        /// <summary>
        /// Gets the texture of this brush object.
        /// </summary>
        /// <value>An <see cref="Image"/> structure that represents the
        /// texture of this brush.</value>
        public virtual Image Image
        {
            get => image;

            set
            {
                if (image == value || Immutable)
                    return;
                image = value;
                UpdateRequired();
            }
        }

        /// <inheritdoc/>
        public override BrushType BrushType => BrushType.Texture;

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"TextureBrush";
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() => (
            Image.GetHashCode(),
            WrapMode,
            UseWrapMode,
            LocalMatrix.GetHashCode(),
            TileModeX,
            TileModeY).GetHashCode();

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        public override bool Equals(object? other)
        {
            var o = other as TextureBrush;
            if (o == null)
                return false;
            return WrapMode == o.WrapMode && UseWrapMode == o.UseWrapMode && TileModeX == o.TileModeX
                && TileModeY == o.TileModeY && LocalMatrix.Equals(o.LocalMatrix) && Image.Equals(o.Image);
        }

        /// <inheritdoc/>
        protected override SKPaint CreateSkiaPaint()
        {
            var result = base.CreateSkiaPaint();
            result.Shader = CreateSkiaShader();
            return result;
        }

        /// <summary>
        /// Creates <see cref="SKShader"/> for this brush.
        /// </summary>
        /// <returns>The <see cref="SKShader"/> instance for this brush.</returns>
        protected virtual SKShader? CreateSkiaShader()
        {
            if (Image == null)
                return null;

            var mx = tileModeX;
            var my = tileModeY;

            if (UseWrapMode)
                SkiaUtils.Convert(WrapMode, out mx, out my);

            var shader = SKShader.CreateBitmap((SKBitmap)image, mx, my, LocalMatrix);
            return shader;
        }
    }
}