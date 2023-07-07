using Alternet.UI;

namespace ControlsSample
{
    internal partial class MainWindow : Window, IPageSite
    {
        private int lastEventNumber = 1;
        private PageContainer pageContainer = new();
        private TreeView eventsControl = new();
        private Grid mainGrid = new();
        private StackPanel mainPanel = new();

        public MainWindow()
        {
            InitializeComponent();

            eventsControl.FullRowSelect = true;
            eventsControl.ShowRootLines = false;
            eventsControl.ShowLines = false;

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


            mainGrid.Children.Add(pageContainer);
            mainGrid.Children.Add(eventsControl);
            Alternet.UI.Grid.SetRow(pageContainer, 0);
            Alternet.UI.Grid.SetRow(eventsControl, 1);

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

            pageContainer.SelectedIndex = 0;

            Children.Add(mainGrid);
        }

        void IPageSite.LogEvent(string message)
        {
            var s = $"{lastEventNumber++}. {message}";
            var item = new TreeViewItem(s);
            eventsControl.Items.Add(item);
            eventsControl.SelectedItem = item;
        }
    }
}