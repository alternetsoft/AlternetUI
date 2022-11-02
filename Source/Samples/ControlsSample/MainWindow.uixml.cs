using Alternet.UI;

namespace ControlsSample
{
    internal partial class MainWindow : Window, IPageSite
    {
        private int lastEventNumber = 1;

        public MainWindow()
        {
            InitializeComponent();

            var pages = pageContainer.Pages;

            pages.Add(new PageContainer.Page("Tree View", new TreeViewPage { Site = this }));
            pages.Add(new PageContainer.Page("Grid", new GridPage { Site = this }));
            pages.Add(new PageContainer.Page("List View", new ListViewPage { Site = this }));
            pages.Add(new PageContainer.Page("List Box", new ListBoxPage { Site = this }));
            pages.Add(new PageContainer.Page("Combo Box", new ComboBoxPage { Site = this }));
            pages.Add(new PageContainer.Page("Progress Bar", new ProgressBarPage { Site = this }));
            pages.Add(new PageContainer.Page("Slider", new SliderPage { Site = this }));
            pages.Add(new PageContainer.Page("Numeric Input", new NumericInputPage { Site = this }));
            pages.Add(new PageContainer.Page("Radio Buttons", new RadioButtonsPage { Site = this }));
            pages.Add(new PageContainer.Page("Check Boxes", new CheckBoxesPage { Site = this }));
            pages.Add(new PageContainer.Page("Text Input", new TextInputPage { Site = this }));
            pages.Add(new PageContainer.Page("Notify Icon", new NotifyIconPage { Site = this }));

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