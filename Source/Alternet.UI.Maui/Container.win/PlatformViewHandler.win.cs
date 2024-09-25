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
    /// Overrides <see cref="SKCanvasViewHandler"/> and changes
    /// platform view to the <see cref="PlatformView"/>.
    /// </summary>
    public class PlatformViewHandler : SKCanvasViewHandler
    {
        /// <inheritdoc/>
        protected override SKXamlCanvas CreatePlatformView()
        {
            return new PlatformView();
        }
    }
}

#endif