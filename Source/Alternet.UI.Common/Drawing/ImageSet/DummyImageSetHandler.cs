using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Implements dummy <see cref="IImageSetHandler"/> provider.
    /// </summary>
    public class DummyImageSetHandler : DummyImageContainer, IImageSetHandler
    {
        /// <summary>
        /// Gets default dummy <see cref="IImageSetHandler"/> provider.
        /// </summary>
        public static IImageSetHandler Default = new DummyImageSetHandler();

        /// <inheritdoc/>
        public SizeI DefaultSize { get; }

        /// <inheritdoc/>
        public SizeI GetPreferredBitmapSizeAtScale(Coord scale)
        {
            return new((int)(DefaultSize.Width * scale), (int)(DefaultSize.Height * scale));
        }

        /// <inheritdoc/>
        public SizeI GetPreferredBitmapSizeFor(IControl control)
        {
            return GetPreferredBitmapSizeAtScale(control.ScaleFactor);
        }
    }
}
