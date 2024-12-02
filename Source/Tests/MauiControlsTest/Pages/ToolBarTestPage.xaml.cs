using AllQuickStarts.Pages;

namespace AllQuickStarts;

public partial class ToolBarTestPage : ContentPage
{
    static ToolBarTestPage()
    {
    }

    public ToolBarTestPage()
    {
        DemoTitleView titleView = new DemoTitleView("ToolBarView");
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