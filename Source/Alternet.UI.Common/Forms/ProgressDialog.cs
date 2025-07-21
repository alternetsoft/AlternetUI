using System;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a dialog window that displays a progress bar and a message to the user.
    /// </summary>
    public class ProgressDialog : DialogWindow
    {
        /// <summary>
        /// The default title of the progress dialog.
        /// </summary>
        public static string DefaultTitle = "Please Wait";

        /// <summary>
        /// The default message displayed in the progress dialog.
        /// </summary>
        public static string DefaultMessage = "Please wait until the operation is complete";

        private readonly Label messageLabel;
        private readonly ProgressBar progressBar;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressDialog"/> class
        /// with default title and message.
        /// </summary>
        public ProgressDialog()
            : this(null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressDialog"/> class
        /// with a specified title and message.
        /// </summary>
        /// <param name="title">The title of the progress dialog. If null, <see cref="DefaultTitle"/>
        /// is used.</param>
        /// <param name="message">The message displayed in the progress dialog. If null,
        /// <see cref="DefaultMessage"/> is used.</param>
        public ProgressDialog(string? title, string? message)
        {
            Title = title ?? DefaultTitle;
            MinimizeEnabled = false;
            MaximizeEnabled = false;
            HasSystemMenu = false;
            CloseEnabled = false;

            StartLocation = WindowStartLocation.CenterScreen;

            messageLabel = new Label
            {
                Text = message ?? DefaultMessage,
                Margin = (0, 0, 0, 10),
            };

            progressBar = new ProgressBar
            {
                MinHeight = 10,
                MinWidth = 200,
            };

            var content = new VerticalStackPanel
            {
                Padding = 20,
                Children = { messageLabel, progressBar },
            };

            content.Parent = this;

            SetSizeToContent();
        }

        /// <summary>
        /// Gets the label control used to display the message in the progress dialog.
        /// </summary>
        public Label MessageLabel => messageLabel;

        /// <summary>
        /// Gets the progress bar control used in the progress dialog.
        /// </summary>
        public ProgressBar ProgressBar => progressBar;

        /// <summary>
        /// Gets or sets the message displayed in the progress dialog.
        /// </summary>
        public virtual string Message
        {
            get => messageLabel.Text;
            set => messageLabel.Text = value;
        }
    }
}
