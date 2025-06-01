using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Alternet.Maui.Extensions;

using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Handlers;

namespace Alternet.Maui
{
    /// <summary>
    /// Defines delegate that displays a prompt dialog to the application user with
    /// the intent to capture a single string value.
    /// </summary>
    /// <param name="owner">The object which called the prompt dialog.</param>
    /// <param name="title">The title of the prompt dialog.</param>
    /// <param name="message">The body text of the prompt dialog.</param>
    /// <param name="accept">Text to be displayed on the 'Accept' button.</param>
    /// <param name="cancel">Text to be displayed on the 'Cancel' button.</param>
    /// <param name="placeholder">The placeholder text to display in the prompt.
    /// Can be <see langword="null"/> when no placeholder is desired.</param>
    /// <param name="maxLength">The maximum length of the user response.</param>
    /// <param name="keyboard">The keyboard type to use for the user response.</param>
    /// <param name="initialValue">A pre-defined response that will be displayed,
    /// and which can be edited by the user.</param>
    /// <returns>A <see cref="Task"/> that displays a prompt display and returns the
    /// string value as entered by the user.</returns>
    public delegate Task<string> DisplayPromptAsyncDelegate(
        object? owner,
        string title,
        string message,
        string accept = "OK",
        string cancel = "Cancel",
        string? placeholder = null,
        int maxLength = -1,
        Microsoft.Maui.Keyboard? keyboard = default,
        string initialValue = "");

    /// <summary>
    /// Represents a simple popup dialog with a title, dialog content, and optional action buttons.
    /// </summary>
    public partial class SimplePopupDialog : BaseContentView
    {
        /// <summary>
        /// Represents the default horizontal and vertical alignment
        /// for <see cref="SimplePopupDialog"/>.
        /// </summary>
        public static Alternet.UI.HVAlignment DefaultAlignedPosition
            = Alternet.UI.HVAlignment.TopRight;

        /// <summary>
        /// Gets or sets the default placeholder text color for the input field.
        /// </summary>
        public static Alternet.Drawing.LightDarkColor DefaultPlaceholderColor
            = new(light: Alternet.Drawing.Color.Gray, dark: Alternet.Drawing.Color.Gray);

        /// <summary>
        /// Gets or sets the default background color for the dialog in light and dark themes.
        /// </summary>
        public static Alternet.Drawing.LightDarkColor DefaultBackColor = new(
                light: Alternet.Drawing.Color.White,
                dark: Alternet.Drawing.Color.FromRgb(30, 30, 30));

        /// <summary>
        /// Gets or sets the default text color for the dialog in light and dark themes.
        /// </summary>
        public static Alternet.Drawing.LightDarkColor DefaultTextColor = new(
            light: Alternet.Drawing.Color.Black,
            dark: Alternet.Drawing.Color.FromRgb(220, 220, 220));

        private readonly SimpleDialogTitleView dialogTitle;
        private readonly Border dialogBorder;
        private readonly VerticalStackLayout contentLayout;
        private readonly VerticalStackLayout dialogLayout;

        private SimpleToolBarView.IToolBarItem? okButton;
        private SimpleToolBarView.IToolBarItem? cancelButton;
        private SimpleToolBarView? buttons;
        private Alternet.UI.HVAlignment? alignment;
        private Alternet.UI.WeakReferenceValue<object> owner = new();

