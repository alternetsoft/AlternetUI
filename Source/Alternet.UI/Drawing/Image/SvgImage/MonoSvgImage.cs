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
        /// Initializes a new instance of the <see cref="MonoSvgImage"/> class.
        /// </summary>
        /// <remarks>
        /// See <see cref="SvgImage.SvgImage(string, SvgImageDataKind)"/> for the details.
        /// </remarks>
        public MonoSvgImage(string urlOrData, SvgImageDataKind kind = SvgImageDataKind.Url)
            : base(urlOrData, kind)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MonoSvgImage"/> class.
        /// </summary>
        /// <remarks>
        /// See <see cref="SvgImage.SvgImage(Stream)"/> for the details.
        /// </remarks>
        public MonoSvgImage(Stream stream)
            : base(stream)
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
    }
}
