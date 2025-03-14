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
    /// <see cref="SvgImage"/> descendant which works with mono (single color) svg images.
    /// </summary>
    public class MonoSvgImage : SvgImage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MonoSvgImage"/> class
        /// loading it from the specified url or xml string.
        /// </summary>
        /// <param name="urlOrData">Image url or data.</param>
        /// <param name="kind">Image data kind.</param>
        /// <param name="throwException">Indicates whether to throw
        /// an exception if loading fails.</param>
        public MonoSvgImage(
            string urlOrData,
            SvgImageDataKind kind = SvgImageDataKind.Url,
            bool throwException = true)
            : base(urlOrData, kind, throwException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MonoSvgImage"/> class
        /// loading it from the specified stream.
        /// </summary>
        /// <param name="stream">Stream with image data.</param>
        /// <param name="throwException">Indicates whether to throw an
        /// exception if loading fails.</param>
        public MonoSvgImage(Stream stream, bool throwException = true)
            : base(stream, throwException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MonoSvgImage"/> class.
        /// </summary>
        public MonoSvgImage()
        {
        }

        /// <inheritdoc/>
        public override SvgImageNumOfColors NumOfColors => SvgImageNumOfColors.One;

        /// <summary>
        /// Creates <see cref="MonoSvgImage"/> from the specified svg file.
        /// </summary>
        /// <param name="path">Path to file.</param>
        /// <returns></returns>
        public static MonoSvgImage FromFile(string path)
        {
            using var stream = FileSystem.Default.OpenRead(path);
            var result = new MonoSvgImage(stream);
            return result;
        }
    }
}
