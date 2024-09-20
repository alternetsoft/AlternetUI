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
    /// Overrides view handler and changes platform view to the <see cref="SKCanvasViewAdv"/>.
    /// </summary>
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