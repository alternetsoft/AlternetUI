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
    public partial class ThreadExceptionWindow : DialogWindow
    {
        /// <summary>
        /// Gets or sets default error image size in device-independent units.
        /// </summary>
        public static int DefaultErrorImageSize = 32;

        /// <summary>
        /// Indicates whether details should be displayed in a separate dialog.
        /// </summary>
        public static bool ShowDetailsInSeparateDialog = false;

        private Exception? exception;
        private bool canContinue = true;
        private bool canQuit = true;
        private TextBox? messageTextBox;
        private string? additionalInfo;
        private Button? detailsButton;

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
            var result = LogUtils.GetExceptionMessageText(Exception, additionalInfo);
            return result;
        }

        /// <summary>
        /// Gets detailed information about the exception.
        /// </summary>
        /// <returns></returns>
        protected virtual string GetDetailsText()
        {
            var result = LogUtils.GetExceptionDetailsText(Exception);
            return result;
        }

        /// <summary>
        /// Initializes the controls and layout for the window, setting up its
        /// appearance, behavior, and child elements.
        /// </summary>
        /// <remarks>This method configures the window's size, layout style,
        /// padding, and other properties
        /// such as its start location and topmost behavior. It also creates
        /// and initializes the message grid and
        /// buttons grid, which are added as child elements to the window.
        /// The window title is determined based on the
        /// active window or a default error title. Derived classes can override
        /// this method to customize the initialization process.</remarks>
        protected virtual void InitializeControls()
        {
            BeginInit();

            CloseEnabled = false;
            Size = (700, 500);
            Layout = LayoutStyle.Vertical;
            Padding = 10;
            HasSystemMenu = false;
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
                    new Label
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

                Border border = new()
                {
                    VerticalAlignment = VerticalAlignment.Fill,
                    Parent = messageGrid,
                };

                messageTextBox = new MultilineTextBox
                {
                    Text = " ",
                    ReadOnly = true,
                    HasBorder = false,
                    MinHeight = 150,
                    Parent = border,
                };

                return messageGrid;
            }
        }

        /// <summary>
        /// Creates and returns a grid of buttons arranged horizontally,
        /// with predefined functionality.
        /// </summary>
        /// <remarks>The grid includes three buttons: a "Details" button, a "Continue" button,
        /// and a "Quit" button. Each button is configured with specific text, alignment,
        /// visibility, and click event handlers.
        /// The visibility and alignment of certain buttons depend
        /// on the state of the application.</remarks>
        /// <returns>A <see cref="HorizontalStackPanel"/> containing the configured buttons.</returns>
        protected virtual AbstractControl CreateButtonsGrid()
        {
            var buttonsGrid = new HorizontalStackPanel();
            buttonsGrid.Padding = 10;

            detailsButton = new Button
            {
                Text = CommonStrings.Default.ButtonDetails + "...",
            };
            detailsButton.Click += OnDetailsButtonClick;
            buttonsGrid.Children.Add(detailsButton);

            var copyButton = new Button
            {
                Text = CommonStrings.Default.ButtonCopy,
                Parent = buttonsGrid,
            };

            copyButton.Click += (s, e) =>
            {
                Clipboard.SetText(messageTextBox?.Text);
            };

            var continueButton =
                new Button
                {
                    Text = CommonStrings.Default.ButtonContinue,
                };
            continueButton.Click += OnContinueButtonClick;
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
            quitButton.Click += OnQuitButtonClick;
            buttonsGrid.Children.Add(quitButton);

            return buttonsGrid;
        }

        /// <summary>
        /// Handles the "Quit" button click event.
        /// </summary>
        /// <param name="sender">The source of the event.
        /// This parameter can be <see langword="null"/>.</param>
        /// <param name="e">The event data associated with the "Quit" button click.</param>
        protected virtual void OnQuitButtonClick(object? sender, EventArgs e)
        {
            ModalResult = ModalResult.Canceled;
        }

        /// <summary>
        /// Handles the "Continue" button click event.
        /// </summary>
        /// <param name="sender">The source of the event.
        /// This parameter can be <see langword="null"/>.</param>
        /// <param name="e">The event data associated with the "Continue" button click.</param>
        protected virtual void OnContinueButtonClick(object? sender, EventArgs e)
        {
            ModalResult = ModalResult.Accepted;
        }

        /// <summary>
        /// Handles the "Details" button click event.
        /// </summary>
        /// <param name="sender">The source of the event.
        /// This parameter can be <see langword="null"/>.</param>
        /// <param name="e">The event data associated with the "Details" button click.</param>
        protected virtual void OnDetailsButtonClick(object? sender, EventArgs e)
        {
            if (ShowDetailsInSeparateDialog)
            {
                var detailsWindow =
                    new WindowWithMemoAndButton(
                        CommonStrings.Default.WindowTitleExceptionDetails,
                        GetDetailsText());
                detailsWindow.ShowDialogAsync(this, (result) =>
                {
                    detailsWindow.Dispose();
                });
            }
            else
            {
                if (messageTextBox is null || detailsButton is null)
                    return;
                detailsButton.Enabled = false;
                messageTextBox.Text += Environment.NewLine + GetDetailsText()
                    + Environment.NewLine;
            }
        }

        /// <summary>
        /// Updates exception text.
        /// </summary>
        private void UpdateExceptionText()
        {
            messageTextBox!.Text = GetMessageText();
        }
    }
}