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
    public partial class PictureBox : UserPaintControl, IValidatorReporter
    {
        private readonly ImagePrimitivePainter primitive = new();
        private string text = string.Empty;
        private bool textVisible = false;
        private ImageToText imageToText = ImageToText.Horizontal;

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
        /// Gets or sets whether to display text in the control.
        /// </summary>
        public bool TextVisible
        {
            get
            {
                return textVisible;
            }

            set
            {
                if (textVisible == value)
                    return;
                textVisible = value;
                primitive.Visible = !value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets a value which specifies display modes for
        /// item image and text.
        /// </summary>
        public ImageToText ImageToText
        {
            get => imageToText;
            set
            {
                if (imageToText == value)
                    return;
                imageToText = value;
                if (ImageVisible && TextVisible)
                    Invalidate();
            }
        }

        /// <inheritdoc/>
        [DefaultValue("")]
        [Localizability(LocalizationCategory.Text)]
        public override string Text
        {
            get
            {
                return text;
            }

            set
            {
                if (text == value)
                    return;
                text = value ?? string.Empty;
                RaiseTextChanged(EventArgs.Empty);
                if(TextVisible)
                    Invalidate();
            }
        }

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

        /// <summary>
        /// Gets or sets the disabled image that is displayed by <see cref="PictureBox"/>.
        /// </summary>
        public Image? DisabledImage
        {
            get
            {
                return StateObjects?.Images?.GetObjectOrNull(GenericControlState.Disabled);
            }

            set
            {
                if (DisabledImage == value)
                    return;
                StateObjects ??= new();
                StateObjects.Images ??= new();
                StateObjects.Images.Disabled = value;
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
        [Browsable(false)]
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
                Invalidate();
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
                Invalidate();
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
                Invalidate();
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
                textVisible = !value;
                Invalidate();
            }
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.PictureBox;

        /// <summary>
        /// Gets or sets the <see cref="ImageSet"/> that is displayed by <see cref="PictureBox"/>.
        /// </summary>
        [Browsable(false)]
        public ImageSet? ImageSet
        {
            get
            {
                return StateObjects?.ImageSets?.GetObjectOrNull(GenericControlState.Normal);
            }

            set
            {
                if (ImageSet == value)
                    return;
                StateObjects ??= new();
                StateObjects.ImageSets ??= new();
                StateObjects.ImageSets.Normal = value;
                RaiseImageChanged(EventArgs.Empty);
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="ImageSet"/> that is displayed by <see cref="PictureBox"/>.
        /// </summary>
        [Browsable(false)]
        public ImageSet? DisabledImageSet
        {
            get
            {
                return StateObjects?.ImageSets?.GetObjectOrNull(GenericControlState.Disabled);
            }

            set
            {
                if (DisabledImageSet == value)
                    return;
                StateObjects ??= new();
                StateObjects.ImageSets ??= new();
                StateObjects.ImageSets.Disabled = value;
                RaiseImageChanged(EventArgs.Empty);
                Invalidate();
            }
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
        internal override ControlHandler CreateHandler()
        {
            return GetEffectiveControlHandlerHactory().CreatePictureBoxHandler(this);
        }

        /// <inheritdoc/>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            Invalidate();
        }

        /// <inheritdoc/>
        protected override void DefaultPaint(Graphics dc, RectD rect)
        {
            BeforePaint(dc, rect);

            DrawDefaultBackground(dc, rect);

            var primitive = Primitive;
            var state = CurrentState;

            if (TextVisible)
            {
                var color = StateObjects?.Colors?.GetObjectOrNull(state)?.ForegroundColor;

                dc.DrawText(
                    Text ?? string.Empty,
                    ChildrenLayoutBounds.Location,
                    Font ?? UI.Control.DefaultFont,
                    color ?? ForeColor,
                    Color.Empty);
            }
            else
            {
                var image = StateObjects?.Images?.GetObjectOrNull(state);
                image ??= Image;
                primitive.Image = image;
                primitive.ImageSet = StateObjects?.ImageSets?.GetObjectOrNormal(state);
                primitive.DestRect = rect;
                primitive.Draw(this, dc);
            }

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

        /// <summary>
        /// Gets size of the image and text.
        /// </summary>
        /// <returns></returns>
        protected virtual SizeD GetImageAndTextSize()
        {
            var image = Image;
            var imageSet = ImageSet;

            SizeD imageSize = SizeD.Empty;
            SizeD textSize = SizeD.Empty;

            if (image is not null)
                imageSize = image.SizeDip(this);
            else
            if (imageSet is not null)
                imageSize = PixelToDip(imageSet.DefaultSize);

            if (TextVisible)
            {
                textSize = GetTextPreferredSize();
            }

            SizeD result = SizeD.Empty;
            if (ImageToText == ImageToText.Horizontal)
            {
                result.Width = imageSize.Width + textSize.Width;
                result.Height = Math.Max(imageSize.Height, textSize.Height);
            }
            else
            {
                result.Height = imageSize.Height + textSize.Height;
                result.Width = Math.Max(imageSize.Width, textSize.Width);
            }

            return result;

            SizeD GetTextPreferredSize()
            {
                var text = Text;
                if (text == null)
                    return new SizeD();

                using var dc = CreateDrawingContext();
                var result = dc.GetTextExtent(text, Font ?? UI.Control.DefaultFont, this);
                return result;
            }
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
                var specifiedWidth = Control.SuggestedWidth;
                var specifiedHeight = Control.SuggestedHeight;
                if (!double.IsNaN(specifiedWidth) && !double.IsNaN(specifiedHeight))
                    return new SizeD(specifiedWidth, specifiedHeight);

                var result = Control.GetImageAndTextSize();
                if (result != SizeD.Empty)
                    return result + Control.Padding.Size;

                return base.GetPreferredSize(availableSize);
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