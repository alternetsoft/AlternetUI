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

        private SizeI defaultSize;

        /// <inheritdoc/>
        public virtual SizeI DefaultSize
        {
            get
            {
                if (defaultSize.AnyIsEmptyOrNegative)
                {
                    if (Images.Count > 0)
                        return Images[0].Size;
                    return 16;
                }

                return defaultSize;
            }

            set
            {
                defaultSize = value;
            }
        }

        /// <inheritdoc/>
        public virtual bool LoadFromStream(Stream stream)
        {
            var bitmap = Bitmap.FromStream(stream);
            if (bitmap.IsOk)
                return Add(bitmap);
            return false;
        }

        /// <inheritdoc/>
        public virtual Image AsImage(SizeI size)
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

                    if (newDistance.Width < oldDistance.Width
                        && newDistance.Height < oldDistance.Height)
                        result = bitmap;
                }
            }

            return result ?? Images.First() ?? Bitmap.Empty;
        }

        /// <inheritdoc/>
        public virtual SizeI GetPreferredBitmapSizeAtScale(Coord scale)
        {
            return new((int)(DefaultSize.Width * scale), (int)(DefaultSize.Height * scale));
        }

        /// <inheritdoc/>
        public virtual SizeI GetPreferredBitmapSizeFor(IControl control)
        {
            return GetPreferredBitmapSizeAtScale(control.ScaleFactor);
        }
    }
}
