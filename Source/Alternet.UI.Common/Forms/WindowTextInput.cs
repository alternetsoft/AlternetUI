using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Implements window with <see cref="Label"/>, <see cref="TextBox"/> and buttons.
    /// </summary>
    public partial class WindowTextInput : DialogWindow
    {
        /// <summary>
        /// Gets or sets whether to use char validator to limit unwanted chars in the input.
        /// </summary>
        public static bool UseCharValidator = true;

        /// <summary>
        /// Gets or sets default margin for the controls.
        /// </summary>
        public static Coord DefaultMargin = 5;

        private static WindowTextInput? textDialog;
        private static WindowTextInput? longDialog;

        private readonly PanelOkCancelButtons buttons = new()
        {
        };

        private readonly Label label = new(CommonStrings.Default.EnterValue)
        {
            Margin = DefaultMargin,
        };

        private readonly TextBoxAndButton edit = new()
        {
            Margin = DefaultMargin,
            MinWidth = 200,
            ButtonsVisible = false,
        };

        private readonly Panel panel = new()
        {
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowTextInput"/> class.
        /// </summary>
        public WindowTextInput()
        {
            Padding = DefaultMargin;
            HasSystemMenu = false;
            CloseEnabled = false;
            MinimizeEnabled = false;
            StartLocation = WindowStartLocation.CenterOwner;
            MaximizeEnabled = false;
            Resizable = false;
            Title = CommonStrings.Default.WindowTitleInput;
            panel.Layout = LayoutStyle.Vertical;

            DoInsideLayout(() =>
            {
                label.Parent = panel;
                edit.HorizontalAlignment = HorizontalAlignment.Stretch;
                edit.InnerOuterBorder = InnerOuterSelector.Outer;
                edit.Parent = panel;
                buttons.Margin = (0, DefaultMargin, 0, 0);
                buttons.Parent = panel;
                panel.Parent = this;
            });

            SetSizeToContent();
            ActiveControl = edit;
            Buttons.UseModalResult = true;
        }

        /// <summary>
        /// Gets label.
        /// </summary>
        [Browsable(false)]
        public Label Label => label;

        /// <summary>
        /// Gets value editor.
        /// </summary>
        [Browsable(false)]
        public TextBoxAndButton Edit => edit;

        /// <summary>
        /// Gets a value indicating whether the message is null or an empty string.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsMessageEmpty => string.IsNullOrEmpty(Message);

        /// <summary>
        /// Gets or sets dialog message.
        /// </summary>
        public virtual string Message
        {
            get => Label.Text;

            set
            {
                if (Message == value)
                    return;
                Label.Text = value;
                var oldVisible = Label.Visible;
                Label.Visible = !IsMessageEmpty;
                if (oldVisible != Label.Visible)
                    SetSizeToContent();
            }
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
        /// Resets the cached dialog instances used by this class.
        /// </summary>
        /// <remarks>
        /// This method calls <see cref="ResetTextDialog"/>, <see cref="ResetLongDialog"/>
        /// and other reset dialog methods to dispose
        /// of the current dialog instances, if they exist. This is useful to clear any existing state
        /// or configuration from the dialogs.
        /// </remarks>
        public static void ResetDialogs()
        {
            ResetTextDialog();
            ResetLongDialog();
        }

        /// <summary>
        /// Disposes and clears the cached single-line text input dialog instance.
        /// </summary>
        /// <remarks>
        /// If the cached instance referenced by single-line text input dialog is not null,
        /// it will be disposed and the reference will be set to null.
        /// </remarks>
        /// <seealso cref="ResetLongDialog"/>
        public static void ResetTextDialog()
        {
            SafeDispose(ref textDialog);
        }

        /// <summary>
        /// Disposes and clears the cached numeric (long) input dialog instance.
        /// </summary>
        /// <remarks>
        /// If the cached instance referenced by (long) input dialog instance is not null,
        /// it will be disposed and the reference will be set to null.
        /// </remarks>
        /// <seealso cref="ResetTextDialog"/>
        public static void ResetLongDialog()
        {
            SafeDispose(ref longDialog);
        }

        /// <summary>
        /// Popups a dialog box with title, message and text input bot.
        /// The user may type in text and press 'OK' or 'Cancel'.
        /// </summary>
        /// <param name="prm">Dialog parameters.</param>
        /// <remarks>
        /// This method uses <see cref="WindowTextInput"/> form instead
        /// of the platform input dialog.
        /// </remarks>
        public static void GetTextFromUserAsync(TextFromUserParams prm)
        {
            textDialog ??= new();
            textDialog.InitAsText(prm);
            prm.OnSetup?.Invoke(textDialog);
            textDialog.ShowDialogAsync(prm.Parent as Window, (result) =>
            {
                var s = textDialog.Edit.Text;
                prm.RaiseActions(result ? s : null);
            });
        }

        /// <summary>
        /// Shows a dialog asking the user for numeric input.
        /// The user may type in number and press 'OK' or 'Cancel'.
        /// </summary>
        /// <param name="prm">Dialog parameters.</param>
        /// <remarks>
        /// This method uses <see cref="WindowTextInput"/> form instead
        /// of the platform input dialog.
        /// </remarks>
        public static void GetLongFromUserAsync(LongFromUserParams prm)
        {
            longDialog ??= new();
            longDialog.InitAsLong(prm);
            prm.OnSetup?.Invoke(longDialog);
            longDialog.ShowDialogAsync(prm.Parent as Window, (result) =>
            {
                long? s;

                try
                {
                    s = (long?)longDialog.Edit.MainControl.TextAsNumber;
                }
                catch
                {
                    s = 0;
                }

                prm.RaiseActions(result ? s : null);
            });
        }

        /// <inheritdoc/>
        public override void SetSizeToContent(
            WindowSizeToContentMode mode = WindowSizeToContentMode.WidthAndHeight,
            SizeD? additionalSpace = null)
        {
            base.SetSizeToContent(mode, additionalSpace);
            var tw = TitleWidth;
            if (Width < tw)
                Width = tw;
        }

        /// <summary>
        /// Assigns dialog properties from the <see cref="LongFromUserParams"/> parameters.
        /// </summary>
        /// <param name="prm">Dialog parameters.</param>
        public virtual void InitAsLong(LongFromUserParams prm)
        {
            Initialize(prm);

            Edit.InnerOuterBorder = InnerOuterSelector.Outer;
            var e = Edit.MainControl;

            e.MinValue = prm.MinValue;
            e.MaxValue = prm.MaxValue;

            e.SetTextAsInt64(prm.DefaultValue ?? 0);

            if (UseCharValidator)
                e.UseCharValidator<long>();
            e.SetErrorText(ValueValidatorKnownError.NumberIsExpected);

            e.DelayedTextChanged -= HandleTextChanged;
            e.DelayedTextChanged += HandleTextChanged;

            void HandleTextChanged(object? sender, EventArgs args)
            {
                Buttons.OkButton.Enabled = !e.HasErrors;
            }
        }

        /// <summary>
        /// Assigns dialog properties from the <see cref="TextFromUserParams"/> parameters.
        /// </summary>
        /// <param name="prm">Dialog parameters.</param>
        public virtual void InitAsText(TextFromUserParams prm)
        {
            Initialize(prm);
            Edit.Text = prm.SafeDefaultValueAsString;
        }

        /// <summary>
        /// Assigns dialog properties from the <see cref="ValueFromUserParams{T}"/> parameters.
        /// </summary>
        /// <param name="prm">Dialog parameters.</param>
        /// <typeparam name="T">Type of the value.</typeparam>
        public virtual void Initialize<T>(ValueFromUserParams<T> prm)
        {
            Title = prm.SafeTitle;
            Message = prm.SafeMessage;
            Buttons.OkButton.Text = prm.SafeAcceptButtonText;
            Buttons.CancelButton.Text = prm.SafeCancelButtonText;
            var maxLength = prm.MaxLength;
            if (maxLength > 0)
            {
                Edit.MainControl.MaxLength = maxLength;
            }
        }
    }
}