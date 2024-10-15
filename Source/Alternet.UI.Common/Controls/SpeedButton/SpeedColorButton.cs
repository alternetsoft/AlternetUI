using System;
using System.ComponentModel;

using Alternet.Drawing;
using Alternet.UI.Extensions;

namespace Alternet.UI
{
    /// <summary>
    /// Implements <see cref="SpeedButton"/> for editing of the <see cref="Color"/> values.
    /// </summary>
    public partial class SpeedColorButton : SpeedButton
    {
        /// <summary>
        /// Gets or sets default size of the color image.
        /// </summary>
        public static SizeD DefaultColorImageSizeDips = 12;

        private Color? color = Color.Black;
        private SizeD colorImageSize = DefaultColorImageSizeDips;
        private ColorDialog? colorDialog;
        private PopupColorListBox? popupWindow;
        private ClickActionKind actionKind = ClickActionKind.ShowPopup;
        private ClickActionKind longTapAction = ClickActionKind.None;
        private Color? disabledImageColor;
        private bool useDisabledImageColor = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpeedColorButton"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public SpeedColorButton(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpeedColorButton"/> class.
        /// </summary>
        public SpeedColorButton()
        {
            TextVisible = true;
            OnColorImageChanged(false);
        }

        /// <summary>
        /// Occurs when <see cref="Value"/> property is changed.
        /// </summary>
        public event EventHandler? ValueChanged;

        /// <summary>
        /// Occurs when <see cref="string"/> is converted to <see cref="Color"/>.
        /// </summary>
        public event EventHandler<ValueConvertEventArgs<string?, Color?>>? StringToColor;

        /// <summary>
        /// Occurs when <see cref="string"/> is converted to <see cref="Color"/>.
        /// </summary>
        public event EventHandler<ValueConvertEventArgs<Color?, string?>>? ColorToString;

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
            /// <see cref="ColorDialog"/> is shown when button is clicked.
            /// </summary>
            ShowDialog,

            /// <summary>
            /// No action is performed when button is clicked.
            /// </summary>
            None,
        }

        /// <summary>
        /// Gets attached popup window with <see cref="ColorListBox"/>.
        /// </summary>
        [Browsable(false)]
        public virtual PopupColorListBox PopupWindow
        {
            get
            {
                if(popupWindow is null)
                {
                    popupWindow = new();
                    popupWindow.AfterHide += PopupWindowAfterHideHandler;
                }

                return popupWindow;
            }
        }

        /// <summary>
        /// Gets or sets whether to show <see cref="ColorDialog"/> when
        /// button is clicked.
        /// </summary>
        [Browsable(false)]
        public bool ShowDialog
        {
            get => actionKind == ClickActionKind.ShowDialog;

            set
            {
                if (ShowDialog = value)
                    return;
                actionKind = ClickActionKind.ShowDialog;
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
        public bool ShowPopupWindow
        {
            get => actionKind == ClickActionKind.ShowPopup;

            set
            {
                if (ShowPopupWindow = value)
                    return;
                actionKind = ClickActionKind.ShowPopup;
            }
        }

        /// <summary>
        /// Gets or sets size of the color image in device-independent units.
        /// </summary>
        public virtual SizeD ColorImageSizeDips
        {
            get => colorImageSize;

            set
            {
                if (colorImageSize == value)
                    return;
                colorImageSize = value;
                OnColorImageChanged();
            }
        }

        /// <summary>
        /// Gets <see cref="ColorDialog"/> used in the control.
        /// </summary>
        [Browsable(false)]
        public virtual ColorDialog ColorDialog
        {
            get
            {
                colorDialog ??= new ColorDialog();
                return colorDialog;
            }
        }

        /// <summary>
        /// Gets or sets selected color.
        /// </summary>
        public virtual Color? Value
        {
            get
            {
                return color;
            }

            set
            {
                if (color == value)
                    return;
                color = value;
                base.Text = GetColorAsString() ?? string.Empty;
                ValueChanged?.Invoke(this, EventArgs.Empty);
                OnColorImageChanged();
            }
        }

        /// <summary>
        /// Gets or sets disabled image color.
        /// </summary>
        /// <remarks>
        /// This color is used for painting color image when control is disabled.
        /// If this property is null, color image will be painted using
        /// <see cref="ColorComboBox.DefaultDisabledImageColor"/>.
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
                OnColorImageChanged();
            }
        }

        /// <summary>
        /// Gets or sets whether to use <see cref="DisabledImageColor"/> for painting of the color image
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
                OnColorImageChanged();
            }
        }

        /// <summary>
        /// Gets or sets <see cref="Value"/> as <see cref="string"/>.
        /// </summary>
        [Browsable(false)]
        public new string? Text
        {
            get
            {
                return GetColorAsString();
            }

            set
            {
                if (StringToColor is not null)
                {
                    var e = new ValueConvertEventArgs<string?, Color?>(value);
                    StringToColor(null, e);
                    if (e.Handled)
                    {
                        Value = e.Result;
                        return;
                    }
                }

                Value = Color.Parse(value);
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
                case ClickActionKind.ShowDialog:
                    ShowColorDialog();
                    break;
            }
        }

        /// <summary>
        /// Shows color dialog.
        /// </summary>
        public virtual void ShowColorDialog()
        {
            if (!Enabled)
                return;

            if (Value is not null)
                ColorDialog.Color = Value;

            ColorDialog.ShowAsync((dlg, dlgResult) =>
            {
                if (IsDisposed)
                    return;
                if (dlgResult)
                    Value = ColorDialog.Color;
            });
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
                if(!IsDisposed)
                    ShowColorSelector(LongTapAction);
            });
        }

        /// <inheritdoc/>
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            ShowColorSelector();
        }

        /// <summary>
        /// Gets color value as string.
        /// </summary>
        /// <returns></returns>
        protected virtual string? GetColorAsString()
        {
            if (ColorToString is not null)
            {
                var e = new ValueConvertEventArgs<Color?, string?>(Value);
                ColorToString(null, e);
                if (e.Handled)
                    return e.Result;
            }

            return Value?.ToDisplayString();
        }

        /// <inheritdoc/>
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            OnColorImageChanged();
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
                Value = PopupWindow.ResultValue ?? Color.Black;
        }

        /// <summary>
        /// Raised when color image is changed.
        /// </summary>
        protected virtual void OnColorImageChanged(bool refresh = true)
        {
            var size = GraphicsFactory.PixelFromDip(colorImageSize, ScaleFactor);

            var imageColor = color ?? Color.Transparent;

            if (!Enabled && useDisabledImageColor)
            {
                var disabledColor = DisabledImageColor ?? ColorComboBox.DefaultDisabledImageColor;
                if (disabledColor is not null)
                    imageColor = disabledColor;
            }

            Image = (Bitmap)imageColor.AsImage(size);

            if(refresh)
                Refresh();
        }
    }
}