using AllQuickStarts.Pages;

namespace AllQuickStarts;

public partial class SpeedButtonTestPage : ContentPage
{
    static SpeedButtonTestPage()
    {
    }

    public SpeedButtonTestPage()
    {
        DemoTitleView titleView = new("SpeedButtonView");
        NavigationPage.SetTitleView(this, titleView);

        InitializeComponent();

        PropertyGridSample.ObjectInit.InitSpeedButton(speedButtonView.Control);

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