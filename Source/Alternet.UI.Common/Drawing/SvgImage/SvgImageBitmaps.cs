using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Defines an interface for providing bitmap representations of SVG images.
    /// </summary>
    /// <typeparam name="TBitmap">The type of the bitmap.</typeparam>
    public interface ISvgImageBitmapsProvider<TBitmap>
    {
        /// <summary>
        /// Gets the size of the specified bitmap.
        /// </summary>
        /// <param name="bitmap">The bitmap.</param>
        /// <returns>The size of the bitmap.</returns>
        SizeI GetBitmapSize(TBitmap bitmap);

        /// <summary>
        /// Creates a bitmap representation of the specified SVG image
        /// with the given width, height, and optional color.
        /// </summary>
        /// <param name="svg">The SVG image.</param>
        /// <param name="width">The width of the bitmap.</param>
        /// <param name="height">The height of the bitmap.</param>
        /// <param name="color">The optional color to apply to the bitmap.</param>
        /// <returns>The created bitmap.</returns>
        TBitmap ToBitmap(SvgImage svg, int width, int height, Color? color = null);

        /// <summary>
        /// Creates a disabled bitmap representation of the specified SVG image
        /// with the given width, height, and dark mode option.
        /// </summary>
        /// <param name="svg">The SVG image.</param>
        /// <param name="width">The width of the bitmap.</param>
        /// <param name="height">The height of the bitmap.</param>
        /// <param name="isDark">Indicates whether the bitmap is for dark mode.</param>
        /// <returns>The created disabled bitmap.</returns>
        TBitmap ToDisabledBitmap(SvgImage svg, int width, int height, bool isDark);

        /// <summary>
        /// Creates a normal bitmap representation of the specified SVG image
        /// with the given width, height, and dark mode option.
        /// </summary>
        /// <param name="svg">The SVG image.</param>
        /// <param name="width">The width of the bitmap.</param>
        /// <param name="height">The height of the bitmap.</param>
        /// <param name="isDark">Indicates whether the bitmap is for dark mode.</param>
        /// <returns>The created normal bitmap.</returns>
        TBitmap ToNormalBitmap(SvgImage svg, int width, int height, bool isDark);
    }

    /// <summary>
    /// Represents a collection of bitmap representations of an SVG image.
    /// </summary>
    public struct SvgImageBitmaps<TBitmap>
    {
        private readonly ISvgImageBitmapsProvider<TBitmap> provider;
        private BaseDictionary<SizeAndBool, TBitmap>? normalBitmaps;
        private BaseDictionary<SizeAndBool, TBitmap>? disabledBitmaps;
        private BaseDictionary<SizeAndColor, TBitmap>? bitmaps;

        private SvgImage? svgImage;

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgImageBitmaps{TBitmap}"/> class.
        /// </summary>
        /// <param name="provider">The provider for creating bitmap representations of the SVG image.</param>
        public SvgImageBitmaps(ISvgImageBitmapsProvider<TBitmap> provider)
        {
            this.provider = provider;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgImageBitmaps{TBitmap}"/>
        /// class with the specified SVG image.
        /// </summary>
        /// <param name="provider">The provider for creating bitmap representations of the SVG image.</param>
        /// <param name="svgImage">The SVG image.</param>
        public SvgImageBitmaps(ISvgImageBitmapsProvider<TBitmap> provider, SvgImage svgImage)
            : this(provider)
        {
            this.svgImage = svgImage;
        }

        /// <summary>
        /// Gets or sets the SVG image.
        /// </summary>
        public SvgImage? SvgImage
        {
            readonly get
            {
                return svgImage;
            }

            set
            {
                if (svgImage == value)
                    return;
                svgImage = value;
            }
        }

        /// <summary>
        /// Creates a bitmap representation of the SVG image with
        /// the specified width, height, and optional color.
        /// </summary>
        /// <param name="width">The width of the bitmap.</param>
        /// <param name="height">The height of the bitmap.</param>
        /// <param name="color">The optional color to apply to the bitmap.</param>
        /// <returns>The created bitmap, or null if the SVG image is not set.</returns>
        public TBitmap? ToBitmap(int width, int height, Color? color = null)
        {
            if (svgImage is null)
                return default;
            bitmaps ??= new();
            var key = new SizeAndColor(new SizeI(width, height), color);
            var result = bitmaps.GetOrAdd(key, CreateWithColor!);

            return result;
        }

        /// <summary>
        /// Resets the cached bitmap representations of the SVG image.
        /// </summary>
        public readonly void ResetBitmaps()
        {
            bitmaps?.Clear();
            disabledBitmaps?.Clear();
            normalBitmaps?.Clear();
        }

        /// <summary>
        /// Resets the cached bitmap representations of the SVG image with color.
        /// </summary>
        public readonly void ResetColoredBitmaps()
        {
            bitmaps?.Clear();
        }
        
        /// <summary>
        /// Resets the cached disabled bitmap representations of the SVG image.
        /// </summary>  
        public readonly void ResetDisabledBitmaps()
        {
            disabledBitmaps?.Clear();
        }

        /// <summary>
        /// Resets the cached normal bitmap representations of the SVG image.
        /// </summary>
        public readonly void ResetNormalBitmaps()
        {
            normalBitmaps?.Clear();
        }

        /// <summary>
        /// Adds a bitmap representation of the SVG image with the specified color.
        /// This method can be used if you need to provide a custom bitmap for a specific svg size and color.
        /// Usually, you don't need to call this method,
        /// as the bitmap can be generated from the svg image automatically.
        /// </summary>
        /// <param name="bitmap">The bitmap to add.</param>
        /// <param name="color">The optional color to apply to the bitmap.</param>
        public void AddBitmap(TBitmap bitmap, Color? color = null)
        {
            bitmaps ??= new();
            var size = provider.GetBitmapSize(bitmap);
            var key = new SizeAndColor(size, color);
            bitmaps.Add(key, bitmap);
        }

        /// <summary>
        /// Adds a normal bitmap representation of the SVG image with the specified dark mode option.
        /// This method can be used if you need to provide a custom bitmap for a specific svg size and dark mode option.
        /// Usually, you don't need to call this method,
        /// as the normal bitmap can be generated from the svg image automatically.
        /// </summary>
        /// <param name="isDark">Indicates whether the bitmap is for dark mode.</param>
        /// <param name="bitmap">The bitmap to add.</param>
        public void AddNormalBitmap(TBitmap bitmap, bool isDark)
        {
            normalBitmaps ??= new();
            var size = provider.GetBitmapSize(bitmap);
            var key = new SizeAndBool(size, isDark);
            normalBitmaps.Add(key, bitmap);
        }

        /// <summary>
        /// Adds a disabled bitmap representation of the SVG image with the specified dark mode option.
        /// This method can be used if you need to provide a custom bitmap for a specific svg size and dark mode option.
        /// Usually, you don't need to call this method,
        /// as the disabled bitmap can be generated from the svg image automatically.
        /// </summary>
        /// <param name="isDark">Indicates whether the bitmap is for dark mode.</param>
        /// <param name="bitmap">The bitmap to add.</param>
        public void AddDisabledBitmap(TBitmap bitmap, bool isDark)
        {
            disabledBitmaps ??= new();
            var size = provider.GetBitmapSize(bitmap);
            var key = new SizeAndBool(size, isDark);
            disabledBitmaps.Add(key, bitmap);
        }

        /// <summary>
        /// Creates a disabled bitmap representation of the SVG image
        /// with the specified width, height, and dark mode option.
        /// </summary>
        /// <param name="width">The width of the bitmap.</param>
        /// <param name="height">The height of the bitmap.</param>
        /// <param name="isDark">Indicates whether the bitmap is for dark mode.</param>
        /// <returns>The created disabled bitmap, or null if the SVG image is not set.</returns>
        public TBitmap? ToDisabledBitmap(int width, int height, bool isDark)
        {
            if (svgImage is null)
                return default;

            disabledBitmaps ??= new();
            var key = new SizeAndBool(new SizeI(width, height), isDark);
            var result = disabledBitmaps.GetOrAdd(key, CreateDisabled!);

            return result;
        }
        
        /// <summary>
        /// Creates a normal bitmap representation of the SVG image
        /// with the specified width, height, and dark mode option.
        /// </summary>
        /// <param name="width">The width of the bitmap.</param>
        /// <param name="height">The height of the bitmap.</param>
        /// <param name="isDark">Indicates whether the bitmap is for dark mode.</param>
        /// <returns>The created normal bitmap, or null if the SVG image is not set.</returns>
        public TBitmap? ToNormalBitmap(int width, int height, bool isDark)
        {
            if (svgImage is null)
                return default;

            normalBitmaps ??= new();
            var key = new SizeAndBool(new SizeI(width, height), isDark);
            var result = normalBitmaps.GetOrAdd(key, CreateNormal!);

            return result;
        }

        private readonly TBitmap? CreateWithColor(SizeAndColor key)
        {
            if (svgImage is null)
                return default;
            return provider.ToBitmap(svgImage, key.Size.Width, key.Size.Height, key.Color);
        }

        private readonly TBitmap? CreateDisabled(SizeAndBool key)
        {
            if (svgImage is null)
                return default;
            return provider.ToDisabledBitmap(svgImage, key.Size.Width, key.Size.Height, key.IsDark);
        }

        private readonly TBitmap? CreateNormal(SizeAndBool key)
        {
            if (svgImage is null)
                return default;
            return provider.ToNormalBitmap(svgImage, key.Size.Width, key.Size.Height, key.IsDark);
        }

        private readonly struct SizeAndBool
        {
            public readonly SizeI Size;
            public readonly bool IsDark;

            public SizeAndBool(SizeI size, bool isDark)
            {
                Size = size;
                IsDark = isDark;
            }

            public override int GetHashCode()
            {
                return (Size, IsDark).GetHashCode();
            }
        }

        private readonly struct SizeAndColor
        {
            public readonly SizeI Size;
            public readonly Color? Color;
            
            public SizeAndColor(SizeI size, Color? color)
            {
                Size = size;
                Color = color;
            }

            public override int GetHashCode()
            {
                return (Size, Color).GetHashCode();
            }
        }
    }
}
