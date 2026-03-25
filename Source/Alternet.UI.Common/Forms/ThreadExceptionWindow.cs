using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    ///  Implements a window that is displayed when an exception occurs in
    ///  the application. When 'Continue' button is clicked, the window is closed with 'Accepted' result
    ///  and the application continues running. When 'Quit' button is clicked, the window is closed with 'Canceled' result
    ///  and the application is closed.
    /// </summary>
    public partial class ThreadExceptionWindow : DialogWindow
    {
        /// <summary>
        /// Gets or sets default error image size in device-independent units.
        /// </summary>
        public static int DefaultErrorImageSize = 32;

        private readonly BaseCollection<ExceptionInfoItem> exceptions = new();

        private bool canContinue = true;
        private bool canQuit = true;
        private bool isDetailed;
        private TextBox? messageTextBox;
        
        private Button detailsButton;
        private Button continueButton;
        private Button copyButton;
        private Button quitButton;
        private Button throwButton;

        private Label instructionsLabel;
        private Label exceptionHeaderLabel;

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

            exceptions.Add(new ExceptionInfoItem(exception, additionalInfo));

            InitializeControls();
            UpdateExceptionText();
        }

        /// <summary>
        /// Specifies the available types of buttons in <see cref="ThreadExceptionWindow"/>.
        /// </summary>
        public enum ButtonKind
        {
            /// <summary>
            /// Not a button.
            /// </summary>
            None,

            /// <summary>
            /// Indicates a button for copying exception information to clipboard.
            /// </summary>
            Copy,

            /// <summary>
            /// Indicates a button for showing detailed information about the exception.
            /// </summary>
            Details,

            /// <summary>
            /// Indicates a button for continuing the application execution after an exception has occurred.
            /// </summary>
            Continue,

            /// <summary>
            /// Indicates a button for quitting the application after an exception has occurred.
            /// </summary>
            Quit,

            /// <summary>
            /// Indicates a button for throwing the exception to be handled by the development environment or default exception handler.
            /// </summary>
            Throw,
        }

        /// <summary>
        /// Gets the collection of exception information items associated with the current operation.
        /// </summary>
        public IReadOnlyList<ExceptionInfoItem> Exceptions => exceptions;

        /// <summary>
        /// Gets the type of the last clicked button in the window, which can be used to determine the user's action and respond accordingly.
        /// </summary>
        public virtual ButtonKind LastClickedButton { get; private set; } = ButtonKind.None;

        /// <summary>
        /// Gets the result of the dialog based on the last button clicked.
        /// </summary>
        /// <remarks>Use this property to determine the user's chosen action after the dialog is closed.
        /// The value reflects the most recent button interaction and can be used to control subsequent application
        /// flow.</remarks>
        public virtual ExceptionDialogResult DialogResult
        {
            get
            {
                return LastClickedButton switch
                {
                    ButtonKind.Continue => ExceptionDialogResult.Continue,
                    ButtonKind.Quit => ExceptionDialogResult.Quit,
                    ButtonKind.Throw => ExceptionDialogResult.Throw,
                    _ => ExceptionDialogResult.None,
                };
            }
        }

        /// <summary>
        /// Gets the "Details" button control, which can be used to show or hide detailed information about the exception when clicked.
        /// </summary>
        public Button DetailsButton => detailsButton;

        /// <summary>
        /// Gets the "Continue" button control, which can be used to allow
        /// the user to continue running the application after an exception has occurred when clicked.
        /// </summary>
        public Button ContinueButton => continueButton;

        /// <summary>
        /// Gets the "Copy" button control, which can be used to copy exception information to clipboard when clicked.
        /// </summary>
        public Button CopyButton => copyButton;

        /// <summary>
        /// Gets the "Throw" button control, which can be used to throw the exception
        /// to be handled by the development environment or default exception handler when clicked.
        /// </summary>
        public Button ThrowButton => throwButton;

        /// <summary>
        /// Gets the "Quit" button control, which can be used to allow the user to quit the application after an exception has occurred when clicked.
        /// </summary>
        public Button QuitButton => quitButton;

        /// <summary>
        /// Gets or sets additional information related to the exception.
        /// </summary>
        public virtual string? AdditionalInfo
        {
            get
            {
                return exceptions.LastOrDefault()?.AdditionalInfo as string;
            }

            set
            {
                var item = exceptions.LastOrDefault();

                if (item is null)
                    return;

                exceptions.TrimCount(1);

                item.AdditionalInfo = value;
                UpdateExceptionText();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether detailed information is included.
        /// </summary>
        public virtual bool IsDetailed
        {
            get
            {
                return isDetailed;
            }

            set
            {
                if(isDetailed == value)
                    return;
                isDetailed = value;
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
                return exceptions.LastOrDefault()?.Exception;
            }

            set
            {
                var item = exceptions.LastOrDefault();
                if (item is null)
                    return;
                item.Exception = value ?? new Exception(ErrorMessages.Default.UnknownException);
                exceptions.TrimCount(1);
                UpdateExceptionText();
            }
        }

        /// <summary>
        /// Gets or sets whether 'Quit' button is visible.
        /// </summary>
        public virtual bool CanQuit
        {
            get => canQuit;

            set
            {
                canQuit = value;
                QuitButton.Visible = value;
            }
        }

        /// <summary>
        /// Gets or sets whether 'Continue' button is visible.
        /// </summary>
        public virtual bool CanContinue
        {
            get => canContinue;

            set
            {
                canContinue = value;
                ContinueButton.Visible = value;
            }
        }

        /// <summary>
        /// Gets message text used to show information about the exception.
        /// </summary>
        /// <returns></returns>
        protected virtual string GetMessageText()
        {
            StringBuilder result = new();

            foreach (var item in Exceptions)
            {
                var isEmpty = result.Length == 0;

                if (!isEmpty)
                {
                    result.AppendLine(LogUtils.SectionSeparator);
                    result.AppendLine();
                }

                var message = LogUtils.GetExceptionMessageText(item.Exception, item.AdditionalInfo, IsDetailed);
                result.AppendLine(message);
            }

            return result.ToString();
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
        [MemberNotNull(nameof(detailsButton), nameof(copyButton), nameof(continueButton), nameof(throwButton), nameof(quitButton))]
        [MemberNotNull(nameof(instructionsLabel), nameof(exceptionHeaderLabel))]
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

            [MemberNotNull(nameof(instructionsLabel), nameof(exceptionHeaderLabel))]
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

                var s1 = ErrorMessages.Default.ErrorHasOccurredInYourApplication;
                var s2 = ErrorMessages.Default.PressContinueToSkipError;
                var s4 = ErrorMessages.Default.PressQuitToCloseApplication;

                var s = s1;

                if (CanContinue)
                {
                    s += Environment.NewLine + s2;
                }

                if (CanQuit)
                {
                    s += Environment.NewLine + s4;
                }

                instructionsLabel = new Label
                {
                    Text = s,
                    Parent = stackPanel,
                };

                instructionsLabel.DrawLabelFlags = DrawLabelFlags.TextHasNewLineChars | DrawLabelFlags.TextHasBold;

                exceptionHeaderLabel = new Label
                {
                    Text = ErrorMessages.Default.ExceptionInformation + ":",
                    Margin = new Thickness(0, 15, 0, 5),
                    Parent = messageGrid,
                };

                Border border = new()
                {
                    VerticalAlignment = VerticalAlignment.Fill,
                    Parent = messageGrid,
                };

                messageTextBox = new MultilineTextBox
                {
                    Text = StringUtils.OneSpace,
                    ReadOnly = true,
                    HasBorder = false,
                    MinHeight = 150,
                    Parent = border,
                };

                return messageGrid;
            }
        }

        /// <summary>
        /// Shows another exception in the same window. This method can be used when another exception occurs while the window is already shown.
        /// </summary>
        /// <param name="ex">The exception to be displayed.</param>
        /// <param name="additionalInfo">Additional information related to the exception.</param>
        public virtual void ShowAnotherException(Exception? ex,  object? additionalInfo)
        {
            if (ex is null)
                return;
            exceptions.Add(new ExceptionInfoItem(ex, additionalInfo));
            UpdateExceptionText();
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
        [MemberNotNull(nameof(detailsButton), nameof(copyButton), nameof(continueButton), nameof(throwButton), nameof(quitButton))]
        protected virtual AbstractControl CreateButtonsGrid()
        {
            var buttonContainer = new HorizontalStackPanel();
            buttonContainer.Padding = 10;

            detailsButton = new Button
            {
                Text = CommonStrings.Default.ButtonDetails,
                Parent = buttonContainer,
            };

            copyButton = new Button
            {
                Text = CommonStrings.Default.ButtonCopy,
                Parent = buttonContainer,
            };

            continueButton = new Button
            {
                Text = CommonStrings.Default.ButtonContinue,
                HorizontalAlignment = HorizontalAlignment.Right,
                Visible = CanContinue,
                Parent = buttonContainer,
            };

            throwButton = new Button
            {
                Text = CommonStrings.Default.ButtonThrow,
                HorizontalAlignment = HorizontalAlignment.Right,
                Visible = false,
                Parent = buttonContainer,
            };

            quitButton = new Button
            {
                Text = CommonStrings.Default.ButtonQuit,
                IsDefault = true,
                Visible = canQuit,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = (5, 0, 0, 0),
                Parent = buttonContainer,
            };

            copyButton.Click += OnCopyButtonClick;
            continueButton.Click += OnContinueButtonClick;
            quitButton.Click += OnQuitButtonClick;
            detailsButton.Click += OnDetailsButtonClick;
            throwButton.Click += OnThrowButtonClick;

            return buttonContainer;
        }

        /// <summary>
        /// Handles the "Quit" button click event.
        /// </summary>
        /// <param name="sender">The source of the event.
        /// This parameter can be <see langword="null"/>.</param>
        /// <param name="e">The event data associated with the "Quit" button click.</param>
        protected virtual void OnQuitButtonClick(object? sender, EventArgs e)
        {
            LastClickedButton = ButtonKind.Quit;
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
            LastClickedButton = ButtonKind.Continue;
            ModalResult = ModalResult.Accepted;
        }

        /// <summary>
        /// Handles the "Copy" button click event.
        /// </summary>
        /// <param name="sender">The source of the event.
        /// This parameter can be <see langword="null"/>.</param>
        /// <param name="e">The event data associated with the "Copy" button click.</param>
        protected virtual void OnCopyButtonClick(object? sender, EventArgs e)
        {
            LastClickedButton = ButtonKind.Copy;
            Clipboard.SetText(messageTextBox?.Text);
        }

        /// <summary>
        /// Handles the "Details" button click event.
        /// </summary>
        /// <param name="sender">The source of the event.
        /// This parameter can be <see langword="null"/>.</param>
        /// <param name="e">The event data associated with the "Details" button click.</param>
        protected virtual void OnDetailsButtonClick(object? sender, EventArgs e)
        {
            LastClickedButton = ButtonKind.Details;
            IsDetailed = !IsDetailed;
        }

        /// <summary>
        /// Handles the "Throw" button click event.
        /// </summary>
        /// <param name="sender">The source of the event.
        /// This parameter can be <see langword="null"/>.</param>
        /// <param name="e">The event data associated with the "Throw" button click.</param>
        protected virtual void OnThrowButtonClick(object? sender, EventArgs e)
        {
            LastClickedButton = ButtonKind.Throw;
            ModalResult = ModalResult.Accepted;
        }

        /// <summary>
        /// Updates exception text.
        /// </summary>
        private void UpdateExceptionText()
        {
            messageTextBox!.Text = GetMessageText();
        }

        /// <summary>
        /// Represents an item that encapsulates an exception and optional additional information
        /// for diagnostic or logging purposes.
        /// </summary>
        public class ExceptionInfoItem : BaseObjectWithAttr
        {
            /// <summary>
            /// Initializes a new instance of the ExceptionInfoItem class with the specified exception and optional
            /// additional information.
            /// </summary>
            /// <param name="e">The exception to be encapsulated by this instance. Cannot be null.</param>
            /// <param name="additionalInfo">Optional additional information related to the exception. May be null.</param>
            public ExceptionInfoItem(Exception e, object? additionalInfo = null)
            {
                Exception = e;
                AdditionalInfo = additionalInfo;
            }

            /// <summary>
            /// Gets or sets the associated exception.
            /// </summary>
            public Exception Exception { get; set; }

            /// <summary>
            /// Gets or sets additional information associated with the object.
            /// </summary>
            public object? AdditionalInfo { get; set; }
        }
    }
}