using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Allows to get known color svg images as <see cref="ImageSet"/> instances.
    /// </summary>
    public class KnownColorSvgImages
    {
        private static readonly AdvDictionary<SizeI, KnownColorSvgImages> Images = new();

        private readonly SizeI size;

        private ImageSet? imgWarning;
        private ImageSet? imgError;
        private ImageSet? imgInformation;

        /// <summary>
        /// Initializes a new instance of the <see cref="KnownColorSvgImages"/> class.
        /// </summary>
        /// <param name="size">Images size.</param>
        public KnownColorSvgImages(SizeI size)
        {
            this.size = size;
        }

        /// <summary>
        /// Gets images size.
        /// </summary>
        public SizeI Size => size;

        /// <summary>
        /// Gets or sets 'Error' image.
        /// </summary>
        public ImageSet ImgError
        {
            get => imgError ??= Load(KnownColorSvgUrls.Error);
            set => imgError = value;
        }

        /// <summary>
        /// Gets or sets 'Warning' image.
        /// </summary>
        public ImageSet ImgWarning
        {
            get => imgWarning ??= Load(KnownColorSvgUrls.Warning);
            set => imgWarning = value;
        }

        /// <summary>
        /// Gets or sets 'Information' image.
        /// </summary>
        public ImageSet ImgInformation
        {
            get => imgInformation ??= Load(KnownColorSvgUrls.Information);
            set => imgInformation = value;
        }

        /// <summary>
        /// Gets <see cref="KnownColorSvgImages"/> for the specified bitmap size.
        /// </summary>
        /// <param name="size">Image size.</param>
        public static KnownColorSvgImages GetForSize(SizeI size)
        {
            var result = Images.GetOrCreate(size, () => new KnownColorSvgImages(size));
            return result;
        }

        /// <summary>
        /// Gets <see cref="KnownColorSvgImages"/> for the specified bitmap size.
        /// </summary>
        /// <param name="size">Image size.</param>
        public static KnownColorSvgImages GetForSize(int size)
            => GetForSize(new SizeI(size));

        internal ImageSet? LoadIfExists(string? url)
        {
            if (url is null)
                return null;
            return AuiToolbar.LoadSvgImage(url, size);
        }

        private ImageSet Load(string url)
        {
            return AuiToolbar.LoadSvgImage(url, size);
        }
    }
}