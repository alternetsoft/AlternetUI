using AllQuickStarts.Pages;

namespace AllQuickStarts;

public partial class SpeedButtonTestPage : ContentPage
{
    static SpeedButtonTestPage()
    {
    }

    public SpeedButtonTestPage()
    {
        DemoTitleView titleView = new DemoTitleView("SpeedButtonView");
        NavigationPage.SetTitleView(this, titleView);

        InitializeComponent();

        labelView.Control.Text = "Hello";
        PropertyGridSample.ObjectInit.SetBackgrounds(labelView.Control);

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