using System;
using System.IO;
using Alternet.Drawing;
using Alternet.UI;

namespace ControlsSample
{
    public partial class CommonDialogsWindow : Window
    {
        private const string CustomTitle = @"Custom Title";
        private FontInfo fontInfo = Control.DefaultFont;

        public CommonDialogsWindow()
        {
            this.SuspendLayout();
            Icon = Application.DefaultIcon;
            InitializeComponent();

            messageBoxButtonsComboBox.Add(MessageBoxButtons.OK);
            messageBoxButtonsComboBox.Add(MessageBoxButtons.OKCancel);
            messageBoxButtonsComboBox.Add(MessageBoxButtons.YesNoCancel);
            messageBoxButtonsComboBox.Add(MessageBoxButtons.YesNo);
            messageBoxButtonsComboBox.SelectedItem = MessageBoxButtons.OKCancel;

            messageBoxIconComboBox.AddEnumValues(typeof(MessageBoxIcon), MessageBoxIcon.None);            
            exceptionTypeComboBox.AddEnumValues(typeof(TestExceptionType), TestExceptionType.FileNotFoundException);

            messageBoxDefaultButtonComboBox.Add(MessageBoxDefaultButton.Button1);
            messageBoxDefaultButtonComboBox.Add(MessageBoxDefaultButton.Button2);
            messageBoxDefaultButtonComboBox.Add(MessageBoxDefaultButton.Button3);
            messageBoxDefaultButtonComboBox.SelectedItem = MessageBoxDefaultButton.Button1;

            this.ResumeLayout();
        }

        enum TestExceptionType
        {
            InvalidOperationException,
            FormatException,
            FileNotFoundException,
        }

        internal void InitEnumComboBox<TEnum>(ComboBox comboBox)
        {
            comboBox.Items.Clear();

            foreach (var value in Enum.GetValues(typeof(TEnum)))
                comboBox.Items.Add(value);

            comboBox.SelectedIndex = 0;
        }

        private string GetInitialDirectory()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        }

        private void ShowOpenFileDialogButton_Click(object? sender, EventArgs e)
        {
            ResultMessage = "";

            var dialog = new OpenFileDialog();

            SetFileDialogProperties(dialog);

            dialog.AllowMultipleSelection = allowMultipleSelectionCheckBox.IsChecked;

            var result = dialog.ShowModal(this);

            if (result == ModalResult.Accepted)
            {
                if (dialog.AllowMultipleSelection)
                    MessageBox.Show("Accepted, FileNames = " + 
                        String.Join(", ", dialog.FileNames), "Dialog Result");
                else
                    ResultMessage = "Open File Dialog Result:" +
                        "Accepted, FileName = " + Path.GetFileName(dialog.FileName);
            }
            else
                ResultMessage = "Open File Dialog Result:" + result.ToString();
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

        private void ShowSaveFileDialogButton_Click(
            object? sender, 
            EventArgs e)
        {
            ResultMessage = "";
            var dialog = new SaveFileDialog();

            SetFileDialogProperties(dialog);

            var result = dialog.ShowModal(this);

            if (result == ModalResult.Accepted)
                ResultMessage = "Save File Dialog Result:" +
                    "Accepted, FileName = " + Path.GetFileName(dialog.FileName);
            else
                ResultMessage = "Save File Dialog Result:" + result.ToString(); 
        }

        private void ShowSelectDirectoryDialogButton_Click(
            object? sender, 
            EventArgs e)
        {
            ResultMessage = "";

            var dialog = new SelectDirectoryDialog();

            if (setInitialDirectoryCheckBox.IsChecked)
                dialog.InitialDirectory = GetInitialDirectory();

            if (setCustomTitleCheckBox.IsChecked)
                dialog.Title = CustomTitle;

            var result = dialog.ShowModal(this);

            if (result == ModalResult.Accepted)
                ResultMessage = "Select Dir Dialog Result:" +
                    "Accepted, FileName = " + Path.GetFileName(dialog.DirectoryName);
            else
                ResultMessage = "Select Dir Dialog Result:" + result.ToString();
        }

        private void ShowMessageBoxButton_Click(object? sender, System.EventArgs e)
        {
            try
            {
                ResultMessage = "";
                var result = MessageBox.Show(
                    "Message Box Text",
                    "Message Box Caption",
                    (MessageBoxButtons)messageBoxButtonsComboBox.SelectedItem!,
                    (MessageBoxIcon)messageBoxIconComboBox.SelectedItem!,
                    (MessageBoxDefaultButton)
                        messageBoxDefaultButtonComboBox.SelectedItem!);

                ResultMessage = "Message Box Result: " + result;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Message Box Argument Exception");
            }
        }

        private void ThrowExceptionButton_Click(object? sender, EventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                MessageBox.Show(
                    "Run this application without debugging to see the thread exception window.",
                    "Common Dialogs Sample");
                return;
            }

            throw (TestExceptionType)exceptionTypeComboBox.SelectedItem! switch
            {
                TestExceptionType.InvalidOperationException => 
                    new InvalidOperationException("Test message"),
                TestExceptionType.FormatException => 
                    new FormatException("Test message"),
                TestExceptionType.FileNotFoundException => 
                    new FileNotFoundException("Test message", "MyFileName.dat"),
                _ => throw new Exception(),
            };
        }

        private void ShowFontDialogButton_Click(object? sender, System.EventArgs e)
        {
            ResultMessage = string.Empty;

            var dialog = new FontDialog
            {
                FontInfo = fontInfo,
                ShowHelp = false,
            };

            if (setCustomTitleCheckBox.IsChecked)
                dialog.Title = CustomTitle;

            var result = dialog.ShowModal(this);

            if (result == ModalResult.Accepted)
            {
                fontInfo = dialog.FontInfo;
                sampleLabel.Font = fontInfo;
                ResultMessage =
                    "Font Dialog Result: Accepted, Font = " + 
                    dialog.FontInfo.ToString()+", Color = " + dialog.Color;
            }
            else
                ResultMessage = "Font Dialog Result: " + result.ToString();
        }

        private string ResultMessage
        {
            set
            {
                (StatusBar as StatusBar)?.SetText(value);
            }
        }

        private void ShowColorDialogButton_Click(object? sender, System.EventArgs e)
        {
            ResultMessage = "";

            var dialog = new ColorDialog
            {
                Color = sampleLabel.RealBackgroundColor,
            };

            if (setCustomTitleCheckBox.IsChecked)
                dialog.Title = CustomTitle;

            var result = dialog.ShowModal(this);

            if (result == ModalResult.Accepted)
            {
                ResultMessage =
                    "Color Dialog Result: Accepted, Color = " + dialog.Color;
                sampleLabel.BackgroundColor = dialog.Color;
            }
            else
            {
                ResultMessage = "Color Dialog Result: " + result.ToString();
                sampleLabel.BackgroundColor = null;
            }
        }
    }
}