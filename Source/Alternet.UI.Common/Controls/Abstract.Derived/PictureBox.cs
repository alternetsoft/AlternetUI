using System;
using System.ComponentModel;
using Alternet.Drawing;
using Alternet.UI.Extensions;

using SkiaSharp;

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
    public partial class PictureBox : GenericControl, IValidatorReporter
    {
        /// <summary>
        /// Gets or sets default value of the <see cref="AbstractControl.ParentBackColor"/>
        /// for the <see cref="PictureBox"/> control.
        /// </summary>
        public static bool DefaultParentBackColor = true;

        /// <summary>
        /// Gets or sets default value of the <see cref="AbstractControl.ParentForeColor"/>
        /// for the <see cref="PictureBox"/> control.
        /// </summary>
        public static bool DefaultParentForeColor = true;

        private readonly ImageDrawable primitive = new();
        private bool textVisible = false;
        private bool isTransparent = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="PictureBox"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public PictureBox(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PictureBox"/> class.
        /// </summary>
        public PictureBox()
        {
            ParentBackColor = DefaultParentBackColor;
            ParentForeColor = DefaultParentForeColor;
            RefreshOptions = ControlRefreshOptions.RefreshOnBorder
                | ControlRefreshOptions.RefreshOnBackground
                | ControlRefreshOptions.RefreshOnImage;
        }

        /// <summary>
        /// Occurs when the <see cref="Image"/> property changes.
        /// </summary>
        public event EventHandler? ImageChanged;

        /// <summary>
        /// Gets or sets a value indicating whether the control is transparent. Default is true.
        /// When true, the control's background is not drawn, allowing the parent control's
        /// background to show through. When false, the control's background
        /// is drawn normally. In both states, the control's border is drawn if applicable.
        /// </summary>
        /// <remarks>Setting this property to a new value will trigger a redraw of the object.</remarks>
        public virtual bool IsTransparent
        {
            get
            {
                return isTransparent;
            }

            set
            {
                if (isTransparent == value)
                    return;
                isTransparent = value;
                Invalidate();
            }
        }

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
                StateObjects.ImageSets = null;
                StateObjects.Images ??= new();
                StateObjects.Images.Normal = value;
                RaiseImageChanged(EventArgs.Empty);
                if(ImageVisible)
                    PerformLayoutAndInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets the SVG image associated with this object.
        /// </summary>
        public virtual SvgImage? SvgImage
        {
            get => primitive.SvgImage;

            set
            {
                if (primitive.SvgImage == value)
                    return;
                primitive.SvgImage = value;
                RaiseImageChanged(EventArgs.Empty);
                if (ImageVisible)
                    PerformLayoutAndInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets the size of the SVG image in pixels.
        /// </summary>
        /// <remarks>Changing this property triggers a layout update and redraw of the associated image if
        /// it is visible.</remarks>
        public virtual int? SvgSize
        {
            get => primitive.SvgSize;

            set
            {
                if (primitive.SvgSize == value)
                    return;
                primitive.SvgSize = value;
                RaiseImageChanged(EventArgs.Empty);
                if (ImageVisible)
                    PerformLayoutAndInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets the SVG color.
        /// </summary>
        /// <remarks>Setting this property triggers a layout update and invalidates the control if the
        /// image is visible.</remarks>
        public virtual Color? SvgColor
        {
            get => primitive.SvgColor;

            set
            {
                if (primitive.SvgColor == value)
                    return;
                primitive.SvgColor = value;
                RaiseImageChanged(EventArgs.Empty);
                if (ImageVisible)
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
                StateObjects.ImageSets = null;
                StateObjects.Images ??= new();
                StateObjects.Images.Disabled = value;
                RaiseImageChanged(EventArgs.Empty);
                if (ImageVisible && !Enabled)
                    PerformLayoutAndInvalidate();
            }
        }

        /// <inheritdoc/>
        public override bool Visible
        {
            get => base.Visible;

            set
            {
                base.Visible = value;
            }
        }

        /// <inheritdoc cref="AbstractControl.Background"/>
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
        /// you can paint. If <see cref="Image"/> is null returns dummy canvas.
        /// </summary>
        [Browsable(false)]
        public virtual Graphics Canvas
        {
            get
            {
                return Image?.GetDrawingContext() ?? MeasureCanvas;
            }
        }

        /// <summary>
        /// Gets or sets whether image is centered horizontally and vertically.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsImageCentered
        {
            get
            {
                return CenterVert && CenterHorz;
            }

            set
            {
                if (IsImageCentered == value)
                    return;
                primitive.CenterHorz = value;
                primitive.CenterVert = value;
                Invalidate();
            }
        }

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
        public virtual ImageSet? ImageSet
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
                StateObjects.Images = null;
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
        public virtual ImageSet? DisabledImageSet
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
                StateObjects.Images = null;
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

        internal ImageDrawable Primitive => primitive;

        void IValidatorReporter.SetErrorStatus(object? sender, bool showError, string? errorText)
        {
            ToolTip = errorText ?? string.Empty;
            ImageVisible = showError;
        }

        /// <summary>
        /// Sets image from <see cref="MessageBoxSvg"/> using the specified
        /// <see cref="MessageBoxIcon"/>.
        /// </summary>
        /// <param name="icon">Icon index to set.</param>
        /// <param name="size">Svg image size.</param>
        public virtual bool SetIcon(MessageBoxIcon icon, int size)
        {
            bool hasImage = false;

            DoInsideLayout(() =>
            {
                if(StateObjects != null)
                    StateObjects.Images = null;
                var svg = MessageBoxSvg.GetImage(icon);
                var iconImage = svg?.AsNormal(size, IsDarkBackground);
                ImageSet = iconImage;
                hasImage = iconImage != null;
            });

            return hasImage;
        }

        /// <summary>
        /// Raises the <see cref="ImageChanged"/> event and calls
        /// <see cref="OnImageChanged(EventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
        public void RaiseImageChanged(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            UpdatePrimitiveImage(VisualControlState.Normal);
            OnImageChanged(e);
            ImageChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Helper method which allows to draw on <see cref="SKCanvas"/>.
        /// </summary>
        /// <param name="action">Draw action with <see cref="SKCanvas"/> and size parameters.</param>
        /// <param name="backgroundColor">Color to fill background. Optional.</param>
        public virtual void Draw(Action<SKCanvas, int, int> action, Color? backgroundColor = null)
        {
            RectI rect = (0, 0, PixelFromDip(Width), PixelFromDip(Height));

            SKBitmap bitmap = new(rect.Width, rect.Height);

            SKCanvas canvas = new(bitmap);

            if(backgroundColor is not null)
                canvas.Clear(backgroundColor);

            action(canvas, rect.Width, rect.Height);

            var image = (Image)bitmap;
            Image = image;
        }

        /// <inheritdoc/>
        public override SizeD GetPreferredSize(PreferredSizeContext context)
        {
            var specifiedWidth = SuggestedWidth;
            var specifiedHeight = SuggestedHeight;
            if (!Coord.IsNaN(specifiedWidth) && !Coord.IsNaN(specifiedHeight))
                return new SizeD(specifiedWidth, specifiedHeight);

            var result = GetImageAndTextSize();
            if (result != SizeD.Empty)
                return result + Padding.Size;

            return base.GetPreferredSize(context);
        }

        /// <inheritdoc/>
        public override void DefaultPaint(PaintEventArgs e)
        {
            var flags = IsTransparent ? DrawDefaultBackgroundFlags.DrawBorder
                : DrawDefaultBackgroundFlags.DrawBorderAndBackground;

            DrawDefaultBackground(e, flags);

            if (TextVisible)
            {
                var rect = e.ClientRectangle;
                var dc = e.Graphics;
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
                    Font ?? UI.AbstractControl.DefaultFont,
                    color ?? ForeColor,
                    Color.Empty);
            }
            else
            {
                DrawDefaultImage(e);
            }
        }

        /// <summary>
        /// Sets the SVG image, along with optional size and color, for the associated primitive.
        /// </summary>
        /// <remarks>This method updates the SVG image and its associated properties for the primitive. 
        /// If the image visibility is enabled, the layout is recalculated
        /// and the display is invalidated.</remarks>
        /// <param name="svg">The <see cref="SvgImage"/> to be applied.
        /// Can be <see langword="null"/> to clear the current SVG image.</param>
        /// <param name="size">The optional size of the SVG image.
        /// If <see langword="null"/>, the default size is used.</param>
        /// <param name="color">The optional color to apply to the SVG image.
        /// If <see langword="null"/>, the default color is used.</param>
        public virtual void SetSvgImage(SvgImage? svg, int? size = null, Color? color = null)
        {
            primitive.SvgImage = svg;
            primitive.SvgSize = size;
            primitive.SvgColor = color;
            RaiseImageChanged(EventArgs.Empty);
            if (ImageVisible)
                PerformLayoutAndInvalidate();
        }

        /// <summary>
        /// Paints image in the default style.
        /// </summary>
        /// <param name="e">Paint arguments.</param>
        public void DrawDefaultImage(PaintEventArgs e)
        {
            var rect = e.ClientRectangle;
            var dc = e.Graphics;
            DrawDefaultImage(dc, rect);
        }

        /// <summary>
        /// Gets size of the image.
        /// </summary>
        public SizeD GetImagePreferredSize()
        {
            return primitive.GetPreferredSize(this);
        }

        /// <summary>
        /// Paints image in the default style.
        /// </summary>
        public virtual void DrawDefaultImage(Graphics dc, RectD rect)
        {
            var primitive = Primitive;
            var state = VisualState;
            UpdatePrimitiveImage(state);
            primitive.Bounds =
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
            SizeD result;

            if (TextVisible)
            {
                result = GetTextPreferredSize();
            }
            else
            {
                result = GetImagePreferredSize();
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
            var result = dc.GetTextExtent(text, Font ?? UI.AbstractControl.DefaultFont);
            return result;
        }

        private void UpdatePrimitiveImage(VisualControlState state)
        {
            var image = StateObjects?.Images?.GetObjectOrNull(state);
            image ??= Image;
            primitive.Image = image;
            primitive.ImageSet = StateObjects?.ImageSets?.GetObjectOrNormal(state);
        }
    }
}