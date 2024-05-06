using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains extension methods for standard classes.
    /// </summary>
    public static class MethodExtensions
    {
        /// <summary>
        /// Get the size that would be best to use for this <see cref="ImageSet"/> at the DPI
        /// scaling factor used by the given control.
        /// </summary>
        /// <param name="control">Control to get DPI scaling factor from.</param>
        /// <returns></returns>
        /// <remarks>
        /// This is just a convenient wrapper for
        /// <see cref="ImageSet.GetPreferredBitmapSizeAtScale"/> calling
        /// that function with the result of <see cref="Control.GetPixelScaleFactor"/>.
        /// </remarks>
        /// <param name="imageSet"><see cref="ImageSet"/> instance.</param>
        public static SizeI GetPreferredBitmapSizeFor(this ImageSet imageSet, IControl control)
        {
            return ((UI.Native.ImageSet)imageSet.NativeObject)
                .GetPreferredBitmapSizeFor(WxPlatformControl.WxWidget(control));
        }
    }
}
