using AllQuickStarts.Pages;

namespace AllQuickStarts;

public partial class HomePage : ContentPage
{
    private readonly WaitPage waitPage = new();

    public HomePage()
    {
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

        AddPage("LabelView", typeof(LabelTestPage));
        AddPage("ColorPickerView", typeof(ColorPickerTestPage));
        AddPage("BorderView", typeof(BorderTestPage));
        AddPage("PictureBoxView", typeof(PictureBoxTestPage));
        AddPage("SpeedButtonView", typeof(SpeedButtonTestPage));
        AddPage("ToolBarView", typeof(ToolBarTestPage));
        AddPage("TabControlView", typeof(TabControlTestPage));
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