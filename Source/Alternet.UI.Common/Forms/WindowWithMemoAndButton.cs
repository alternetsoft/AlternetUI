using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a dialog window that displays multi-line message.
    /// </summary>
    /// <remarks>This window provides a read-only text box for displaying multi-line message
    /// and a "Close" button for dismissing the dialog. It is designed to be modal,
    /// centered relative to its owner, and configured
    /// with predefined settings such as size, padding, and top-most behavior.</remarks>
    public partial class WindowWithMemoAndButton : DialogWindow
    {
        private readonly MultilineTextBox detailsTextBox = new()
        {
            HasBorder = false,
            ReadOnly = true,
        };

        private readonly Button copyButton = new()
        {
            Text = CommonStrings.Default.ButtonCopy,
            HorizontalAlignment = UI.HorizontalAlignment.Left,
            Margin = new Thickness(0, 10, 0, 0),
        };

        private readonly Button closeButton = new()
        {
            Text = CommonStrings.Default.ButtonClose,
            HorizontalAlignment = UI.HorizontalAlignment.Right,
            Margin = new Thickness(0, 10, 0, 0),
            IsCancel = true,
            IsDefault = true,
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowWithMemoAndButton"/> class.
        /// </summary>
        public WindowWithMemoAndButton(string title, string details)
        {
            InitializeControls(title, details);
        }

        /// <summary>
        /// Gets the memo text box that displays the message.
        /// </summary>
        public MultilineTextBox Memo => detailsTextBox;

        /// <summary>
        /// Gets the button used to initiate a copy to clipboard operation.
        /// </summary>
        public Button CopyButton => copyButton;

        /// <summary>
        /// Gets the button that closes the dialog.
        /// </summary>
        public Button CloseButton => closeButton;

        /// <summary>
        /// Displays a dialog window with the specified title and details.
        /// </summary>
        /// <remarks>This method creates a dialog window and displays it asynchronously.
        /// The dialog is disposed of automatically after it is closed.
        /// Ensure that both <paramref name="title"/> and <paramref name="details"/>
        /// are valid strings to avoid unexpected behavior.</remarks>
        /// <param name="title">The title of the dialog window. Cannot be null or empty.</param>
        /// <param name="details">The details or message to display in the dialog window.
        /// Cannot be null or empty.</param>
        public static void ShowDialog(string title, string details)
        {
            var window = new WindowWithMemoAndButton(title, details);
            window.ShowDialogAsync(null, (result) =>
            {
                window.Dispose();
            });
        }

        /// <summary>
        /// Initializes the controls and layout for the window, setting up its
        /// appearance and behavior.
        /// </summary>
        /// <remarks>This method configures the window with predefined settings,
        /// including size, padding, and  positioning. It creates a grid layout containing
        /// a read-only text box for displaying details and a "Close" button.
        /// The "Close" button is set as the default and cancel button for the window.</remarks>
        /// <param name="details">The text to display in the read-only
        /// details section of the window.</param>
        /// <param name="title">The text to display in title of the window.</param>
        protected virtual void InitializeControls(string title, string details)
        {
            BeginInit();
            Title = title;
            Padding = new Thickness(10);
            MinimizeEnabled = false;
            MaximizeEnabled = false;
            HasSystemMenu = false;
            CloseEnabled = false;
            StartLocation = WindowStartLocation.CenterOwner;
            TopMost = true;
            Size = (600, 500);

            Border border = new();
            border.VerticalAlignment = VerticalAlignment.Fill;

            detailsTextBox.Text = details;
            detailsTextBox.Parent = border;

            Layout = LayoutStyle.Vertical;
            border.Parent = this;

            var buttonPanel = new Panel();
            buttonPanel.Parent = this;
            buttonPanel.VerticalAlignment = VerticalAlignment.Bottom;

            copyButton.Click += (o, e) => Clipboard.SetText(detailsTextBox.Text);
            copyButton.Parent = buttonPanel;

            closeButton.Click += (o, e) => Close();
            closeButton.Parent = buttonPanel;

            buttonPanel.Parent = this;
            EndInit();
        }
    }
}