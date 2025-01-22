using AllQuickStarts.Pages;

namespace AllQuickStarts;

public partial class TabControlTestPage : ContentPage
{
    static TabControlTestPage()
    {
    }

    public TabControlTestPage()
    {
        DemoTitleView titleView = new DemoTitleView("TabControlView");
        NavigationPage.SetTitleView(this, titleView);

        InitializeComponent();

        PropertyGridSample.ObjectInit.InitGenericTabControl(tabControlView.Control, false);

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