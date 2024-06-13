using System.Collections.ObjectModel;
using System.Diagnostics;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using SharpHook;

using CommunityToolkit.Maui.Core.Platform;

namespace SpinPaint;

public partial class MainPage : ContentPage
{
    private readonly Alternet.UI.SkiaContainer skiaContainer = new();
    private readonly Alternet.UI.SkiaSampleControl skiaSample = new();

    static MainPage()
    {
    }

    public MainPage()
    {
        /*var testForm = new DrawingSample.MainWindow();*/

        skiaContainer.BackgroundColor = Colors.Cornsilk;
        skiaContainer.Margin = new(5);

        skiaContainer.Control = skiaSample;

        InitializeComponent();

        panel.BackgroundColor = Colors.CornflowerBlue;
        panel.Padding = new(10);

        ListView1.ItemsSource = MyItems;
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

        panel.Children.Add(skiaContainer);

        openLogFileButton.Clicked += OpenLogFileButton_Clicked;
        keyboardButton.Clicked += KeyboardButton_Clicked;

        Alternet.UI.App.Log("Hello Maui");
    }

    private void KeyboardButton_Clicked(object? sender, EventArgs e)
    {
        var handler = Alternet.UI.Keyboard.Handler;
        if (handler.IsSoftKeyboardShowing(null))
            handler.HideKeyboard(null);
        else
            handler.ShowKeyboard(null);
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