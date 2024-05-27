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
        Alternet.UI.BaseApplication.Handler = new Alternet.UI.MauiApplicationHandler();
    }

    public MainPage()
    {
        var testForm = new DrawingSample.MainWindow();

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


        openLogFileButton.Clicked += OpenLogFileButton_Clicked;
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