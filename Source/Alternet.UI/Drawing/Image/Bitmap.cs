using System;
using System.IO;
using System.Runtime.CompilerServices;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Encapsulates a bitmap, which consists of the pixel data for a graphics image and
    /// its attributes.
    /// A <see cref="Bitmap"/> is an object used to work with images defined by pixel data.
    /// </summary>
    public class Bitmap : Image
    {
        /// <summary>
        /// Creates a bitmap compatible with the given <see cref="Graphics"/>, inheriting
        /// its magnification factor.
        /// </summary>
        /// <param name="width">The width of the bitmap in pixels, must be strictly positive.</param>
        /// <param name="height">The height of the bitmap in pixels, must be strictly positive.</param>
        /// <param name="dc"><see cref="Graphics"/> from which the scaling factor is inherited.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Bitmap(int width, int height, Graphics dc)
            : base(NativeDrawing.Default.CreateImageFromGraphics(width, height, dc.NativeObject))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bitmap"/> class with the image from
        /// <see cref="ImageSet"/>.
        /// </summary>
        /// <param name="imageSet">Source of the image.</param>
        /// <param name="size">Size of the image in device pixels.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Bitmap(ImageSet imageSet, SizeI size)
            : base(size)
        {
            ((UI.Native.ImageSet)imageSet.NativeObject).InitImage(
                (UI.Native.Image)NativeObject,
                size.Width,
                size.Height);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bitmap"/> class with the image from
        /// <see cref="ImageSet"/>.
        /// </summary>
        /// <param name="imageSet">Source of the image.</param>
        /// <param name="control">Control used to get dpi.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Bitmap(ImageSet imageSet, Control control)
        {
            NativeObject = NativeDrawing.Default.CreateImage();
            ((UI.Native.ImageSet)imageSet.NativeObject).InitImageFor(
                (UI.Native.Image)NativeObject,
                control.WxWidget);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bitmap"/> class from a stream.
        /// </summary>
        /// <param name="stream">Stream with bitmap.</param>
        /// <param name="bitmapType">Type of the bitmap.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Bitmap(Stream stream, BitmapType bitmapType = BitmapType.Any)
            : base(stream, bitmapType)
        {
        }

        /// <summary>
        /// Creates a bitmap compatible with the given <see cref="Graphics"/> from
        /// the given <see cref="GenericImage"/>.
        /// </summary>
        /// <param name="genericImage">Platform-independent image object.</param>
        /// <param name="dc"><see cref="Graphics"/> from which the scaling
        /// factor is inherited.</param>
        /// <remarks>
        /// This constructor initializes the bitmap with the data of the given image, which
        /// must be valid, but inherits the scaling factor from the given device context
        /// instead of simply using the default factor of 1.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Bitmap(GenericImage genericImage, Graphics dc)
            : base(NativeDrawing.Default.CreateImageFromGraphicsAndGenericImage(
                genericImage.Handle,
                dc.NativeObject))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bitmap"/> class from
        /// the specified <see cref="GenericImage"/>.
        /// </summary>
        /// <param name="genericImage">Generic image.</param>
        /// <param name="depth">Specifies the depth of the bitmap.
        /// Some platforms only support (1) for monochrome and (-1) for the current color setting.
        /// A depth of 32 including an alpha channel is supported under MSW, Mac and Linux.
        /// If this parameter is omitted
        /// (= -1), the display depth of the screen is used.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Bitmap(GenericImage genericImage, int depth = -1)
            : base(genericImage, depth)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bitmap"/> class
        /// with the specified size in device pixels.
        /// </summary>
        /// <param name="width">The width in pixels used to create the image.</param>
        /// <param name="height">The height in pixels used to create the image.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Bitmap(double width, double height)
            : base(width, height)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bitmap"/> class
        /// with the specified size in device pixels.
        /// </summary>
        /// <param name="width">The width in pixels used to create the image.</param>
        /// <param name="height">The height in pixels used to create the image.</param>
        /// <param name="depth">Specifies the depth of the bitmap.
        /// Some platforms only support (1) for monochrome and (-1) for the current color setting.
        /// A depth of 32 including an alpha channel is supported under MSW, Mac and Linux.
        /// If this parameter is omitted
        /// (= -1), the display depth of the screen is used.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Bitmap(int width, int height, int depth = 32)
            : base(width, height, depth)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bitmap"/> class.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Bitmap()
            : base(SizeI.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bitmap"/> class with the specified size.
        /// </summary>
        /// <param name="size">The size, in device pixels, of the new <see cref="Bitmap"/>.</param>
        /// <param name="depth">Specifies the depth of the bitmap.
        /// Some platforms only support (1) for monochrome and (-1) for the current color setting.
        /// A depth of 32 including an alpha channel is supported under MSW, Mac and Linux.
        /// If this parameter is omitted
        /// (= -1), the display depth of the screen is used.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Bitmap(SizeI size, int depth = 32)
            : base(size, -1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bitmap"/> class with the specified size
        /// amd scaling factor from the <paramref name="control"/>.
        /// </summary>
        /// <param name="size">The size, in device pixels, of the new <see cref="Bitmap"/>.</param>
        /// <param name="control">The control from which pixel scaling factor is used.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Bitmap(SizeI size, Control control)
            : base(size)
        {
            ScaleFactor = control.GetPixelScaleFactor();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bitmap" /> class from the specified
        /// existing image, scaled to the specified size.
        /// </summary>
        /// <param name="original">The <see cref="Image" /> from which to create the
        /// new <see cref="Bitmap" />.</param>
        /// <param name="newSize">The <see cref="SizeI" /> structure that represent the
        /// size of the new <see cref="Bitmap" />.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Bitmap(Image original, SizeI newSize)
            : base(original, newSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bitmap"/> class from the specified
        /// existing image.
        /// </summary>
        /// <param name="image">The <see cref="Image"/> from which to create the
        /// new <see cref="Bitmap"/>.</param>
        /// <remarks>
        /// Full image data is copied from the original image.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Bitmap(Image image)
            : base()
        {
            ((UI.Native.Image)NativeObject).CopyFrom((UI.Native.Image)image.NativeObject);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bitmap"/> class from the specified
        /// data stream.
        /// </summary>
        /// <param name="stream">The data stream used to load the bitmap.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Bitmap(Stream? stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bitmap"/> class from the specified
        /// file or resource url.
        /// </summary>
        /// <param name="url">The file or embedded resource url used
        /// to load the image.
        /// </param>
        /// <remarks>
        /// See <see cref="Image.FromUrl(string)"/> for the details.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Bitmap(string url)
            : base(url)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bitmap"/> class.
        /// </summary>
        /// <param name="nativeImage">Native image instance.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Bitmap(object nativeImage)
            : base(nativeImage)
        {
        }

        /// <summary>
        /// Gets <see cref="Graphics"/> which allows to draw on the image.
        /// Same as <see cref="GetDrawingContext"/>.
        /// </summary>
        [Browsable(false)]
        public virtual Graphics Canvas => GetDrawingContext();

        /// <summary>
        /// Converts the specified <see cref='GenericImage'/> to a <see cref='Bitmap'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Bitmap(GenericImage image) => new(image);

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class
        /// from the specified url which points to svg file or resource.
        /// </summary>
        /// <remarks>
        /// This is similar to <see cref="Image.FromSvgUrl"/> but uses
        /// <see cref="Control.GetDPI"/> and <see cref="ToolBar.GetDefaultImageSize(double)"/>
        /// to get appropriate image size which is best suitable for toolbars.
        /// </remarks>
        /// <param name="url">The file or embedded resource url with Svg data used
        /// to load the image.</param>
        /// <param name="control">Control which <see cref="Control.GetDPI"/> method
        /// is used to get DPI.</param>
        /// <returns><see cref="Image"/> instance loaded from Svg data for use
        /// on the toolbars.</returns>
        /// <param name="color">Svg fill color. Optional.
        /// If provided, svg fill color is changed to the specified value.</param>
        public static Image FromSvgUrlForToolbar(string url, Control control, Color? color = null)
        {
            SizeD deviceDpi = control.GetDPI();
            var width = ToolBar.GetDefaultImageSize(deviceDpi.Width);
            var height = ToolBar.GetDefaultImageSize(deviceDpi.Height);
            var result = Image.FromSvgUrl(url, width, height, color);
            return result;
        }

        /// <summary>
        /// Creates a clone of this image with fully copied image data.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public new Bitmap Clone()
        {
            return new Bitmap(this);
        }

        /// <summary>
        /// Gets <see cref="Graphics"/> for this image on which you can paint.
        /// </summary>
        /// <returns></returns>
        public virtual Graphics GetDrawingContext()
        {
            var dc = WxGraphics.FromImage(this);
            return dc;
        }
    }
}