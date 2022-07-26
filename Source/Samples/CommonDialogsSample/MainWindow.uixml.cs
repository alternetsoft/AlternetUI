using System;
using Alternet.UI;

namespace CommonDialogsSample
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ShowOpenFileDialogButton_Click(object sender, System.EventArgs e)
        {
            var dialog = new OpenFileDialog();
            var result = dialog.ShowModal();

            if (result == ModalResult.Accepted)
                MessageBox.Show("Accepted, FileName = " + dialog.FileName, "Dialog Result");
            else
                MessageBox.Show(result.ToString(), "Dialog Result");
        }

        private void ShowSaveFileDialogButton_Click(object sender, System.EventArgs e)
        {
            var dialog = new SaveFileDialog();
            var result = dialog.ShowModal();

            if (result == ModalResult.Accepted)
                MessageBox.Show("Accepted, FileName = " + dialog.FileName, "Dialog Result");
            else
                MessageBox.Show(result.ToString(), "Dialog Result");
        }

        private void ShowSelectDirectoryDialogButton_Click(object sender, System.EventArgs e)
        {
            var dialog = new SelectDirectoryDialog();
            var result = dialog.ShowModal();

            if (result == ModalResult.Accepted)
                MessageBox.Show("Accepted, FileName = " + dialog.DirectoryName, "Dialog Result");
            else
                MessageBox.Show(result.ToString(), "Dialog Result");
        }
    }
}