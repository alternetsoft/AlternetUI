using AllQuickStarts.Pages;

namespace AllQuickStarts;

public partial class HomePage : ContentPage
{
    private readonly Alternet.Maui.WaitContentPage waitPage = new();

    static HomePage()
    {
    }

    public HomePage()
    {
        Alternet.UI.DebugUtils.RegisterExceptionsLogger((e) =>
        {
        });

        Content = new VerticalStackLayout
        {
            Children = {
                new Label
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Text = "AlterNET MAUI Controls Test",
                    Margin = 10,
                }
            }
        };

        AddPage("CollectionView", typeof(CollectionViewExamplePage));
        AddPage("SimpleToolBarView", typeof(ToolBarTestPage));
        AddPage("ColorPickerView", typeof(ColorPickerTestPage));

        AddPage("TabbedPage", typeof(TabbedPageTest));
        /*
        AddPage("LabelView", typeof(LabelTestPage));
        AddPage("BorderView", typeof(BorderTestPage));
        AddPage("PictureBoxView", typeof(PictureBoxTestPage));
        AddPage("SpeedButtonView", typeof(SpeedButtonTestPage));
        AddPage("TabControlView", typeof(TabControlTestPage));
        */
        (Content as IView).InvalidateArrange();
    }

    public void AddPage(string text, Type type)
    {
        var button = new Button { Text = text, Margin = 10 };
        button.Clicked += async (s, e) =>
        {
            await Dispatcher.DispatchAsync(() =>
            {
                return Navigation.PushAsync(waitPage);
            });
            var page = Activator.CreateInstance(type) as Page;
            await Navigation.PushAsync(page);
            try
            {
            }
            finally
            {
                Navigation.RemovePage(waitPage);
            }
        };

        (Content as Layout)?.Add(button);
    }
}