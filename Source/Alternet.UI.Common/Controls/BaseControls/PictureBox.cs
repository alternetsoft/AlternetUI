using System;
using System.ComponentModel;
using Alternet.Drawing;
using Alternet.UI.Extensions;

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
    public partial class PictureBox : GraphicControl, IValidatorReporter
    {
        private readonly ImagePrimitivePainter primitive = new();
        private bool textVisible = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="PictureBox"/> class.
        /// </summary>
        public PictureBox()
        {
            RefreshOptions = ControlRefreshOptions.RefreshOnBorder
                | ControlRefreshOptions.RefreshOnBackground
                | ControlRefreshOptions.RefreshOnImage;
        }

        /// <summary>
        /// Occurs when the <see cref="Image"/> property changes.
        /// </summary>
        public event EventHandler? ImageChanged;

        /// <summary>
        /// Gets or sets whether to display text in the control.
        /// </summary>
        public virtual bool TextVisible
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
                PerformLayoutAndInvalidate();
            }
        }

        /// <inheritdoc/>
        [DefaultValue("")]
        public override string Text
        {
            get
            {
                return base.Text;
            }

            set
            {
                if (base.Text == value)
                    return;
                base.Text = value;
                if (!ImageVisible)
                    PerformLayoutAndInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets the image that is displayed by <see cref="PictureBox"/>.
        /// </summary>
        [DefaultValue(null)]
        public virtual Image? Image
        {
            get
            {
                return StateObjects?.Images?.GetObjectOrNull(VisualControlState.Normal);
            }

            set
            {
                if (Image == value)
                {
                    if (ImageVisible)
                        Invalidate();
                    return;
                }

                StateObjects ??= new();
                StateObjects.Images ??= new();
                StateObjects.Images.Normal = value;
                RaiseImageChanged(EventArgs.Empty);
                if(ImageVisible)
                    PerformLayoutAndInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets the disabled image that is displayed by <see cref="PictureBox"/>.
        /// </summary>
        public virtual Image? DisabledImage
        {
            get
            {
                return StateObjects?.Images?.GetObjectOrNull(VisualControlState.Disabled);
            }

            set
            {
                if (DisabledImage == value)
                    return;
                StateObjects ??= new();
                StateObjects.Images ??= new();
                StateObjects.Images.Disabled = value;
                RaiseImageChanged(EventArgs.Empty);
                if (ImageVisible && !Enabled)
                    PerformLayoutAndInvalidate();
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

        /*/// <summary>
        /// Gets <see cref="Graphics"/> for the <see cref="Image"/> on which
        /// you can paint.
        /// </summary>
        [Browsable(false)]
        public virtual Graphics? Canvas
        {
            get
            {
                return Image?.GetDrawingContext();
            }
        }*/

        /// <summary>
        /// Gets or sets whether to center image vertically in the control rectangle.
        /// Default is <c>true</c>. This property is used when image is not stretched.
        /// </summary>
        public virtual bool CenterVert
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
        public virtual bool CenterHorz
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
        /// Gets or sets a value indicating whether to draw image stretched
        /// to the size of the control.
        /// </summary>
        public virtual bool ImageStretch
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
        public virtual bool ImageVisible
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
                PerformLayoutAndInvalidate();
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
                return StateObjects?.ImageSets?.GetObjectOrNull(VisualControlState.Normal);
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
                return StateObjects?.ImageSets?.GetObjectOrNull(VisualControlState.Disabled);
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

        /// <inheritdoc/>
        public override Thickness Padding
        {
            get => base.Padding;
            set
            {
                if (Padding == value)
                    return;
                base.Padding = value;
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
        public override SizeD GetPreferredSize(SizeD availableSize)
        {
            var specifiedWidth = SuggestedWidth;
            var specifiedHeight = SuggestedHeight;
            if (!double.IsNaN(specifiedWidth) && !double.IsNaN(specifiedHeight))
                return new SizeD(specifiedWidth, specifiedHeight);

            var result = GetImageAndTextSize();
            if (result != SizeD.Empty)
                return result + Padding.Size;

            return base.GetPreferredSize(availableSize);
        }

        /// <inheritdoc/>
        public override void DefaultPaint(Graphics dc, RectD rect)
        {
            DrawDefaultBackground(dc, rect);

            if (TextVisible)
            {
                var state = VisualState;

                var color = StateObjects?.Colors?.GetObjectOrNull(state)?.ForegroundColor;
                var origin = rect.Location;

                if (CenterHorz || CenterVert)
                {
                    var textSize = GetTextPreferredSize();
                    var textRect = new RectD(rect.Location, textSize);
                    var alignedRect = textRect.CenterIn(rect, CenterHorz, CenterVert);
                    origin = alignedRect.Location;
                }

                dc.DrawText(
                    Text ?? string.Empty,
                    origin,
                    Font ?? UI.Control.DefaultFont,
                    color ?? ForeColor,
                    Color.Empty);
            }
            else
            {
                DrawDefaultImage(dc, rect);
            }
        }

        /// <summary>
        /// Paints image in the default style.
        /// </summary>
        /// <param name="dc">Drawing context.</param>
        /// <param name="rect"></param>
        public virtual void DrawDefaultImage(Graphics dc, RectD rect)
        {
            var primitive = Primitive;
            var state = VisualState;

            var image = StateObjects?.Images?.GetObjectOrNull(state);
            image ??= Image;
            primitive.Image = image;
            primitive.ImageSet = StateObjects?.ImageSets?.GetObjectOrNormal(state);
            primitive.DestRect =
                (rect.Location + Padding.LeftTop, rect.Size - Padding.Size);
            primitive.Draw(this, dc);
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
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            Invalidate();
        }

        /// <summary>
        /// Gets size of the image and text.
        /// </summary>
        /// <returns></returns>
        protected virtual SizeD GetImageAndTextSize()
        {
            SizeD result = SizeD.Empty;

            var image = Image;
            var imageSet = ImageSet;

            if (TextVisible)
            {
                result = GetTextPreferredSize();
            }
            else
            {
                if (image is not null)
                    result = image.SizeDip(this);
                else
                if (imageSet is not null)
                    result = PixelToDip(imageSet.DefaultSize);
            }

            return result;
        }

        /// <summary>
        /// Gets size of the text.
        /// </summary>
        /// <returns></returns>
        protected virtual SizeD GetTextPreferredSize()
        {
            var text = Text;
            if (text == null)
                return new SizeD();

            using var dc = CreateDrawingContext();
            var result = dc.GetTextExtent(text, Font ?? UI.Control.DefaultFont, this);
            return result;
        }
    }
}