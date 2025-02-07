using System;
using System.Collections.Generic;
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
        /// <summary>
        /// Gets or sets default error image size in device-independent units.
        /// </summary>
        public static int DefaultErrorImageSize = 32;

        private Exception? exception;
        private bool canContinue = true;
        private bool canQuit = true;
        private TextBox? messageTextBox;
        private string? additionalInfo;

        /// <summary>
        ///  Initializes a new instance of the
        ///  <see cref="ThreadExceptionWindow"/> class.
        /// </summary>
        public ThreadExceptionWindow()
        {
            InitializeControls();
        }

        /// <summary>
        ///  Initializes a new instance of the
        ///  <see cref="ThreadExceptionWindow"/> class.
        /// </summary>
        /// <param name="exception">Exception information.</param>
        /// <param name="additionalInfo">Additional information.</param>
        /// <param name="canContinue">Whether 'Continue' button is visible.</param>
        /// <param name="canQuit">Whether 'Quit' button is visible.</param>
        public ThreadExceptionWindow(
            Exception exception,
            string? additionalInfo = null,
            bool canContinue = true,
            bool canQuit = true)
        {
            this.canContinue = canContinue;
            this.canQuit = canQuit;
            this.additionalInfo = additionalInfo;
            this.exception = exception;
            InitializeControls();
            UpdateExceptionText();
        }

        /// <summary>
        /// Gets or sets additional information related to the exception.
        /// </summary>
        public virtual string? AdditionalInfo
        {
            get
            {
                return additionalInfo;
            }

            set
            {
                if (additionalInfo == value)
                    return;
                additionalInfo = value;
                UpdateExceptionText();
            }
        }

        /// <summary>
        /// Gets or sets an exception for which this window is shown.
        /// </summary>
        public virtual Exception? Exception
        {
            get
            {
                return exception;
            }

            set
            {
                if (exception == value)
                    return;
                exception = value;
                UpdateExceptionText();
            }
        }

        /// <summary>
        /// Gets or sets whether 'Quit' button is visible.
        /// </summary>
        public virtual bool CanQuit
        {
            get => canQuit;

            set => canQuit = value;
        }

        /// <summary>
        /// Gets or sets whether 'Continue' button is visible.
        /// </summary>
        public virtual bool CanContinue
        {
            get => canContinue;

            set => canContinue = value;
        }

        /// <summary>
        /// Gets message text used to show information about the exception.
        /// </summary>
        /// <returns></returns>
        protected virtual string GetMessageText()
        {
            string text = string.Empty;

            if (Exception is not null)
            {
                text = "Type: " + Exception.GetType().FullName;

                if (Exception.Message != null)
                    text += "\n" + "Message: " + Exception.Message;

                if(Exception is BaseException baseException)
                {
                    var s = baseException.AdditionalInformation;
                    if (!string.IsNullOrEmpty(s))
                    {
                        text += "\n" + s;
                    }
                }
            }

            if (additionalInfo is not null)
            {
                text += "\n" + additionalInfo;
            }

            return text;
        }

        /// <summary>
        /// Gets detailed information about the exception.
        /// </summary>
        /// <returns></returns>
        protected virtual string GetDetailsText()
        {
            var e = Exception;

            if (e is null)
                return string.Empty;

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

            CloseEnabled = false;
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

            if (App.FirstWindow() is not null)
            {
                var activeWindow = ActiveWindow;
                if (activeWindow is null || activeWindow.Title.Length == 0)
                    Title = ErrorMessages.Default.ErrorTitle;
                else
                    Title = activeWindow.Title;
            }
            else
                Title = ErrorMessages.Default.ErrorTitle;

            EndInit();

            AbstractControl CreateMessageGrid()
            {
                var messageGrid = new VerticalStackPanel();

                var firstSection = new HorizontalStackPanel();
                firstSection.Parent = messageGrid;

                var imageSizeInPixels = this.PixelFromDip(DefaultErrorImageSize);

                var errorImagePictureBox = new PictureBox
                {
                    VerticalAlignment = UI.VerticalAlignment.Top,
                    HorizontalAlignment = UI.HorizontalAlignment.Left,
                    Margin = new Thickness(0, 0, 10, 0),
                    ImageSet = MessageBoxSvg
                    .GetImage(MessageBoxIcon.Error)?.AsImageSet(imageSizeInPixels),
                };
                errorImagePictureBox.Parent = firstSection;

                var stackPanel = new StackPanel
                {
                    Orientation = StackPanelOrientation.Vertical,
                };
                stackPanel.Parent = firstSection;

                var s1 = "Error has occurred in your application.";
                var s2 = "If you click 'Continue', the application will ignore this error";
                var s3 = "and attempt to continue.";
                var s4 = "If you click 'Quit', the application will close immediately.";

                List<string> lines = new();

                lines.Add(s1);

                if (CanContinue)
                {
                    lines.Add(s2);
                    lines.Add(s3);
                }

                if (CanQuit)
                {
                    lines.Add(s4);
                }

                foreach (var line in lines)
                {
                    var label = new Label
                    {
                        Text = line,
                        Parent = stackPanel,
                    };
                }

                messageGrid.Children.Add(
                    new Label
                    {
                        Text = "Exception information" + ":",
                        Margin = new Thickness(0, 15, 0, 5),
                    });

                messageTextBox = new MultilineTextBox
                {
                    Text = " ",
                    ReadOnly = true,
                    MinHeight = 150,
                    VerticalAlignment = VerticalAlignment.Fill,
                };

                messageGrid.Children.Add(messageTextBox);

                return messageGrid;
            }

            AbstractControl CreateButtonsGrid()
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
                continueButton.Visible = CanContinue;

                var quitButton = new Button
                {
                    Text = CommonStrings.Default.ButtonQuit,
                    IsDefault = true,
                    Visible = canQuit,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Margin = (5, 0, 0, 0),
                };
                quitButton.Click += QuitButton_Click;
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

        /// <summary>
        /// Updates exception text.
        /// </summary>
        private void UpdateExceptionText()
        {
            messageTextBox!.Text = GetMessageText();
        }

        private void DetailsButton_Click(object? sender, EventArgs e)
        {
            var detailsWindow =
                new ThreadExceptionDetailsWindow(GetDetailsText());
            detailsWindow.ShowDialogAsync(this, (result) =>
            {
                detailsWindow.Dispose();
            });
        }
    }
}