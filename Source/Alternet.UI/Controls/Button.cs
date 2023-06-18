using System;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a button control.
    /// </summary>
    public class Button : ButtonBase
    {
        /// <summary>
        /// Initializes a new <see cref="Button"/> instance.
        /// </summary>
        public Button()
        {
        }

        /// <summary>
        /// Initializes a new <see cref="Button"/> instance with the specified text.
        /// </summary>
        public Button(string text)
        {
            Text = text;
        }

        /// <summary>
        /// Specifies a set of images for different <see cref="Button"/> states.
        /// </summary>
        public ControlStateImages StateImages
        {
            get => Handler.StateImages;
            
            set
            {
                if (value == null)
                    throw new ArgumentNullException();

                Handler.StateImages = value;
            }
        }

        /// <summary>
        /// Gets or sets the image that is displayed on a button control.
        /// </summary>
        /// <value>
        /// The <see cref="Image"/> displayed on the button control. The default value is <see langword="null"/>.
        /// </value>
        public Image? Image
        {
            get
            {
                return StateImages.NormalImage;
            }

            set
            {
                StateImages.NormalImage = value;
            }
        }

        /// <inheritdoc/>
        public new ButtonHandler Handler
        {
            get
            {
                CheckDisposed();
                return (ButtonHandler)base.Handler;
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether a <see cref="Button"/> is the default button. In a modal dialog,
        /// a user invokes the default button by pressing the ENTER key.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the <see cref="Button"/> is the default button; otherwise, <see
        /// langword="false"/>. The default is <see langword="false"/>.
        /// </value>
        public bool IsDefault { get => Handler.IsDefault; set => Handler.IsDefault = value; }

        /// <summary>
        /// Gets or sets a value that indicates whether a <see cref="Button"/> is a Cancel button. In a modal dialog, a
        /// user can activate the Cancel button by pressing the ESC key.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the <see cref="Button"/> is a Cancel button; otherwise, <see langword="false"/>.
        /// The default is <see langword="false"/>.
        /// </value>
        public bool IsCancel { get => Handler.IsCancel; set => Handler.IsCancel = value; }
    }
}