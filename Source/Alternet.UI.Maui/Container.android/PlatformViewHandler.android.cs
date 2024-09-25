using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SkiaSharp.Views.Maui.Handlers;

#if ANDROID

using SkiaSharp.Views.Android;

namespace Alternet.UI
{
    /// <summary>
    /// Overrides <see cref="SKCanvasViewHandler"/> and changes
    /// platform view to the <see cref="PlatformView"/>.
    /// </summary>
    public class PlatformViewHandler : SKCanvasViewHandler
    {
        /// <inheritdoc/>
        protected override SKCanvasView CreatePlatformView()
        {
            return new PlatformView(Context);
        }
    }
}

#endif