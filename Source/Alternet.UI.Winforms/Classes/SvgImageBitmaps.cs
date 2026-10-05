using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alternet.Winforms
{
    /// <summary>
    /// Provides a set of methods to convert <see cref="Alternet.Drawing.SvgImage"/> to <see cref="Image"/>.
    /// </summary>
    public class SvgImageBitmaps
    {
        private Alternet.Drawing.SvgImageBitmapsData<Image> bitmaps = new (SvgImageBitmapsProvider.Instance);

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
        /// Gets or sets the <see cref="Alternet.Drawing.SvgImage"/>
        /// associated with this <see cref="SvgImageBitmaps"/>.
        /// </summary>
        public Drawing.SvgImage? SvgImage
        {
            get => bitmaps.SvgImage;
            set => bitmaps.SvgImage = value;
        }

        /// <inheritdoc cref="Drawing.SvgImageBitmapsData{TBitmap}.SvgSizeRelative"/>
        public Drawing.RelativeSize? SvgSizeRelative
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
        /// Gets or sets the size of the SVG image. This property is used to determine
        /// the base size of the bitmap representations of the SVG image when scale factor is 1.0f.
        /// If this property is not set, it is considered to be 16x16 pixels by default.
        /// Changing this property will reset the cached bitmap representations.
        /// </summary>
        public Size? SvgSize
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
        public Image? ToDisabledBitmap(Control control, bool isDark)
        {
            float scaleFactor = WinformsUtils.GetScaleFactor(control);
            return bitmaps.ToDisabledBitmap(scaleFactor, isDark);
        }

        /// <summary>
        /// Gets a bitmap representation of the SVG image with the specified scale factor and optional color.
        /// </summary>
        /// <param name="scaleFactor">The scale factor to apply to the SVG image.</param>
        /// <param name="color">The optional color to apply to the bitmap.</param>
        /// <returns>The bitmap representation of the SVG, or null if the SVG image is not set.</returns>
        public Image? ToBitmap(float scaleFactor, Color? color = null)
        {
            return bitmaps.ToBitmap(scaleFactor, color);
        }

        /// <summary>
        /// Creates a normal bitmap representation of the SVG image with the specified control and dark mode option.
        /// Control is used to get the scale factor for the bitmap.
        /// </summary>
        /// <param name="control">The WinForms control.</param>
        /// <param name="isDark">Indicates whether the bitmap is for dark mode.</param>
        /// <returns>The created normal bitmap, or null if the SVG image is not set.</returns>
        public Image? ToNormalBitmap(Control control, bool isDark)
        {
            float scaleFactor = WinformsUtils.GetScaleFactor(control);
            return bitmaps.ToNormalBitmap(scaleFactor, isDark);
        }

        /// <summary>
        /// Gets the effective size of the SVG image based on the specified scale factor.
        /// </summary>
        /// <param name="scaleFactor">The scale factor to apply.</param>
        /// <returns>The effective size of the SVG image.</returns>
        public Size EffectiveSvgSize(float scaleFactor = 1.0f)
        {
            return bitmaps.EffectiveSvgSize(scaleFactor);
        }

        /// <summary>
        /// Gets the effective size of the SVG image based on the specified control.
        /// Control is used to get the scale factor for the effective size.
        /// </summary>
        /// <param name="control">The WinForms control.</param>
        /// <returns>The effective size of the SVG image.</returns>
        public Size EffectiveSvgSize(Control control)
        {
            float scaleFactor = WinformsUtils.GetScaleFactor(control);
            return bitmaps.EffectiveSvgSize(scaleFactor);
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
}
