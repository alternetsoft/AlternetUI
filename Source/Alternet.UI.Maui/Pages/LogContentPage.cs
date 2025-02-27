using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

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
    private static Alternet.UI.AdvDictionary<string, Action>? actions;
    private static Alternet.UI.AdvDictionary<string, Action>? testActions;

    private readonly ObservableCollection<string> items = new();
    private readonly ListView listView;
    private readonly TitleWithTwoButtonsView titleView;

    /// <summary>
    /// Initializes a new instance of the <see cref="LogContentPage"/> class.
    /// </summary>
    public LogContentPage()
    {
        titleView = new("Application Log", this);
        titleView.SettingsButton.IsVisible = true;
        titleView.KeyboardButton.IsVisible = false;

        titleView.SettingsButtonClick += async (s, e) =>
        {
            await ShowActionsDialog("Run debug action", ActionsDictionary);
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

        Alternet.UI.App.Log("Hello, this is log.");
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

    /// <summary>
    /// Gets debug actions dictionary.
    /// </summary>
    public Alternet.UI.AdvDictionary<string, Action> ActionsDictionary
    {
        get
        {
            if (actions is not null)
                return actions;

            actions = new();

            Alternet.UI.LogUtils.EnumLogActions(Fn);

            Fn(
                "> Show Test Actions Dialog",
                async () => await ShowActionsDialog("Test actions", TestActionsDictionary));

            void Fn(string title, Action action)
            {
                if (title.StartsWith("Test "))
                    return;
                actions!.TryAdd(title, action);
            }

            return actions;
        }
    }

    /// <summary>
    /// Gets test actions dictionary.
    /// </summary>
    public Alternet.UI.AdvDictionary<string, Action> TestActionsDictionary
    {
        get
        {
            if (testActions is not null)
                return testActions;

            testActions = new();

            Alternet.UI.LogUtils.EnumLogActions(Fn);

            void Fn(string title, Action action)
            {
                if (!title.StartsWith("Test "))
                    return;
                testActions!.TryAdd(title, action);
            }

            return testActions;
        }
    }

    /// <summary>
    /// Gets title view.
    /// </summary>
    public TitleWithTwoButtonsView TitleView => titleView;

    /// <summary>
    /// Shows actions dialog.
    /// </summary>
    public async Task ShowActionsDialog(
        string title,
        Alternet.UI.AdvDictionary<string, Action> actions)
    {
        string[] buttons = actions.Keys.Order().ToArray();

        string actionTitle = await DisplayActionSheet(
            title,
            "Cancel",
            null,
            buttons);

        if (actionTitle is null)
            return;

        if (!ActionsDictionary.TryGetValue(actionTitle, out var action))
        {
            Alternet.UI.App.Log("Action not found: " + actionTitle);
            return;
        }

        action?.Invoke();
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