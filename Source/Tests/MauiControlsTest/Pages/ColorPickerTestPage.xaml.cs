using AllQuickStarts.Pages;

namespace AllQuickStarts;

public partial class ColorPickerTestPage : ContentPage
{
    static ColorPickerTestPage()
    {
    }

    public ColorPickerTestPage()
    {
        DemoTitleView titleView = new("ColorPickerView");
        NavigationPage.SetTitleView(this, titleView);

        InitializeComponent();

        if (!Alternet.UI.App.IsDesktopDevice)
        {
        }
        else
        {
        }

        BindingContext = this;

        if (!Alternet.UI.App.IsDesktopOs)
        {
        }

        if (!Alternet.UI.App.IsWindowsOS)
        {
        }
    }
}