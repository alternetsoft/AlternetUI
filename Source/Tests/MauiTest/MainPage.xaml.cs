using System.Collections.ObjectModel;
using System.Diagnostics;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

using CommunityToolkit.Maui.Core.Platform;

using Alternet.UI.Extensions;
using Microsoft.Maui.Graphics.Platform;

namespace SpinPaint;

public partial class MainPage : ContentPage
{
    internal readonly Alternet.UI.PaintActionsControl customDrawControl;
    
    static MainPage()
    {
        Alternet.UI.DebugUtils.DebugCallIf(true, () =>
        {
            Alternet.UI.PlessMouse.ShowTestMouseInControl = true;
        });
    }

    public MainPage()    
    {
        Alternet.UI.DebugUtils.RegisterExceptionsLogger();

        InitializeComponent();

        logControl.SelectionMode = SelectionMode.Single;

        customDrawControl = new();
        customDrawControl.Name = "customDrawControl";

        customDrawControl.SetPaintAction((control, canvas, rect) =>
        {
        });

        skiaContainer.BackgroundColor = Colors.Cornsilk;

        skiaContainer.Interior.Required();
        skiaContainer.Interior.HorzScrollBar?.SetVisible(false);

        skiaContainer.Control = customDrawControl;

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

        Alternet.UI.StaticControlEvents.FocusedChanged += Control_FocusedControlChanged;

        colorPicker.SelectedColor = Alternet.Drawing.Color.LightBlue;
        colorPicker.SelectedIndexChanged += (s, e) =>
        {
            Log($"Color changed: {colorPicker.SelectedColor?.NameLocalized}, {colorPicker.SelectedColor}");
        };

        /*
        labelView.Control.Text = "Hello";
        PropertyGridSample.ObjectInit.SetBackgrounds(labelView.Control);

        pictureBoxView.Control.Image = Alternet.UI.KnownSvgImages.ImgFileSave.AsImage(64);
        pictureBoxView.UseUnscaledDrawImage = true;
        pictureBoxView.Control.ImageStretch = false;
        pictureBoxView.Control.HasBorder = true;
        PropertyGridSample.ObjectInit.SetBackgrounds(pictureBoxView.Control);
        */
    }

    private void Control_FocusedControlChanged(object? sender, EventArgs e)
    {
        var name = Alternet.UI.AbstractControl.FocusedControl?.Name ?? "null";
        Log($"FocusedControlChanged: {name}");
    }

    private void SkiaContainer_HandlerChanged(object? sender, EventArgs e)
    {
#if WINDOWS
        if (skiaContainer.Handler?.PlatformView is not SkiaSharp.Views.Windows.SKXamlCanvas platformView)
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
            var alternetArgs = Alternet.UI.MauiKeyboardHandler.Default.Convert(null!, e);
            Log($" KeyPress => {alternetArgs.KeyChar}");
        };
        platformView.KeyDown += (s, e) =>
        {
            var alternetArgs = Alternet.UI.MauiKeyboardHandler.Default.Convert(null!, e);
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

    private async void Button1_Clicked(object? sender, EventArgs e)
    {
        await Alternet.UI.Clipboard.SetDataObjectAsync(new Alternet.UI.DataObject("Hello"));
        var result = await Alternet.UI.Clipboard.GetDataObjectAsync();
        var test = result?.ToString();
        Alternet.UI.App.LogIf(test, false);
    }

    public ObservableCollection<SimpleItem> MyItems { get; set; } = new();

    private void App_LogMessage(object? sender, string e)
    {
        SimpleItem OneNewItem = new();
        OneNewItem.Text = e;
        MyItems.Add(OneNewItem);
        logControl.SelectedItem = MyItems[MyItems.Count - 1];

        this.Title = e;
    }

    public class SimpleItem
    {
        public string Text { get; set; } = "";
    }
}