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
    /// <see cref="SvgImage"/> descendant which works with color svg images.
    /// </summary>
    public class ColorSvgImage : SvgImage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorSvgImage"/> class.
        /// </summary>
        /// <param name="urlOrData">Image url or data.</param>
        /// <param name="kind">Image data kind.</param>
        /// <param name="throwException">Indicates whether to throw an exception
        /// if loading fails.</param>
        public ColorSvgImage(
            string urlOrData,
            SvgImageDataKind kind = SvgImageDataKind.Url,
            bool throwException = true)
            : base(urlOrData, kind, throwException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorSvgImage"/> class.
        /// </summary>
        /// <param name="stream">Stream with image data.</param>
        /// <param name="throwException">Indicates whether to throw an
        /// exception if loading fails.</param>
        public ColorSvgImage(Stream stream, bool throwException = true)
            : base(stream, throwException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorSvgImage"/> class.
        /// </summary>
        public ColorSvgImage()
        {
        }

        /// <inheritdoc/>
        public override SvgImageNumOfColors NumOfColors => SvgImageNumOfColors.Many;

        /// <summary>
        /// Creates <see cref="ColorSvgImage"/> from the specified svg file.
        /// </summary>
        /// <param name="path">Path to file.</param>
        /// <param name="throwException">Indicates whether to throw an
        /// exception if loading fails.</param>
        /// <returns></returns>
        public static ColorSvgImage FromFile(string path, bool throwException = true)
        {
            try
            {
                using var stream = FileSystem.Default.OpenRead(path);
                var result = new ColorSvgImage(stream, throwException);
                return result;
            }
            catch (Exception e)
            {
                if (!throwException)
                {
                    var result = new ColorSvgImage(null!, false);
                    result.LoadingError = e;
                    return result;
                }

                throw;
            }
        }
    }
}
