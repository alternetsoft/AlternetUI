using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.Drawing
{
    /// <summary>
    /// <see cref="SvgImage"/> descendant which works with two color svg images.
    /// </summary>
    internal partial class TwoColorSvgImage : SvgImage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TwoColorSvgImage"/> class
        /// loading it from the specified url or xml string.
        /// </summary>
        /// <param name="urlOrData">Image url or data.</param>
        /// <param name="kind">Image data kind.</param>
        /// <param name="throwException">Indicates whether to throw
        /// an exception if loading fails.</param>
        public TwoColorSvgImage(
            string urlOrData,
            SvgImageDataKind kind = SvgImageDataKind.Url,
            bool throwException = true)
            : base(urlOrData, kind, throwException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TwoColorSvgImage"/> class
        /// loading it from the specified stream.
        /// </summary>
        /// <param name="stream">Stream with image data.</param>
        /// <param name="throwException">Indicates whether to throw an
        /// exception if loading fails.</param>
        public TwoColorSvgImage(Stream stream, bool throwException = true)
            : base(stream, throwException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TwoColorSvgImage"/> class.
        /// </summary>
        public TwoColorSvgImage()
        {
        }

        /// <inheritdoc/>
        public override SvgImageNumOfColors NumOfColors => SvgImageNumOfColors.Two;
    }
}
