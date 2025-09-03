using AllQuickStarts.Pages;

namespace AllQuickStarts;

public partial class HomePage : ContentPage
{
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
        AddPage("VirtualTreeControlView", typeof(VirtualTreeControlExamplePage));

        (Content as IView).InvalidateArrange();
    }

    public void AddPage(string text, Type type)
    {
        var button = new Button { Text = text, Margin = 10 };
        button.Clicked += async (s, e) =>
        {
            var page = Activator.CreateInstance(type) as Page;
            await Navigation.PushAsync(page);
        };

        (Content as Layout)?.Add(button);
    }
}