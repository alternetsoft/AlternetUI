using AllQuickStarts.Pages;

namespace AllQuickStarts;

public partial class BorderTestPage : ContentPage
{
    static BorderTestPage()
    {
    }

    public BorderTestPage()
    {
        DemoTitleView titleView = new DemoTitleView("BorderView");
        NavigationPage.SetTitleView(this, titleView);

        InitializeComponent();

        PropertyGridSample.ObjectInit.SetBackgrounds(borderView.Control);

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