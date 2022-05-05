using System;
using Alternet.UI;

namespace WindowPropertiesSample
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            UpdateControls();
        }

        TestWindow? testWindow;

        public TestWindow? TestWindow
        {
            get => testWindow;
            set
            {
                testWindow = value;
                UpdateControls();
            }
        }

        private void CreateAndShowWindowButton_Click(object sender, System.EventArgs e)
        {
            TestWindow = new TestWindow();
            TestWindow.Show();
            TestWindow.Closed += TestWindow_Closed;
        }

        void UpdateControls()
        {
            createAndShowWindowButton.Enabled = TestWindow == null;
        }


        private void TestWindow_Closed(object? sender, WindowClosedEventArgs e)
        {
            TestWindow = null;
        }
    }
}