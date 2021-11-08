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

            pages.Add(new PageContainer.Page("Tree View", new TreeViewPage(this)));
            pages.Add(new PageContainer.Page("Grid", new GridPage(this)));
            pages.Add(new PageContainer.Page("List View", new ListViewPage(this)));
            pages.Add(new PageContainer.Page("List Box", new ListBoxPage(this)));
            pages.Add(new PageContainer.Page("Combo Box", new ComboBoxPage(this)));
            pages.Add(new PageContainer.Page("Progress Bar", new ProgressBarPage(this)));
            pages.Add(new PageContainer.Page("Slider", new SliderPage(this)));
            pages.Add(new PageContainer.Page("Numeric Input", new NumericInputPage(this)));
            pages.Add(new PageContainer.Page("Radio Buttons", new RadioButtonsPage(this)));
            pages.Add(new PageContainer.Page("Check Boxes", new CheckBoxesPage(this)));
            pages.Add(new PageContainer.Page("Text Input", new TextInputPage(this)));

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