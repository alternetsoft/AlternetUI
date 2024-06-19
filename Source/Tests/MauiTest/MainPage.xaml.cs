using System.Collections.ObjectModel;
using System.Diagnostics;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using SharpHook;

using CommunityToolkit.Maui.Core.Platform;

using Alternet.UI.Extensions;

namespace SpinPaint;

public partial class MainPage : ContentPage, IDrawable
{
    private readonly Alternet.UI.SkiaSampleControl skiaSample;

#if WINDOWS
    private readonly Alternet.UI.SkiaWritableBitmap bitmap = new();
#endif

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

        logControl.ItemsSource = MyItems;
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
        graphicsView.HandlerChanged += GraphicsView_HandlerChanged;
        graphicsView.HandlerChanging += GraphicsView_HandlerChanging;

        var platformView = graphicsView.GetPlatformView();

        if (platformView is not null)
        {

        }
    }

    private void GraphicsView_HandlerChanging(object? sender, HandlerChangingEventArgs e)
    {
    }

    private void GraphicsView_HandlerChanged(object? sender, EventArgs e)
    {
        var platformView = graphicsView.GetPlatformView();

        if (platformView is null)
            return;


#if WINDOWS

        platformView.AllowFocusOnInteraction = true;
        platformView.IsTabStop = true;
        platformView.PointerEntered += (s, e) =>
        {

        };
        platformView.PointerExited += (s, e) =>
        {

        };
        platformView.PointerPressed += (s, e) =>
        {

        };
        platformView.PointerWheelChanged += (s, e) =>
        {

        };
        platformView.PointerReleased += (s, e) =>
        {

        };
        platformView.CharacterReceived += (s, e) =>
        {
            var alternetArgs = Alternet.UI.MauiKeyboardHandler.Convert(null!, e);
            Log($" KeyPress => {alternetArgs.KeyChar}");
        };
        platformView.KeyDown += (s, e) =>
        {
            var alternetArgs = Alternet.UI.MauiKeyboardHandler.Convert(null!, e);
            var isPressed = Alternet.UI.Keyboard.IsKeyDown(alternetArgs.Key);
            Log($"KeyDown {e.Key} => {alternetArgs.Key} {isPressed}");
            Window.Title = alternetArgs.ToString();
        };
#elif IOS || MACCATALYST
#elif ANDROID
        platformView.HorizontalScrollBarEnabled = true;
        platformView.VerticalScrollBarEnabled = true;
        platformView.Focusable = true;
        platformView.FocusableInTouchMode = true;
#endif

    }

    private void Log(object? s)
    {
        Alternet.UI.App.Log(s);
    }

    private void GraphicsView_StartInteraction(object? sender, TouchEventArgs e)
    {
        graphicsView.Focus(Alternet.UI.FocusState.Pointer);
    }

    private void GraphicsView_Unfocused(object? sender, FocusEventArgs e)
    {
        Log("GraphicsView_Unfocused");
    }

    private void GraphicsView_Focused(object? sender, FocusEventArgs e)
    {
        Log("GraphicsView_Focused");
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
        var platformView = graphicsView.GetPlatformView();

        if (platformView is null)
            return;
    }

    public ObservableCollection<SimpleItem> MyItems { get; set; } = new();

    private void App_LogMessage(object? sender, string e)
    {
        SimpleItem OneNewitem = new();
        OneNewitem.Text = e;
        MyItems.Add(OneNewitem);
        logControl.SelectedItem = MyItems[MyItems.Count - 1];
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.FillColor = Colors.Yellow;
        canvas.FillRectangle(dirtyRect);

#if WINDOWS
        bitmap.ActualHeight = graphicsView.Height;
        bitmap.ActualWidth = graphicsView.Width;
        bitmap.Dpi = (float)graphicsView.Scale;

        bitmap.DoInvalidate(OnPaintSurface);

        void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;

            canvas.Clear(SKColors.Brown);
        }
#endif

    }

    public class SimpleItem
    {
        public string Text { get; set; } = "";
    }
}