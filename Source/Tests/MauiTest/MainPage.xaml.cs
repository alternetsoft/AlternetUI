using System.Collections.ObjectModel;
using System.Diagnostics;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using SharpHook;

namespace SpinPaint;

public partial class MainPage : ContentPage
{
    private readonly Alternet.UI.SkiaContainer skiaContainer = new();
    private readonly Alternet.UI.SampleControl skiaSample = new();
    private readonly Button button = new();

    static MainPage()
    {
        Alternet.UI.MauiPlatform.Initialize();
    }

    public MainPage()
    {
        skiaContainer.BackgroundColor = Colors.Cornsilk;
        skiaContainer.Margin = new(5);

        skiaContainer.HeightRequest = 300;
        skiaContainer.WidthRequest = 500;
        skiaContainer.Control = skiaSample;

        button.Text = "Hello";

        InitializeComponent();

        panel.BackgroundColor = Colors.CornflowerBlue;
        panel.Padding = new(10);

        ListView1.ItemsSource = MyItems;
        BindingContext = this;

        App.LogMessage += App_LogMessage;

        //panel.Children.Add(mauiContainer);
        //panel.Children.Add(button);
        panel.Children.Add(skiaContainer);

        Alternet.UI.BaseApplication.LogFileIsEnabled = true;
        Alternet.UI.BaseApplication.Log($"Pixel width: {DeviceDisplay.Current.MainDisplayInfo.Width} / Pixel Height: {DeviceDisplay.Current.MainDisplayInfo.Height}");
        Alternet.UI.BaseApplication.Log($"Density: {DeviceDisplay.Current.MainDisplayInfo.Density}");
        Alternet.UI.BaseApplication.Log($"Orientation: {DeviceDisplay.Current.MainDisplayInfo.Orientation}");
        Alternet.UI.BaseApplication.Log($"Rotation: {DeviceDisplay.Current.MainDisplayInfo.Rotation}");
        Alternet.UI.BaseApplication.Log($"Refresh Rate: {DeviceDisplay.Current.MainDisplayInfo.RefreshRate}");

        openLogFileButton.Clicked += OpenLogFileButton_Clicked;
        /*
            Pixel width: 3840 / Pixel Height: 2160
            18:53:04 :: Density: 2
            18:53:04 :: Orientation: Landscape
            18:53:04 :: Rotation: Rotation0
            18:53:05 :: Refresh Rate: 60
        */

    }

    private void OpenLogFileButton_Clicked(object? sender, EventArgs e)
    {
        Alternet.UI.AppUtils.OpenLogFile();
    }

    public ObservableCollection<SimpleItem> MyItems { get; set; } = new();

    private void App_LogMessage(object? sender, string e)
    {
        SimpleItem OneNewitem = new();
        OneNewitem.Text = e;
        MyItems.Add(OneNewitem);
    }

    public class SimpleItem
    {
        public string Text { get; set; } = "";
    }
}