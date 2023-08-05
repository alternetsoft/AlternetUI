using Alternet.UI;
using Alternet.Drawing;

namespace ControlsSample
{
    internal partial class MainWindow : Window, IPageSite
    {
        private readonly PageContainer pageContainer = new();
        private readonly TreeView eventsControl = new();
        private readonly Grid mainGrid = new();
        private readonly Control mainGridParent = new();
        private readonly LinkLabel? linkLabel;
        private readonly LinkLabel? linkLabel2;
        private string? lastEventMessage = null;
        private int lastEventNumber = 1;

        public MainWindow()
        {
            Icon = ImageSet.FromUrlOrNull("embres:ControlsSample.Sample.ico");
            InitializeComponent();

            eventsControl.MakeAsListBox();

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
            linkLabel = new LinkLabel()
            {
                Text = "Home Page",
                Margin = new Thickness(5,0,5,10),
                Url = homePage,
            };
            linkLabel.LinkClicked += LinkLabel_LinkClicked;
            
            linkLabel2 = new LinkLabel()
            {
                Text = "Documentation",
                Margin = new Thickness(5,0,10,10),
                Url = $"{docsHomePage}introduction/getting-started.html",
            };
            linkLabel2.LinkClicked += LinkLabel_LinkClicked;

            headerPanel.Children.Add(linkLabel);
            headerPanel.Children.Add(linkLabel2);

            eventsControl.Margin = new(0,10,0,0);
            mainGrid.Children.Add(headerPanel);
            mainGrid.Children.Add(pageContainer);
            mainGrid.Children.Add(eventsControl);
            Alternet.UI.Grid.SetRow(headerPanel, 0);
            Alternet.UI.Grid.SetRow(pageContainer, 1);
            Alternet.UI.Grid.SetRow(eventsControl, 2);

            var pages = pageContainer.Pages;

            void AddPage(string title, Control control)
            {
                pages.Add(new PageContainer.Page(title,control));
            }

            AddPage("Tree View", new TreeViewPage { Site = this });
            AddPage("List View", new ListViewPage { Site = this });
            AddPage("List Box", new ListBoxPage { Site = this });
            AddPage("Combo Box", new ComboBoxPage { Site = this });
            AddPage("Check List Box", new CheckListBoxPage { Site = this });
            AddPage("Tab Control", new TabControlPage { Site = this });
            AddPage("Progress Bar", new ProgressBarPage { Site = this });
            AddPage("Button", new ButtonPage { Site = this });
            AddPage("Slider", new SliderPage { Site = this });
            AddPage("Grid", new GridPage { Site = this });
            AddPage("Numeric Input", new NumericInputPage { Site = this });
            AddPage("Radio Button", new RadioButtonsPage { Site = this });
            AddPage("Check Box", new CheckBoxesPage { Site = this });
            AddPage("Text Input", new TextInputPage { Site = this });
            AddPage("Date Time", new DateTimePage { Site = this });
            AddPage("Notify Icon", new NotifyIconPage { Site = this });
            AddPage("Web Browser", new WebBrowserPage { Site = this });
            AddPage("Splitter Panel", new SplitterPanelPage { Site = this });
            AddPage("Layout Panel",new LayoutPanelPage { Site = this });
            AddPage("All Samples", new AllSamplesPage { Site = this });

            pageContainer.SelectedIndex = 0;

            mainGridParent.Padding = 10;
            mainGridParent.Children.Add(mainGrid);
            Children.Add(mainGridParent);
        }

        private void LinkLabel_LinkClicked(
            object? sender,
            System.ComponentModel.CancelEventArgs e)
        {
            if (sender is not LinkLabel linkLabel)
                return;
            LogEvent(linkLabel.Url);
        }

        string? IPageSite.LastEventMessage => lastEventMessage;

        void IPageSite.LogEvent(string message) => LogEvent(message);

        void LogEvent(string message)
        {
            lastEventMessage = message;

            var s = $"{lastEventNumber++}. {message}";
            var item = new TreeViewItem(s);
            eventsControl.Items.Add(item);
            eventsControl.SelectedItem = item;
        }
    }
}