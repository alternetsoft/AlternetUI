using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Winforms
{
    /// <summary>
    /// Provides a set of methods to convert <see cref="Alternet.Drawing.SvgImage"/> to <see cref="Bitmap"/>.
    /// </summary>
    public class SvgImageBitmapsProvider : Alternet.Drawing.ISvgImageBitmapsProvider<Bitmap>
    {
        /// <summary>
        /// Gets the instance of <see cref="SvgImageBitmapsProvider"/>.
        /// </summary>
        public static SvgImageBitmapsProvider Instance { get; } = new SvgImageBitmapsProvider();

        /// <inheritdoc/>
        public Drawing.SizeI GetBitmapSize(Bitmap bitmap)
        {
            return new Drawing.SizeI(bitmap.Width, bitmap.Height);
        }

        /// <inheritdoc/>
        public Bitmap ToBitmap(Drawing.SvgImage svg, int width, int height, Drawing.Color? color = null)
        {
            if (color is null)
                return SkiaWinforms.ToBitmap(svg, width, height);
            else
                return SkiaWinforms.ToBitmap(svg, width, height, color);
        }

        /// <inheritdoc/>
        public Bitmap ToDisabledBitmap(Drawing.SvgImage svg, int width, int height, bool isDark)
        {
            return SkiaWinforms.ToDisabledBitmap(svg, width, height, isDark);
        }

        /// <inheritdoc/>
        public Bitmap ToNormalBitmap(Drawing.SvgImage svg, int width, int height, bool isDark)
        {
            return SkiaWinforms.ToNormalBitmap(svg, width, height, isDark);
        }
    }
}
