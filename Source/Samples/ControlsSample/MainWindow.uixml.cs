using Alternet.UI;
using Alternet.Drawing;

namespace ControlsSample
{
    internal partial class MainWindow : Window, IPageSite
    {
        private string? lastEventMessage = null;
        private int lastEventNumber = 1;
        private PageContainer pageContainer = new();
        private TreeView eventsControl = new();
        private Grid mainGrid = new();
        private Control mainGridParent = new();
        private StackPanel mainPanel = new();

        public MainWindow()
        {
            Icon = ImageSet.FromUrlOrNull("embres:ControlsSample.Sample.ico");
            InitializeComponent();

            eventsControl.FullRowSelect = true;
            eventsControl.ShowRootLines = false;
            eventsControl.ShowLines = false;

            mainGrid.RowDefinitions.Add(new RowDefinition
            {
                Height = new GridLength(1, GridUnitType.Auto)
            }
            );
            mainGrid.RowDefinitions.Add(new RowDefinition
            {
                Height = new GridLength(100, GridUnitType.Star)
            }
            );
            mainGrid.RowDefinitions.Add(new RowDefinition
                {
                    Height = new GridLength(150, GridUnitType.Pixel)
                }
            );

            var headerPanel = new HorizontalStackPanel()
            {
                HorizontalAlignment = HorizontalAlignment.Right,
            };

            var homePage = @"https://www.alternet-ui.com/";
            var docsHomePage = @"https://docs.alternet-ui.com/";
            var linkLabel = new LinkLabel()
            {
                Text = "Home Page",
                Margin = new Thickness(5,0,5,10),
                Url = homePage,
            };
            var linkLabel2 = new LinkLabel()
            {
                Text = "Documentation",
                Margin = new Thickness(5,0,10,10),
                Url = $"{docsHomePage}introduction/getting-started.html",
            };
            headerPanel.Children.Add(linkLabel);
            headerPanel.Children.Add(linkLabel2);
            //LayoutFactory.SetDebugBackgroundToParents(linkLabel2);

            eventsControl.Margin = new(0,10,0,0);
            mainGrid.Children.Add(headerPanel);
            mainGrid.Children.Add(pageContainer);
            mainGrid.Children.Add(eventsControl);
            Alternet.UI.Grid.SetRow(headerPanel, 0);
            Alternet.UI.Grid.SetRow(pageContainer, 1);
            Alternet.UI.Grid.SetRow(eventsControl, 2);

            var pages = pageContainer.Pages;

            pages.Add(new PageContainer.Page("Tree View", new TreeViewPage { Site = this }));
            pages.Add(new PageContainer.Page("List View", new ListViewPage { Site = this }));
            pages.Add(new PageContainer.Page("List Box", new ListBoxPage { Site = this }));
            pages.Add(new PageContainer.Page("Combo Box", new ComboBoxPage { Site = this }));
            pages.Add(new PageContainer.Page("Check List Box", new CheckListBoxPage { Site = this }));
            pages.Add(new PageContainer.Page("Tab Control", new TabControlPage { Site = this }));
            pages.Add(new PageContainer.Page("Progress Bar", new ProgressBarPage { Site = this }));
            pages.Add(new PageContainer.Page("Button", new ButtonPage { Site = this }));
            pages.Add(new PageContainer.Page("Slider", new SliderPage { Site = this }));
            pages.Add(new PageContainer.Page("Grid", new GridPage { Site = this }));
            pages.Add(new PageContainer.Page("Numeric Input", new NumericInputPage { Site = this }));
            pages.Add(new PageContainer.Page("Radio Buttons", new RadioButtonsPage { Site = this }));
            pages.Add(new PageContainer.Page("Check Boxes", new CheckBoxesPage { Site = this }));
            pages.Add(new PageContainer.Page("Text Input", new TextInputPage { Site = this }));
            pages.Add(new PageContainer.Page("Date Time", new DateTimePage { Site = this }));
            pages.Add(new PageContainer.Page("Notify Icon", new NotifyIconPage { Site = this }));
            pages.Add(new PageContainer.Page("Web Browser", new WebBrowserPage { Site = this }));
            pages.Add(new PageContainer.Page(
                "Splitter Panel", 
                new SplitterPanelPage { Site = this }));

            pageContainer.SelectedIndex = 0;

            mainGridParent.Padding = 10;
            mainGridParent.Children.Add(mainGrid);
            Children.Add(mainGridParent);
        }

        string? IPageSite.LastEventMessage => lastEventMessage;

        void IPageSite.LogEvent(string message)
        {
            lastEventMessage = message;

            var s = $"{lastEventNumber++}. {message}";
            var item = new TreeViewItem(s);
            eventsControl.Items.Add(item);
            eventsControl.SelectedItem = item;
        }
    }
}