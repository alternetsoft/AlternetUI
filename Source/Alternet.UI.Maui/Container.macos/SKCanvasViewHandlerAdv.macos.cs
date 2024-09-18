using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if MACCATALYST

using SkiaSharp.Views.iOS;
using SkiaSharp.Views.Maui.Handlers;

using UIKit;

namespace Alternet.UI
{
    public class SKCanvasViewHandlerAdv : SKCanvasViewHandler
    {
        /// <inheritdoc/>
        protected override SKCanvasView CreatePlatformView()
        {
            return new SKCanvasViewAdv
            {
                BackgroundColor = UIColor.Clear,
            };
        }
    }
}

#endif