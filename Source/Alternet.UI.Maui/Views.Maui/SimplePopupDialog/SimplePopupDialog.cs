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
        /// Represents the default alignment origin for <see cref="SimplePopupDialog"/>.
        /// </summary>
        public static AlignOrigin DefaultAlignedOrigin = AlignOrigin.Owner;

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
        private AlignOrigin alignOrigin = DefaultAlignedOrigin;
        private Alternet.UI.WeakReferenceValue<object> weakOwner = new();

        static SimplePopupDialog()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimplePopupDialog"/> class.
        /// </summary>
        public SimplePopupDialog()
        {
            IsVisible = false;

            dialogTitle = CreateDialogTitle();
            dialogBorder = CreateDialogBorder();

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
        /// Occurs when the "OK" button is clicked.
        /// </summary>
        public event EventHandler? OkButtonClicked;

        /// <summary>
        /// Occurs when the "Cancel" button is clicked.
        /// </summary>
        public event EventHandler? CancelButtonClicked;

        /// <summary>
        /// Specifies the origin point used for alignment calculations.
        /// </summary>
        /// <remarks>The alignment origin determines the reference point for positioning elements.
        /// Use <see cref="Owner"/> to align relative to the owning element,
        /// or <see cref="Layout"/>  to align relative to the layout container.</remarks>
        public enum AlignOrigin
        {
            /// <summary>
            /// Align relative to the owning element.
            /// </summary>
            Owner,

            /// <summary>
            /// align relative to the layout container.
            /// </summary>
            Layout,
        }

        /// <summary>
        /// Gets or sets a value indicating whether the dialog should close when
        /// the 'Cancel' button is clicked.
        /// </summary>
        public virtual bool CloseWhenCancelButtonClicked { get; set; } = true;

        /// <summary>
        /// Gets or sets the action to be executed when the "Cancel" button is pressed.
        /// </summary>
        public virtual Action? CancelButtonAction { get; set; }

        /// <summary>
        /// Gets or sets the action to be executed when the "OK" button is pressed.
        /// </summary>
        public virtual Action? OkButtonAction { get; set; }

        /// <summary>
        /// Gets the 'Ok' button of the dialog.
        /// </summary>
        public SimpleToolBarView.IToolBarItem? OkButton
        {
            get
            {
                Buttons.Required();
                return okButton;
            }

            protected set
            {
                okButton = value;
            }
        }

        /// <summary>
        /// Gets the 'Cancel' button of the dialog.
        /// </summary>
        public SimpleToolBarView.IToolBarItem? CancelButton
        {
            get
            {
                Buttons.Required();
                return cancelButton;
            }

            protected set
            {
                cancelButton = value;
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
            get => weakOwner.Value;
            set
            {
                weakOwner.Value = value;
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
        /// Gets a value indicating whether the "OK" button is required.
        /// </summary>
        public virtual bool NeedOkButton => true;

        /// <summary>
        /// Gets a value indicating whether the "Cancel" button is required.
        /// </summary>
        public virtual bool NeedCancelButton => true;

        /// <summary>
        /// Gets the toolbar containing the action buttons of the dialog.
        /// </summary>
        public virtual SimpleToolBarView Buttons
        {
            get
            {
                if(buttons is null)
                {
                    buttons = new SimpleToolBarView();
                    buttons.Margin = new(0, 5, 0, 0);

                    buttons.AddExpandingSpace();

                    if (NeedOkButton)
                    {
                        AddOkButton();
                    }

                    if (NeedCancelButton)
                    {
                        AddCancelButton();
                    }

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
        public virtual Alternet.UI.HVAlignment? Alignment
        {
            get => alignment;

            set
            {
                if (alignment == value)
                    return;
                alignment = value;
                if (alignment is null)
                    return;
                SetAlignedPosition(Owner, alignment.Value, alignOrigin);
            }
        }

        /// <summary>
        /// Hides all instances of <see cref="SimplePopupDialog"/> within the specified container.
        /// </summary>
        /// <remarks>This method searches the provided container for all views of type
        /// <see cref="SimplePopupDialog"/> and hides them.
        /// It is typically used to manage the visibility of popup dialogs
        /// in a UI.</remarks>
        /// <param name="container">The <see cref="VisualElement"/> that
        /// contains the dialogs to be hidden.</param>
        /// <param name="noHide">The type of dialog which will not be hidden.</param>
        public static void HideDialogs(VisualElement? container, Type? noHide = null)
        {
            UI.MauiUtils.HideViewsInContainer<SimplePopupDialog>(container, noHide);
        }

        /// <summary>
        /// Sets the aligned position of the dialog within the specified
        /// layout using the provided alignment.
        /// </summary>
        /// <param name="owner">The view in which the dialog is to be aligned.</param>
        /// <param name="origin">Specifies the origin point used for alignment calculations.</param>
        /// <param name="align">The horizontal and vertical alignment to apply.</param>
        /// <returns>True if the alignment was successfully applied; otherwise, false.</returns>
        public virtual bool SetAlignedPosition(
            object? owner,
            Alternet.UI.HVAlignment? align,
            AlignOrigin origin = AlignOrigin.Owner)
        {
            align ??= DefaultAlignedPosition;

            if (align is null)
                return false;

            var result = SetAlignedPosition(
                owner,
                align.Value.Horizontal,
                align.Value.Vertical,
                origin);
            return result;
        }

        /// <summary>
        /// Creates a new <see cref="Border"/> instance for the entry views
        /// configured with default styling.
        /// </summary>
        /// <remarks>This method provides a pre-configured <see cref="Border"/> suitable
        /// for use as an entry border. The returned border can be further customized
        /// by the caller if needed.</remarks>
        /// <returns>A <see cref="Border"/> object with default styling.</returns>
        public virtual Border CreateEntryBorder()
        {
            return new Border
            {
                StrokeThickness = 1,
                Margin = new Thickness(5),
                Padding = new Thickness(0),
                StrokeShape = new RoundRectangle { CornerRadius = 5 },
            };
        }

        /// <summary>
        /// Creates a dialog border with predefined styling.
        /// </summary>
        /// <returns>A <see cref="Border"/> object configured to be used as dialog border.</returns>
        public virtual Border CreateDialogBorder()
        {
            var result = new Border
            {
                StrokeShape = new RoundRectangle { CornerRadius = 10 },
                StrokeThickness = 1,
                MinimumWidthRequest = 300,
            };

            return result;
        }

        /// <summary>
        /// Creates and returns a new instance of <see cref="SimpleDialogTitleView"/>.
        /// </summary>
        /// <returns>A new <see cref="SimpleDialogTitleView"/> instance representing
        /// the dialog title view.</returns>
        public virtual SimpleDialogTitleView CreateDialogTitle()
        {
            var result = new SimpleDialogTitleView();
            return result;
        }

        /// <summary>
        /// Sets the aligned position of the dialog within the specified
        /// layout using the provided alignment.
        /// </summary>
        /// <param name="owner">The view in which the dialog is to be aligned.</param>
        /// <param name="origin">Specifies the origin point used for alignment calculations.</param>
        /// <returns>True if the alignment was successfully applied; otherwise, false.</returns>
        /// <param name="horz">The horizontal alignment to apply.</param>
        /// <param name="vert">The vertical alignment to apply.</param>
        public virtual bool SetAlignedPosition(
            object? owner,
            Alternet.UI.HorizontalAlignment horz,
            Alternet.UI.VerticalAlignment vert,
            AlignOrigin origin = AlignOrigin.Owner)
        {
            Owner = owner;

            var layout = UI.MauiUtils.GetObjectAbsoluteLayout(owner);
            var view = UI.MauiUtils.GetObjectView(owner);

            alignment = new(horz, vert);
            alignOrigin = origin;

            if (layout is null)
                return false;

            UpdateParent(layout);

            var thisBounds = this.Bounds.ToRectD();
            var containerBounds = GetContainerBounds();

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

            Drawing.RectD GetContainerBounds()
            {
                if(origin == AlignOrigin.Layout || view is null)
                    return layout.Bounds.ToRectD();
                Drawing.SizeD size = ((Coord)view.Bounds.Size.Width, (Coord)view.Bounds.Size.Height);
                Drawing.PointD location = Drawing.PointD.Empty;

                while (view is not AbsoluteLayout && view is not null)
                {
                    location.X += (Coord)view.Bounds.X;
                    location.Y += (Coord)view.Bounds.Y;
                    view = view.Parent as View;
                }

                return (location, size);
            }
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
            OkButtonAction?.Invoke();
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
            CancelButtonAction?.Invoke();
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
        /// Adds a "Cancel" button to the dialog if one does not already exist.
        /// </summary>
        /// <remarks>The "Cancel" button triggers the <see cref="OnCancelButtonClicked"/>
        /// method when clicked,  passing <see cref="UI.DialogCloseAction.CancelButton"/>
        /// as the action. If a "Cancel" button already
        /// exists, this method does nothing.</remarks>
        public virtual void AddCancelButton()
        {
            if (cancelButton is not null)
                return;
            cancelButton = Buttons.AddButtonCancel(() =>
            {
                OnCancelButtonClicked(UI.DialogCloseAction.CancelButton);
            });
        }

        /// <summary>
        /// Adds an "OK" button to the dialog if one does not already exist.
        /// </summary>
        /// <remarks>When the button is clicked, the
        /// <see cref="OnOkButtonClicked"/> method is invoked with the
        /// <see cref="UI.DialogCloseAction.OkButton"/> action.</remarks>
        public virtual void AddOkButton()
        {
            if (okButton is not null)
                return;
            okButton = Buttons.AddButtonOk(() =>
            {
                OnOkButtonClicked(UI.DialogCloseAction.OkButton);
            });
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
            var placeholderColor = Alternet.UI.MauiUtils
                .Convert(DefaultPlaceholderColor.LightOrDark(IsDark));
            return placeholderColor;
        }

        /// <summary>
        /// Updates the position of the dialog based on its alignment within the parent layout.
        /// </summary>
        protected virtual void OnUpdatePosition()
        {
            SetAlignedPosition(Owner, alignment, alignOrigin);
        }

        /// <summary>
        /// Calculates the margin to be applied to a dialog label.
        /// </summary>
        /// <remarks>The default implementation returns a uniform margin of 5 units on all sides.
        /// Override this method in a derived class to provide custom margin values.</remarks>
        /// <returns>A <see cref="Thickness"/> structure representing the margin values
        /// for the label.</returns>
        protected virtual Thickness GetLabelMargin()
        {
            return new Thickness(5, 5, 5, 5);
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
                    if (Owner is VisualElement view)
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
