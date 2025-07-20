using System;
using System.Collections.Generic;
using System.IO;
using Alternet.Drawing;
using Alternet.UI;

namespace ControlsSample
{
    public partial class CommonDialogsWindow : Window
    {
        public static string LoremIpsum =
"Beneath a sky stitched with teacup clouds, the girl tiptoed across checkerboard moss. " +
"Each step made a peculiar sound—like libraries whispering to mushrooms. " +
"Trees bent inward to eavesdrop, their leaves rustling riddles only crickets could decipher." +
Environment.NewLine + Environment.NewLine +
"The map she carried was drawn entirely in nonsense, but somehow it felt correct. " +
"It pulsed faintly in her hands, humming with ink made from stolen dreams and marmalade." +
Environment.NewLine + Environment.NewLine +
"“Left is usually right,” said the rabbit-shaped shadow, bowing courteously. " +
"“Unless, of course, you're upside-down.”" +
Environment.NewLine + Environment.NewLine +
"And so, with a smile too wide for logic, she stepped forward—into a world where clocks " +
"melted politely and hats outgrew heads.";

        private const string CustomTitle = @"Custom Title";
        private FontInfo fontInfo = AbstractControl.DefaultFont;
        private string messageBoxText = "Message Box Text";

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

            List<MessageBoxButtons> messageBoxButtons =
                [
                    MessageBoxButtons.OK,
                    MessageBoxButtons.OKCancel,
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxButtons.YesNo,
                ];

            messageBoxButtonsComboBox.UseContextMenuAsPopup = true;

            if (MessageBox.UseInternalDialog)
            {
                messageBoxButtonsComboBox.AddEnumValues(typeof(MessageBoxButtons));
            }
            else
            {
                messageBoxButtonsComboBox.AddRange(messageBoxButtons);
            }

            messageBoxButtonsComboBox.Value = MessageBoxButtons.OKCancel;

            messageBoxIconComboBox.SetValue(MessageBoxIcon.None);

            exceptionTypeComboBox.EnumType = typeof(TestExceptionType);
            exceptionTypeComboBox.Value = TestExceptionType.FileNotFoundException;

            List<MessageBoxDefaultButton> messageBoxDefaultButtons =
                [
                    MessageBoxDefaultButton.Button1,
                    MessageBoxDefaultButton.Button2,
                    MessageBoxDefaultButton.Button3,
                ];

            messageBoxDefaultButtonComboBox.AddRange(messageBoxDefaultButtons);
            messageBoxDefaultButtonComboBox.UseContextMenuAsPopup = true;
            messageBoxDefaultButtonComboBox.Value = MessageBoxDefaultButton.Button1;

            tabControl.MinSizeGrowMode = WindowSizeToContentMode.Height;

            this.ResumeLayout();

            showMessageBoxButton.ContextMenuStrip.Add("Toggle message box text",
                () => ToggleText(ref messageBoxText, "Message Box Text"));
        }

        private void ToggleText(ref string toggledText, string text)
        {
            if (toggledText == text)
                toggledText = LoremIpsum;
            else
                toggledText = text;
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
                    messageBoxText,
                    "Message Box Caption",
                    (MessageBoxButtons?)messageBoxButtonsComboBox.Value ?? MessageBoxButtons.OK,
                    (MessageBoxIcon?)messageBoxIconComboBox.Value ?? MessageBoxIcon.None,
                    (MessageBoxDefaultButton?)
                        messageBoxDefaultButtonComboBox.Value ?? MessageBoxDefaultButton.Button1,
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

            throw (TestExceptionType)exceptionTypeComboBox.Value! switch
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