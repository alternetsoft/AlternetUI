using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if WINDOWS

using SkiaSharp.Views.Maui.Handlers;
using SkiaSharp.Views.Windows;

namespace Alternet.UI
{
    /// <summary>
    /// Overrides view handler and changes platform view to the <see cref="SKCanvasViewAdv"/>.
    /// </summary>
    public class SKCanvasViewHandlerAdv : SKCanvasViewHandler
    {
        /// <inheritdoc/>
        protected override SKXamlCanvas CreatePlatformView()
        {
            return new SKCanvasViewAdv();
        }
    }
}

#endif