using System.Collections.ObjectModel;
using System.Diagnostics;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using SharpHook;

namespace SpinPaint;

public partial class MainPage : ContentPage
{
    private readonly Alternet.UI.MauiContainer mauiContainer = new();
    private readonly Alternet.UI.SkiaContainer skiaContainer = new();
    private readonly Alternet.UI.SampleControl mauiSample = new();
    private readonly Alternet.UI.SampleControl skiaSample = new();
    private readonly Button button = new();

    static MainPage()
    {
        Alternet.UI.MauiPlatform.Initialize();
    }

    public MainPage()
    {
        mauiContainer.HeightRequest = 100;
        mauiContainer.WidthRequest = 100;
        skiaContainer.HeightRequest = 100;
        skiaContainer.WidthRequest = 100;
        mauiContainer.Control = mauiSample;
        skiaContainer.Control = skiaSample;

        button.Text = "Hello";

        InitializeComponent();

        ListView1.ItemsSource = MyItems;
        BindingContext = this;

        App.LogMessage += App_LogMessage;

        panel.Children.Add(mauiContainer);
        panel.Children.Add(button);
        panel.Children.Add(skiaContainer);
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