        static SimplePopupDialog()
        {
/*
            Microsoft.Maui.Handlers.PageHandler.Mapper
                .AppendToMapping(nameof(PageHandler), (handler, view) =>
            {
#if WINDOWS
                handler.PlatformView.PreviewKeyUp += (s, e) =>
                {
                };

                handler.PlatformView.PreviewKeyDown += (s, e) =>
                {
                    if (e.Key == Windows.System.VirtualKey.Escape)
                    {
                        BaseEntry.FocusedEntry?.RaiseEscapeClicked();
                    }
                };
#endif
            });
*/
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimplePopupDialog"/> class.
        /// </summary>
        public SimplePopupDialog()
        {
            IsVisible = false;

            dialogTitle = new SimpleDialogTitleView();

            dialogBorder = new Border
            {
                StrokeShape = new RoundRectangle { CornerRadius = 10 },
                StrokeThickness = 1,
                MinimumWidthRequest = 300,
            };

            dialogTitle.CloseClicked += (s, e) =>
            {
                OnCancelButtonClicked(UI.DialogCloseAction.CloseButtonInTitleBar);
            };

            dialogLayout = new VerticalStackLayout();
            dialogLayout.VerticalOptions = LayoutOptions.Start;

            dialogLayout.Children.Add(dialogTitle);

            contentLayout = new VerticalStackLayout();
            contentLayout.Padding = 10;

            dialogLayout.Children.Add(contentLayout);

            dialogBorder.Content = dialogLayout;

            Content = dialogBorder;

            DialogBorder.SizeChanged += (s, e) =>
            {
                OnUpdatePosition();
            };

            ResetColors();
        }

        /// <summary>
        /// Occurs when the 'OK' button is clicked.
        /// </summary>
        public event EventHandler? OkButtonClicked;

        /// <summary>
        /// Occurs when the 'Cancel' button is clicked.
        /// </summary>
        public event EventHandler? CancelButtonClicked;

        /// <summary>
        /// Gets or sets a value indicating whether the dialog should close when
        /// the 'Cancel' button is clicked.
        /// </summary>
        public virtual bool CloseWhenCancelButtonClicked { get; set; } = true;

        /// <summary>
        /// Gets the 'Ok' button of the dialog.
        /// </summary>
        public SimpleToolBarView.IToolBarItem OkButton
        {
            get
            {
                Buttons.Required();
                return okButton!;
            }
        }

        /// <summary>
        /// Gets the 'Cancel' button of the dialog.
        /// </summary>
        public SimpleToolBarView.IToolBarItem CancelButton
        {
            get
            {
                Buttons.Required();
                return cancelButton!;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the dialog should close when
        /// the 'OK' button is clicked.
        /// </summary>
        public virtual bool CloseWhenOkButtonClicked { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the owner control should
        /// be focused after the dialog is closed.
        /// </summary>
        public virtual bool FocusOwnerAfterClose { get; set; } = true;

        /// <summary>
        /// Gets or sets the owner of the dialog.
        /// </summary>
        /// <value>
        /// The owner object of the dialog, which can be used to associate the dialog with
        /// a specific parent or context.
        /// </value>
        public virtual object? Owner
        {
            get => owner.Value;
            set
            {
                owner.Value = value;
            }
        }

        /// <summary>
        /// Gets or sets the title of the dialog.
        /// </summary>
        public virtual string Title
        {
            get => dialogTitle.Title;
            set => dialogTitle.Title = value;
        }

        /// <summary>
        /// Gets the title view of the dialog.
        /// </summary>
        public SimpleDialogTitleView DialogTitle => dialogTitle;

        /// <summary>
        /// Gets the border of the dialog.
        /// </summary>
        public Border DialogBorder => dialogBorder;

        /// <summary>
        /// Gets the layout containing the content of the dialog.
        /// </summary>
        public VerticalStackLayout ContentLayout => contentLayout;

        /// <summary>
        /// Gets the toolbar containing the action buttons of the dialog.
        /// </summary>
        public SimpleToolBarView Buttons
        {
            get
            {
                if(buttons is null)
                {
                    buttons = new SimpleToolBarView();
                    buttons.Margin = new(0, 5, 0, 0);

                    buttons.AddExpandingSpace();
                    okButton = buttons.AddButtonOk(() =>
                    {
                        OnOkButtonClicked(UI.DialogCloseAction.OkButton);
                    });

                    cancelButton = buttons.AddButtonCancel(() =>
                    {
                        OnCancelButtonClicked(UI.DialogCloseAction.CancelButton);
                    });

                    contentLayout.Children.Add(buttons);
                }

                return buttons;
            }
        }

        /// <summary>
        /// Gets the layout containing the entire dialog.
        /// </summary>
        public VerticalStackLayout DialogLayout => dialogLayout;

        /// <summary>
        /// Gets or sets the alignment of the dialog within its parent layout.
        /// </summary>
        public Alternet.UI.HVAlignment? Alignment
        {
            get => alignment;

            set
            {
                if (alignment == value)
                    return;
                alignment = value;
                if (alignment is null)
                    return;
                SetAlignedPosition(Parent as AbsoluteLayout, alignment.Value);
            }
        }

        /// <summary>
        /// Sets the aligned position of the dialog within the specified
        /// layout using the provided alignment.
        /// </summary>
        /// <param name="layout">The parent layout in which the dialog is to be aligned.</param>
        /// <param name="align">The horizontal and vertical alignment to apply.</param>
        /// <returns>True if the alignment was successfully applied; otherwise, false.</returns>
        public virtual bool SetAlignedPosition(AbsoluteLayout? layout, Alternet.UI.HVAlignment? align)
        {
            align ??= DefaultAlignedPosition;

            if (align is null)
                return false;

            var result = SetAlignedPosition(layout, align.Value.Horizontal, align.Value.Vertical);
            return result;
        }

        /// <summary>
        /// Sets the aligned position of the dialog within the specified layout using
        /// the provided horizontal and vertical alignment.
        /// </summary>
        /// <param name="layout">The parent layout in which the dialog is to be aligned.</param>
        /// <param name="horz">The horizontal alignment to apply.</param>
        /// <param name="vert">The vertical alignment to apply.</param>
        /// <returns>True if the alignment was successfully applied; otherwise, false.</returns>
        public virtual bool SetAlignedPosition(
            AbsoluteLayout? layout,
            Alternet.UI.HorizontalAlignment horz,
            Alternet.UI.VerticalAlignment vert)
        {
            alignment = new(horz, vert);

            if (layout is null)
                return false;

            UpdateParent(layout);

            var thisBounds = this.Bounds.ToRectD();
            var containerBounds = layout.Bounds.ToRectD();

            if (thisBounds.SizeIsEmpty || containerBounds.SizeIsEmpty)
                return false;

            var alignedBounds = Alternet.UI.AlignUtils.AlignRectInRect(
                thisBounds,
                containerBounds,
                horz,
                vert,
                false);
            SetAbsolutePosition(layout, alignedBounds.X, alignedBounds.Y);
            alignment = new(horz, vert);

            return true;
        }

        /// <summary>
        /// Sets the absolute position of the dialog within the specified layout.
        /// </summary>
        /// <param name="layout">The parent layout in which the dialog is to be positioned.</param>
        /// <param name="x">The X-coordinate of the position.</param>
        /// <param name="y">The Y-coordinate of the position.</param>
        public virtual void SetAbsolutePosition(AbsoluteLayout layout, double x, double y)
        {
            UpdateParent(layout);
            alignment = null;
            layout.SetLayoutBounds(
                this,
                new Rect(x, y, -1, -1));
        }

        /// <summary>
        /// Handles the event when the 'Ok' button is clicked or 'Enter' key is pressed.
        /// </summary>
        public virtual void OnOkButtonClicked(Alternet.UI.DialogCloseAction action)
        {
            if (CloseWhenOkButtonClicked)
                IsVisible = false;
            OkButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Handles the event when the 'Cancel' button is clicked, 'X' button in the top-right
        /// corner of the dialog title is clicked or 'Escape' key is pressed.
        /// </summary>
        public virtual void OnCancelButtonClicked(Alternet.UI.DialogCloseAction action)
        {
            if (CloseWhenCancelButtonClicked)
                IsVisible = false;
            CancelButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public override void RaiseSystemColorsChanged()
        {
            ResetColors();
        }

        /// <summary>
        /// Resets the colors of the dialog based on the current theme (light or dark).
        /// </summary>
        public virtual void ResetColors()
        {
            var backColor = GetBackColor();
            var borderColor = GetBorderColor();
/*
            var textColor = GetTextColor();
            var placeholderColor = GetPlaceholderColor();
*/
            dialogBorder.Stroke = borderColor;
            dialogBorder.BackgroundColor = backColor;
        }

        /// <summary>
        /// Gets the background color of the dialog based on the current theme.
        /// </summary>
        /// <returns>The background color.</returns>
        public virtual Color GetBackColor()
        {
            var backColor = Alternet.UI.MauiUtils.Convert(DefaultBackColor.LightOrDark(IsDark));
            return backColor;
        }

        /// <summary>
        /// Gets the foreground color of the dialog based on the current theme.
        /// </summary>
        /// <returns>The foreground color.</returns>
        public virtual Color GetTextColor()
        {
            var textColor = Alternet.UI.MauiUtils.Convert(DefaultTextColor.LightOrDark(IsDark));
            return textColor;
        }

        /// <summary>
        /// Gets the border color of the dialog based on the current theme.
        /// </summary>
        /// <returns>The border color.</returns>
        public virtual Color GetBorderColor()
        {
            var borderColor = dialogTitle.GetPressedBorderColor();
            return borderColor;
        }

        /// <summary>
        /// Gets the placeholder color for the dialog based on the current theme
        /// </summary>
        /// <returns>The placeholder color.</returns>
        public virtual Color GetPlaceholderColor()
        {
            var placeholderColor = Alternet.UI.MauiUtils.Convert(DefaultPlaceholderColor.LightOrDark(IsDark));
            return placeholderColor;
        }

        /// <summary>
        /// Updates the position of the dialog based on its alignment within the parent layout.
        /// </summary>
        protected virtual void OnUpdatePosition()
        {
            SetAlignedPosition(Parent as AbsoluteLayout, alignment);
        }

        /// <inheritdoc/>
        protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(IsVisible))
            {
                if (IsVisible)
                {
                }
                else
                {
                    if (owner.Value is VisualElement view)
                    {
                        Alternet.UI.App.AddBackgroundInvokeAction(() =>
                        {
                            view.Focus();
                        });
                    }
                }
            }
        }

        /// <inheritdoc/>
        protected override void OnParentSizeChanged(object? sender, EventArgs e)
        {
            OnUpdatePosition();
        }

        /// <inheritdoc/>
        protected override void OnParentChanged()
        {
            base.OnParentChanged();
            OnUpdatePosition();
        }

        private void UpdateParent(AbsoluteLayout layout)
        {
            if (Parent is not null && Parent != layout)
                throw new Exception("Parent is already assigned");
            if (Parent is null)
                layout.Add(this);
        }
    }
}
