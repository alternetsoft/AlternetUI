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
            h.AddHandler<Alternet.Maui.BorderView, PlatformViewHandler>();
            h.AddHandler<Alternet.Maui.LabelView, PlatformViewHandler>();
            h.AddHandler<Alternet.Maui.PictureBoxView, PlatformViewHandler>();
            h.AddHandler<Alternet.Maui.SpeedButtonView, PlatformViewHandler>();
            h.AddHandler<Alternet.Maui.ToolBarView, PlatformViewHandler>();
            h.AddHandler<Alternet.Maui.TabControlView, PlatformViewHandler>();
            h.AddHandler<Alternet.Maui.VirtualListBoxView, PlatformViewHandler>();
            h.AddHandler<Alternet.Maui.VirtualTreeControlView, PlatformViewHandler>();
        });

        ControlView.InitMauiHandler();

        return builder;
    }
}