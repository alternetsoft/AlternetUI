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
        private readonly ImagePrimitivePainter primitive = new();

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
                return StateObjects?.Images?.GetObjectOrNull(GenericControlState.Normal);
            }

            set
            {
                if (Image == value)
                    return;
                StateObjects ??= new();
                StateObjects.Images ??= new();
                StateObjects.Images.Normal = value;
                RaiseImageChanged(EventArgs.Empty);
                Invalidate();
            }
        }

        /// <inheritdoc cref="Control.Background"/>
        [Browsable(true)]
        public override Brush? Background
        {
            get => base.Background;
            set
            {
                base.Background = value;
            }
        }

        /// <summary>
        /// Gets <see cref="Graphics"/> for the <see cref="Image"/> on which
        /// you can paint.
        /// </summary>
        public Graphics? Canvas
        {
            get
            {
                return Image?.GetDrawingContext();
            }
        }

        /// <summary>
        /// Gets or sets whether to center image vertically in the control rectangle.
        /// Default is <c>true</c>. This property is used when image is not stretched.
        /// </summary>
        public bool CenterVert
        {
            get
            {
                return primitive.CenterVert;
            }

            set
            {
                if (primitive.CenterVert == value)
                    return;
                primitive.CenterVert = value;
                Refresh();
            }
        }

        /// <summary>
        /// Gets or sets whether to center image horizontally in the control rectangle.
        /// Default is <c>true</c>. This property is used when image is not stretched.
        /// </summary>
        public bool CenterHorz
        {
            get
            {
                return primitive.CenterHorz;
            }

            set
            {
                if (primitive.CenterHorz == value)
                    return;
                primitive.CenterHorz = value;
                Refresh();
            }
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

        [Browsable(false)]
        internal new Color? ForegroundColor
        {
            get => null;
            set { }
        }

        [Browsable(false)]
        internal new LayoutDirection LayoutDirection
        {
            get => LayoutDirection.Default;
            set { }
        }

        [Browsable(false)]
        internal new bool IsBold
        {
            get => false;
            set { }
        }

        [Browsable(false)]
        internal new Font? Font
        {
            get => null;
            set { }
        }

        internal ImagePrimitivePainter Primitive => primitive;

        void IValidatorReporter.SetErrorStatus(object? sender, bool showError, string? errorText)
        {
            ToolTip = errorText ?? string.Empty;
            ImageVisible = showError;
        }

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

        /// <inheritdoc/>
        protected override void DefaultPaint(Graphics dc, RectD rect)
        {
            BeforePaint(dc, rect);

            DrawDefaultBackground(dc, rect);

            var primitive = Primitive;
            var state = CurrentState;

            primitive.Image = StateObjects?.Images?.GetObjectOrNormal(state);
            primitive.DestRect = rect;
            primitive.Draw(this, dc);

            AfterPaint(dc, rect);
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
        protected override void OnCurrentStateChanged(EventArgs e)
        {
            base.OnCurrentStateChanged(e);

            if (StateObjects is null)
                return;

            if (StateObjects.HasOtherBackgrounds || StateObjects.HasOtherImages
                || StateObjects.HasOtherBorders)
                Refresh();
        }

        /// <inheritdoc/>
        protected override ControlHandler CreateHandler()
        {
            return GetEffectiveControlHandlerHactory().CreatePictureBoxHandler(this);
        }

        internal class PictureBoxHandler : NativeControlHandler<PictureBox, Native.Panel>
        {
            protected override bool NeedsPaint => true;

            public override void OnPaint(Graphics drawingContext)
            {
                Control.DefaultPaint(drawingContext, Control.DrawClientRectangle);
            }

            public override SizeD GetPreferredSize(SizeD availableSize)
            {
                if (Control.Image == null)
                    return base.GetPreferredSize(availableSize);

                var specifiedWidth = Control.SuggestedWidth;
                var specifiedHeight = Control.SuggestedHeight;
                if (!double.IsNaN(specifiedWidth) && !double.IsNaN(specifiedHeight))
                    return new SizeD(specifiedWidth, specifiedHeight);

                return Control.Image.SizeDip(Control);
            }

            internal override Native.Control CreateNativeControl()
            {
                var result = new Native.Panel
                {
                    AcceptsFocusAll = false,
                };
                return result;
            }
        }
    }
}