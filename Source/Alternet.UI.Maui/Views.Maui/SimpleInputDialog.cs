using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI.Extensions;

using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Handlers;

namespace Alternet.Maui
{
    /// <summary>
    /// Represents a simple input dialog with a title, message, input field, and action buttons.
    /// </summary>
    public partial class SimpleInputDialog : BaseContentView
    {
        /// <summary>
        /// Gets or sets the default placeholder text color for the input field.
        /// </summary>
        public static Alternet.Drawing.LightDarkColor DefaultPlaceholderColor
            = new(light: Alternet.Drawing.Color.Gray, dark: Alternet.Drawing.Color.Gray);

        /// <summary>
        /// Gets or sets the default background color for the dialog in light and dark themes.
        /// </summary>
        public static Alternet.Drawing.LightDarkColor DefaultBackColor
            = new(light: Alternet.Drawing.Color.White, dark: Alternet.Drawing.Color.FromRgb(30, 30, 30));

        /// <summary>
        /// Gets or sets the default text color for the dialog in light and dark themes.
        /// </summary>
        public static Alternet.Drawing.LightDarkColor DefaultTextColor
            = new(light: Alternet.Drawing.Color.Black, dark: Alternet.Drawing.Color.FromRgb(220, 220, 220));

        private readonly BaseEntry entry;
        private readonly SimpleDialogTitleView dialogTitle;
        private readonly Border entryBorder;
        private readonly Border dialogBorder;
        private readonly VerticalStackLayout contentLayout;
        private readonly Label label;
        private readonly SimpleToolBarView buttons;
        private readonly VerticalStackLayout dialogLayout;

        private Alternet.UI.HVAlignment? alignment;
        private Alternet.UI.WeakReferenceValue<object> owner = new();

