using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

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
                Label.Image = MessageBoxSvg.GetAsBitmap(value.Icon);
                Label.ImageVerticalAlignment = VerticalAlignment.Top;
                Title = value.Caption ?? string.Empty;
                Buttons.SetButtons(value.Buttons);
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

            StateFlags &= ~ControlFlags.StartLocationApplied;

            ShowDialogAsync(value.Owner, (result) =>
            {
                value.Result = DialogFactory.ToDialogResult(Buttons.LastButtonClicked);
                value.OnClose?.Invoke(value);
            });
        }
    }
}