using Alternet.UI;

namespace MenuSample
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenMenuItem_Click(object sender, System.EventArgs e) => MessageBox.Show("Open");

        private void ExportToPdfMenuItem_Click(object sender, System.EventArgs e) => MessageBox.Show("Export to PDF");

        private void ExportToPngMenuItem_Click(object sender, System.EventArgs e) => MessageBox.Show("Export to PNG");

        private void ExitMenuItem_Click(object sender, System.EventArgs e) => Close();

        private void AboutMenuItem_Click(object sender, System.EventArgs e) => MessageBox.Show("AlterNET UI Menu Sample Application.", "About");
    }
}