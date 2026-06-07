using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.Drawing
{
    public partial class SvgImage
    {
        /// <summary>
        /// Creates new <see cref="SvgImageSet"/> and loads there this svg image
        /// with the specified size and color.
        /// </summary>
        /// <param name="size">Svg image size in pixels.</param>
        /// <param name="color">Color of the mono svg image. Optional.</param>
        /// <returns></returns>
        public SvgImageSet? CreateSvgImageSet(int size, Color? color = null)
        {
            return CreateSvgImageSet((size, size), color);
        }

        /// <summary>
        /// Creates new <see cref="SvgImageSet"/> and loads there this svg image
        /// with the specified size and color.
        /// </summary>
        /// <param name="size">Svg image size in pixels.</param>
        /// <param name="color">Color of the mono svg image. Optional.</param>
        /// <returns>Image set containing the loaded SVG image. Returned image set is immutable.</returns>
        public virtual SvgImageSet? CreateSvgImageSet(SizeI size, Color? color = null)
        {
            SvgImageSet result = new (this, size, color);
            result.SetImmutable();
            return result;
        }

    }
}
