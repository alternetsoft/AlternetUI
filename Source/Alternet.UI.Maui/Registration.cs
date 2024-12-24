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
            h.AddHandler<ControlView, PlatformViewHandler>();
            h.AddHandler<BorderView, PlatformViewHandler>();
            h.AddHandler<LabelView, PlatformViewHandler>();
            h.AddHandler<PictureBoxView, PlatformViewHandler>();
            h.AddHandler<SpeedButtonView, PlatformViewHandler>();
            h.AddHandler<ToolBarView, PlatformViewHandler>();
            h.AddHandler<TabControlView, PlatformViewHandler>();
        });

        ControlView.InitMauiHandler();

        return builder;
    }
}