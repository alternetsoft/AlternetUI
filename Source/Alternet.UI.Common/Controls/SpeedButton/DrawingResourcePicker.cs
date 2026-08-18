using System;
using System.ComponentModel;

using Alternet.Drawing;
using Alternet.UI.Extensions;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Implements <see cref="SpeedButton"/> for editing of the <see cref="DrawingResource"/> values.
    /// </summary>
    [ControlCategory(KnownControlCategory.Editors)]
    public partial class DrawingResourcePicker : SpeedButton
    {
        /// <summary>
        /// Gets or sets whether to assign default control colors
        /// in the constructor. Default is <c>true</c>.
        /// </summary>
        public static bool DefaultUseControlColors = true;

        private DrawingResource? data;
        private SizeD valueImageSize = SpeedColorButton.DefaultColorImageSizeDips;
        private PopupColorListBox? popupWindow;
        private ClickActionKind actionKind = ClickActionKind.ShowPopup;
        private ClickActionKind longTapAction = ClickActionKind.None;
        private Color? disabledImageColor;
        private bool useDisabledImageColor = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpeedColorButton"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public DrawingResourcePicker(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpeedColorButton"/> class.
        /// </summary>
        public DrawingResourcePicker()
        {
            TextVisible = true;
            OnValueImageChanged(false);
            ShowComboBoxImageAtRight();
            ClickTrigger = ClickTriggerKind.MouseDown;
            UseTheme = KnownTheme.StaticBorder;
            UseControlColors(DefaultUseControlColors);
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
        /// Gets or sets the title displayed when <see cref="Color.Empty"/> is selected.
        /// </summary>
        public virtual string? EmptyColorTitle { get; set; }

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
                    popupWindow.Title = CommonStrings.Default.WindowTitleSelectColor;
                    popupWindow.AfterHide += PopupWindowAfterHideHandler;
                }

                return popupWindow;
            }
        }

        /// <summary>
        /// Gets or sets what happens when the user clicks this button.
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
        /// button is clicked.
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
        /// Gets or sets size of the color image in device-independent units.
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
        /// Gets or sets selected color.
        /// </summary>
        public virtual DrawingResource? Value
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
                base.Text = data?.Title ?? string.Empty;
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
        public virtual Color? DisabledImageColor
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
        /// of the color image
        /// when control is disabled.
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
        /// Gets or sets <see cref="Value"/> as <see cref="string"/>.
        /// </summary>
        [Browsable(false)]
        internal new string? Text
        {
            get
            {
                return string.Empty;
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
        /// Shows color popup or dialog (depends on the value of <see cref="ActionKind"/> property).
        /// Called when control is clicked.
        /// </summary>
        public virtual void ShowColorSelector(ClickActionKind? kind = null)
        {
            switch (kind ?? ActionKind)
            {
                case ClickActionKind.ShowPopup:
                    ShowColorPopup();
                    break;
            }
        }

        /// <summary>
        /// Shows color popup.
        /// </summary>
        public virtual void ShowColorPopup()
        {
            if (!Enabled)
                return;

            PopupWindow.MainControl.Value = Value;
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
                    ShowColorSelector(LongTapAction);
            });
        }

        /// <inheritdoc/>
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            ShowColorSelector();
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
        protected virtual void PopupWindowAfterHideHandler(object? sender, EventArgs e)
        {
            if (PopupWindow.PopupResult == ModalResult.Accepted)
                Value = PopupWindow.ResultAsDrawingResource;
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
            DrawingResource? imageResource = data ?? Color.Transparent;

            if (!Enabled && useDisabledImageColor)
            {
                var disabledColor = DisabledImageColor ?? ColorListBox.DefaultDisabledImageColor;
                if (disabledColor is not null)
                    imageResource = disabledColor;
            }

            if (imageResource is null)
            {
                LabelImage = null;
            }
            else
            {
                if(imageResource.HasBrush)
                    LabelImage = imageResource.Brush?.AsImageWithBorder(valueImageSize, ScaleFactor);
                else
                    if (imageResource.HasColor)
                        LabelImage = imageResource.Color?.AsBrush.AsImageWithBorder(valueImageSize, ScaleFactor);
                    else
                        LabelImage = null;
            }

            if (refresh)
                Refresh();
        }
    }
}