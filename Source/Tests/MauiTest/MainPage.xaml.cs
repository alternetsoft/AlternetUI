using System.Collections.ObjectModel;
using System.Diagnostics;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using SharpHook;

using CommunityToolkit.Maui.Core.Platform;

namespace SpinPaint;

public partial class MainPage : ContentPage, IDrawable
{
    private readonly Alternet.UI.SkiaSampleControl skiaSample;

    static MainPage()
    {
    }

    public MainPage()
    {
        /*var testForm = new DrawingSample.MainWindow();*/

        InitializeComponent();

        skiaSample = new();

        skiaContainer.BackgroundColor = Colors.Cornsilk;

        skiaContainer.Control = skiaSample;

        panel.BackgroundColor = Colors.CornflowerBlue;
        panel.Padding = new(10);

        ListView1.ItemsSource = MyItems;
        BindingContext = this;

        App.LogMessage += App_LogMessage;

        /*var ho = skiaContainer.HorizontalOptions;
        ho.Expands = true;
        ho.Alignment = LayoutAlignment.Fill;
        skiaContainer.HorizontalOptions = ho;

        var vo = skiaContainer.VerticalOptions;
        vo.Expands = true;
        vo.Alignment = LayoutAlignment.Fill;
        skiaContainer.VerticalOptions = vo;*/

        button1.Clicked += Button1_Clicked;
        button2.Clicked += Button2_Clicked;

        skiaContainer.WidthRequest = 350;
        graphicsView.WidthRequest = 350;
        graphicsView.Drawable = this;

        graphicsView.Focused += GraphicsView_Focused;
        graphicsView.Unfocused += GraphicsView_Unfocused;
        graphicsView.StartInteraction += GraphicsView_StartInteraction;

    }

    private void GraphicsView_StartInteraction(object? sender, TouchEventArgs e)
    {
#if WINDOWS
        var platformView = graphicsView.Handler?.PlatformView as Microsoft.Maui.Graphics.Platform.PlatformGraphicsView;

        if (platformView is null)
            return;

        platformView.Focus(Microsoft.UI.Xaml.FocusState.Pointer);
#endif
    }

    private void GraphicsView_Unfocused(object? sender, FocusEventArgs e)
    {
        Alternet.UI.App.Log("GraphicsView_Unfocused");
    }

    private void GraphicsView_Focused(object? sender, FocusEventArgs e)
    {
        Alternet.UI.App.Log("GraphicsView_Focused");
    }

    private void Button2_Clicked(object? sender, EventArgs e)
    {
        Alternet.UI.MauiUtils.EnumViewsToLog(panel);
        Alternet.UI.AppUtils.OpenLogFile();
        /*
        var handler = Alternet.UI.Keyboard.Handler;
        if (handler.IsSoftKeyboardShowing(null))
            handler.HideKeyboard(null);
        else
            handler.ShowKeyboard(null);
        */
    }

    private void Button1_Clicked(object? sender, EventArgs e)
    {
        skiaContainer.Log($"Uses {graphicsView.Handler?.PlatformView?.GetType().Name}");

#if WINDOWS

        var platformView = graphicsView.Handler?.PlatformView as Microsoft.Maui.Graphics.Platform.PlatformGraphicsView;

        if (platformView is null)
            return;

        platformView.AllowFocusOnInteraction = true;
        platformView.FocusEngaged += (s, e) =>
        {
            skiaContainer.Log("Focused");
        };

#endif

        /*skiaContainer.Log($"Uses {skiaContainer.Handler?.PlatformView?.GetType().Name}");
        Alternet.UI.MauiUtils.AddAllViewsToParent(panel);*/
    }

    public ObservableCollection<SimpleItem> MyItems { get; set; } = new();

    private void App_LogMessage(object? sender, string e)
    {
        SimpleItem OneNewitem = new();
        OneNewitem.Text = e;
        MyItems.Add(OneNewitem);
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.FillColor = Colors.Yellow;
        canvas.FillRectangle(dirtyRect);
    }

    public class SimpleItem
    {
        public string Text { get; set; } = "";
    }
}