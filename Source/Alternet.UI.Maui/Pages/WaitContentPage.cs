using System;
using System.Threading.Tasks;

using Microsoft.Maui.Controls;
using Microsoft.Maui.Dispatching;

namespace Alternet.Maui;

/// <summary>
/// <see cref="ContentPage"/> descendant with 'Please wait' label.
/// </summary>
public partial class WaitContentPage : Alternet.UI.DisposableContentPage
{
    private static WaitContentPage? defaultPage;

    /// <summary>
    /// Initializes a new instance of the <see cref="WaitContentPage"/> class.
    /// </summary>
    public WaitContentPage()
    {
        Content = new VerticalStackLayout
        {
            Children =
            {
                new Label
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Text = Alternet.UI.Localization.CommonStrings.Default.LoadingPleaseWait,
                },
            },
        };
    }

    /// <summary>
    /// Gets default <see cref="WaitContentPage"/>.
    /// </summary>
    public static WaitContentPage Default
    {
        get
        {
            return defaultPage ??= new WaitContentPage();
        }
    }

    /// <summary>
    /// Shows wait page, pushes page returned by <paramref name="getPageFunc"/>
    /// and hides wait page.
    /// </summary>
    /// <param name="navigation">Navigation interface used to show pages.</param>
    /// <param name="getPageFunc">Function which returns page to show.</param>
    /// <param name="useWaitPage">Whether to show wait page.</param>
    /// <returns></returns>
    public void NavigationPush(
        INavigation navigation,
        Func<Page> getPageFunc,
        bool useWaitPage = true)
    {
        if (useWaitPage)
        {
            navigation.PushAsync(this)
            .ContinueWith(
                (t) =>
                {
                    Dispatcher.Dispatch(() =>
                    {
                        var page = getPageFunc();
                        navigation.PushAsync(page);
                    });
                })
            .ContinueWith(
                (t) =>
                {
                    navigation.RemovePage(this);
                });
        }
        else
        {
            var page = getPageFunc();
            navigation.PushAsync(page).Wait();
        }
    }
}