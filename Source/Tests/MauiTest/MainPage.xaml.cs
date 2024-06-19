using System.Collections.ObjectModel;
using System.Diagnostics;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using SharpHook;

using CommunityToolkit.Maui.Core.Platform;

using Alternet.UI.Extensions;
using Microsoft.Maui.Graphics.Platform;

namespace SpinPaint;

public partial class MainPage : ContentPage
{
    private readonly Alternet.UI.SkiaSampleControl skiaSample;

    static MainPage()
    {
    }

    public MainPage()
    {
        InitializeComponent();

        skiaSample = new();

        skiaContainer.BackgroundColor = Colors.Cornsilk;

        skiaContainer.Control = skiaSample;

        panel.BackgroundColor = Colors.CornflowerBlue;
        panel.Padding = new(10);

        logControl.ItemsSource = MyItems;
        BindingContext = this;

        App.LogMessage += App_LogMessage;

        var ho = skiaContainer.HorizontalOptions;
        ho.Expands = true;
        ho.Alignment = LayoutAlignment.Fill;
        skiaContainer.HorizontalOptions = ho;

        var vo = skiaContainer.VerticalOptions;
        vo.Expands = true;
        vo.Alignment = LayoutAlignment.Fill;
        skiaContainer.VerticalOptions = vo;

        button1.Clicked += Button1_Clicked;
        button2.Clicked += Button2_Clicked;

        skiaContainer.HandlerChanged += SkiaContainer_HandlerChanged;
        skiaContainer.Focused += GraphicsView_Focused;
        skiaContainer.Unfocused += GraphicsView_Unfocused;
    }

    private void SkiaContainer_HandlerChanged(object? sender, EventArgs e)
    {
#if WINDOWS
        var platformView = skiaContainer.Handler?.PlatformView as SkiaSharp.Views.Windows.SKXamlCanvas;

        if (platformView is null)
            return;

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
        var platformView = skiaContainer.Handler?.PlatformView;

        if (platformView is null)
            return;

        /*
        platformView.HorizontalScrollBarEnabled = true;
        platformView.VerticalScrollBarEnabled = true;
        platformView.Focusable = true;
        platformView.FocusableInTouchMode = true;
        */
#endif

    }

    private void Log(object? s)
    {
        Alternet.UI.App.Log(s);
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
    }

    public ObservableCollection<SimpleItem> MyItems { get; set; } = new();

    private void App_LogMessage(object? sender, string e)
    {
        SimpleItem OneNewitem = new();
        OneNewitem.Text = e;
        MyItems.Add(OneNewitem);
        logControl.SelectedItem = MyItems[MyItems.Count - 1];
    }

    public class SimpleItem
    {
        public string Text { get; set; } = "";
    }
}