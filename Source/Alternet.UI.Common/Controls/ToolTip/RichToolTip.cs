using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /*
      Default title could be bold or DefaultFont.MakeLarger().
    */

    /// <summary>
    /// Allows to show a tool tip with more customizations than a standard tooltip.
    /// Additionally to the tooltip message <see cref="RichToolTip"/> allows to
    /// specify title, image and other options.
    /// </summary>
    public class RichToolTip : UserControl, IRichToolTip, IToolTipProvider
    {
        /// <summary>
        /// Gets or sets default tooltip border color.
        /// This is used when <see cref="DefaultToolTipBorder"/>
        /// is created.
        /// </summary>
        public static Color? DefaultToolTipBorderColor;

        /// <summary>
        /// Gets or sets default image margin in device-independent units.
        /// </summary>
        public static Thickness DefaultImageMargin = 5;

        /// <summary>
        /// Gets or sets default text margin in device-independent units.
        /// </summary>
        public static Thickness DefaultMessageMargin = 5;

        /// <summary>
        /// Gets or sets default title margin in device-independent units.
        /// </summary>
        public static Thickness DefaultTitleMargin = 5;

        /// <summary>
        /// Gets or sets default distance between image and title in device-independent units.
        /// </summary>
        public static int ImageToTextDistance = 5;

        /// <summary>
        /// Gets or sets default value for the tooltip minimal image size (in device-independent units).
        /// This is used for svg image size when standard <see cref="MessageBoxIcon"/>
        /// images are shown.
        /// </summary>
        public static Coord DefaultMinImageSize = 24;

        private static BorderSettings? defaultToolTipBorder;

        private readonly TemplateControls.RichToolTip<GenericLabel> template = new();
        private readonly PictureBox picture = new();

        private BorderSettings? toolTipBorder;
        private Color? toolTipBackgroundColor;
        private Color? toolTipForegroundColor;
        private Color? toolTipTitleForegroundColor;
        private Font? toolTipTitleFont;
        private int timeoutInMilliseconds;
        private int showDelayInMilliseconds;
        private MessageBoxIcon? toolTipIcon;
        private Brush? toolTipBackgroundBrush;
        private ImageSet? toolTipImage;

        /// <summary>
        /// Initializes a new instance of the <see cref="RichToolTip"/> class.
        /// </summary>
        public RichToolTip()
        {
            template.Parent = this;
            picture.Visible = false;
            picture.ImageStretch = false;
            picture.Alignment = HVAlignment.TopLeft;
            picture.Parent = this;
        }

        /// <summary>
        /// Gets or sets default tooltip border.
        /// </summary>
        public static BorderSettings DefaultToolTipBorder
        {
            get
            {
                if (defaultToolTipBorder is null)
                {
                    defaultToolTipBorder = new();
                    defaultToolTipBorder.Color = DefaultToolTipBorderColor;
                }

                return defaultToolTipBorder;
            }

            set
            {
                defaultToolTipBorder = value;
            }
        }

        /// <summary>
        /// Gets whether custom tooltip border is specified.
        /// </summary>
        public static bool IsCustomToolTipBorderSpecified
        {
            get
            {
                var useDefault =
                    DefaultToolTipBorderColor is null
                    && defaultToolTipBorder is null;

                return !useDefault;
            }
        }

        /// <summary>
        /// Gets real default tooltip border. This property returns <see cref="BorderSettings.Default"/>
        /// if <see cref="DefaultToolTipBorderColor"/> and <see cref="DefaultToolTipBorder"/>
        /// are not specified.
        /// </summary>
        public static BorderSettings RealDefaultToolTipBorder
        {
            get
            {
                if (IsCustomToolTipBorderSpecified)
                    return DefaultToolTipBorder;
                return BorderSettings.Default;
            }
        }

        /// <summary>
        /// Gets or sets default background color of the tooltip.
        /// </summary>
        public static Color DefaultToolTipBackgroundColor { get; set; }
            = Color.LightDark(light: (249, 249, 249), dark: (44, 44, 44));

        /// <summary>
        /// Gets or sets default foreground color of the tooltip.
        /// </summary>
        public static Color DefaultToolTipForegroundColor { get; set; }
            = Color.LightDark(light: Color.Black, dark: Color.White);

        /// <summary>
        /// Gets or sets default foreground color of the tooltip.
        /// </summary>
        public static Color DefaultToolTipTitleForegroundColor { get; set; }
            = Color.LightDark(light: (0, 51, 153), dark: (156, 220, 254));

        /// <summary>
        /// Gets or sets whether to draw point at center under the debug environment.
        /// </summary>
        [Browsable(false)]
        public bool ShowDebugRectangleAtCenter { get; set; }

        /// <inheritdoc/>
        [Browsable(false)]
        public virtual BorderSettings? ToolTipBorder
        {
            get => toolTipBorder;
            set
            {
                if (toolTipBorder == value)
                    return;
                toolTipBorder = value;
                HideToolTip();
            }
        }

        /// <summary>
        /// Gets <see cref="PictureBox"/> used to store tooltip picture.
        /// Image is filled after tooltip show. You can use this property to perform
        /// alignment of the tooltip inside the container.
        /// </summary>
        [Browsable(false)]
        public PictureBox ToolTipPicture => picture;

        /// <summary>
        /// Gets template control used to layout tooltip elements.
        /// </summary>
        [Browsable(false)]
        public AbstractControl ToolTipTemplate => template;

        /// <inheritdoc/>
        public virtual Color? ToolTipBackgroundColor
        {
            get => toolTipBackgroundColor;
            set
            {
                if (toolTipBackgroundColor == value)
                    return;
                toolTipBackgroundColor = value;
                HideToolTip();
            }
        }

        /// <inheritdoc/>
        public virtual Color? ToolTipForegroundColor
        {
            get => toolTipForegroundColor;
            set
            {
                if (toolTipForegroundColor == value)
                    return;
                toolTipForegroundColor = value;
                HideToolTip();
            }
        }

        /// <inheritdoc/>
        public virtual Color? ToolTipTitleForegroundColor
        {
            get => toolTipTitleForegroundColor;
            set
            {
                if (toolTipTitleForegroundColor == value)
                    return;
                toolTipTitleForegroundColor = value;
                HideToolTip();
            }
        }

        /// <inheritdoc/>
        public virtual Font? ToolTipTitleFont
        {
            get => toolTipTitleFont;
            set
            {
                if (toolTipTitleFont == value)
                    return;
                toolTipTitleFont = value;
                HideToolTip();
            }
        }

        /// <summary>
        /// Gets or sets whether tooltip is visible.
        /// </summary>
        public virtual bool ToolTipVisible
        {
            get => picture.Visible;

            set
            {
                if (ToolTipVisible == value)
                    return;
                if (value)
                    ShowToolTip();
                else
                    HideToolTip();
            }
        }

        /// <inheritdoc/>
        public virtual int TimeoutInMilliseconds
        {
            get => timeoutInMilliseconds;
            set
            {
                if (timeoutInMilliseconds == value)
                    return;
                timeoutInMilliseconds = value;
                HideToolTip();
            }
        }

        /// <inheritdoc/>
        public virtual int ShowDelayInMilliseconds
        {
            get => showDelayInMilliseconds;
            set
            {
                if (showDelayInMilliseconds == value)
                    return;
                showDelayInMilliseconds = value;
                HideToolTip();
            }
        }

        /// <summary>
        /// Gets or sets tooltip location inside in client coordinated of this control.
        /// This value is measured in device-independent units.
        /// </summary>
        public virtual PointD ToolTipLocation
        {
            get
            {
                return (Padding.Left, Padding.Top);
            }

            set
            {
                Padding = Padding.WithLeft(value.X).WithTop(value.Y);
            }
        }

        /// <inheritdoc/>
        public virtual MessageBoxIcon? ToolTipIcon
        {
            get => toolTipIcon;
            set
            {
                if (toolTipIcon == value)
                    return;
                toolTipIcon = value;
                HideToolTip();
            }
        }

        /// <inheritdoc/>
        public virtual Brush? ToolTipBackgroundBrush
        {
            get => toolTipBackgroundBrush;
            set
            {
                if (toolTipBackgroundBrush == value)
                    return;
                toolTipBackgroundBrush = value;
                HideToolTip();
            }
        }

        /// <inheritdoc/>
        public virtual ImageSet? ToolTipImage
        {
            get => toolTipImage;
            set
            {
                if (toolTipImage == value)
                    return;
                toolTipImage = value;
                HideToolTip();
            }
        }

        /// <summary>
        /// Resets all tooltip color properties to the default values.
        /// </summary>
        public virtual IRichToolTip ResetToolTipColors()
        {
            ToolTipTitleForegroundColor = DefaultToolTipTitleForegroundColor;
            ToolTipBackgroundBrush = null;
            ToolTipBackgroundColor = DefaultToolTipBackgroundColor;
            ToolTipForegroundColor = DefaultToolTipForegroundColor;

            SetToolTipBackgroundColor(ToolTipBackgroundColor);
            SetToolTipForegroundColor(ToolTipForegroundColor);
            SetTitleForegroundColor(ToolTipTitleForegroundColor);
            return this;
        }

        /// <summary>
        /// Sets simple tooltip contents.
        /// </summary>
        /// <param name="message">Tooltip message.</param>
        /// <param name="systemColors">If <c>true</c>, sets tooltip colors
        /// to <see cref="FontAndColor.SystemColorInfo"/>.</param>
        public virtual IRichToolTip SetToolTip(object message, bool systemColors = true)
        {
            ResetToolTipColors();
            SetIcon(MessageBoxIcon.None);
            Text = message?.ToString() ?? string.Empty;
            Title = string.Empty;

            if (systemColors)
            {
                var colors = FontAndColor.SystemColorInfo;
                SetToolTipBackgroundColor(colors.BackgroundColor);
                SetToolTipForegroundColor(colors.ForegroundColor);
            }

            return this;
        }

        /// <summary>
        /// Hides tooltip.
        /// </summary>
        public new virtual IRichToolTip HideToolTip()
        {
            try
            {
                picture.Hide();
                return this;
            }
            catch
            {
                return this;
            }
        }

        /// <summary>
        /// Sets tooltip contents.
        /// </summary>
        /// <param name="title">Tooltip title.</param>
        /// <param name="message">Tooltip message.</param>
        /// <param name="icon">Tooltip standard icon.</param>
        /// <param name="timeoutMilliseconds">
        /// Timeout in milliseconds after which tooltip will be hidden. Optional. If not specified,
        /// default timeout value is used. If 0 is specified, tooltip will not be hidden after timeout.
        /// </param>
        public virtual IRichToolTip SetToolTip(
            object? title,
            object? message,
            MessageBoxIcon? icon = null,
            uint? timeoutMilliseconds = null)
        {
            ResetToolTipColors();
            TitleAsObject = title ?? string.Empty;
            Text = message?.ToString() ?? string.Empty;

            if (icon is not null)
            {
                if (string.IsNullOrEmpty(Title))
                    icon = MessageBoxIcon.None;
                SetIcon(icon.Value);
            }

            if(timeoutMilliseconds is not null)
            {
                SetTimeout(timeoutMilliseconds.Value);
            }

            return this;
        }

        /// <summary>
        /// Sets tooltip background brush.
        /// </summary>
        /// <param name="brush">Background brush.</param>
        public virtual IRichToolTip SetToolTipBackgroundBrush(Brush? brush)
        {
            ToolTipBackgroundBrush = brush;
            SetToolTipBackgroundColor(brush?.AsColor);
            return this;
        }

        /// <summary>
        /// Sets tooltip background color.
        /// </summary>
        /// <param name="color">Background color.</param>
        public virtual IRichToolTip SetToolTipBackgroundColor(Color? color)
        {
            ToolTipBackgroundColor = color;
            return this;
        }

        /// <summary>
        /// Sets foreground color of the tooltip message.
        /// </summary>
        /// <param name="color">Foreground color of the message.</param>
        public virtual IRichToolTip SetToolTipForegroundColor(Color? color)
        {
            ToolTipForegroundColor = color;
            return this;
        }

        /// <summary>
        /// Sets foreground color of the tooltip title.
        /// </summary>
        /// <param name="color">Foreground color of the title.</param>
        public virtual IRichToolTip SetTitleForegroundColor(Color? color)
        {
            ToolTipTitleForegroundColor = color;
            return this;
        }

        /// <summary>
        /// Sets timeout after which the tooltip should disappear, in milliseconds.
        /// If 0 is specified, tooltip will not be hidden automatically.
        /// Optionally specify a show delay.
        /// </summary>
        /// <remarks>
        /// By default the tooltip is hidden after system-dependent interval of time
        /// elapses but this method can be used to change this or also disable
        /// hiding the tooltip automatically entirely by passing 0 in this parameter
        /// (but doing this can result in native version not being used).
        /// </remarks>
        /// <param name="milliseconds">Timeout value.</param>
        /// <param name="millisecondsShowdelay">Show delay value.</param>
        public virtual IRichToolTip SetTimeout(uint milliseconds, uint millisecondsShowdelay = 0)
        {
            TimeoutInMilliseconds = (int)milliseconds;
            ShowDelayInMilliseconds = (int)millisecondsShowdelay;
            return this;
        }

        /// <summary>
        /// Sets the small icon to show in the tooltip.
        /// </summary>
        /// <param name="bitmap">Icon of the tooltip.</param>
        public virtual IRichToolTip SetIcon(ImageSet? bitmap)
        {
            ToolTipImage = bitmap;
            ToolTipIcon = null;
            return this;
        }

        /// <summary>
        /// Sets the title text font.
        /// </summary>
        /// <remarks>
        /// By default it's emphasized using the font style
        /// or colour appropriate for the current platform.
        /// </remarks>
        /// <param name="font">Font of the title.</param>
        public virtual IRichToolTip SetTitleFont(Font? font)
        {
            ToolTipTitleFont = font;
            return this;
        }

        /// <summary>
        /// Sets tooltip with contents filled from the template data.
        /// </summary>
        /// <param name="template">Template with tooltip data.</param>
        /// <param name="backColor">Background color. Optional. If not specified, background color
        /// of the template control is used.</param>
        /// <returns></returns>
        public virtual IRichToolTip SetToolTipFromTemplate(
            TemplateControl template,
            Color? backColor = null)
        {
            ResetToolTipColors();
            backColor ??= template.BackgroundColor;

            var image = TemplateUtils.GetTemplateAsImage(template, backColor);
            OnlyImage((ImageSet)image);
            if (backColor is not null)
                SetToolTipBackgroundColor(backColor);
            return this;
        }

        /// <summary>
        /// Sets text to an empty string.
        /// </summary>
        /// <returns></returns>
        public virtual IRichToolTip ResetText()
        {
            Text = string.Empty;
            return this;
        }

        /// <summary>
        /// Sets title to an empty string.
        /// </summary>
        /// <returns></returns>
        public virtual IRichToolTip ResetTitle()
        {
            Title = string.Empty;
            return this;
        }

        /// <summary>
        /// Sets tooltip to show only the specified image.
        /// </summary>
        /// <param name="image">Image to show.</param>
        /// <returns></returns>
        public virtual IRichToolTip OnlyImage(ImageSet? image)
        {
            return ResetTitle().ResetText().SetIcon(image);
        }

        /// <inheritdoc/>
        public override SizeD GetPreferredSize(SizeD availableSize)
        {
            return base.GetPreferredSize(availableSize);
        }

        /// <summary>
        /// Shows the tooltip at the specified location inside the.
        /// Location coordinates are in device-independent units.
        /// </summary>
        public virtual IRichToolTip ShowToolTip(PointD? location = null)
        {
            template.DoInsideLayout(() =>
            {
                if(location is not null)
                    ToolTipLocation = location.Value;

                template.Normal = RealDefaultToolTipBorder;
                template.HasBorder = true;
                template.BackgroundColor
                = ToolTipBackgroundColor ?? RichToolTip.DefaultToolTipBackgroundColor;
                template.RaiseBackgroundColorChanged();
                template.ForegroundColor
                = ToolTipForegroundColor ?? RichToolTip.DefaultToolTipForegroundColor;
                template.TitleLabel.Text = Title;
                template.TitleLabel.ParentForeColor = false;
                template.TitleLabel.ParentFont = false;

                template.TitleLabel.Font
                    = ToolTipTitleFont ?? RealFont.Scaled(1.5);
                template.TitleLabel.ForegroundColor
                    = ToolTipTitleForegroundColor ?? RichToolTip.DefaultToolTipTitleForegroundColor;
                template.MessageLabel.Text = Text;

                template.TitleLabel.Visible = !string.IsNullOrEmpty(Title);
                template.MessageLabel.Visible = !string.IsNullOrEmpty(Text);

                var sizeInPixels
                = GraphicsFactory.PixelFromDip(RichToolTip.DefaultMinImageSize, ScaleFactor);

                var titleVisible = template.TitleLabel.Visible;
                var showImage = titleVisible;

                var imageMargin = DefaultImageMargin;

                if (titleVisible)
                {
                    imageMargin = imageMargin.WithRight(imageMargin.Right + ImageToTextDistance);
                }

                if (ToolTipImage != null)
                {
                    template.PictureBox.Visible = true;
                    template.PictureBox.ImageSet = ToolTipImage;
                }
                else
                {
                    var hasIcon
                        = template.PictureBox.SetIcon(ToolTipIcon ?? MessageBoxIcon.None, sizeInPixels);
                    template.PictureBox.Visible = hasIcon && showImage;
                }

                imageMargin.Reset(!template.PictureBox.Visible);

                var titleMargin = DefaultTitleMargin;
                titleMargin.Reset(!template.TitleLabel.Visible);

                var messageMargin = DefaultMessageMargin;
                messageMargin.Reset(!template.MessageLabel.Visible);

                template.PictureBox.Margin = imageMargin;
                template.TitleLabel.Margin = titleMargin;
                template.MessageLabel.Margin = messageMargin;
            });

            var image = TemplateUtils.GetTemplateAsImage(template, template.BackgroundColor);
            picture.BackgroundColor = template.BackgroundColor;
            picture.Background = template.BackgroundColor?.AsBrush;
            picture.Image = image;
            picture.Show();
            return this;
        }

        /// <inheritdoc/>
        public override void DefaultPaint(Graphics dc, RectD rect)
        {
            DrawDefaultBackground(dc, rect);

            if(DebugUtils.IsDebugDefined && ShowDebugRectangleAtCenter)
            {
                dc.FillRectangleAtCenter(
                    LightDarkColors.Red.AsBrush,
                    ClientRectangle,
                    3);
            }
        }

        /// <summary>
        /// Sets the standard icon to show in the tooltip.
        /// </summary>
        /// <param name="icon">One of the standard information/warning/error icons
        /// (the question icon doesn't make sense for a tooltip)</param>
        public virtual IRichToolTip SetIcon(MessageBoxIcon? icon)
        {
            ToolTipIcon = icon;
            ToolTipImage = null;
            return this;
        }

        /// <summary>
        /// Shows simple tooltip on the screen.
        /// </summary>
        /// <param name="message">Tooltip message.</param>
        /// <param name="systemColors">If <c>true</c>, sets tooltip colors
        /// to <see cref="FontAndColor.SystemColorInfo"/>.</param>
        /// <param name="location">Location where tooltip will be shown.</param>
        public IRichToolTip ShowToolTip(
            object message,
            bool systemColors = true,
            PointD? location = null)
        {
            return SetToolTip(message, systemColors).ShowToolTip(location);
        }

        /// <summary>
        /// Shows tooltip on the screen.
        /// </summary>
        /// <param name="title">Tooltip title.</param>
        /// <param name="message">Tooltip message.</param>
        /// <param name="icon">Tooltip standard icon.</param>
        /// <param name="timeoutMilliseconds">
        /// Timeout in milliseconds after which tooltip will be hidden. Optional. If not specified,
        /// default timeout value is used. If 0 is specified, tooltip will not be hidden after timeout.
        /// </param>
        /// <param name="location">Location where tooltip will be shown.</param>
        public virtual IRichToolTip ShowToolTip(
            object? title,
            object? message,
            MessageBoxIcon? icon = null,
            uint? timeoutMilliseconds = null,
            PointD? location = null)
        {
            return SetToolTip(title, message, icon, timeoutMilliseconds).ShowToolTip(location);
        }

        /// <summary>
        /// Shows tooltip with contents filled from the template data.
        /// </summary>
        /// <param name="template">Template with tooltip data.</param>
        /// <param name="backColor">Background color. Optional. If not specified, background color
        /// of the template control is used.</param>
        /// <returns></returns>
        /// <param name="location">Location where tooltip will be shown.</param>
        public virtual IRichToolTip ShowToolTipFromTemplate(
            TemplateControl template,
            Color? backColor = null,
            PointD? location = null)
        {
            return SetToolTipFromTemplate(template, backColor).ShowToolTip(location);
        }

        IRichToolTip? IToolTipProvider.Get(object? sender)
        {
            return this;
        }

        /// <inheritdoc/>
        protected override void OnTitleChanged(EventArgs e)
        {
            base.OnTitleChanged(e);
            HideToolTip();
        }

        /// <inheritdoc/>
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            HideToolTip();
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            HideToolTip();
            base.DisposeManaged();
        }
    }
}