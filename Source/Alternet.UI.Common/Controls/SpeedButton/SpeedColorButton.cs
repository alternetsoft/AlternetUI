using System;
using System.ComponentModel;

using Alternet.Drawing;
using Alternet.UI.Extensions;
using Alternet.UI.Localization;

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
        private ClickActionKind ctrlAction = ClickActionKind.ShowDialog;
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
            base.Text = GetColorAsString() ?? string.Empty;
            TextVisible = true;
            OnColorImageChanged(false);
            ShowComboBoxImageAtRight();
            ClickTrigger = ClickTriggerKind.MouseDown;
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
        public string? EmptyColorTitle { get; set; }

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
                    popupWindow.Title = CommonStrings.Default.WindowTitleSelectColor;
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
        /// Gets or sets what happens when the user clicks this button while Ctrl key is pressed.
        /// </summary>
        public virtual ClickActionKind CtrlActionKind
        {
            get => ctrlAction;
            set => ctrlAction = value;
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
                OnColorImageChanged();
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
                if (DisposingOrDisposed)
                    return;
                if (dlgResult)
                    Value = ColorDialog.Color;
            });
        }

        /// <summary>
        /// Finds an existing color in the list or adds the specified color if it does not exist.
        /// </summary>
        /// <param name="value">The <see cref="Color"/> to find or add.</param>
        /// <param name="title">An optional title associated with the color.
        /// If <see langword="null"/>, no title is assigned.</param>
        /// <returns>The existing <see cref="Color"/> if found; otherwise,
        /// the newly added <see cref="Color"/>.  Returns <see langword="null"/>
        /// if the operation fails.</returns>
        public virtual Color? FindOrAddColor(Color? value, string? title = null)
        {
            return ListBox.FindOrAddColor(value, title);
        }

        /// <summary>
        /// Adds color to the list of colors.
        /// </summary>
        /// <param name="title">Color title. Optional. If not specified,
        /// <see cref="Color.NameLocalized"/> will be used.</param>
        /// <param name="value">Color value.</param>
        public virtual ListControlItem AddColor(Color? value, string? title = null)
        {
            return ListBox.AddColor(value, title);
        }

        /// <summary>
        /// Selects a color and updates the current value to the selected color.
        /// </summary>
        /// <remarks>This method finds or adds the specified color to the collection
        /// and sets it as the current value. If the color already exists,
        /// it is reused; otherwise, it is added with the provided title.</remarks>
        /// <param name="value">The color to be selected.</param>
        /// <param name="title">An optional title associated with
        /// the selected color. If <see langword="null"/>, no custom title is assigned.</param>
        public virtual void Select(Color? value, string? title = null)
        {
            var color = FindOrAddColor(value, title);
            Value = color;
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

            if(Keyboard.IsControlPressed)
                ShowColorSelector(ctrlAction);
            else
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

            var result = Value?.ToDisplayString();

            if(string.IsNullOrEmpty(result) && Value == Color.Empty)
            {
                var item = ListBox.Find(Color.Empty);
                result = item?.DisplayText ?? item?.Text ?? EmptyColorTitle;
            }

            return result;
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

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            SafeDispose(ref colorDialog);
            SafeDispose(ref popupWindow);

            base.DisposeManaged();
        }

        /// <summary>
        /// Raised when color image is changed.
        /// </summary>
        protected virtual void OnColorImageChanged(bool refresh = true)
        {
            var imageColor = color ?? Color.Transparent;

            if (!Enabled && useDisabledImageColor)
            {
                var disabledColor = DisabledImageColor ?? ColorListBox.DefaultDisabledImageColor;
                if (disabledColor is not null)
                    imageColor = disabledColor;
            }

            LabelImage = imageColor.AsImageWithBorder(colorImageSize, ScaleFactor);

            if(refresh)
                Refresh();
        }
    }
}