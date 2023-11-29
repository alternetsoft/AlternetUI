using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    /// <summary>
    /// Lists known image option names for use in <see cref="GenericImage"/>.
    /// </summary>
    /// <remarks>
    /// <see cref="Resolution"/>, <see cref="ResolutionX"/> and
    /// <see cref="ResolutionY"/> options define the resolution of the image in the units
    /// corresponding to <see cref="ResolutionUnit"/> options value.
    /// The <see cref="Resolution"/> option can be set
    /// before saving the image to set both horizontal and vertical resolution to the same value.
    /// <see cref="ResolutionX"/> and <see cref="ResolutionY"/> options are set by the
    /// image handlers if they support the image resolution (currently BMP, JPEG and TIFF handlers do)
    /// and the image provides the resolution information and can be queried after loading the image.
    /// </remarks>
    /// <remarks>
    /// If either of <see cref="MaxWidth"/> and <see cref="MaxHeight"/> options is specified,
    /// the loaded image will be scaled down (preserving its aspect ratio) so that its width
    /// is less than the max width given if it is not 0 and its height is less than the max
    /// height given if it is not 0. This is typically used for loading thumbnails and the advantage
    /// of using these options compared to calling <see cref="GenericImage.Rescale"/> after
    /// loading is that some handlers (only JPEG one right now) support rescaling the image during
    /// loading which is vastly more efficient than loading the entire huge image and rescaling
    /// it later (if these options are not supported by the handler, this is still what happens
    /// however). These options must be set before loading file to have any effect.
    /// </remarks>
    /// <remarks>
    /// Note: Be careful when combining the options <see cref="TiffSamplesPerPixel"/>,
    /// <see cref="TiffBitsPerSample"/>, and <see cref="TiffPhotometric"/>.
    /// While some measures are taken to prevent illegal combinations and/or values,
    /// it is still easy to abuse them and come up with invalid results in the form of
    /// either corrupted images or crashes.
    /// </remarks>
    public class GenericImageOptionNames
    {
        /// <summary>
        /// Image option name for setting JPEG quality used when saving. This is an integer
        /// in 0..100 range with 0 meaning very poor and 100 excellent (but very badly compressed).
        /// This option is currently ignored for the other formats.
        /// </summary>
        public const string Quality = "quality";

        /// <summary>
        /// Image option name for getting the name of the file from which the image was loaded.
        /// </summary>
        public const string FileName = "FileName";

        /// <summary>
        /// Image option name for setting resolution.
        /// </summary>
        public const string Resolution = "Resolution";

        /// <summary>
        /// Image option name for setting horizontal resolution.
        /// </summary>
        public const string ResolutionX = "ResolutionX";

        /// <summary>
        /// Image option name for setting vertical resolution.
        /// </summary>
        public const string ResolutionY = "ResolutionY";

        /// <summary>
        /// Image option name for setting resolution unit. The value of this option determines
        /// whether the resolution of the image is specified in centimetres or inches,
        /// see <see cref="GenericImageResolutionUnit"/> enum elements. 
        /// </summary>
        public const string ResolutionUnit = "ResolutionUnit";

        /// <summary>
        /// Image option name for setting maximal width.
        /// </summary>
        public const string MaxWidth = "MaxWidth";

        /// <summary>
        /// Image option name for setting maximal height.
        /// </summary>
        public const string MaxHeight = "MaxHeight";

        /// <summary>
        /// Image option name for setting an original width. This option will return the
        /// original width of the image if
        /// <see cref="MaxWidth"/> is specified.
        /// </summary>
        public const string OriginalWidth = "OriginalWidth";

        /// <summary>
        /// Image option name for setting an original height. This option will return the
        /// original height of the image if
        /// <see cref="MaxHeight"/> is specified.
        /// </summary>
        public const string OriginalHeight = "OriginalHeight";

        /// <summary>
        /// Gif image option name for setting a comment. The comment text that is read from or
        /// written to the GIF file. In an animated GIF each frame can have its own comment.
        /// If there is only a comment in the first frame of a GIF it will not be repeated in
        /// other frames.
        /// </summary>
        public const string GifComment = "GifComment";

        /// <summary>
        /// Gif image option name for setting how to deal with transparent pixels. By default, the color
        /// of transparent pixels is changed to bright pink, so that if the image is accidentally
        /// drawn without transparency, it will be obvious. Normally, this would not be noticed,
        /// as these pixels will not be rendered. But in some cases it might be useful to load a
        /// GIF without making any modifications to its colors. Supports
        /// <see cref="GifTransparencyValueHighlight"/> and
        /// <see cref="GifTransparencyValueUnchanged"/> values.
        /// </summary>
        public const string GifTransparency = "Transparency";

        /// <summary>
        /// Defines value for the <see cref="GifTransparency"/> propertty.
        /// Use to convert transparent pixels to pink (default).
        /// </summary>
        public const string GifTransparencyValueHighlight = "Highlight";

        /// <summary>
        /// Defines value for the <see cref="GifTransparency"/> propertty.
        /// Use to keep the colors correct.
        /// </summary>
        public const string GifTransparencyValueUnchanged = "Unchanged";

        /// <summary>
        /// Png image option name for setting format when saving a PNG file,
        /// see <see cref="GenericImagePngType"/> for the supported values.
        /// </summary>
        public const string PngFormat = "PngFormat";

        /// <summary>
        /// Png image option name for setting bit depth for channels (R/G/B/A).
        /// </summary>
        public const string PngBitDepth = "PngBitDepth";

        /// <summary>
        /// Png image option name for setting filter when saving a PNG file,
        /// See <see cref="GenericImagePngSetFilter"/> for possible values.
        /// </summary>
        public const string PngFilter = "PngF";

        /// <summary>
        /// Png image option name for setting a compression level (0..9) when saving a PNG file.
        /// A high value creates smaller, but slower
        /// PNG file. Note that unlike other formats (e.g.JPEG) the PNG format is always lossless
        /// and thus this compression level doesn't tradeoff the image quality.
        /// </summary>
        public const string PngCompressionLevel = "PngZL";

        /// <summary>
        /// Png image option name for setting compression memory usage level (1..9) when
        /// saving a PNG file. A high value means the
        /// saving process consumes more memory, but may create smaller PNG file.
        /// </summary>
        public const string PngCompressionMemLevel = "PngZM";

        /// <summary>
        /// Png image option name for setting a compression strategy. Possible values are 0 for
        /// default strategy, 1 for filter, and 2 for Huffman - only.
        /// You can use OptiPNG (http://optipng.sourceforge.net/) to get a suitable value for your
        /// application.
        /// </summary>
        public const string PngCompressionStrategy = "PngZS";

        /// <summary>
        /// Png image option name for setting internal buffer size (in bytes) used when
        /// saving a PNG file. Ideally this should be as big
        /// as the resulting PNG file. Use this option if your application produces images with
        /// small size variation.
        /// </summary>
        public const string PngCompressionBufferSize = "PngZB";

        /// <summary>
        /// Tiff image option name for setting number of bits per sample (channel). Currently values
        /// of 1 and 8 are supported.
        /// A value of 1 results in a black and white image. A value of 8 (the default) can
        /// mean greyscale
        /// or RGB, depending on the value of <see cref="TiffSamplesPerPixel"/>.
        /// </summary>
        public const string TiffBitsPerSample = "BitsPerSample";

        /// <summary>
        /// Tiff image option name for setting number of samples (channels) per pixel. Currently values
        /// of 1 and 3 are supported.
        /// A value of 1 results in either a greyscale (by default) or black and white image,
        /// depending on the value of <see cref="TiffBitsPerSample"/>. A value of 3
        /// (the default) will result in an RGB image.
        /// </summary>
        public const string TiffSamplesPerPixel = "SamplesPerPixel";

        /// <summary>
        /// Tiff image option name for setting compression type. By default it is set
        /// to 1 (<see cref="GenericImageTiffCompression.None"/>). Typical other values are
        /// 5 (<see cref="GenericImageTiffCompression.LZW"/>) and 7
        /// (<see cref="GenericImageTiffCompression.JPEG"/>).
        /// See <see cref="GenericImageTiffCompression"/> for more options.
        /// </summary>
        public const string TiffCompression = "Compression";

        /// <summary>
        /// Tiff image option name for setting the photometric interpretation.
        /// By default it is set to 2
        /// (<see cref="GenericImageTiffPhotometric.RGB"/>) for RGB images and 0
        /// (<see cref="GenericImageTiffPhotometric.MINISWHITE"/>) for greyscale or
        /// black and white images.
        /// It can also be set to 1 (<see cref="GenericImageTiffPhotometric.MINISBLACK"/>) to treat the
        /// lowest value as black and
        /// highest as white. If you want a greyscale image it is also sufficient to only specify
        /// <see cref="TiffPhotometric"/> and set it to either
        /// <see cref="GenericImageTiffPhotometric.MINISWHITE"/> or
        /// <see cref="GenericImageTiffPhotometric.MINISBLACK"/>. The other values are taken care of.
        /// </summary>
        public const string TiffPhotometric = "Photometric";

        /// <summary>
        /// Tiff image option name for setting image descriptor.
        /// </summary>
        public const string TiffImageDescriptor = "ImageDescriptor";
    }
}