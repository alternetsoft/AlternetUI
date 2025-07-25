using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Implements window which can show <see cref="MessageBox"/> like dialogs.
    /// </summary>
    public partial class WindowMessageBox : DialogWindow
    {
        /// <summary>
        /// Gets or sets default minimum width for the message box.
        /// </summary>
        public static Coord? DefaultMinWidth = 250;

        /// <summary>
        /// Gets or sets default maximal width for the text in the message box.
        /// </summary>
        public static Coord DefaultMaxTextWidth = 500;

        /// <summary>
        /// Gets or sets default margin for the controls.
        /// </summary>
        public static Coord DefaultMargin = 5;

        private static WindowMessageBox? textDialog;

        private readonly PanelOkCancelButtons buttons = new(autoCreateButtons: false)
        {
        };

        private readonly Label label = new(CommonStrings.Default.EnterValue)
        {
            Margin = DefaultMargin,
        };

        private readonly Panel panel = new()
        {
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowTextInput"/> class.
        /// </summary>
        public WindowMessageBox()
        {
            Label.WordWrap = true;
            Padding = DefaultMargin;
            HasSystemMenu = false;
            CloseEnabled = false;
            MinimizeEnabled = false;
            StartLocation = WindowStartLocation.CenterOwner;
            MaximizeEnabled = false;
            Resizable = false;
            Layout = LayoutStyle.Vertical;
            panel.Layout = LayoutStyle.Vertical;
            Buttons.UseModalResult = true;

            if(DefaultMinWidth is not null)
                MinWidth = DefaultMinWidth;

            Label.MaxTextWidth = DefaultMaxTextWidth;

            DoInsideLayout(() =>
            {
                label.Parent = panel;
                buttons.Margin = (0, DefaultMargin, 0, 0);
                buttons.Parent = panel;
            });

            panel.VerticalAlignment = VerticalAlignment.Center;
            panel.HorizontalAlignment = HorizontalAlignment.Center;
            panel.Parent = this;
        }

        /// <summary>
        /// Gets the default instance of the <see cref="WindowMessageBox"/> class.
        /// </summary>
        public static new WindowMessageBox Default
        {
            get
            {
                if (textDialog == null)
                    textDialog = new WindowMessageBox();
                return textDialog;
            }
        }

        /// <summary>
        /// Gets label.
        /// </summary>
        [Browsable(false)]
        public Label Label => label;

        /// <summary>
        /// Gets or sets dialog message.
        /// </summary>
        public virtual string Message
        {
            get => Label.Text;
            set => Label.Text = value;
        }

        /// <inheritdoc/>
        public override ControlFlags StateFlags
        {
            get => base.StateFlags;
            protected set
            {
                value &= ~ControlFlags.StartLocationApplied;
                base.StateFlags = value;
            }
        }

        /// <summary>
        /// Gets panel with buttons.
        /// </summary>
        [Browsable(false)]
        public PanelOkCancelButtons Buttons => buttons;

        /// <summary>
        /// Displays a message box with the specified information.
        /// </summary>
        /// <param name="value">The <see cref="MessageBoxInfo"/> object containing the
        /// details to display in the message box. Cannot be <see langword="null"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/>
        /// is <see langword="null"/>.</exception>
        public static void ShowMessageBox(MessageBoxInfo value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            WindowMessageBox.Default.ShowAsMessageBox(value);
        }

        /// <summary>
        /// Configures the message box with "Yes", "No", "Cancel", and "All" buttons,
        /// and sets the specified title and message.
        /// </summary>
        /// <param name="title">The title to display on the message box.</param>
        /// <param name="message">The message content to display within the message box.</param>
        /// <param name="buttonLabels">An optional array of custom labels for the buttons.
        /// The array should contain four elements corresponding to
        /// the "Yes", "No", "Cancel", and "All" buttons, respectively.
        /// If <paramref name="buttonLabels"/> is null or an
        /// element is null, the default label for that button is used.</param>
        /// <returns>The current instance of <see cref="WindowMessageBox"/> configured
        /// with the specified settings.</returns>
        public virtual WindowMessageBox WithYesNoCancelAll(
            string title,
            string message,
            string[]? buttonLabels = null)
        {
            DoInsideLayout(() =>
            {
                Title = title;
                Message = message;
                SetMessageIcon(MessageBoxIcon.Question);

                Buttons.SetButtons(
                    KnownButton.Yes,
                    KnownButton.No,
                    KnownButton.Cancel,
                    KnownButton.All);
                Buttons.SetButtonText(KnownButton.Yes, buttonLabels?[0]);
                Buttons.SetButtonText(KnownButton.No, buttonLabels?[1]);
                Buttons.SetButtonText(KnownButton.Cancel, buttonLabels?[2]);
                Buttons.SetButtonText(KnownButton.All, buttonLabels?[3]);
                Label.ImageVerticalAlignment = VerticalAlignment.Center;
            });

            SetSizeToContent();

            Buttons.SetDefaultButtonExclusive(KnownButton.Yes);
            Buttons.SetCancelButtonExclusive(KnownButton.Cancel);
            ActiveControl = Buttons.GetDefaultButton();
            Buttons.SetDefaultButtonExclusive(null);

            return this;
        }

        /// <summary>
        /// Sets the icon displayed in the message box.
        /// </summary>
        /// <remarks>The method updates the image of the label to reflect
        /// the specified icon.</remarks>
        /// <param name="icon">The <see cref="MessageBoxIcon"/> to display,
        /// or <see langword="null"/> to display no icon.</param>
        public virtual void SetMessageIcon(MessageBoxIcon? icon)
        {
            Label.Image = MessageBoxSvg.GetAsBitmap(icon);
        }

        /// <summary>
        /// Displays a message box with the specified information.
        /// </summary>
        /// <remarks>This method sets up the message box with the provided information
        /// and displays it asynchronously. The result of the user's interaction
        /// with the message box is stored in the <see
        /// cref="MessageBoxInfo.Result"/> property, and
        /// the <see cref="MessageBoxInfo.OnClose"/> callback is invoked upon closing.</remarks>
        /// <param name="value">The <see cref="MessageBoxInfo"/> object containing the details
        /// to display in the message box, such as text,
        /// icon, caption, buttons, and default button.</param>
        public virtual void ShowAsMessageBox(MessageBoxInfo value)
        {
            DoInsideLayout(() =>
            {
                Label.Text = value.Text?.ToString() ?? string.Empty;
                SetMessageIcon(value.Icon);
                Label.ImageVerticalAlignment = VerticalAlignment.Top;
                Title = value.Caption ?? string.Empty;
                Buttons.SetButtons(value.Buttons);
                Buttons.ResetButtonsText();
            });

            Buttons.ButtonClickedAction = () =>
            {
            };

            SetSizeToContent();

            Buttons.SetDefaultButtonExclusive(value.DefaultButton);
            Buttons.SetCancelButtonExclusive(value.Buttons);
            ActiveControl = Buttons.GetDefaultButton();
            if(Buttons.VisibleButtonCount != 1)
                Buttons.SetDefaultButtonExclusive(null);

            ShowDialogAsync(value.Owner, (result) =>
            {
                value.Result = DialogFactory.ToDialogResult(Buttons.LastButtonClicked);
                value.OnClose?.Invoke(value);
            });
        }
    }
}