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
    public struct SvgImageBitmapsData<TBitmap>
    {
        private readonly ISvgImageBitmapsProvider<TBitmap> provider;
        private BaseDictionary<SizeAndBool, TBitmap>? normalBitmaps;
        private BaseDictionary<SizeAndBool, TBitmap>? disabledBitmaps;
        private BaseDictionary<SizeAndColor, TBitmap>? bitmaps;

        private SvgImage? svgImage;
        private SizeI? svgSize;
        private RelativeSize? svgSizeRelative;
        private ThemedColor? svgColorNormal;
        private ThemedColor? svgColorDisabled;

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgImageBitmapsData{TBitmap}"/> class.
        /// </summary>
        /// <param name="provider">The provider for creating bitmap representations of the SVG image.</param>
        public SvgImageBitmapsData(ISvgImageBitmapsProvider<TBitmap> provider)
        {
            this.provider = provider;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgImageBitmapsData{TBitmap}"/>
        /// class with the specified SVG image.
        /// </summary>
        /// <param name="provider">The provider for creating bitmap representations of the SVG image.</param>
        /// <param name="svgImage">The SVG image.</param>
        public SvgImageBitmapsData(ISvgImageBitmapsProvider<TBitmap> provider, SvgImage svgImage)
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
        /// Gets or sets the size of the SVG image. This property is used to determine
        /// the base size of the bitmap representations of the SVG image when scale factor is 1.0f.
        /// If this property is not set, it is considered to be 16x16 pixels by default.
        /// Changing this property will reset the cached bitmap representations.
        /// </summary>
        public SizeI? SvgSize
        {
            readonly get
            {
                return svgSize;
            }

            set
            {
                if (svgSize == value)
                    return;
                svgSize = value;
                ResetBitmaps();
            }
        }

        /// <summary>
        /// Gets or sets the relative size of the SVG image. This property is used to determine
        /// the size of the bitmap representations of the SVG image relative to the base size.
        /// If this property is not set, the default size will be used.
        /// Changing this property will reset the cached bitmap representations.
        /// </summary>
        /// <remarks>
        /// It is suggested to use this property instead of <see cref="SvgSize"/>
        /// if you want to automatically scale the SVG image size based on the display DPI or other factors.
        /// </remarks>
        public RelativeSize? SvgSizeRelative
        {
            readonly get
            {
                return svgSizeRelative;
            }
            set
            {
                if (svgSizeRelative == value)
                    return;
                svgSizeRelative = value;
                ResetBitmaps();
            }
        }

        /// <summary>
        /// Gets or sets the color of the normal SVG image. This property is used to determine
        /// the color of the bitmap representations of the SVG image when it is in the normal state.
        /// If this property is not set, the default color for the normal state will be used.
        /// Changing this property will reset the cached bitmap representations.
        /// </summary>
        public ThemedColor? SvgColorNormal
        {
            readonly get
            {
                return svgColorNormal;
            }

            set
            {
                if (svgColorNormal == value)
                    return;
                svgColorNormal = value;
                ResetNormalBitmaps();
            }
        }

        /// <summary>
        /// Gets or sets the color of the disabled SVG image. This property is used to determine
        /// the color of the bitmap representations of the SVG image when it is in the disabled state.
        /// If this property is not set, the default color for the disabled state will be used.
        /// Changing this property will reset the cached bitmap representations.
        /// </summary>
        public ThemedColor? SvgColorDisabled
        {
            readonly get
            {
                return svgColorDisabled;
            }

            set
            {
                if (svgColorDisabled == value)
                    return;
                svgColorDisabled = value;
                ResetDisabledBitmaps();
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
        /// Creates a disabled bitmap representation of the SVG image
        /// with the specified scale factor and dark mode option.
        /// </summary>
        /// <param name="scaleFactor">The scale factor to apply.</param>
        /// <param name="isDark">Indicates whether the bitmap is for dark mode.</param>
        /// <returns>The created disabled bitmap, or null if the SVG image is not set.</returns>
        public TBitmap? ToDisabledBitmap(float scaleFactor, bool isDark)
        {
            if (svgImage is null)
                return default;

            var size = EffectiveSvgSize(scaleFactor);
            return ToDisabledBitmap(size.Width, size.Height, isDark);
        }

        /// <summary>
        /// Gets the effective size of the SVG image based on the specified scale factor.
        /// </summary>
        /// <param name="scaleFactor">The scale factor to apply.</param>
        /// <returns>The effective size of the SVG image.</returns>
        public readonly SizeI EffectiveSvgSize(float scaleFactor = 1.0f)
        {
            var size = DrawingUtils.EffectiveSvgSize(scaleFactor, svgSize, svgSizeRelative);
            return size;
        }

        /// <summary>
        /// Creates a disabled bitmap representation of the SVG image
        /// with the specified scale factor and dark mode option.
        /// </summary>
        /// <param name="scaleFactor">The scale factor to apply.</param>
        /// <param name="isDark">Indicates whether the bitmap is for dark mode.</param>
        /// <returns>The created normal bitmap, or null if the SVG image is not set.</returns>
        public TBitmap? ToNormalBitmap(float scaleFactor, bool isDark)
        {
            if (svgImage is null)
                return default;

            var size = EffectiveSvgSize(scaleFactor);
            return ToNormalBitmap(size.Width, size.Height, isDark);
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

            if(svgColorDisabled is not null)
            {
                var color = svgColorDisabled.GetColor(key.IsDark);
                return provider.ToBitmap(svgImage, key.Size.Width, key.Size.Height, color);
            }

            return provider.ToDisabledBitmap(svgImage, key.Size.Width, key.Size.Height, key.IsDark);
        }

        private readonly TBitmap? CreateNormal(SizeAndBool key)
        {
            if (svgImage is null)
                return default;

            if (svgColorNormal is not null)
            {
                var color = svgColorNormal.GetColor(key.IsDark);
                return provider.ToBitmap(svgImage, key.Size.Width, key.Size.Height, color);
            }

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

    /// <summary>
    /// Provides a set of methods to convert <see cref="Alternet.Drawing.SvgImage"/> to <see cref="Image"/>.
    /// </summary>
    public class SvgImageBitmaps
    {
        private Alternet.Drawing.SvgImageBitmapsData<Image> bitmaps = new(SvgImageBitmapsProvider.Instance);

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgImageBitmaps"/> class.
        /// </summary>
        public SvgImageBitmaps()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgImageBitmaps"/> class
        /// with the specified <see cref="Alternet.Drawing.SvgImage"/>.
        /// </summary>
        public SvgImageBitmaps(Drawing.SvgImage svg)
        {
            bitmaps.SvgImage = svg;
        }

        /// <summary>
        /// Gets or sets the <see cref="Alternet.Drawing.SvgImage"/> associated with this <see cref="SvgImageBitmaps"/>.
        /// </summary>
        public Drawing.SvgImage? SvgImage
        {
            get => bitmaps.SvgImage;
            set => bitmaps.SvgImage = value;
        }

        /// <summary>
        /// Gets or sets the size of the SVG image. This property is used to determine
        /// the base size of the bitmap representations of the SVG image when scale factor is 1.0f.
        /// If this property is not set, it is considered to be 16x16 pixels by default.
        /// Changing this property will reset the cached bitmap representations.
        /// </summary>
        public SizeI? SvgSize
        {
            get
            {
                return bitmaps.SvgSize;
            }

            set
            {
                bitmaps.SvgSize = value;
            }
        }

        /// <inheritdoc cref="SvgImageBitmapsData{TBitmap}.SvgSizeRelative"/>
        public RelativeSize? SvgSizeRelative
        {
            get
            {
                return bitmaps.SvgSizeRelative;
            }
            set
            {
                bitmaps.SvgSizeRelative = value;
            }
        }

        /// <summary>
        /// Gets or sets the color of the normal SVG image. This property is used to determine
        /// the color of the bitmap representations of the SVG image when it is in the normal state.
        /// If this property is not set, the default color for the normal state will be used.
        /// Changing this property will reset the cached bitmap representations.
        /// </summary>
        public Drawing.ThemedColor? SvgColorNormal
        {
            get
            {

                var c = bitmaps.SvgColorNormal;
                if (c is null)
                    return null;
                else
                    return c;
            }

            set
            {
                bitmaps.SvgColorNormal = value;
            }
        }

        /// <summary>
        /// Gets or sets the color of the disabled SVG image. This property is used to determine
        /// the color of the bitmap representations of the SVG image when it is in the disabled state.
        /// If this property is not set, the default color for the disabled state will be used.
        /// Changing this property will reset the cached bitmap representations.
        /// </summary>
        public Drawing.ThemedColor? SvgColorDisabled
        {
            get
            {
                var c = bitmaps.SvgColorDisabled;
                if (c is null)
                    return null;
                else
                    return c;
            }

            set
            {
                bitmaps.SvgColorDisabled = value;
            }
        }

        /// <summary>
        /// Creates a disabled bitmap representation of the SVG image
        /// with the specified scale factor and dark mode option.
        /// </summary>
        /// <param name="scaleFactor">The scale factor to apply.</param>
        /// <param name="isDark">Indicates whether the bitmap is for dark mode.</param>
        /// <returns>The created disabled bitmap, or null if the SVG image is not set.</returns>
        public Image? ToDisabledBitmap(float scaleFactor, bool isDark)
        {
            return bitmaps.ToDisabledBitmap(scaleFactor, isDark);
        }


        /// <summary>
        /// Creates a disabled bitmap representation of the SVG image with the specified control and dark mode option.
        /// Control is used to get the scale factor for the bitmap.
        /// </summary>
        /// <param name="control">The WinForms control.</param>
        /// <param name="isDark">Indicates whether the bitmap is for dark mode.</param>
        /// <returns>The created disabled bitmap, or null if the SVG image is not set.</returns>
        public Image? ToDisabledBitmap(AbstractControl control, bool isDark)
        {
            float scaleFactor = control.ScaleFactor;
            return bitmaps.ToDisabledBitmap(scaleFactor, isDark);
        }

        /// <summary>
        /// Creates a normal bitmap representation of the SVG image with the specified control and dark mode option.
        /// Control is used to get the scale factor for the bitmap.
        /// </summary>
        /// <param name="control">The WinForms control.</param>
        /// <param name="isDark">Indicates whether the bitmap is for dark mode.</param>
        /// <returns>The created normal bitmap, or null if the SVG image is not set.</returns>
        public Image? ToNormalBitmap(AbstractControl control, bool isDark)
        {
            float scaleFactor = control.ScaleFactor;
            return bitmaps.ToNormalBitmap(scaleFactor, isDark);
        }

        /// <summary>
        /// Creates a disabled bitmap representation of the SVG image
        /// with the specified scale factor and dark mode option.
        /// </summary>
        /// <param name="scaleFactor">The scale factor to apply.</param>
        /// <param name="isDark">Indicates whether the bitmap is for dark mode.</param>
        /// <returns>The created normal bitmap, or null if the SVG image is not set.</returns>
        public Image? ToNormalBitmap(float scaleFactor, bool isDark)
        {
            return bitmaps.ToNormalBitmap(scaleFactor, isDark);
        }

        /// <summary>
        /// Creates a bitmap representation of the SVG image with
        /// the specified width, height, and optional color.
        /// </summary>
        /// <param name="width">The width of the bitmap.</param>
        /// <param name="height">The height of the bitmap.</param>
        /// <param name="color">The optional color to apply to the bitmap.</param>
        /// <returns>The created bitmap, or null if the SVG image is not set.</returns>
        public Image? ToBitmap(int width, int height, Color? color = null)
        {
            return bitmaps.ToBitmap(width, height, color);
        }

        /// <summary>
        /// Resets the cached bitmap representations of the SVG image.
        /// </summary>
        public void ResetBitmaps()
        {
            bitmaps.ResetBitmaps();
        }

        /// <summary>
        /// Resets the cached bitmap representations of the SVG image with color.
        /// </summary>
        public void ResetColoredBitmaps()
        {
            bitmaps.ResetColoredBitmaps();
        }

        /// <summary>
        /// Resets the cached disabled bitmap representations of the SVG image.
        /// </summary>  
        public void ResetDisabledBitmaps()
        {
            bitmaps.ResetDisabledBitmaps();
        }

        /// <summary>
        /// Resets the cached normal bitmap representations of the SVG image.
        /// </summary>
        public void ResetNormalBitmaps()
        {
            bitmaps.ResetNormalBitmaps();
        }

        /// <summary>
        /// Adds a bitmap representation of the SVG image with the specified color.
        /// This method can be used if you need to provide a custom bitmap for a specific svg size and color.
        /// Usually, you don't need to call this method,
        /// as the bitmap can be generated from the svg image automatically.
        /// </summary>
        /// <param name="bitmap">The bitmap to add.</param>
        /// <param name="color">The optional color to apply to the bitmap.</param>
        public void AddBitmap(Image bitmap, Color? color = null)
        {
            bitmaps.AddBitmap(bitmap, color);
        }

        /// <summary>
        /// Adds a normal bitmap representation of the SVG image with the specified dark mode option.
        /// This method can be used if you need to provide a custom bitmap for a specific svg size and dark mode option.
        /// Usually, you don't need to call this method,
        /// as the normal bitmap can be generated from the svg image automatically.
        /// </summary>
        /// <param name="isDark">Indicates whether the bitmap is for dark mode.</param>
        /// <param name="bitmap">The bitmap to add.</param>
        public void AddNormalBitmap(Image bitmap, bool isDark)
        {
            bitmaps.AddNormalBitmap(bitmap, isDark);
        }

        /// <summary>
        /// Adds a disabled bitmap representation of the SVG image with the specified dark mode option.
        /// This method can be used if you need to provide a custom bitmap for a specific svg size and dark mode option.
        /// Usually, you don't need to call this method,
        /// as the disabled bitmap can be generated from the svg image automatically.
        /// </summary>
        /// <param name="isDark">Indicates whether the bitmap is for dark mode.</param>
        /// <param name="bitmap">The bitmap to add.</param>
        public void AddDisabledBitmap(Image bitmap, bool isDark)
        {
            bitmaps.AddDisabledBitmap(bitmap, isDark);
        }

        /// <summary>
        /// Gets the effective size of the SVG image based on the specified scale factor.
        /// </summary>
        /// <param name="scaleFactor">The scale factor to apply.</param>
        /// <returns>The effective size of the SVG image.</returns>
        public SizeI EffectiveSvgSize(float scaleFactor = 1.0f)
        {
            return bitmaps.EffectiveSvgSize(scaleFactor);
        }

        /// <summary>
        /// Gets the effective size of the SVG image based on the specified control.
        /// Control is used to get the scale factor for the effective size.
        /// </summary>
        /// <param name="control">The WinForms control.</param>
        /// <returns>The effective size of the SVG image.</returns>
        public SizeI EffectiveSvgSize(Control control)
        {
            float scaleFactor = control.ScaleFactor;
            return bitmaps.EffectiveSvgSize(scaleFactor);
        }

        /// <summary>
        /// Creates a disabled bitmap representation of the SVG image
        /// with the specified width, height, and dark mode option.
        /// </summary>
        /// <param name="width">The width of the bitmap.</param>
        /// <param name="height">The height of the bitmap.</param>
        /// <param name="isDark">Indicates whether the bitmap is for dark mode.</param>
        /// <returns>The created disabled bitmap, or null if the SVG image is not set.</returns>
        public Image? ToDisabledBitmap(int width, int height, bool isDark)
        {
            return bitmaps.ToDisabledBitmap(width, height, isDark);
        }

        /// <summary>
        /// Creates a normal bitmap representation of the SVG image
        /// with the specified width, height, and dark mode option.
        /// </summary>
        /// <param name="width">The width of the bitmap.</param>
        /// <param name="height">The height of the bitmap.</param>
        /// <param name="isDark">Indicates whether the bitmap is for dark mode.</param>
        /// <returns>The created normal bitmap, or null if the SVG image is not set.</returns>
        public Image? ToNormalBitmap(int width, int height, bool isDark)
        {
            return bitmaps.ToNormalBitmap(width, height, isDark);
        }
    }

    /// <summary>
    /// Provides a set of methods to convert <see cref="Alternet.Drawing.SvgImage"/> to <see cref="Image"/>.
    /// </summary>
    public class SvgImageBitmapsProvider : ISvgImageBitmapsProvider<Image>
    {
        /// <summary>
        /// Gets the instance of <see cref="SvgImageBitmapsProvider"/>.
        /// </summary>
        public static SvgImageBitmapsProvider Instance { get; } = new SvgImageBitmapsProvider();

        /// <inheritdoc/>
        public SizeI GetBitmapSize(Image bitmap)
        {
            return new SizeI(bitmap.Width, bitmap.Height);
        }

        /// <inheritdoc/>
        public Image ToBitmap(SvgImage svg, int width, int height, Color? color = null)
        {
            return svg.CreateImage(new SizeI(width, height), color);
        }

        /// <inheritdoc/>
        public Image ToDisabledBitmap(SvgImage svg, int width, int height, bool isDark)
        {
            return svg.CreateImage(new SizeI(width, height), KnownSvgColor.Disabled, isDark);
        }

        /// <inheritdoc/>
        public Image ToNormalBitmap(SvgImage svg, int width, int height, bool isDark)
        {
            return svg.CreateImage(new SizeI(width, height), KnownSvgColor.Normal, isDark);
        }
    }
}