using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Impements methods and properties to work with toolbar svg images.
    /// These are rectangular images with small size. This class allows
    /// to speed up loading and getting of different states (and sizes) of the
    /// same image.
    /// </summary>
    public class SvgImage : BaseObject
    {
        private readonly string? url;
        private readonly string? urlOrData;
        private readonly SvgImageDataKind kind;
        private Stream? stream;
        private Data?[] data = new Data?[16];

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgImage"/> class.
        /// </summary>
        /// <param name="urlOrData">Image url or data.</param>
        /// <param name="kind">Image data kind.</param>
        protected SvgImage(string urlOrData, SvgImageDataKind kind = SvgImageDataKind.Auto)
        {
            if (kind == SvgImageDataKind.Url)
                url = urlOrData;
            this.urlOrData = urlOrData;
            this.kind = kind;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgImage"/> class.
        /// </summary>
        /// <param name="stream">Stream with image data.</param>
        protected SvgImage(Stream stream)
        {
            this.stream = stream;
        }

        /// <summary>
        /// Gets whether image has single color.
        /// </summary>
        public virtual bool IsMono => NumOfColors == SvgImageNumOfColors.One;

        /// <summary>
        /// Gets number of colors in svg, passed in the constructor.
        /// </summary>
        public virtual SvgImageNumOfColors NumOfColors => SvgImageNumOfColors.Uknown;

        /// <summary>
        /// Gets image url.
        /// </summary>
        public virtual string? Url => url;

        internal virtual SvgImageDataKind Kind => kind;

        /// <summary>
        /// Gets <see cref="Stream"/> with image data.
        /// </summary>
        internal virtual Stream? Stream
        {
            get
            {
                if (stream is null)
                {
                    if (urlOrData is null)
                        return null;
                    stream = ResourceLoader.StreamFromUrl(urlOrData);
                }

                return stream;
            }
        }

        /// <summary>
        /// Gets svg image as <see cref="ImageSet"/> with default toolbar image size.
        /// </summary>
        public virtual ImageSet? AsImageSet() => AsImageSet(ToolBar.GetDefaultImageSize().Width);

        /// <summary>
        /// Gets svg image as <see cref="Image"/> with default toolbar image size.
        /// </summary>
        public virtual Image? AsImage() => AsImage(ToolBar.GetDefaultImageSize().Width);

        /// <summary>
        /// Gets svg image as <see cref="Image"/>.
        /// </summary>
        /// <param name="size">Image size</param>
        /// <returns></returns>
        public virtual Image? AsImage(int size) => AsImageSet(size)?.AsImage();

        /// <summary>
        /// Gets svg image as <see cref="ImageSet"/>.
        /// </summary>
        /// <param name="size">Image size</param>
        /// <returns></returns>
        public virtual ImageSet? AsImageSet(int size)
        {
            Resize(size);
            var result = data[size]?.OriginalImage;
            if (result is not null)
                return result;
            result = LoadImage(size);
            data[size] ??= new();
            data[size]!.OriginalImage = result;
            return result;
        }

        /// <summary>
        /// Gets image with the specified size and <see cref="KnownSvgColor.Normal"/> color.
        /// </summary>
        /// <param name="size">Image size in pixels.</param>
        /// <param name="isDark">Whether color theme is dark.</param>
        /// <returns></returns>
        public virtual ImageSet? AsNormal(int size, bool isDark)
        {
            return AsImageSet(size, KnownSvgColor.Normal, isDark);
        }

        /// <summary>
        /// Gets image with the specified size and <see cref="KnownSvgColor.Disabled"/> color.
        /// </summary>
        /// <param name="size">Image size in pixels.</param>
        /// <param name="isDark">Whether color theme is dark.</param>
        /// <returns></returns>
        public virtual ImageSet? AsDisabled(int size, bool isDark)
        {
            return AsImageSet(size, KnownSvgColor.Disabled, isDark);
        }

        /// <summary>
        /// Gets image with the specified size and <see cref="KnownSvgColor.Normal"/> color.
        /// </summary>
        /// <param name="size">Image size in pixels.</param>
        /// <param name="isDark">Whether color theme is dark.</param>
        /// <returns></returns>
        public virtual Image? AsNormalImage(int size, bool isDark)
        {
            return AsImageSet(size, KnownSvgColor.Normal, isDark)?.AsImage();
        }

        /// <summary>
        /// Gets image with the specified size and <see cref="KnownSvgColor.Disabled"/> color.
        /// </summary>
        /// <param name="size">Image size in pixels.</param>
        /// <param name="isDark">Whether color theme is dark.</param>
        /// <returns></returns>
        public virtual Image? AsDisabledImage(int size, bool isDark)
        {
            return AsImageSet(size, KnownSvgColor.Disabled, isDark)?.AsImage();
        }

        /// <summary>
        /// Gets image with the specified size and known svg color.
        /// </summary>
        /// <param name="size">Image size in pixels.</param>
        /// <param name="knownColor">Known image color.</param>
        /// <param name="isDark">Whether color theme is dark.</param>
        /// <returns></returns>
        public virtual ImageSet? AsImageSet(int size, KnownSvgColor knownColor, bool isDark)
        {
            if (!IsMono && knownColor == KnownSvgColor.Normal)
                return AsImageSet(size);

            Resize(size);
            var result = data[size]?.KnownColorImages[(int)knownColor];
            if (result is not null)
                return result;

            var color = SvgColors.GetSvgColor(knownColor, isDark);
            result = LoadImage(size, color);
            data[size] ??= new();
            data[size]!.KnownColorImages[(int)knownColor] = result;
            return result;
        }

        internal void Resize(int size)
        {
            if (size > short.MaxValue || size < 0)
                throw new ArgumentOutOfRangeException(nameof(size));

            if (data.Length <= size)
                Array.Resize(ref data, size + 1);
        }

        private ImageSet? LoadImage(int size, Color? color = null)
        {
            if (Stream is null)
                return null;
            Stream.Seek(0, SeekOrigin.Begin);
            var result = ImageSet.FromSvgStream(Stream, size, size, color);
            return result;
        }

        internal class Data
        {
            public ImageSet? OriginalImage;
            public ImageSet?[] KnownColorImages;

            public Data()
            {
                KnownColorImages = new ImageSet?[(int)KnownSvgColor.MaxValue + 1];
            }
        }
    }
}
