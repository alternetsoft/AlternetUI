using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if IOS || MACCATALYST

using SkiaSharp.Views.iOS;
using SkiaSharp.Views.Maui.Handlers;

using UIKit;

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
            return new PlatformView
            {
                BackgroundColor = UIColor.Clear,
            };
        }
    }
}

#endif