using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Alternet.Maui;

/// <summary>
/// <see cref="ContentPage"/> descendant with view that shows log items.
/// </summary>
public partial class LogContentPage : Alternet.UI.DisposableContentPage
{
    private static LogContentPage? defaultPage;

    private readonly ObservableCollection<string> items = new();
    private readonly ListView listView;

    /// <summary>
    /// Initializes a new instance of the <see cref="LogContentPage"/> class.
    /// </summary>
    public LogContentPage()
    {
        var titleView = new StackLayout
        {
            Orientation = StackOrientation.Horizontal,
            Children =
                {
                    new Label
                    {
                        Text = "Application Log",
                        FontSize = 24,
                        Margin = 10,
                        VerticalOptions = LayoutOptions.Center,
                    },
                },
        };

        NavigationPage.SetTitleView(this, titleView);

        listView = new ListView
        {
            ItemsSource = items,
            ItemTemplate = new DataTemplate(() =>
            {
                var label = new Label
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Start,
                    Margin = new Thickness(10),
                };

                label.SetBinding(Label.TextProperty, ".");

                return new ViewCell { View = label };
            }),
        };

        Content = listView;

        Alternet.UI.App.LogMessage += OnLogMessage;
        Alternet.UI.App.LogRefresh += OnLogRefresh;
    }

    /// <summary>
    /// Gets default <see cref="LogContentPage"/>.
    /// </summary>
    public static LogContentPage Default
    {
        get
        {
            return defaultPage ??= new LogContentPage();
        }
    }

    /// <inheritdoc/>
    protected override void DisposeResources()
    {
        Alternet.UI.App.LogMessage -= OnLogMessage;
        Alternet.UI.App.LogRefresh -= OnLogRefresh;
        base.DisposeResources();
    }

    private void OnLogMessage(object? sender, UI.LogMessageEventArgs e)
    {
        if (e.Message is null)
            return;
        items.Add(e.Message);
    }

    private void OnLogRefresh(object? sender, EventArgs e)
    {
    }
}