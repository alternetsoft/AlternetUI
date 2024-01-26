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

        private Color color = Color.Black;
        private SizeI colorImageSize = DefaultColorImageSize;
        private ColorDialog? colorDialog;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpeedColorButton"/> class.
        /// </summary>
        public SpeedColorButton()
        {
            UpdateImage();
        }

        /// <summary>
        /// Gets or sets size of the color image.
        /// </summary>
        public SizeI ColorImageSize
        {
            get => colorImageSize;
            set
            {
                if (colorImageSize == value)
                    return;
                colorImageSize = value;
                UpdateImage();
            }
        }

        /// <summary>
        /// Gets or sets selected color.
        /// </summary>
        public Color Value
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
                UpdateImage();
                Invalidate();
            }
        }

        /// <inheritdoc/>
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            colorDialog ??= new ColorDialog();
            colorDialog.Color = Value;
            if (colorDialog.ShowModal() != ModalResult.Accepted)
                return;
            Value = colorDialog.Color;
        }

        /// <summary>
        /// Updates color image using <see cref="Value"/> and <see cref="ColorImageSize"/>
        /// settings.
        /// </summary>
        protected virtual void UpdateImage()
        {
            Image = (Bitmap)color.AsImage(colorImageSize);
        }
    }
}