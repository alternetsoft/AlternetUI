using Microsoft.Maui.Controls;

namespace Alternet.Maui;

/// <summary>
/// <see cref="ContentPage"/> descendant with 'Please wait' label.
/// </summary>
public partial class WaitContentPage : ContentPage
{
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
}