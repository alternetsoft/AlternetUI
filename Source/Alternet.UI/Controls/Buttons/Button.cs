using System;
using System.ComponentModel;
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
                Handler.StateImages = value ?? throw new ArgumentNullException();
            }
        }

        /// <inheritdoc/>
        public override ControlId ControlKind => ControlId.Button;

        /// <summary>
        /// Gets or sets a value indicating whether the control has a border.
        /// </summary>
        public bool HasBorder
        {
            get
            {
                return Handler.HasBorder;
            }

            set
            {
                Handler.HasBorder = value;
            }
        }

        /// <summary>
        /// Gets or sets the image that is displayed on a button control.
        /// </summary>
        /// <value>
        /// The <see cref="Image"/> displayed on the button control. The default
        /// value is <see langword="null"/>.
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

        /// <summary>
        /// Gets a <see cref="ButtonHandler"/> associated with this class.
        /// </summary>
        [Browsable(false)]
        public new ButtonHandler Handler
        {
            get
            {
                CheckDisposed();
                return (ButtonHandler)base.Handler;
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether a <see cref="Button"/> is
        /// the default button. In a modal dialog,
        /// a user invokes the default button by pressing the ENTER key.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the <see cref="Button"/> is the default
        /// button; otherwise, <see
        /// langword="false"/>. The default is <see langword="false"/>.
        /// </value>
        public bool IsDefault
        {
            get => Handler.IsDefault;
            set => Handler.IsDefault = value;
        }

        /// <summary>
        /// Gets or sets a value that indicates whether a <see cref="Button"/>
        /// is a Cancel button. In a modal dialog, a
        /// user can activate the Cancel button by pressing the ESC key.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the <see cref="Button"/> is a Cancel
        /// button; otherwise, <see langword="false"/>.
        /// The default is <see langword="false"/>.
        /// </value>
        public bool IsCancel
        {
            get => Handler.IsCancel;
            set => Handler.IsCancel = value;
        }

        /// <summary>
        /// Gets or sets visibility of the text in the bitmap.
        /// </summary>
        public bool TextVisible
        {
            get => Handler.TextVisible;
            set => Handler.TextVisible = value;
        }

        /// <summary>
        /// Sets the position at which the text is displayed.
        /// </summary>
        /// <remarks>
        /// Valid positions are left, top, right, bottom, default.
        /// </remarks>
        public GenericDirection TextAlign
        {
            get => Handler.TextAlign;
            set => Handler.TextAlign = value;
        }

        /// <summary>
        /// Gets or sets value indicating whether this control accepts
        /// input or not (i.e. behaves like a static text) and so doesn't need focus.
        /// </summary>
        /// <remarks>
        /// Default value is true.
        /// </remarks>
        internal bool AcceptsFocus
        {
            get
            {
                return Handler.AcceptsFocus;
            }

            set
            {
                Handler.AcceptsFocus = value;
            }
        }

        /// <summary>
        /// Gets or sets value indicating whether this control accepts
        /// focus from keyboard or not.
        /// </summary>
        /// <remarks>
        /// Default value is true.
        /// </remarks>
        /// <returns>
        /// Return false to indicate that while this control can,
        /// in principle, have focus if the user clicks
        /// it with the mouse, it shouldn't be included
        /// in the TAB traversal chain when using the keyboard.
        /// </returns>
        internal bool AcceptsFocusFromKeyboard
        {
            get
            {
                return Handler.AcceptsFocusFromKeyboard;
            }

            set
            {
                Handler.AcceptsFocusFromKeyboard = value;
            }
        }

        /// <summary>
        /// Indicates whether this control or one of its children accepts focus.
        /// </summary>
        /// <remarks>
        /// Default value is true.
        /// </remarks>
        internal bool AcceptsFocusRecursively
        {
            get
            {
                return Handler.AcceptsFocusRecursively;
            }

            set
            {
                Handler.AcceptsFocusRecursively = value;
            }
        }

        /// <summary>
        /// Sets the position at which the image is displayed.
        /// </summary>
        /// <remarks>
        /// This method should only be called if the button does have
        /// an associated image.
        /// </remarks>
        /// <param name="dir">New image position (left, top, right, bottom).</param>
        public void SetImagePosition(GenericDirection dir)
        {
            if (dir == GenericDirection.Left || dir == GenericDirection.Right ||
                dir == GenericDirection.Top || dir == GenericDirection.Bottom)
            Handler.SetImagePosition(dir);
        }

        /// <summary>
        /// Sets the margins between the image and the text of the button.
        /// </summary>
        /// <remarks>
        /// This method is currently only implemented under Windows.
        /// If it is not called, default margin is used around the image.
        /// </remarks>
        /// <param name="x">New horizontal margin.</param>
        /// <param name="y">New vertical margin.</param>
        public void SetImageMargins(double x, double y)
        {
            Handler.SetImageMargins(x, y);
        }

        /// <inheritdoc/>
        protected override ControlHandler CreateHandler()
        {
            return GetEffectiveControlHandlerHactory().CreateButtonHandler(this);
        }
    }
}