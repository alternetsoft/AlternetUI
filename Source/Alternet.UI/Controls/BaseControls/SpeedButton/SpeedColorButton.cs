using System;
using Alternet.Drawing;

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
        /// Gets or sets whether to show <see cref="ColorDialog"/> when
        /// button is clicked.
        /// </summary>
        public virtual bool ShowDialog { get; set; } = true;

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
                Invalidate();
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

        /// <inheritdoc/>
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            if (!ShowDialog || !Enabled)
                return;
            colorDialog ??= new ColorDialog();
            if(Value is not null)
                colorDialog.Color = Value;
            if (colorDialog.ShowModal() == ModalResult.Accepted)
            {
                Value = colorDialog.Color;
            }

            Refresh();
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