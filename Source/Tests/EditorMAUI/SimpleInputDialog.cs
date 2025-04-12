using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui.Controls.Shapes;

using Alternet.UI.Extensions;

namespace Alternet.Maui
{
    public partial class SimpleInputDialog : BaseContentView
    {
        public static Alternet.Drawing.LightDarkColor DefaultBackColor
            = new(light: Alternet.Drawing.Color.White, dark: Alternet.Drawing.Color.FromRgb(30, 30, 30));

        public static Alternet.Drawing.LightDarkColor DefaultTextColor
            = new(light: Alternet.Drawing.Color.Black, dark: Alternet.Drawing.Color.FromRgb(220, 220, 220));

        private readonly Entry entry;
        private readonly SimpleDialogTitleView dialogTitle;
        private readonly Border entryBorder;
        private readonly Border dialogBorder;
        private readonly VerticalStackLayout contentLayout;
        private readonly Label label;
        private readonly SimpleToolBarView buttons;
        private readonly VerticalStackLayout dialogLayout;

        private Alternet.UI.HVAlignment? alignment;

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
                OnCancelButtonClicked();
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
                StrokeShape = new RoundRectangle { CornerRadius = 5 }
            };

            entry = new Entry
            {
                Placeholder = "Type here",
            };

            entryBorder.Content = entry;
            contentLayout.Children.Add(entryBorder);

            buttons = new SimpleToolBarView();
            buttons.Margin = new(0, 5, 0, 0);

            buttons.AddExpandingSpace();
            buttons.AddButtonOk(OnOkButtonClicked);
            buttons.AddButtonCancel(OnCancelButtonClicked);

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

        public event EventHandler? OkButtonClicked;

        public event EventHandler? CancelButtonClicked;

        public virtual bool CloseWhenCancelButtonClicked { get; set; } = true;

        public virtual bool CloseWhenOkButtonClicked { get; set; } = true;

        public virtual string Title
        {
            get
            {
                return dialogTitle.Title;
            }

            set
            {
                dialogTitle.Title = value;
            }
        }

        public virtual string Message
        {
            get
            {
                return label.Text;
            }

            set
            {
                label.Text = value;
            }
        }

        public virtual string Placeholder
        {
            get
            {
                return entry.Placeholder;
            }

            set
            {
                entry.Placeholder = value;
            }
        }

        public Entry Entry => entry;

        public SimpleDialogTitleView DialogTitle => dialogTitle;

        public Border EntryBorder => entryBorder;

        public Border DialogBorder => dialogBorder;

        public VerticalStackLayout ContentLayout => contentLayout;

        public Label Label => label;

        public SimpleToolBarView Buttons => buttons;

        public VerticalStackLayout DialogLayout => dialogLayout;

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

        public static SimpleInputDialog CreateGoToLineDialog()
        {
            SimpleInputDialog result = new();

            result.Title = "Go To Line";
            result.Message = "Line number";
            result.Placeholder = "1";
            result.Entry.Keyboard = Keyboard.Numeric;

            return result;
        }

        public virtual bool SetAlignedPosition(AbsoluteLayout? layout, Alternet.UI.HVAlignment? align)
        {
            alignment = align;
            if (align is null)
                return false;

            if (!IsVisible && layout is not null)
            {
                SetAbsolutePosition(layout, 0, 0);
            }

            return SetAlignedPosition(layout, align.Value.Horizontal, align.Value.Vertical);
        }

        public virtual bool SetAlignedPosition(
            AbsoluteLayout? layout,
            Alternet.UI.HorizontalAlignment? horz,
            Alternet.UI.VerticalAlignment? vert)
        {
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

            return true;
        }

        public virtual void SetAbsolutePosition(AbsoluteLayout layout, double x, double y)
        {
            UpdateParent(layout);
            layout.SetLayoutBounds(
                this,
                new Rect(x, y, -1, -1));
        }

        public virtual void OnOkButtonClicked()
        {
            if(CloseWhenOkButtonClicked)
                IsVisible = false;
            OkButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        public virtual void OnCancelButtonClicked()
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

        public virtual void ResetColors()
        {
            var isDark = Alternet.UI.MauiUtils.IsDarkTheme;

            var backColor = Alternet.UI.MauiUtils.Convert(DefaultBackColor.LightOrDark(isDark));
            var textColor = Alternet.UI.MauiUtils.Convert(DefaultTextColor.LightOrDark(isDark));

            var borderColor = dialogTitle.GetPressedBorderColor();
            var placeHolderColor = textColor;

            dialogBorder.Stroke = borderColor;
            dialogBorder.BackgroundColor = backColor;
            label.TextColor = textColor;
            entryBorder.Stroke = borderColor;
            entryBorder.BackgroundColor = backColor;
            entry.TextColor = textColor;
            entry.PlaceholderColor = placeHolderColor;
        }

        protected virtual void OnUpdatePosition()
        {
            SetAlignedPosition(Parent as AbsoluteLayout, alignment);
        }

        protected virtual void OnParentSizeChanged(object? sender, EventArgs e)
        {
            OnUpdatePosition();
        }

        protected override void OnParentChanged()
        {
            base.OnParentChanged();
            OnUpdatePosition();
        }

        protected override void OnParentChanging(ParentChangingEventArgs args)
        {
            base.OnParentChanging(args);

            var oldParent = args.OldParent as AbsoluteLayout;
            var newParent = args.NewParent as AbsoluteLayout;

            if (oldParent is not null)
            {
                oldParent.SizeChanged -= OnParentSizeChanged;
            }

            if (newParent is not null)
            {
                newParent.SizeChanged += OnParentSizeChanged;
            }
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