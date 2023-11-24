using System;
using System.ComponentModel;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a picture box control for displaying an image.
    /// </summary>
    /// <remarks>
    /// Set the <see cref="Image"/> property to the Image you want to display.
    /// </remarks>
    [DefaultProperty("Image")]
    [DefaultBindingProperty("Image")]
    [ControlCategory("Common")]
    public class PictureBox : UserPaintControl, IValidatorReporter
    {
        private readonly DrawImagePrimitive primitive = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="PictureBox"/> class.
        /// </summary>
        public PictureBox()
        {
            BehaviorOptions = ControlOptions.DrawDefaultBackground | ControlOptions.DrawDefaultBorder
                | ControlOptions.RefreshOnCurrentState;
        }

        /// <summary>
        /// Occurs when the <see cref="Image"/> property changes.
        /// </summary>
        public event EventHandler? ImageChanged;

        /// <summary>
        /// Gets or sets the image that is displayed by <see cref="PictureBox"/>.
        /// </summary>
        public Image? Image
        {
            get
            {
                return primitive.Image;
            }

            set
            {
                if (primitive.Image == value)
                    return;
                primitive.Image = value;
                RaiseImageChanged(EventArgs.Empty);
            }
        }
        
        /// <inheritdoc/>
        [Browsable(false)]
        internal new Color? ForegroundColor
        {
            get => null;
            set { }
        }

        /// <inheritdoc/>
        [Browsable(false)]
        internal new LayoutDirection LayoutDirection
        {
            get => LayoutDirection.Default;
            set { }
        }

        /// <inheritdoc/>
        [Browsable(false)]
        internal new bool IsBold
        {
            get => false;
            set { }
        }

        /// <inheritdoc/>
        [Browsable(false)]
        internal new Font? Font
        {
            get => null;
            set { }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to draw image stretched to the size of the control.
        /// </summary>
        public bool ImageStretch
        {
            get
            {
                return primitive.Stretch;
            }

            set
            {
                if (primitive.Stretch == value)
                    return;
                primitive.Stretch = value;
                Refresh();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to draw image.
        /// </summary>
        public bool ImageVisible
        {
            get
            {
                return primitive.Visible;
            }

            set
            {
                if (primitive.Visible == value)
                    return;
                primitive.Visible = value;
                Refresh();
            }
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.PictureBox;

        internal DrawImagePrimitive Primitive => primitive;

        /// <summary>
        /// Raises the <see cref="ImageChanged"/> event and calls
        /// <see cref="OnImageChanged(EventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
        public void RaiseImageChanged(EventArgs e)
        {
            OnImageChanged(e);
            ImageChanged?.Invoke(this, e);
        }

        internal override void DefaultPaint(DrawingContext dc, Rect rect)
        {
            BeforePaint(dc, rect);

            DrawDefaultBackground(dc, rect);

            var primitive = Primitive;
            primitive.DestRect = rect;
            primitive.Draw(dc);

            AfterPaint(dc, rect);
        }

        void IValidatorReporter.SetErrorStatus(object? sender, bool showError, string? errorText)
        {
            ToolTip = errorText ?? string.Empty;
            ImageVisible = showError;
        }

        /// <summary>
        /// Called when the value of the <see cref="Image"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
        protected virtual void OnImageChanged(EventArgs e)
        {
        }

        /// <inheritdoc/>
        protected override void OnCurrentStateChanged()
        {
            base.OnCurrentStateChanged();
            if (Backgrounds?.HasOtherStates ?? false)
                Refresh();
        }

        /// <inheritdoc/>
        protected override ControlHandler CreateHandler()
        {
            return GetEffectiveControlHandlerHactory().CreatePictureBoxHandler(this);
        }
    }
}