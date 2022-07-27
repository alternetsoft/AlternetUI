using System;
using Alternet.UI;

namespace CommonDialogsSample
{
    public partial class MainWindow : Window
    {
        private const string InitialDirectory = @"C:\Users";
        private const string CustomTitle = @"Custom Title";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ShowOpenFileDialogButton_Click(object sender, System.EventArgs e)
        {
            var dialog = new OpenFileDialog();

            SetFileDialogProperties(dialog);

            dialog.AllowMultipleSelection = allowMultipleSelectionCheckBox.IsChecked;

            var result = dialog.ShowModal(this);

            if (result == ModalResult.Accepted)
            {
                if (dialog.AllowMultipleSelection)
                    MessageBox.Show("Accepted, FileNames = " + String.Join(", ", dialog.FileNames), "Dialog Result");
                else
                    MessageBox.Show("Accepted, FileName = " + dialog.FileName, "Dialog Result");
            }
            else
                MessageBox.Show(result.ToString(), "Dialog Result");
        }

        private void SetFileDialogProperties(FileDialog dialog)
        {
            if (setInitialDirectoryCheckBox.IsChecked)
                dialog.InitialDirectory = InitialDirectory;

            if (setCustomTitleCheckBox.IsChecked)
                dialog.Title = CustomTitle;

            if (setFilterCheckBox.IsChecked)
            {
                dialog.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
                dialog.SelectedFilterIndex = 1;
            }
        }

        private void ShowSaveFileDialogButton_Click(object sender, System.EventArgs e)
        {
            var dialog = new SaveFileDialog();

            SetFileDialogProperties(dialog);

            var result = dialog.ShowModal(this);

            if (result == ModalResult.Accepted)
                MessageBox.Show("Accepted, FileName = " + dialog.FileName, "Dialog Result");
            else
                MessageBox.Show(result.ToString(), "Dialog Result");
        }

        private void ShowSelectDirectoryDialogButton_Click(object sender, System.EventArgs e)
        {
            var dialog = new SelectDirectoryDialog();

            if (setInitialDirectoryCheckBox.IsChecked)
                dialog.InitialDirectory = InitialDirectory;

            if (setCustomTitleCheckBox.IsChecked)
                dialog.Title = CustomTitle;

            var result = dialog.ShowModal(this);

            if (result == ModalResult.Accepted)
                MessageBox.Show("Accepted, FileName = " + dialog.DirectoryName, "Dialog Result");
            else
                MessageBox.Show(result.ToString(), "Dialog Result");
        }
    }
}