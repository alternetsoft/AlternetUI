using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a text label control.
    /// </summary>
    /// <remarks>
    /// <see cref="Label" /> controls are typically used to provide descriptive text
    /// for a control.
    /// For example, you can use a <see cref="Label" /> to add descriptive text for
    /// a <see cref="TextBox"/> control to inform the
    /// user about the type of data expected in the control.
    /// <see cref="Label" /> controls can also be used
    /// to add descriptive text to a <see cref="Window"/> to provide the user
    /// with helpful information.
    /// For example, you can add a <see cref="Label" /> to the top of a
    /// <see cref="Window"/> that provides instructions
    /// to the user on how to input data in the controls on the form.
    /// <see cref="Label" /> controls can be
    /// also used to display run time information on the status of an application.
    /// For example,
    /// you can add a <see cref="Label" /> control to a form to display the status
    /// of each file as a list of files is processed.
    /// </remarks>
    [DefaultProperty("Text")]
    [DefaultBindingProperty("Text")]
    [ControlCategory("Common")]
    public partial class Label : GenericControl
    {
        /// <summary>
        /// Gets or sets the mnemonic marker character. Default is '&amp;'.
        /// </summary>
        public static char DefaultMnemonicMarker = '&';

        /// <summary>
        /// Gets or sets whether to show mnemonic markers in the text.
        /// </summary>
        public static bool DefaultMnemonicMarkerEnabled = false;

        /// <summary>
        /// Gets or sets whether to show debug corners when control is painted.
        /// </summary>
        public static bool ShowDebugCorners = false;

        private bool imageVisible = true;
        private int? mnemonicCharIndex = null;
        private HVAlignment alignment;
        private string? textPrefix;
        private string? textSuffix;
        private string? textFormat;
        private Graphics.DrawLabelParams prm;
        private bool isVerticalText;
        private bool? mnemonicMarkerEnabled;
        private Coord? maxTextWidth;
        private bool wordWrap;
        private VerticalAlignment? imageVerticalAlignment;
        private HorizontalAlignment? imageHorizontalAlignment;

        /// <summary>
        /// Initializes a new instance of the <see cref="Label"/> class
        /// with the specified parent control.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public Label(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Label"/> class
        /// with the specified text.
        /// </summary>
        /// <param name="text">Value of the text property.</param>
        public Label(string? text)
            : this()
        {
            Text = text ?? string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Label"/> class
        /// with the specified text and parent control.
        /// </summary>
        /// <param name="text">Text displayed on this label.</param>
        /// <param name="parent">Parent of the control.</param>
        public Label(Control parent, string text)
            : this()
        {
            Text = text ?? string.Empty;
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Label"/> class with the specified text lines.
        /// This constructor allows you to create a label with multiple lines of text,
        /// which will be separated by new line characters. Also this constructor
        /// turns on word wrapping by default.
        /// </summary>
        /// <param name="text">A collection of strings representing the lines of text
        /// to be displayed in the label. Each element in the collection
        /// represents a separate line.</param>
        public Label(IEnumerable<string> text)
            : this()
        {
            WordWrap = true;
            Text = string.Join(Environment.NewLine, text);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Label"/> class with
        /// the specified parent control and text content.
        /// </summary>
        /// <param name="parent">The parent control that will contain this label.
        /// Cannot be <see langword="null"/>.</param>
        /// <param name="text">A collection of strings representing the text content of the label.
        /// Each string in the collection will be displayed as a separate line.</param>
        public Label(Control parent, IEnumerable<string> text)
            : this(text)
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Label"/> class.
        /// </summary>
        public Label()
        {
            HorizontalAlignment = HorizontalAlignment.Left;
            ParentBackColor = true;
            ParentForeColor = true;
            CanSelect = false;
            TabStop = false;
            RefreshOptions = ControlRefreshOptions.RefreshOnBorder
                | ControlRefreshOptions.RefreshOnColor
                | ControlRefreshOptions.RefreshOnBackground
                | ControlRefreshOptions.RefreshOnImage
                | ControlRefreshOptions.RefreshOnState;
        }

        /// <summary>
        /// Occurs before the text is drawn.
        /// </summary>
        public event EventHandler? BeforeDrawText;

        /// <summary>
        /// Occurs when the <see cref="Image"/> property changes.
        /// </summary>
        public event EventHandler? ImageChanged;

        /// <summary>
        /// Gets parameters for drawing label.
        /// </summary>
        [Browsable(false)]
        public Graphics.DrawLabelParams DrawLabelParams => prm;

        /// <summary>
        /// Gets or sets whether text is word wrapped in order to fit label in the parent's
        /// client area. Default is False.
        /// </summary>
        public virtual bool WordWrap
        {
            get => wordWrap;
            set
            {
                if (wordWrap == value)
                    return;
                wordWrap = value;
                PerformLayoutAndInvalidate();
            }
        }

        /// <inheritdoc/>
        public override bool ParentBackColor
        {
            get => base.ParentBackColor;
            set
            {
                base.ParentBackColor = value;
            }
        }

        /// <inheritdoc/>
        public override Color? BackgroundColor
        {
            get => base.BackgroundColor;
            set
            {
                base.BackgroundColor = value;
            }
        }

        /// <summary>
        /// Gets or sets maximal width of text in the control.
        /// </summary>
        /// <remarks>
        /// Wraps <see cref="AbstractControl.Text"/> so that each of its lines becomes at most width
        /// dips wide if possible (the lines are broken at words boundaries so it
        /// might not be the case if words are too long).
        /// </remarks>
        /// <remarks>
        /// If width is negative or null, no wrapping is done. Note that this width is not
        /// necessarily the total width of the control, since a padding for the
        /// border (depending on the controls border style) may be added.
        /// </remarks>
        public virtual Coord? MaxTextWidth
        {
            get
            {
                return maxTextWidth;
            }

            set
            {
                if (value <= 0)
                    value = null;
                if (maxTextWidth == value)
                    return;
                maxTextWidth = value;
                PerformLayoutAndInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets whether to process mnemonic markers in the text.
        /// If this property is not specified (default),
        /// <see cref="DefaultMnemonicMarkerEnabled"/> is used
        /// to determine whether mnemonic markers are processed.
        /// </summary>
        public virtual bool? MnemonicMarkerEnabled
        {
            get => mnemonicMarkerEnabled;

            set
            {
                if (mnemonicMarkerEnabled == value)
                    return;
                mnemonicMarkerEnabled = value;
                PerformLayoutAndInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets text prefix.
        /// </summary>
        /// <remarks>
        /// Value of this property is shown at the beginning of the text
        /// when control is painted.
        /// </remarks>
        public virtual string? TextPrefix
        {
            get
            {
                return textPrefix;
            }

            set
            {
                if (textPrefix == value)
                    return;
                textPrefix = value;
                PerformLayoutAndInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the text should be rendered vertically.
        /// </summary>
        /// <remarks>
        /// When this property is set, the layout is refreshed to reflect the vertical text orientation.
        /// </remarks>
        public virtual bool IsVerticalText
        {
            get => isVerticalText;
            set
            {
                if (isVerticalText == value)
                    return;
                isVerticalText = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets text format.
        /// </summary>
        /// <remarks>
        /// This property is used to format text with
        /// <see cref="string.Format(string, object)"/> before
        /// it is displayed. Sample of the format: Hello, {0}.
        /// </remarks>
        public virtual string? TextFormat
        {
            get
            {
                return textFormat;
            }

            set
            {
                if (textFormat == value)
                    return;
                textFormat = value;
                PerformLayoutAndInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets text suffix.
        /// </summary>
        /// <remarks>
        /// Value of this property is shown at the end of the text
        /// when control is painted.
        /// </remarks>
        public virtual string? TextSuffix
        {
            get
            {
                return textSuffix;
            }

            set
            {
                if (textSuffix == value)
                    return;
                textSuffix = value;
                PerformLayoutAndInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets alignment of the text.
        /// </summary>
        [Browsable(false)]
        public HVAlignment TextAlignment
        {
            get
            {
                return alignment;
            }

            set
            {
                if (alignment == value)
                    return;
                alignment = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets vertical alignment of the text.
        /// </summary>
        public VerticalAlignment TextAlignmentVertical
        {
            get
            {
                return alignment.Vertical;
            }

            set
            {
                if (alignment.Vertical == value)
                    return;
                alignment = new(alignment.Horizontal, value);
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets vertical alignment of the text.
        /// </summary>
        public HorizontalAlignment TextAlignmentHorizontal
        {
            get
            {
                return alignment.Horizontal;
            }

            set
            {
                if (alignment.Horizontal == value)
                    return;
                alignment = new(value, alignment.Vertical);
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

        /// <summary>
        /// Gets or sets underlined character index.
        /// </summary>
        /// <remarks>
        /// By default equals -1.
        /// </remarks>
        [DefaultValue(null)]
        public virtual int? MnemonicCharIndex
        {
            get
            {
                return mnemonicCharIndex;
            }

            set
            {
                if (mnemonicCharIndex == value)
                    return;
                mnemonicCharIndex = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the vertical alignment of the image.
        /// </summary>
        public virtual VerticalAlignment? ImageVerticalAlignment
        {
            get => imageVerticalAlignment;

            set
            {
                if (imageVerticalAlignment == value)
                    return;
                imageVerticalAlignment = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the horizontal alignment of the image.
        /// </summary>
        public virtual HorizontalAlignment? ImageHorizontalAlignment
        {
            get => imageHorizontalAlignment;

            set
            {
                if (imageHorizontalAlignment == value)
                    return;
                imageHorizontalAlignment = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the image that is displayed near the text.
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
                    return;
                StateObjects ??= new();
                StateObjects.Images ??= new();
                StateObjects.Images.Normal = value;
                RaiseImageChanged(EventArgs.Empty);
                PerformLayoutAndInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets the disabled image that is displayed near the text.
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
                PerformLayoutAndInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to draw image.
        /// </summary>
        public virtual bool ImageVisible
        {
            get
            {
                return imageVisible;
            }

            set
            {
                if (imageVisible == value)
                    return;
                imageVisible = value;
                PerformLayoutAndInvalidate();
            }
        }

        [Browsable(false)]
        internal new LayoutStyle? Layout
        {
            get => base.Layout;
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

        /// <summary>
        /// Changes visual style of the control to look like <see cref="LinkLabel"/>.
        /// </summary>
        public virtual void MakeAsLinkLabel()
        {
            IsUnderline = true;
            ParentForeColor = false;
            ForeColor = LinkLabel.DefaultNormalColor ?? LightDarkColors.Blue;
            Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Sets the images for the control.
        /// </summary>
        /// <param name="image">The normal image to display.</param>
        /// <param name="disabledImage">The image to display when disabled.</param>
        public virtual void SetImages(Image? image, Image? disabledImage)
        {
            PerformLayoutAndInvalidate(() =>
            {
                Image = image;
                DisabledImage = disabledImage;
            });
        }

        /// <summary>
        /// Draws text in the default style.
        /// </summary>
        /// <param name="dc">Drawing context.</param>
        /// <param name="foreColor">Foreground color of the text.</param>
        /// <param name="font">Text font.</param>
        /// <param name="backColor">Background color of the text.</param>
        /// <param name="rect">Rectangle to draw in.</param>
        public virtual RectD DrawDefaultText(
            Graphics dc,
            RectD rect,
            Color? foreColor = null,
            Color? backColor = null,
            Font? font = null)
        {
            var state = VisualState;
            RectD paddedRect = (
                rect.Location + Padding.LeftTop,
                rect.Size - Padding.Size);

            var labelImage = GetImage(state);
            var labelText = GetFormattedText();

            if (labelImage is null && labelText == string.Empty)
                return RectD.Empty;

            var labelFont = font ?? GetLabelFont(state);
            var labelForeColor = foreColor ?? GetLabelForeColor(state);
            var labelBackColor = backColor ?? GetLabelBackColor(state);

            labelText = GetWithoutMnemonicMarkers(labelText, out var mnemonicCharIndex);
            labelText = GetWrappedText(labelText);

            prm = new(
                labelText,
                labelFont,
                labelForeColor,
                labelBackColor,
                labelImage,
                paddedRect,
                alignment,
                mnemonicCharIndex);

            prm.IsVerticalText = isVerticalText;
            prm.Visible = foreColor != Color.Empty;

            if (WordWrap)
                prm.Flags |= DrawLabelFlags.TextHasNewLineChars;

            prm.ImageVerticalAlignment = imageVerticalAlignment;
            prm.ImageHorizontalAlignment = imageHorizontalAlignment;

            var result = DrawDefaultText(dc);
            return result;

            string GetWrappedText(string s)
            {
                if(!WordWrap)
                    return s;

                var mw = paddedRect.Width;

                if (maxTextWidth is not null)
                    mw = Math.Min(maxTextWidth.Value, mw);

                var result = DrawingUtils.WrapTextToMultipleLines(
                    s,
                    mw,
                    labelFont,
                    dc);
                return result;
            }
        }

        /// <summary>
        /// Sets the default combo box image for the control.
        /// </summary>
        public virtual void SetDefaultComboBoxImage(int? size = null)
        {
            SetSvgImage(
                ControlAndButton.DefaultBtnComboBoxSvg,
                KnownButton.TextBoxCombo,
                size);
        }

        /// <summary>
        /// Sets the SVG image for the control.
        /// </summary>
        /// <param name="svg">The SVG image to be set. If null, known button image will be used.</param>
        /// <param name="btn">The known button type. If null, svg image should be specified.</param>
        /// <param name="size">The optional size for the image.
        /// If not specified, the default size for the SVG image would be used.</param>
        public virtual void SetSvgImage(
            SvgImage? svg,
            KnownButton? btn,
            int? size = null)
        {
            var (normalImage, disabledImage)
                = ToolBarUtils.GetNormalAndDisabledSvgImages(svg, btn, this, size);

            SetImages(normalImage, disabledImage);
        }

        /// <summary>
        /// Draws text in the default style using the specified parameters.
        /// </summary>
        /// <param name="dc">Drawing context.</param>
        /// <returns></returns>
        public virtual RectD DrawDefaultText(Graphics dc)
        {
            BeforeDrawText?.Invoke(this, EventArgs.Empty);
            var result = dc.DrawLabel(ref prm);
            return result;
        }

        /// <summary>
        /// Gets image for the specified control state.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public virtual Image? GetImage(VisualControlState state = VisualControlState.Normal)
        {
            var image = StateObjects?.Images?.GetObjectOrNull(state);
            image ??= Image;
            var imageOverride = ImageVisible ? image : null;
            return imageOverride;
        }

        /// <inheritdoc/>
        public override void DefaultPaint(PaintEventArgs e)
        {
            e.ClipRectangle = ClientRectangle;
            DrawDefaultBackground(e);

            var state = VisualState;
            var border = GetBorderSettings(state);

            var rect = e.ClipRectangle;
            var dc = e.Graphics;

            if(border is not null)
                rect = rect.DeflatedWithPadding(border.Width);

            DrawDefaultText(dc, rect);
            DefaultPaintDebug(e);
        }

        /// <inheritdoc/>
        public override SizeD GetPreferredSize(SizeD availableSize)
        {
            if (availableSize.AnyIsEmptyOrNegative)
                return SizeD.Empty;

            var result = GetDefaultPreferredSize(
                        availableSize,
                        withPadding: true,
                        (size) =>
                        {
                            var measured = DrawDefaultText(
                                MeasureCanvas,
                                (PointD.Empty, size),
                                Color.Empty);
                            return measured.Size;
                        });

            result = result.Ceiling();

            return result;
        }

        /// <summary>
        /// Default method for calculating preferred size.
        /// </summary>
        public virtual SizeD GetDefaultPreferredSize(
            SizeD availableSize,
            bool withPadding,
            Func<SizeD, SizeD> func)
        {
            var suggested = SuggestedSize;

            var isNanSuggestedWidth = suggested.IsNanWidth;
            var isNanSuggestedHeight = suggested.IsNanHeight;

            var containerSize = suggested;

            if (!isNanSuggestedWidth && !isNanSuggestedHeight)
            {
                return containerSize;
            }

            if (isNanSuggestedWidth)
                containerSize.Width = availableSize.Width;

            if (isNanSuggestedHeight)
                containerSize.Height = availableSize.Height;

            var paddingSize = Padding.Size;

            containerSize -= paddingSize;

            var measured = func(containerSize);

            if (!isNanSuggestedWidth)
                measured.Width = suggested.Width;
            else
                measured.Width += paddingSize.Width;

            if (!isNanSuggestedHeight)
                measured.Height = suggested.Height;
            else
                measured.Height += paddingSize.Height;

            return measured;
        }

        /// <summary>
        /// Called when the value of the <see cref="Image"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
        protected virtual void OnImageChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Gets formatted text with <see cref="TextSuffix"/> and <see cref="TextPrefix"/>.
        /// </summary>
        /// <returns></returns>
        protected virtual string GetFormattedText()
        {
#pragma warning disable
            var image = GetImage();
#pragma warning restore
            var prefix = TextPrefix;
            var labelText = Text;

            var result = $"{prefix}{labelText}{TextSuffix}" ?? string.Empty;
            if (textFormat is not null)
                result = string.Format(textFormat, result);
            return result;
        }

        /// <inheritdoc/>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            Invalidate();
        }

        /// <inheritdoc/>
        protected override void OnAfterParentSizeChanged(object? sender, HandledEventArgs e)
        {
            base.OnAfterParentSizeChanged(sender, e);
        }

        /// <inheritdoc/>
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
        }

        /// <inheritdoc/>
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
        }

        /// <inheritdoc/>
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            PerformLayoutAndInvalidate();
        }

        /// <summary>
        /// Removes mnemonic markers from a string and returns the index of the
        /// mnemonic character, if present.
        /// </summary>
        /// <param name="s">The input string containing mnemonic markers.</param>
        /// <param name="mnemonicCharIndex">
        /// The output index of the mnemonic character in the returned string.
        /// Returns -1 if no mnemonic marker is found.
        /// </param>
        /// <returns>
        /// The input string with mnemonic markers removed.
        /// Double markers are converted to a single literal character.
        /// </returns>
        /// <remarks>
        /// If <see cref="MnemonicCharIndex"/> is not null, it is assigned to
        /// <paramref name="mnemonicCharIndex"/>.
        /// In this case text is not processed and is used as is.
        /// </remarks>
        protected virtual string GetWithoutMnemonicMarkers(string s, out int mnemonicCharIndex)
        {
            if (MnemonicCharIndex.HasValue)
            {
                mnemonicCharIndex = MnemonicCharIndex.Value;
                return s;
            }

            var checkMnemonic = MnemonicMarkerEnabled ?? DefaultMnemonicMarkerEnabled;
            if (checkMnemonic)
            {
                var result = StringUtils.GetWithoutMnemonicMarkers(
                            s,
                            out mnemonicCharIndex,
                            DefaultMnemonicMarker);
                return result;
            }
            else
            {
                mnemonicCharIndex = -1;
                return s;
            }
        }

        [Conditional("DEBUG")]
        private void DefaultPaintDebug(PaintEventArgs e)
        {
            if (ShowDebugCorners)
                BorderSettings.DrawDesignCorners(e.Graphics, e.ClipRectangle);
        }
    }
}
