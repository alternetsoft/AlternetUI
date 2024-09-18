using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui.Hosting;

using SkiaSharp.Views.Maui.Handlers;

namespace Alternet.UI;

/// <summary>
/// Extends <see cref="MauiAppBuilder"/> with Alternet.UI registration method.
/// </summary>
public static class Registration
{
    /// <summary>
    /// Alternet.UI registration method.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static MauiAppBuilder UseAlternetUI(this MauiAppBuilder builder)
    {
        builder.ConfigureMauiHandlers(h =>
        {
#if MACCATALYST
            h.AddHandler<SkiaContainer, SKCanvasViewHandlerAdv>();
#else
            h.AddHandler<SkiaContainer, SKCanvasViewHandler>();
#endif
        });

        return builder;
    }
}