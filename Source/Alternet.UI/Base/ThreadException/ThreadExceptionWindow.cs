using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    ///  Implements a window that is displayed when an exception occurs in
    ///  the application.
    /// </summary>
    public class ThreadExceptionWindow : DialogWindow
    {
        private readonly Exception exception;
        private bool canContinue;
        private TextBox? messageTextBox;

        /// <summary>
        ///  Initializes a new instance of the
        ///  <see cref="ThreadExceptionWindow"/> class.
        /// </summary>
        /// <param name="exception">Exception information.</param>
        /// <param name="additionalInfo">Additional information.</param>
        /// <param name="canContinue">Whether continue button is visible.</param>
        public ThreadExceptionWindow(
            Exception exception,
            string? additionalInfo = null,
            bool canContinue = true)
        {
            this.exception = exception;
            this.canContinue = canContinue;
            InitializeControls();

            if(Application.FirstWindow() is not null)
            {
                var activeWindow = ActiveWindow;
                if (activeWindow is null || activeWindow.Title.Length == 0)
                    Title = ErrorMessages.Default.ErrorTitle;
                else
                    Title = activeWindow.Title;
            }
            else
                Title = ErrorMessages.Default.ErrorTitle;

            var s = GetMessageText(exception);

            if(additionalInfo is not null)
            {
                s += "\n" + additionalInfo;
            }

            messageTextBox!.Text = s;
        }

        /// <summary>
        /// Shows <see cref="ThreadExceptionWindow"/> on the screen.
        /// </summary>
        /// <param name="exception">Exception information.</param>
        /// <param name="additionalInfo">Additional information.</param>
        /// <param name="canContinue">Whether continue button is visible.</param>
        /// <returns><c>true</c> if continue pressed, <c>false</c> otherwise.</returns>
        public static bool Show(
            Exception exception,
            string? additionalInfo = null,
            bool canContinue = true)
        {
            using var errorWindow =
                new ThreadExceptionWindow(exception, additionalInfo, canContinue);
            if (Application.IsRunning)
            {
                return errorWindow.ShowModal() == ModalResult.Accepted;
            }
            else
            {
                errorWindow.canContinue = false;
                Application.Current.Run(errorWindow);
                return false;
            }
        }

        private static string GetMessageText(Exception e)
        {
            var text = "Type: " + e.GetType().FullName;

            if (e.Message != null)
                text += "\n" + "Message: " + e.Message;

            return text;
        }

        private static string GetDetailsText(Exception e)
        {
            StringBuilder detailsTextBuilder = new();
            string newline = "\n";
            string separator = "----------------------------------------\n";
            string sectionseparator = "\n************** {0} **************\n";

            detailsTextBuilder.Append(string.Format(
                CultureInfo.CurrentCulture,
                sectionseparator,
                "Exception Text"));
            detailsTextBuilder.Append(e.ToString());
            detailsTextBuilder.Append(newline);
            detailsTextBuilder.Append(newline);
            detailsTextBuilder.Append(
                string.Format(
                    CultureInfo.CurrentCulture,
                    sectionseparator,
                    "Loaded Assemblies"));

            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                AssemblyName name = asm.GetName();
                string? location = asm.Location;
                string? fileVer = "n/a";

                try
                {
                    if (location is not null &&
                        location.Length > 0)
                    {
                        fileVer =
                            FileVersionInfo.GetVersionInfo(location).FileVersion;
                    }
                }
                catch (FileNotFoundException)
                {
                }

                const string ExDlgMsgLoadedAssembliesEntry =
                    "{0}\n    Assembly Version: {1}\n" +
                    "    Win32 Version: {2}\n    CodeBase: {3}\n";

                detailsTextBuilder.Append(
                    string.Format(
                        ExDlgMsgLoadedAssembliesEntry,
                        name.Name,
                        name.Version,
                        fileVer,
                        location));
                detailsTextBuilder.Append(separator);
            }

            detailsTextBuilder.Append(newline);
            detailsTextBuilder.Append(newline);

            return detailsTextBuilder.ToString();
        }

        private void InitializeControls()
        {
            BeginInit();

            Size = (700, 500);
            Layout = LayoutStyle.Vertical;
            Padding = 10;
            MinimizeEnabled = false;
            MaximizeEnabled = false;
            StartLocation = WindowStartLocation.CenterScreen;
            TopMost = true;

            var messageGrid = CreateMessageGrid();
            messageGrid.VerticalAlignment = VerticalAlignment.Fill;
            messageGrid.Parent = this;

            var buttonsGrid = CreateButtonsGrid();
            buttonsGrid.VerticalAlignment = UI.VerticalAlignment.Bottom;
            buttonsGrid.Parent = this;

            EndInit();

            Control CreateMessageGrid()
            {
                var messageGrid = new VerticalStackPanel();

                var firstSection = new HorizontalStackPanel();
                firstSection.Parent = messageGrid;

                var errorImagePictureBox = new PictureBox
                {
                    VerticalAlignment = UI.VerticalAlignment.Top,
                    HorizontalAlignment = UI.HorizontalAlignment.Left,
                    Margin = new Thickness(0, 0, 10, 0),
                    ImageSet = KnownColorSvgImages.ImgError.AsImageSet(64),
                };
                errorImagePictureBox.Parent = firstSection;

                var stackPanel = new StackPanel
                {
                    Orientation = StackPanelOrientation.Vertical,
                };
                stackPanel.Parent = firstSection;

                var s1 = "Unhandled exception has occurred in your application.";
                var s2 = "If you click Continue, the application will ignore this error";
                var s3 = "and attempt to continue.";
                var s4 = "If you click Quit, the application will close immediately.";

                string[] lines;

                if(canContinue)
                    lines = new[] { s1, s2, s3, s4 };
                else
                    lines = new[] { s1, s4 };

                foreach (var line in lines)
                {
                    var label = new Label { Text = line };
                    stackPanel.Children.Add(label);
                }

                messageGrid.Children.Add(
                    new Label
                    {
                        Text = "Exception information" + ":",
                        Margin = new Thickness(0, 15, 0, 5),
                    });

                messageTextBox = new TextBox
                {
                    Text = " ",
                    ReadOnly = true,
                    Multiline = true,
                    MinHeight = 150,
                    VerticalAlignment = VerticalAlignment.Fill,
                };

                messageGrid.Children.Add(messageTextBox);

                return messageGrid;
            }

            Control CreateButtonsGrid()
            {
                var buttonsGrid = new HorizontalStackPanel();
                buttonsGrid.Padding = 10;

                var detailsButton = new Button
                {
                    Text = CommonStrings.Default.ButtonDetails + "...",
                };
                detailsButton.Click += DetailsButton_Click;
                buttonsGrid.Children.Add(detailsButton);

                var continueButton =
                    new Button
                    {
                        Text = CommonStrings.Default.ButtonContinue,
                    };
                continueButton.Click += ContinueButton_Click;
                continueButton.HorizontalAlignment = HorizontalAlignment.Right;
                buttonsGrid.Children.Add(continueButton);
                continueButton.Visible = canContinue;

                var quitButton = new Button { Text = CommonStrings.Default.ButtonQuit };
                quitButton.Click += QuitButton_Click;
                quitButton.IsDefault = true;
                quitButton.IsCancel = true;
                quitButton.HorizontalAlignment = HorizontalAlignment.Right;
                buttonsGrid.Children.Add(quitButton);

                return buttonsGrid;
            }
        }

        private void QuitButton_Click(object? sender, EventArgs e)
        {
            ModalResult = ModalResult.Canceled;
            Close();
        }

        private void ContinueButton_Click(object? sender, EventArgs e)
        {
            ModalResult = ModalResult.Accepted;
            Close();
        }

        private void DetailsButton_Click(object? sender, EventArgs e)
        {
            using var detailsWindow =
                new ThreadExceptionDetailsWindow(GetDetailsText(exception));
            detailsWindow.ShowModal(this);
        }
    }
}