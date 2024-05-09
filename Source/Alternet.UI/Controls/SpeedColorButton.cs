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
        public static SizeI DefaultColorImageSize = 24;

        private Color? color = Color.Black;
        private SizeI colorImageSize = DefaultColorImageSize;
        private ColorDialog? colorDialog;
        private PopupColorListBox? popupWindow;
        private bool showDialog = false;
        private bool showPopupWindow = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpeedColorButton"/> class.
        /// </summary>
        public SpeedColorButton()
        {
            TextVisible = true;
            UpdateImage();
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
        /// Gets attached popup window with <see cref="ColorListBox"/>.
        /// </summary>
        public PopupColorListBox PopupWindow
        {
            get
            {
                if(popupWindow is null)
                {
                    popupWindow = new();
                    popupWindow.AfterHide += PopupWindow_AfterHide;
                }

                return popupWindow;
            }
        }

        /// <summary>
        /// Gets or sets whether to show <see cref="ColorDialog"/> when
        /// button is clicked.
        /// </summary>
        public virtual bool ShowDialog
        {
            get => showDialog;

            set
            {
                if (showDialog = value)
                    return;
                showDialog = value;
                if (value)
                    showPopupWindow = false;
            }
        }

        /// <summary>
        /// Gets or sets whether to show popup window with <see cref="ColorListBox"/> when
        /// button is clicked.
        /// </summary>
        public virtual bool ShowPopupWindow
        {
            get => showPopupWindow;

            set
            {
                if (showPopupWindow = value)
                    return;
                showPopupWindow = value;
                if (value)
                    showDialog = false;
            }
        }

        /// <summary>
        /// Gets or sets size of the color image.
        /// </summary>
        public virtual SizeI ColorImageSize
        {
            get => colorImageSize;
            set
            {
                if (colorImageSize == value)
                    return;
                colorImageSize = value;
                UpdateImage();
                Invalidate();
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
                base.Text = InternalAsString() ?? string.Empty;
                ValueChanged?.Invoke(this, EventArgs.Empty);
                UpdateImage();
                Refresh();
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
                return InternalAsString();
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

        /// <inheritdoc/>
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            if (!Enabled)
                return;

            if (ShowDialog)
            {
                colorDialog ??= new ColorDialog();
                if (Value is not null)
                    colorDialog.Color = Value;
                if (colorDialog.ShowModal() == ModalResult.Accepted)
                    Value = colorDialog.Color;
            }
            else
            if(ShowPopupWindow)
            {
                PopupWindow.MainControl.Value = Value;
                PopupWindow.ShowPopup(this);
            }
        }

        /// <summary>
        /// Updates color image using <see cref="Value"/> and <see cref="ColorImageSize"/>
        /// settings.
        /// </summary>
        protected virtual void UpdateImage()
        {
            if (color is null)
                Image = (Bitmap)Color.Transparent.AsImage(colorImageSize);
            else
            {
                Image = (Bitmap)color.AsImage(colorImageSize);
            }
        }

        private void PopupWindow_AfterHide(object? sender, EventArgs e)
        {
            if (PopupWindow.PopupResult == ModalResult.Accepted)
                Value = PopupWindow.ResultValue ?? Color.Black;
        }

        private string? InternalAsString()
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
    }
}