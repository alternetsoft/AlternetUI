using Alternet.Drawing;
using Alternet.UI;

namespace WindowPropertiesSample
{
    public partial class TestWindow : Window
    {
        public TestWindow()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, System.EventArgs e)
        {
            ModalResult = ModalResult.Accepted;
            Close();
        }

        private void CancelButton_Click(object sender, System.EventArgs e)
        {
            ModalResult = ModalResult.Canceled;
            Close();
        }

        private void ShowModalButton_Click(object sender, System.EventArgs e)
        {
            var testWindow = new TestWindow();

            testWindow.ShowModal();
            MessageBox.Show("ModalResult: " + testWindow.ModalResult);
            testWindow.Dispose();
        }
    }
}
