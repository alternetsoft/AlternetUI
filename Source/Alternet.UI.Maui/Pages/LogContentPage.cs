using System;
using System.Collections.Concurrent;
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
    private static readonly ConcurrentQueue<string> items = new();

    private static Alternet.UI.BaseDictionary<string, Action>? actions;
    private static Alternet.UI.BaseDictionary<string, Action>? testActions;

    private readonly CollectionView listView;
    private readonly TitleWithTwoButtonsView titleView;

    static LogContentPage()
    {
    }

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

        var itemsArray = items.ToArray();

        ObservableCollection<string> itemsClone = new(itemsArray);

        listView = new CollectionView
        {
            ItemsSource = itemsClone,

            /*
            ItemTemplate = new DataTemplate(() =>
            {
                var label = new Label
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Start,
                    Margin = new Thickness(10),
                };

                label.SetBinding(Label.TextProperty, ".");

                return label;
            }),
            */
        };

        Content = listView;
    }

    /// <summary>
    /// Gets debug actions dictionary.
    /// </summary>
    public Alternet.UI.BaseDictionary<string, Action> ActionsDictionary
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
    public Alternet.UI.BaseDictionary<string, Action> TestActionsDictionary
    {
        get
        {
            if (testActions is not null)
                return testActions;

            testActions = new();

            Alternet.UI.LogUtils.EnumLogActions(Fn);

            var members = Alternet.UI.AssemblyUtils.GetAllPublicMembers(
                "Test",
                Alternet.UI.KnownAssemblies.AllLoadedAlternet).ToArray();

            foreach(var member in members)
            {
                var item = Alternet.UI.LogUtils.ActionAndTitleFromTestMethod(member);
                if (item is null)
                    continue;
                testActions!.TryAdd(item.Value.Title, item.Value.Action);
            }

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
    /// Binds the application log to the log message event handler.
    /// </summary>
    public static void BindApplicationLog()
    {
        Alternet.UI.App.LogMessage -= OnLogMessage;
        Alternet.UI.App.LogMessage += OnLogMessage;
    }

    /// <summary>
    /// Shows actions dialog.
    /// </summary>
    public async Task ShowActionsDialog(
        string title,
        Alternet.UI.BaseDictionary<string, Action> actions)
    {
        string[] buttons = actions.Keys.Order().ToArray();

        string actionTitle = await DisplayActionSheetAsync(
            title,
            "Cancel",
            null,
            buttons);

        if (actionTitle is null || actionTitle == "Cancel")
            return;

        if (!actions.TryGetValue(actionTitle, out var action))
        {
            Alternet.UI.App.Log("Action not found: " + actionTitle);
            return;
        }

        action?.Invoke();
    }

    /// <inheritdoc/>
    protected override void DisposeResources()
    {
        base.DisposeResources();
    }

    private static void OnLogMessage(object? sender, UI.LogMessageEventArgs e)
    {
        if (e.Message is null)
            return;
        items.Enqueue(e.Message);
    }
}