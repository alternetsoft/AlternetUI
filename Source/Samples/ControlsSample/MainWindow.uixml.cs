using Alternet.UI;

namespace ControlsSample
{
    internal partial class MainWindow : Window, IPageSite
    {
        private int lastEventNumber = 1;
        private PageContainer pageContainer = new PageContainer();
        private ListBox eventsListBox = new ListBox();

        public MainWindow()
        {
            InitializeComponent();

            mainGrid.Children.Add(pageContainer);
            mainGrid.Children.Add(eventsListBox);
            Alternet.UI.Grid.SetRow(pageContainer, 0);
            Alternet.UI.Grid.SetRow(eventsListBox, 1);

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

            Grid.SetRow(eventsListBox, 1);

            pageContainer.SelectedIndex = 0;
        }

        void IPageSite.LogEvent(string message)
        {
            eventsListBox.Items.Add($"{lastEventNumber++}. {message}");
            eventsListBox.SelectedIndex = eventsListBox.Items.Count - 1;
        }
    }
}