using Alternet.UI;

namespace ControlsTest
{
    internal partial class MainTestWindow : Window, ITestPageSite
    {
        private int lastEventNumber = 1;

        public MainTestWindow()
        {
            InitializeComponent();

            var pages = pageContainer.Pages;

            pages.Add(new PageContainer.Page("Web Browser", new WebBrowserTestPage { Site = this }));

            Grid.SetRow(eventsListBox, 1);

            pageContainer.SelectedIndex = 0;
        }

        void ITestPageSite.LogEvent(string message)
        {
            eventsListBox.Items.Add($"{lastEventNumber++}. {message}");
            eventsListBox.SelectedIndex = eventsListBox.Items.Count - 1;
        }
    }
}