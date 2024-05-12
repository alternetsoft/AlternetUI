using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Implements generic label control.
    /// </summary>
    public partial class GenericLabel : GraphicControl
    {
        private Color? textBackColor;
        private bool imageVisible = true;
        private int? mnemonicCharIndex = null;
        private GenericAlignment alignment = GenericAlignment.TopLeft;
        private string? textPrefix;
        private string? textSuffix;
        private string? textFormat;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericLabel"/> class.
        /// </summary>
        /// <param name="text">Value of the <see cref="Text"/> property.</param>
        public GenericLabel(string? text)
            : this()
        {
            Text = text ?? string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericLabel"/> class.
        /// </summary>
        public GenericLabel()
        {
            RefreshOptions = ControlRefreshOptions.RefreshOnBorder
                | ControlRefreshOptions.RefreshOnColor
                | ControlRefreshOptions.RefreshOnBackground
                | ControlRefreshOptions.RefreshOnImage;
        }

        /// <summary>
        /// Occurs when the <see cref="Image"/> property changes.
        /// </summary>
        public event EventHandler? ImageChanged;

        /// <summary>
        /// Gets or sets text prefix.
        /// </summary>
        /// <remarks>
        /// Value of this property is shown at the beginning of <see cref="Text"/>
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
        /// Value of this property is shown at the end of <see cref="Text"/>
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
        /// Gets or sets background color of the text.
        /// </summary>
        /// <remarks>
        /// By default is <c>null</c>. It means now additional background is painted
        /// under the text.
        /// </remarks>
        [DefaultValue(null)]
        public virtual Color? TextBackColor
        {
            get
            {
                return textBackColor;
            }

            set
            {
                if (textBackColor == value)
                    return;
                textBackColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets alignment of the text.
        /// </summary>
        public GenericAlignment TextAlignment
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
        /// Gets or sets the image that is displayed near the text.
        /// </summary>
        [DefaultValue(null)]
        public virtual Image? Image
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
                PerformLayoutAndInvalidate();
            }
        }

        /// <inheritdoc/>
        [DefaultValue("")]
        [Localizability(LocalizationCategory.Text)]
        public override string Text
        {
            get
            {
                return base.Text;
            }

            set
            {
                if (Text == value)
                    return;
                base.Text = value;
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
        /// Draws text in the default style.
        /// </summary>
        /// <param name="dc">Drawing context.</param>
        /// <param name="foreColor">Foreground color of the text.</param>
        /// <param name="font">Text font.</param>
        /// <param name="backColor">Background color of the text.</param>
        /// <param name="rect">Rectangle to draw in.</param>
        public virtual void DrawDefaultText(
            Graphics dc,
            RectD rect,
            Color? foreColor = null,
            Color? backColor = null,
            Font? font = null)
        {
            var state = CurrentState;
            var paddedRect = (
                rect.Location + Padding.LeftTop,
                rect.Size - Padding.Size);

            var labelImage = GetImage(state);
            var labelText = GetFormattedText();

            if (labelImage is null && labelText == string.Empty)
                return;

            dc.DrawLabel(
                labelText,
                font ?? GetLabelFont(state),
                foreColor ?? GetLabelForeColor(state),
                backColor ?? GetLabelBackColor(state),
                labelImage,
                paddedRect,
                TextAlignment,
                GetMnemonicCharIndex());
        }

        /// <summary>
        /// Gets image for the specified control state.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public virtual Image? GetImage(GenericControlState state = GenericControlState.Normal)
        {
            var image = StateObjects?.Images?.GetObjectOrNull(state);
            image ??= Image;
            var imageOverride = ImageVisible ? image : null;
            return imageOverride;
        }

        /// <inheritdoc/>
        public override void DefaultPaint(Graphics dc, RectD rect)
        {
            DrawDefaultBackground(dc, rect);
            DrawDefaultText(dc, rect);
        }

        /// <inheritdoc/>
        public override SizeD GetPreferredSize(SizeD availableSize)
        {
            var specifiedWidth = SuggestedWidth;
            var specifiedHeight = SuggestedHeight;

            SizeD result = 0;

            var text = GetFormattedText();
            if (!string.IsNullOrEmpty(text))
            {
                result = MeasureCanvas.GetTextExtent(
                    text,
                    GetLabelFont(GenericControlState.Normal),
                    this);
            }

            var image = GetImage();

            if(image is not null)
            {
                result.Width += PixelToDip(image.Width);
            }

            if (!double.IsNaN(specifiedWidth))
                result.Width = Math.Max(result.Width, specifiedWidth);

            if (!double.IsNaN(specifiedHeight))
                result.Height = Math.Max(result.Height, specifiedHeight);

            return result + Padding.Size;
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
        /// Gets <see cref="Font"/> which is used to draw labels text.
        /// </summary>
        /// <returns></returns>
        protected virtual Font GetLabelFont(GenericControlState state)
        {
            var font = StateObjects?.Colors?.GetObjectOrNull(state)?.Font;

            var result = font ?? Font ?? UI.Control.DefaultFont;
            if (IsBold)
                result = result.AsBold;
            return result;
        }

        /// <summary>
        /// Gets <see cref="Color"/> which is used to draw background of the label text.
        /// </summary>
        /// <returns>By default returns <see cref="Color.Empty"/> which means do not
        /// draw background under the label text. In this case control's background
        /// is used.</returns>
        protected virtual Color GetLabelBackColor(GenericControlState state)
        {
            var color = StateObjects?.Colors?.GetObjectOrNull(state)?.BackgroundColor;

            return color ?? TextBackColor ?? Color.Empty;
        }

        /// <summary>
        /// Gets <see cref="Color"/> which is used to draw label text.
        /// </summary>
        /// <returns></returns>
        protected virtual Color GetLabelForeColor(GenericControlState state)
        {
            var color = StateObjects?.Colors?.GetObjectOrNull(state)?.ForegroundColor;

            if(color is null)
            {
                if (Enabled)
                    color = ForeColor;
                else
                    color = SystemColors.GrayText;
            }

            return color;
        }

        /// <summary>
        /// Gets formatted text with <see cref="TextSuffix"/> and <see cref="TextPrefix"/>.
        /// </summary>
        /// <returns></returns>
        protected virtual string GetFormattedText()
        {
            var image = GetImage();
            var prefix = TextPrefix;
            var labelText = Text;

            if (image is not null && prefix is null && !string.IsNullOrEmpty(labelText))
                prefix = StringUtils.OneSpace;

            var result = $"{prefix}{labelText}{TextSuffix}" ?? string.Empty;
            if (textFormat is not null)
                result = string.Format(textFormat, result);
            return result;
        }

        /// <summary>
        /// Gets mnemonic char index from <see cref="MnemonicCharIndex"/> or -1.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// You can override this method in order to provide additional functionality.
        /// </remarks>
        protected virtual int GetMnemonicCharIndex()
        {
            var result = MnemonicCharIndex ?? -1;
            return result;
        }
    }
}
