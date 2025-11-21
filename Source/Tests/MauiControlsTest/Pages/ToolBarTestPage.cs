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
            async () =>
            {
                await DisplayAlertAsync("Title", "Message", "OK");
            });

        var btn2 = toolbar.AddButton("Settings");

        var bold1 = toolbar.AddStickyButton(null, "1", Alternet.UI.KnownSvgImages.ImgBold);

        var bold2 = toolbar.AddStickyButton(null, "2", Alternet.UI.KnownSvgImages.ImgBold);

        var bold3 = toolbar.AddStickyButton(null, "3", Alternet.UI.KnownSvgImages.ImgBold);

        var labelItem = toolbar.AddLabel("Label");

        var btn1 = toolbar.AddButton("Disabled", null, Alternet.UI.KnownSvgImages.ImgItalic);
        btn1.IsEnabled = false;

        btn2.Clicked += (s, e) =>
        {
            btn1.IsEnabled = !btn1.IsEnabled;
        };

        toolbar.Remove(bold3);

        toolbar.AddExpandingSpace();
        toolbar.IsBottomBorderVisible = true;

        var btnRight = toolbar.AddButton("AtRight");

        var panel1 = new Grid();

        panel1.RowDefinitions =
        [
            new RowDefinition { Height = GridLength.Auto },
            new RowDefinition { Height = GridLength.Star },
        ];

        panel1.Add(toolbar, 0, 0);
        var collectionView1 = CollectionViewExamplePage.CreateSampleCollectionView();
        collectionView1.ItemsSource = CollectionViewExamplePage.SampleItems;
        collectionView1.SelectionMode = SelectionMode.Single;
        panel1.Add(collectionView1, 0, 1);

        var panel2 = new Grid();
        var collectionView2 = CollectionViewExamplePage.CreateSampleCollectionView();
        collectionView2.ItemsSource = CollectionViewExamplePage.SampleItems2;
        collectionView2.SelectionMode = SelectionMode.Single;
        panel2.Children.Add(collectionView2);

        var tabControl = new SimpleTabControlView();

        var tab1 = tabControl.Add("Tab with CollectionView", () => panel2);
        var tab2 = tabControl.Add("Tab with ToolBar", () => panel1);
        tabControl.SelectedTab = tab2;

        Content = tabControl;
    }
}
