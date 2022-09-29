using System;
using System.IO;
using Alternet.UI;

namespace CommonDialogsSample
{
    public partial class MainWindow : Window
    {
        private const string CustomTitle = @"Custom Title";

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnInitialize()
        {
            InitializeEnumComboBox<MessageBoxButtons>(messageBoxButtonsComboBox);
            InitializeEnumComboBox<MessageBoxDefaultButton>(messageBoxDefaultButtonComboBox);
            InitializeEnumComboBox<MessageBoxIcon>(messageBoxIconComboBox);
            InitializeEnumComboBox<TestExceptionType>(exceptionTypeComboBox);
        }

        enum TestExceptionType
        {
            InvalidOperationException,
            FormatException,
            FileNotFoundException,
        }

        void InitializeEnumComboBox<TEnum>(ComboBox comboBox)
        {
            comboBox.Items.Clear();

            foreach (var value in Enum.GetValues(typeof(TEnum)))
                comboBox.Items.Add(value);

            comboBox.SelectedIndex = 0;
        }

        string GetInitialDirectory()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        }

        private void ShowOpenFileDialogButton_Click(object sender, EventArgs e)
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
                dialog.InitialDirectory = GetInitialDirectory();

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
                dialog.InitialDirectory = GetInitialDirectory();

            if (setCustomTitleCheckBox.IsChecked)
                dialog.Title = CustomTitle;

            var result = dialog.ShowModal(this);

            if (result == ModalResult.Accepted)
                MessageBox.Show("Accepted, FileName = " + dialog.DirectoryName, "Dialog Result");
            else
                MessageBox.Show(result.ToString(), "Dialog Result");
        }

        private void ShowMessageBoxButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                var result = MessageBox.Show(
                    "Message Box Text",
                    "Message Box Caption",
                    (MessageBoxButtons)messageBoxButtonsComboBox.SelectedItem!,
                    (MessageBoxIcon)messageBoxIconComboBox.SelectedItem!,
                    (MessageBoxDefaultButton)messageBoxDefaultButtonComboBox.SelectedItem!);

                MessageBox.Show("Result: " + result, "Message Box Result");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Message Box Argument Exception");
            }
        }

        private void ThrowExceptionButton_Click(object sender, EventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                MessageBox.Show("Run this application without debugging to see the thread exception window.", "Common Dialogs Sample");
                return;
            }

            throw (TestExceptionType)exceptionTypeComboBox.SelectedItem! switch
            {
                TestExceptionType.InvalidOperationException => new InvalidOperationException("Test message"),
                TestExceptionType.FormatException => new FormatException("Test message"),
                TestExceptionType.FileNotFoundException => new FileNotFoundException("Test message", "MyFileName.dat"),
                _ => throw new Exception(),
            };
        }
    }
}