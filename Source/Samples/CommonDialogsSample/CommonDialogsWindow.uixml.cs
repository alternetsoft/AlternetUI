using System;
using System.IO;
using Alternet.Drawing;
using Alternet.UI;

namespace ControlsSample
{
    public partial class CommonDialogsWindow : Window
    {
        private const string CustomTitle = @"Custom Title";
        private FontInfo fontInfo = AbstractControl.DefaultFont;

        public CommonDialogsWindow(WindowKind kind)
            : base(kind)
        {
            Initialize();
        }

        public CommonDialogsWindow()
        {
            Initialize();
        }

        private void Initialize()
        {
            this.SuspendLayout();
            Icon = App.DefaultIcon;
            InitializeComponent();

            messageBoxButtonsComboBox.Add(MessageBoxButtons.OK);
            messageBoxButtonsComboBox.Add(MessageBoxButtons.OKCancel);
            messageBoxButtonsComboBox.Add(MessageBoxButtons.YesNoCancel);
            messageBoxButtonsComboBox.Add(MessageBoxButtons.YesNo);
            messageBoxButtonsComboBox.SelectedItem = MessageBoxButtons.OKCancel;

            messageBoxIconComboBox.AddEnumValues(typeof(MessageBoxIcon), MessageBoxIcon.None);
            exceptionTypeComboBox.AddEnumValues(
                typeof(TestExceptionType),
                TestExceptionType.FileNotFoundException);

            messageBoxDefaultButtonComboBox.Add(MessageBoxDefaultButton.Button1);
            messageBoxDefaultButtonComboBox.Add(MessageBoxDefaultButton.Button2);
            messageBoxDefaultButtonComboBox.Add(MessageBoxDefaultButton.Button3);
            messageBoxDefaultButtonComboBox.SelectedItem = MessageBoxDefaultButton.Button1;

            tabControl.MinSizeGrowMode = WindowSizeToContentMode.Height;

            this.ResumeLayout();
        }

        enum TestExceptionType
        {
            InvalidOperationException,
            FormatException,
            FileNotFoundException,
        }

        private string GetInitialDirectory()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        }

        private void ShowOpenFileDialogButton_Click(object? sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();

            SetFileDialogProperties(dialog);

            dialog.AllowMultipleSelection = allowMultipleSelectionCheckBox.IsChecked;

            dialog.ShowAsync(this, (result) =>
            {
                if (result)
                {
                    string logResult = LogUtils.SectionSeparator + Environment.NewLine;

                    logResult += "Open File Dialog Result: Accepted, FileNames: ";

                    if (dialog.AllowMultipleSelection)
                    {
                        logResult += Environment.NewLine + "    ";
                        logResult += String.Join($",{Environment.NewLine}    ", dialog.FileNames);
                    }
                    else
                        logResult += Path.GetFileName(dialog.FileName);

                    logResult += Environment.NewLine + LogUtils.SectionSeparator;
                    LogResult(logResult);
                }
                else
                    LogResult("Open File Dialog Result:" + result.ToString());
            });
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
            var dialog = new SaveFileDialog();

            SetFileDialogProperties(dialog);

            dialog.ShowAsync(this, (result) =>
            {
                if (result)
                    LogResult("Save File Dialog Result:" +
                        "Accepted, FileName = " + Path.GetFileName(dialog.FileName));
                else
                    LogResult("Save File Dialog Result:" + result.ToString());
            });
        }

        private void ShowSelectDirectoryDialogButton_Click(
            object? sender, 
            EventArgs e)
        {
            var dialog = new SelectDirectoryDialog();

            if (setInitialDirectoryCheckBox.IsChecked)
                dialog.InitialDirectory = GetInitialDirectory();

            if (setCustomTitleCheckBox.IsChecked)
                dialog.Title = CustomTitle;

            dialog.ShowAsync(this, (result) =>
            {
                if (result)
                    LogResult("Select Dir Dialog Result:" +
                        "Accepted, FileName = " + Path.GetFileName(dialog.DirectoryName));
                else
                    LogResult("Select Dir Dialog Result:" + result.ToString());
            });
        }

        private void ShowMessageBoxButton_Click(object? sender, System.EventArgs e)
        {
            try
            {
                MessageBox.Show(
                    "Message Box Text",
                    "Message Box Caption",
                    (MessageBoxButtons)messageBoxButtonsComboBox.SelectedItem!,
                    (MessageBoxIcon)messageBoxIconComboBox.SelectedItem!,
                    (MessageBoxDefaultButton)
                        messageBoxDefaultButtonComboBox.SelectedItem!,
                    (e) =>
                    {
                        LogResult("Message Box Result: " + e.Result);
                    });
            }
            catch (ArgumentException ex)
            {
                LogResult($"Message Box Argument Exception: {ex.Message}");
            }
        }

        private void ThrowExceptionButton_Click(object? sender, EventArgs e)
        {
            ExceptionUtils.ForceUnhandledExceptionToUseDialog();

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
            var dialog = new FontDialog
            {
                FontInfo = fontInfo,
                ShowHelp = false,
            };

            if (setCustomTitleCheckBox.IsChecked)
                dialog.Title = CustomTitle;

            dialog.ShowAsync(this, (result) =>
            {
                if (result)
                {
                    fontInfo = dialog.FontInfo;
                    sampleLabel.Font = fontInfo;
                    LogResult(
                        "Font Dialog Result: Accepted, Font = " +
                        dialog.FontInfo.ToString() + ", Color = " + dialog.Color);
                }
                else
                    LogResult("Font Dialog Result: " + result.ToString());
            });
        }

        private void LogResult(string? value)
        {
            if (value is null)
                return;
            App.Log(value);
        }

        private void ShowColorDialogButton_Click(object? sender, System.EventArgs e)
        {
            var dialog = new ColorDialog
            {
                Color = sampleLabel.RealBackgroundColor,
            };

            if (setCustomTitleCheckBox.IsChecked)
                dialog.Title = CustomTitle;

            dialog.ShowAsync(this, (result) =>
            {
                sampleLabel.ParentBackColor = false;
                sampleLabel.ParentForeColor = false;

                if (result)
                {
                    LogResult(
                        "Color Dialog Result: Accepted, Color = " + dialog.Color);
                    sampleLabel.BackgroundColor = dialog.Color;
                }
                else
                {
                    LogResult("Color Dialog Result: Cancel, Color = " + dialog.Color);
                    sampleLabel.BackgroundColor = null;
                }
            });
        }
    }
}