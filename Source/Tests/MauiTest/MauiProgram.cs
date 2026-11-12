using SkiaSharp.Views.Maui.Controls.Hosting;
using CommunityToolkit.Maui;

using Alternet.UI;

namespace SpinPaint;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        
        builder.UseMauiApp<App>()
            .UseSkiaSharp()
            .UseAlternetUI()
            .UseMauiCommunityToolkit();

        return builder.Build();
    }
}