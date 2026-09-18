using System;
using System.ComponentModel;

using Alternet.Drawing;
using Alternet.UI.Extensions;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Implements <see cref="SpeedButton"/> for editing of the <see cref="ThemedDrawingResource"/> values.
    /// In the editor, the <see cref="ThemedDrawingResource"/> value can be changed by selecting an item from the list box.
    /// Item image is painted using the <see cref="ThemedDrawingResource"/> value.
    /// <see cref="ThemedDrawingResource"/> can be defined by a brush, pen, or color.
    /// </summary>
    [ControlCategory(KnownControlCategory.Editors)]
    public partial class DrawingResourcePicker : SpeedButton
    {
        /// <summary>
        /// Gets or sets the default value indicating whether to show the drop-down image.
        /// </summary>
        public static bool DefaultShowDropDownImage = true;

        /// <summary>
        /// Gets or sets the default left padding for the text displayed in the control.
        /// </summary>
        public static float DefaultTextLeftPadding = 7;

        /// <summary>
        /// Gets or sets the default right padding for the text displayed in the control.
        /// </summary>
        public static float DefaultTextRightPadding = 7;

        /// <summary>
        /// Gets or sets default shape of the item image.
        /// </summary>
        public static DrawingShapeType? DefaultValueImageShape = DrawingShapeType.Circle;

        /// <summary>
        /// Gets or sets whether to assign default control colors
        /// in the constructor. Default is <c>true</c>.
        /// </summary>
        public static bool DefaultUseControlColors = true;

        private IThemedDrawingResource? data;
        private SizeD valueImageSize = SpeedColorButton.DefaultColorImageSizeDips;
        private PopupColorListBox? popupWindow;
        private ClickActionKind actionKind = ClickActionKind.ShowPopup;
        private ClickActionKind longTapAction = ClickActionKind.None;
        private ThemedColor? disabledImageColor;
        private bool useDisabledImageColor = true;
        private DrawingShapeType? valueImageShape = DefaultValueImageShape;
        private ThemedColor? valueImageBorder;
        private bool useFontSizeAsValueImageSize = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawingResourcePicker"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public DrawingResourcePicker(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawingResourcePicker"/> class.
        /// </summary>
        public DrawingResourcePicker()
        {
            TextVisible = true;
            OnValueImageChanged(false);
            ShowComboBoxImageAtRight(DefaultShowDropDownImage);
            ClickTrigger = ClickTriggerKind.MouseDown;
            UseTheme = KnownTheme.StaticBorder;
            UseControlColors(DefaultUseControlColors);
            Label.Padding = Label.Padding.WithLeftRight(DefaultTextLeftPadding, DefaultTextRightPadding);
        }

        /// <summary>
        /// Occurs when <see cref="Value"/> property is changed.
        /// </summary>
        public event EventHandler? ValueChanged;

        /// <summary>
        /// Enumerates possible actions when the user clicks on the button.
        /// </summary>
        public enum ClickActionKind
        {
            /// <summary>
            /// Popup with <see cref="ColorListBox"/> is shown when button is clicked.
            /// </summary>
            ShowPopup,

            /// <summary>
            /// No action is performed when button is clicked.
            /// </summary>
            None,
        }

        /// <summary>
        /// Gets or sets the border color of the value image.
        /// </summary>
        [Browsable(false)]
        public virtual ThemedColor? ValueImageBorder
        {
            get => valueImageBorder;
            set
            {
                if (value == valueImageBorder)
                    return;
                valueImageBorder = value;
                OnValueImageChanged(refresh: true);
            }
        }

        /// <summary>
        /// Gets or sets the shape of the value image.
        /// </summary>
        public virtual DrawingShapeType? ValueImageShape
        {
            get => valueImageShape;
            set
            {
                if (value == valueImageShape)
                    return;
                valueImageShape = value;
                OnValueImageChanged(refresh: true);
            }
        }

        /// <summary>
        /// Gets the underlying <see cref="ColorListBox"/> control used within the popup window.
        /// </summary>
        [Browsable(false)]
        public ColorListBox ListBox
        {
            get
            {
                return PopupWindow.MainControl;
            }
        }

        /// <summary>
        /// Gets attached popup window with <see cref="ColorListBox"/>.
        /// </summary>
        [Browsable(false)]
        public virtual PopupColorListBox PopupWindow
        {
            get
            {
                if (popupWindow is null)
                {
                    popupWindow = new(defaultColors: false);
                    popupWindow.Title = CommonStrings.Default.WindowTitleSelectValue;
                    popupWindow.AfterHide += OnPopupWindowAfterHide;
                }

                return popupWindow;
            }
        }

        /// <summary>
        /// Gets or sets what happens when the user clicks this control.
        /// </summary>
        public virtual ClickActionKind ActionKind
        {
            get
            {
                return actionKind;
            }

            set
            {
                if (actionKind == value)
                    return;
                actionKind = value;
            }
        }

        /// <summary>
        /// Gets or sets whether to show popup window with <see cref="ColorListBox"/> when
        /// control is clicked.
        /// </summary>
        [Browsable(false)]
        public virtual bool ShowPopupWindow
        {
            get => actionKind == ClickActionKind.ShowPopup;

            set
            {
                if (ShowPopupWindow == value)
                    return;
                if (value)
                    actionKind = ClickActionKind.ShowPopup;
                else
                    actionKind = ClickActionKind.None;
            }
        }

        /// <summary>
        /// Gets or sets size of the value image in device-independent units.
        /// </summary>
        public virtual SizeD ValueImageSizeDips
        {
            get => valueImageSize;

            set
            {
                if (valueImageSize == value)
                    return;
                valueImageSize = value;
                OnValueImageChanged();
            }
        }

        /// <summary>
        /// Gets or sets selected value.
        /// </summary>
        public virtual IThemedDrawingResource? Value
        {
            get
            {
                return data;
            }

            set
            {
                if (data == value)
                    return;
                data = value;
                var s = data?.GetValue(this).Title ?? StringUtils.OneSpace;

                if (s.Length == 0)
                    s = StringUtils.OneSpace;

                base.Text = s;

                ValueChanged?.Invoke(this, EventArgs.Empty);
                OnValueImageChanged();
            }
        }

        /// <summary>
        /// Gets or sets disabled image color.
        /// </summary>
        /// <remarks>
        /// This color is used for painting color image when control is disabled.
        /// If this property is null, color image will be painted using
        /// <see cref="ColorListBox.DefaultDisabledImageColor"/>.
        /// </remarks>
        public virtual ThemedColor? DisabledImageColor
        {
            get
            {
                return disabledImageColor;
            }

            set
            {
                if (disabledImageColor == value)
                    return;
                disabledImageColor = value;
                if (Enabled)
                    return;
                OnValueImageChanged();
            }
        }

        /// <summary>
        /// Gets or sets whether to use <see cref="DisabledImageColor"/> for painting
        /// of the color image when control is disabled.
        /// </summary>
        public virtual bool UseDisabledImageColor
        {
            get
            {
                return useDisabledImageColor;
            }

            set
            {
                if (useDisabledImageColor == value)
                    return;
                useDisabledImageColor = value;
                if (Enabled)
                    return;
                OnValueImageChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to use the font size as the value image size.
        /// </summary>
        [Browsable(true)]
        public virtual bool UseFontSizeAsValueImageSize
        {
            get => useFontSizeAsValueImageSize;
            set
            {
                if (useFontSizeAsValueImageSize == value)
                    return;
                useFontSizeAsValueImageSize = value;
                OnValueImageChanged(refresh: true);
            }
        }

        /// <summary>
        /// Gets or sets <see cref="Value"/> as <see cref="string"/>.
        /// </summary>
        [Browsable(false)]
        public override string Text
        {
            get
            {
                return base.Text;
            }

            set
            {
            }
        }

        /// <summary>
        /// Gets or sets action to call on long tap event.
        /// </summary>
        internal virtual ClickActionKind LongTapAction
        {
            get
            {
                return longTapAction;
            }

            set
            {
                if (longTapAction == value)
                    return;
                longTapAction = value;
                CanLongTap = longTapAction != ClickActionKind.None;
            }
        }

        internal new Image? Image
        {
            get => base.Image;
            set => base.Image = value;
        }

        internal new Image? DisabledImage
        {
            get => base.DisabledImage;
            set => base.DisabledImage = value;
        }

        /// <summary>
        /// Shows popup or does other action depending on the value of <see cref="ActionKind"/> property.
        /// Called when control is clicked.
        /// </summary>
        public virtual void ShowPopup(ClickActionKind? kind = null)
        {
            switch (kind ?? ActionKind)
            {
                case ClickActionKind.ShowPopup:
                    ShowPopup();
                    break;
            }
        }

        /// <summary>
        /// Adds drawng resource item to the list of items.
        /// </summary>
        /// <param name="value">Drawing resource value.</param>
        public virtual ListControlItem Add(IThemedDrawingResource value)
        {
            var item = ListBox.CreateItem(value, () => IsDarkBackground);
            ListBox.Add(item);
            return item;
        }

        /// <summary>
        /// Selects specified <see cref="IThemedDrawingResource"/> in the list box.
        /// </summary>
        /// <param name="newValue">The new value to select.</param>
        public virtual void Select(IThemedDrawingResource? newValue)
        {
            if (newValue is null)
            {
                PopupWindow.MainControl.SelectedIndex = null;
                return;
            }

            for (int i = 0; i < PopupWindow.MainControl.Count; i++)
            {
                var item = PopupWindow.MainControl[i];

                if (item is null)
                    continue;

                if (item.Value is IThemedDrawingResource itemResource)
                {
                    if (itemResource == newValue)
                    {
                        PopupWindow.MainControl.SelectedIndex = i;
                        return;
                    }
                }
            }

            PopupWindow.MainControl.SelectedIndex = null;
        }

        /// <summary>
        /// Shows popup.
        /// </summary>
        public virtual void ShowPopup()
        {
            if (!Enabled)
                return;

            Select(Value);
            PopupWindow.ShowPopup(this);
        }

        /// <inheritdoc/>
        protected override void OnLongTap(LongTapEventArgs e)
        {
            if (!Enabled)
                return;
            App.AddIdleTask(() =>
            {
                if (!IsDisposed)
                    ShowPopup(LongTapAction);
            });
        }

        /// <inheritdoc/>
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            ShowPopup();
        }

        /// <inheritdoc/>
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            OnValueImageChanged();
        }

        /// <summary>
        /// Fired after popup window is closed. Applies color selected in the popup window
        /// to the control.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments</param>
        protected virtual void OnPopupWindowAfterHide(object? sender, EventArgs e)
        {
            if (PopupWindow.PopupResult == ModalResult.Accepted)
            {
                Value = PopupWindow.ResultAsThemedDrawingResource;
            }
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            SafeDispose(ref popupWindow);

            base.DisposeManaged();
        }

        /// <inheritdoc/>
        public override void DefaultPaint(PaintEventArgs e)
        {
            base.DefaultPaint(e);
        }

        /// <summary>
        /// Calculates effective size of the color image.
        /// </summary>
        /// <returns></returns>
        public virtual SizeD EffectiveValueImageSize()
        {
            if (UseFontSizeAsValueImageSize)
            {
                var height = MeasureCanvas.GetTextExtent("Wg", RealFont).Height - 2;
                height = Math.Max(height, valueImageSize.Height);

                var width = height;
                if (ValueImageShape != DrawingShapeType.Circle)
                    width = (height / 2) * 3;

                return new SizeD(width, height);
            }

            return valueImageSize;
        }

        /// <inheritdoc/>
        protected override void OnSystemColorsChanged(EventArgs e)
        {
            UseControlColors(DefaultUseControlColors);
            base.OnSystemColorsChanged(e);
        }

        /// <summary>
        /// Raised when item image is changed.
        /// </summary>
        protected virtual void OnValueImageChanged(bool refresh = true)
        {
            IThemedDrawingResource? imageResource = data;

            if (!Enabled && useDisabledImageColor)
            {
                var disabledColor = DisabledImageColor ?? ColorListBox.DefaultDisabledImageColor;
                if (disabledColor is not null)
                    imageResource = new ThemedDrawingResource(
                        new DrawingResource(disabledColor.Light), new DrawingResource(disabledColor.Dark));
            }

            imageResource ??= new ThemedDrawingResource(new DrawingResource(Color.Empty), new DrawingResource(Color.Empty));

            Brush? brush;

            var val = imageResource.GetValue(this);

            if (val.HasBrush)
                brush = val.Brush;
            else
                if (val.HasColor)
                {
                    brush = val.Color?.AsBrush;
                }
                else
                {
                    brush = Color.Empty.AsBrush;
                }

            LabelImage = brush?.AsImageWithBorder(
                EffectiveValueImageSize(),
                ScaleFactor,
                (ValueImageBorder ?? ListControlItem.DefaultImageBorderColor).GetColor(this),
                ValueImageShape);

            if (refresh)
                Refresh();
        }
    }
}