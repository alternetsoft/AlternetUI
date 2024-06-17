using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui.Hosting;

using SkiaSharp.Views.Maui.Handlers;

namespace Alternet.UI;

public static class Registration
{
    public static MauiAppBuilder UseAlternetUI(this MauiAppBuilder builder)
    {
        builder.ConfigureMauiHandlers(h =>
        {
            h.AddHandler<SkiaContainer, SKCanvasViewHandler>();
        });

        return builder;
    }
}