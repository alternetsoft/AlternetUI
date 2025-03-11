using AllQuickStarts.Pages;

namespace AllQuickStarts;

using Microsoft.Maui.Controls;

using Alternet.Maui;

public partial class ToolBarTestPage : ContentPage
{
    public ToolBarTestPage()
    {
        var setBackgroundColor = false;

        var toolbar = new SimpleToolBarView();

        if(setBackgroundColor)
            toolbar.BackgroundColor = Colors.DarkKhaki;

        toolbar.AddButton(
            "Search",
            "This is tooltip",
            Alternet.UI.KnownSvgImages.ImgArrowDown,
            () =>
            {
            });

        var btn2 = toolbar.AddButton("Settings");

        toolbar.AddStickyButton(null, null, Alternet.UI.KnownSvgImages.ImgBold);

        toolbar.AddStickyButton(null, null, Alternet.UI.KnownSvgImages.ImgBold);

        toolbar.AddLabel("Label");

        var btn1 = toolbar.AddButton("Disabled", null, Alternet.UI.KnownSvgImages.ImgItalic);
        btn1.IsEnabled = false;

        btn2.Clicked += (s, e) =>
        {
            btn1.IsEnabled = !btn1.IsEnabled;
        };

        toolbar.AddExpandingSpace();

        var btnRight = toolbar.AddButton("AtRight");


        var tabControl = new SimpleTabControlView();

        tabControl.Header.AddStickyButton("Tab 1");
        tabControl.Header.AddStickyButton("Tab 2");
        tabControl.SelectFirstTab();

        var panel = new VerticalStackLayout();

        panel.Children.Add(tabControl);
        panel.Children.Add(toolbar);

        Content = panel;
    }
}
