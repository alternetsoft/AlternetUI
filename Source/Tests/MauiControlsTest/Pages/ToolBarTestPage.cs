using AllQuickStarts.Pages;

namespace AllQuickStarts;

using Microsoft.Maui.Controls;

using Alternet.Maui;

public partial class ToolBarTestPage : ContentPage
{
    public ToolBarTestPage()
    {
        var toolbar = new SimpleToolBarView();
        toolbar.AddButton("Search");
        toolbar.AddButton("Settings");

        var panel = new VerticalStackLayout();

        panel.Children.Add(toolbar);

        Content = panel;
    }
}
