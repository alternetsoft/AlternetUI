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
    internal class TwoColorSvgImage : SvgImage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TwoColorSvgImage"/> class.
        /// </summary>
        /// <remarks>
        /// See <see cref="SvgImage.SvgImage(string, SvgImageDataKind)"/> for the details.
        /// </remarks>
        public TwoColorSvgImage(string urlOrData, SvgImageDataKind kind = SvgImageDataKind.Url)
            : base(urlOrData, kind)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TwoColorSvgImage"/> class.
        /// </summary>
        /// <remarks>
        /// See <see cref="SvgImage.SvgImage(Stream)"/> for the details.
        /// </remarks>
        public TwoColorSvgImage(Stream stream)
            : base(stream)
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
