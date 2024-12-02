using AllQuickStarts.Pages;

namespace AllQuickStarts;

public partial class PictureBoxTestPage : ContentPage
{
    static PictureBoxTestPage()
    {
    }

    public PictureBoxTestPage()
    {
        DemoTitleView titleView = new DemoTitleView("PictureBoxView");
        NavigationPage.SetTitleView(this, titleView);

        InitializeComponent();

        PropertyGridSample.ObjectInit.SetBackgrounds(pictureBoxView.Control);

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