        static SimpleInputDialog()
        {
            Microsoft.Maui.Handlers.PageHandler.Mapper.AppendToMapping(nameof(PageHandler), (handler, view) =>
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
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleInputDialog"/> class.
        /// </summary>
        public SimpleInputDialog()
        {
            IsVisible = false;

            dialogTitle = new SimpleDialogTitleView();
            dialogTitle.Title = "Input";

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

            label = new Label
            {
                Text = "Enter value:",
                Margin = new Thickness(5, 5, 5, 5),
            };

            contentLayout.Children.Add(label);

            entryBorder = new Border
            {
                StrokeThickness = 1,
                Margin = new Thickness(5),
                Padding = new Thickness(0),
                StrokeShape = new RoundRectangle { CornerRadius = 5 },
            };

            entry = new BaseEntry
            {
                Placeholder = "Type here",
            };

            entry.EscapeClicked += (s, e) =>
            {
                OnCancelButtonClicked(UI.DialogCloseAction.EscapeKey);
            };

            entry.Completed += (s, e) =>
            {
                OnOkButtonClicked(UI.DialogCloseAction.EnterKey);
            };

            entryBorder.Content = entry;
            contentLayout.Children.Add(entryBorder);

            buttons = new SimpleToolBarView();
            buttons.Margin = new(0, 5, 0, 0);

            buttons.AddExpandingSpace();
            buttons.AddButtonOk(() =>
            {
                OnOkButtonClicked(UI.DialogCloseAction.OkButton);
            });

            buttons.AddButtonCancel(() =>
            {
                OnCancelButtonClicked(UI.DialogCloseAction.CancelButton);
            });

            contentLayout.Children.Add(buttons);

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
        /// Occurs when the OK button is clicked.
        /// </summary>
        public event EventHandler? OkButtonClicked;

        /// <summary>
        /// Occurs when the Cancel button is clicked.
        /// </summary>
        public event EventHandler? CancelButtonClicked;

        /// <summary>
        /// Gets or sets a value indicating whether the dialog should close when the Cancel button is clicked.
        /// </summary>
        public virtual bool CloseWhenCancelButtonClicked { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the dialog should close when the OK button is clicked.
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
        /// Gets or sets the message displayed in the dialog.
        /// </summary>
        public virtual string Message
        {
            get => label.Text;
            set => label.Text = value;
        }

        /// <summary>
        /// Gets or sets the text entered in the input field.
        /// </summary>
        public virtual string Text
        {
            get => entry.Text;
            set => entry.Text = value;
        }

        /// <summary>
        /// Gets or sets the placeholder text for the input field.
        /// </summary>
        public virtual string Placeholder
        {
            get => entry.Placeholder;
            set => entry.Placeholder = value;
        }

        /// <summary>
        /// Gets the input field of the dialog.
        /// </summary>
        public Entry Entry => entry;

        /// <summary>
        /// Gets the title view of the dialog.
        /// </summary>
        public SimpleDialogTitleView DialogTitle => dialogTitle;

        /// <summary>
        /// Gets the border of the input field.
        /// </summary>
        public Border EntryBorder => entryBorder;

        /// <summary>
        /// Gets the border of the dialog.
        /// </summary>
        public Border DialogBorder => dialogBorder;

        /// <summary>
        /// Gets the layout containing the content of the dialog.
        /// </summary>
        public VerticalStackLayout ContentLayout => contentLayout;

        /// <summary>
        /// Gets the label displaying the message in the dialog.
        /// </summary>
        public Label Label => label;

        /// <summary>
        /// Gets the toolbar containing the action buttons of the dialog.
        /// </summary>
        public SimpleToolBarView Buttons => buttons;

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
        /// Creates a new instance of <see cref="SimpleInputDialog"/> configured as a "Go To Line" dialog.
        /// </summary>
        /// <returns>A <see cref="SimpleInputDialog"/> instance with pre-configured title,
        /// message, and numeric input keyboard.</returns>
        public static SimpleInputDialog CreateGoToLineDialog()
        {
            SimpleInputDialog result = new();

            result.Title = "Go To Line";
            result.Message = "Line number";
            result.Text = "1";
            result.Entry.Keyboard = Keyboard.Numeric;

            return result;
        }

        /// <summary>
        /// Sets the aligned position of the dialog within the specified layout using the provided alignment.
        /// </summary>
        /// <param name="layout">The parent layout in which the dialog is to be aligned.</param>
        /// <param name="align">The horizontal and vertical alignment to apply.</param>
        /// <returns>True if the alignment was successfully applied; otherwise, false.</returns>
        public virtual bool SetAlignedPosition(AbsoluteLayout? layout, Alternet.UI.HVAlignment? align)
        {
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
            if(CloseWhenOkButtonClicked)
                IsVisible = false;
            OkButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Handles the event when the 'Cancel' button is clicked, 'X' button in the top-right
        /// corner of the dialog title is clicked or 'Escape' key is pressed.
        /// </summary>
        public virtual void OnCancelButtonClicked(Alternet.UI.DialogCloseAction action)
        {
            if(CloseWhenCancelButtonClicked)
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
            var isDark = Alternet.UI.MauiUtils.IsDarkTheme;

            var backColor = Alternet.UI.MauiUtils.Convert(DefaultBackColor.LightOrDark(isDark));
            var textColor = Alternet.UI.MauiUtils.Convert(DefaultTextColor.LightOrDark(isDark));

            var borderColor = dialogTitle.GetPressedBorderColor();
            var placeholderColor = Alternet.UI.MauiUtils.Convert(DefaultPlaceholderColor.LightOrDark(isDark));

            dialogBorder.Stroke = borderColor;
            dialogBorder.BackgroundColor = backColor;
            label.TextColor = textColor;
            entryBorder.Stroke = borderColor;
            entryBorder.BackgroundColor = backColor;
            entry.TextColor = textColor;
            entry.PlaceholderColor = placeholderColor;
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

            if(propertyName == nameof(IsVisible))
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