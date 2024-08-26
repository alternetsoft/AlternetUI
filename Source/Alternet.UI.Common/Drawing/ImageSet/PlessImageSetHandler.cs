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
    /// Implements dummy <see cref="IImageSetHandler"/> provider.
    /// </summary>
    public class PlessImageSetHandler : ImageContainer, IImageSetHandler
    {
        /// <summary>
        /// Gets default dummy <see cref="IImageSetHandler"/> provider.
        /// </summary>
        public static IImageSetHandler Default = new PlessImageSetHandler();

        /// <inheritdoc/>
        public SizeI DefaultSize { get; set; }

        /// <inheritdoc/>
        bool IImageSetHandler.LoadFromStream(Stream stream)
        {
            var bitmap = Bitmap.FromStream(stream);
            if (bitmap.IsOk)
                return Add(bitmap);
            return false;
        }

        /// <inheritdoc/>
        public Image AsImage(SizeI size)
        {
            Image? result = null;

            foreach(var bitmap in Images)
            {
                if (result is null)
                    result = bitmap;
                else
                {
                    var newDistance = SizeI.Subtract(bitmap.Size, size).Abs;
                    var oldDistance = SizeI.Subtract(result.Size, size).Abs;

                    if (newDistance.Width < oldDistance.Width && newDistance.Height < oldDistance.Height)
                        result = bitmap;
                }
            }

            return result ?? Images.First() ?? Bitmap.Empty;
        }

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
