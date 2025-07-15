using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Allows to show a tool tip with more customizations than a standard tooltip.
    /// Additionally to the tooltip message <see cref="RichToolTip"/> allows to
    /// specify title, image and other options.
    /// </summary>
    public partial class RichToolTip : ScrollViewer, IRichToolTip, IToolTipProvider
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
        /// Gets or sets default max width of the tooltip.
        /// </summary>
        public static Coord? DefaultMaxWidth = 500;

        /// <summary>
        /// Gets or sets default value for the tooltip minimal image size
        /// (in device-independent units).
        /// This is used for svg image size when standard <see cref="MessageBoxIcon"/>
        /// images are shown.
        /// </summary>
        public static Coord DefaultMinImageSize = 24;

        private static BorderSettings? defaultToolTipBorder;

        private readonly TemplateControls.RichToolTipTemplate template = new();
        private readonly ImageDrawable drawable = new();

        private int? timeoutInMilliseconds;
        private int showDelayInMilliseconds;
        private Timer? showTimer;
        private Timer? hideTimer;
        private HVAlignment toolTipAlignment;

#pragma warning disable
        private RichToolTipParams data = new();
#pragma warning restore

        /// <summary>
        /// Initializes a new instance of the <see cref="RichToolTip"/> class.
        /// </summary>
        public RichToolTip()
        {
            drawable.Visible = false;
            ParentBackColor = true;
            ParentForeColor = true;
        }

        /// <summary>
        /// Occurs when <see cref="ToolTipVisible"/> property is changed.
        /// </summary>
        [Category("Behavior")]
        public event EventHandler? ToolTipVisibleChanged;

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
        /// Gets real default tooltip border. This property returns
        /// <see cref="BorderSettings.Default"/>
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
        public static LightDarkColor DefaultToolTipBackgroundColor { get; set; }
            = Color.LightDark(light: (249, 249, 249), dark: (44, 44, 44));

        /// <summary>
        /// Gets or sets default foreground color of the tooltip.
        /// </summary>
        public static LightDarkColor DefaultToolTipForegroundColor { get; set; }
            = Color.LightDark(light: Color.Black, dark: Color.White);

        /// <summary>
        /// Gets or sets default foreground color of the tooltip.
        /// </summary>
        public static LightDarkColor DefaultToolTipTitleForegroundColor { get; set; }
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
            get => data.Border;
            set
            {
                if (data.Border == value)
                    return;
                data.Border = value;
                HideToolTip();
            }
        }

        /// <summary>
        /// Gets template control used to layout tooltip elements.
        /// </summary>
        [Browsable(false)]
        public AbstractControl ToolTipTemplate => template;

        /// <inheritdoc/>
        public virtual Color? ToolTipBackgroundColor
        {
            get => data.BackgroundColor;
            set
            {
                if (data.BackgroundColor == value)
                    return;
                data.BackgroundColor = value;
                HideToolTip();
            }
        }

        /// <inheritdoc/>
        public virtual Color? ToolTipForegroundColor
        {
            get => data.ForegroundColor;
            set
            {
                if (data.ForegroundColor == value)
                    return;
                data.ForegroundColor = value;
                HideToolTip();
            }
        }

        /// <summary>
        /// Gets or sets vertical and horizontal alignment of the tooltip inside the container.
        /// </summary>
        [Browsable(false)]
        public virtual HVAlignment ToolTipAlignment
        {
            get
            {
                return toolTipAlignment;
            }

            set
            {
                value = value.WithoutStretchOrFill();
                if (toolTipAlignment == value)
                    return;
                toolTipAlignment = value;
                if (ToolTipVisible)
                    Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets horizontal alignment of the tooltip inside the container.
        /// </summary>
        public HorizontalAlignment ToolTipHorizontalAlignment
        {
            get => ToolTipAlignment.Horizontal;

            set => ToolTipAlignment = ToolTipAlignment.WithHorizontal(value);
        }

        /// <summary>
        /// Gets or sets vertical alignment of the tooltip inside the container.
        /// </summary>
        public VerticalAlignment ToolTipVerticalAlignment
        {
            get => ToolTipAlignment.Vertical;

            set => ToolTipAlignment = ToolTipAlignment.WithVertical(value);
        }

        /// <inheritdoc/>
        public virtual Color? ToolTipTitleForegroundColor
        {
            get => data.TitleForegroundColor;
            set
            {
                if (data.TitleForegroundColor == value)
                    return;
                data.TitleForegroundColor = value;
                HideToolTip();
            }
        }

        /// <inheritdoc/>
        public virtual Font? ToolTipTitleFont
        {
            get => data.TitleFont;
            set
            {
                if (data.TitleFont == value)
                    return;
                data.TitleFont = value;
                HideToolTip();
            }
        }

        /// <summary>
        /// Gets or sets whether tooltip is visible.
        /// </summary>
        public virtual bool ToolTipVisible
        {
            get => drawable.Visible;

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
        public virtual int? TimeoutInMilliseconds
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
            get => data.Icon;
            set
            {
                if (data.Icon == value)
                    return;
                data.Icon = value;
                HideToolTip();
            }
        }

        /// <inheritdoc/>
        public virtual Brush? ToolTipBackgroundBrush
        {
            get => data.BackgroundBrush;
            set
            {
                if (data.BackgroundBrush == value)
                    return;
                data.BackgroundBrush = value;
                HideToolTip();
            }
        }

        /// <inheritdoc/>
        public virtual ImageSet? ToolTipImage
        {
            get => data.Image;
            set
            {
                if (data.Image == value)
                    return;
                data.Image = value;
                HideToolTip();
            }
        }

        RichToolTip? IRichToolTip.ToolTipControl => this;

        AbstractControl? IRichToolTip.AbstractToolTipControl => this;

        private Timer TimerForShow
        {
            get
            {
                return showTimer ??= new()
                {
                    AutoReset = false,
                };
            }
        }

        private Timer TimerForHide
        {
            get
            {
                return hideTimer ??= new()
                {
                    AutoReset = false,
                };
            }
        }

        /// <summary>
        /// Creates an image representation of a rich tooltip based
        /// on the specified template and parameters.
        /// </summary>
        /// <remarks>This method configures the provided template with
        /// the specified parameters, including
        /// text, title, colors, and optional image or icon.
        /// If no content is provided, a default informational icon is
        /// displayed. The resulting tooltip is rendered as an image.</remarks>
        /// <param name="template">The <see cref="TemplateControls.RichToolTipTemplate"/>
        /// used to define the layout and appearance of the
        /// tooltip.</param>
        /// <param name="data">The <see cref="RichToolTipParams"/> containing
        /// the content and styling options for the tooltip, such as
        /// text, title, colors, font, and image.</param>
        /// <returns>An <see cref="Image"/> object representing the rendered tooltip.</returns>
        public static Image CreateToolTipImage(
            TemplateControls.RichToolTipTemplate template,
            RichToolTipParams data)
        {
            template.DoInsideLayout(
            () =>
            {
                if (data.MaxWidth is not null)
                    template.MaxWidth = data.MaxWidth;

                template.Font = data.Font ?? Control.DefaultFont;
                template.NormalBorder = RealDefaultToolTipBorder;
                template.HasBorder = true;
                template.BackgroundColor
                = data.BackgroundColor ?? RichToolTip.DefaultToolTipBackgroundColor;
                template.RaiseBackgroundColorChanged();
                template.ForegroundColor
                = data.ForegroundColor ?? RichToolTip.DefaultToolTipForegroundColor;
                template.TitleLabel.ParentForeColor = false;
                template.TitleLabel.ParentFont = false;
                template.TitleLabel.Text = data.Title;

                template.TitleLabel.Font
                    = data.TitleFont ?? template.Font?.Scaled(1.5);
                template.TitleLabel.ForegroundColor
                    = data.TitleForegroundColor ?? RichToolTip.DefaultToolTipTitleForegroundColor;
                template.MessageLabel.Text = data.Text;

                template.TitleLabel.Visible = !string.IsNullOrEmpty(data.Title);
                template.MessageLabel.Visible = !string.IsNullOrEmpty(data.Text);

                var sizeInPixels
                = GraphicsFactory.PixelFromDip(RichToolTip.DefaultMinImageSize, data.ScaleFactor);

                var titleVisible = template.TitleLabel.Visible;
                var messageVisible = template.MessageLabel.Visible;
                var anyTextVisible = titleVisible || messageVisible;

                var imageMargin = DefaultImageMargin;

                if (titleVisible)
                {
                    imageMargin = imageMargin.WithRight(imageMargin.Right + ImageToTextDistance);
                }

                if (data.Image != null)
                {
                    template.PictureBox.ImageSet = data.Image;
                    template.PictureBox.Visible = true;
                }
                else
                {
                    var hasIcon = template.PictureBox.SetIcon(
                        data.Icon ?? MessageBoxIcon.None,
                        sizeInPixels);
                    template.PictureBox.Visible = hasIcon;
                }

                var imageVisible = template.PictureBox.Visible;
                var anyVisible = imageVisible || anyTextVisible;

                if (!anyVisible)
                {
                    template.PictureBox.SetIcon(MessageBoxIcon.Information, sizeInPixels);
                    template.PictureBox.Visible = true;
                    imageVisible = true;
                }

                template.PictureBox.Margin = imageMargin;
            },
            false);

            var image = TemplateUtils.GetTemplateAsImage(template, template.BackgroundColor);
            return image;
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
                showTimer?.Stop();
                hideTimer?.Stop();
                if (drawable.Visible)
                {
                    drawable.Visible = false;
                    RaiseToolTipVisibleChanged(EventArgs.Empty);
                    LayoutMaxSize = null;
                    Invalidate();
                }

                return this;
            }
            catch(Exception e)
            {
                App.LogError(e);
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
        /// Timeout in milliseconds after which tooltip will be hidden.
        /// Optional. If not specified,
        /// default timeout value is used. If 0 is specified,
        /// tooltip will not be hidden after timeout.
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

            SetIcon(icon ?? MessageBoxIcon.None);

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
        /// </remarks>
        /// <param name="milliseconds">Timeout value.</param>
        /// <param name="millisecondsShowDelay">Show delay value.</param>
        public virtual IRichToolTip SetTimeout(uint? milliseconds, uint millisecondsShowDelay = 0)
        {
            TimeoutInMilliseconds = (int?)milliseconds;
            ShowDelayInMilliseconds = (int)millisecondsShowDelay;
            return this;
        }

        /// <summary>
        /// Resets <see cref="TimeoutInMilliseconds"/> and <see cref="ShowDelayInMilliseconds"/>
        /// to the default values.
        /// </summary>
        /// <returns></returns>
        public virtual IRichToolTip ResetTimeout()
        {
            TimeoutInMilliseconds = null;
            ShowDelayInMilliseconds = 0;
            return this;
        }

        /// <summary>
        /// Sets the small icon to show in the tooltip.
        /// </summary>
        /// <param name="bitmap">Icon of the tooltip.</param>
        public virtual IRichToolTip SetIcon(ImageSet? bitmap)
        {
            ToolTipImage = bitmap;
            return this;
        }

        /// <summary>
        /// Sets the title text font.
        /// </summary>
        /// <remarks>
        /// By default it's emphasized using the font style
        /// or color appropriate for the current platform.
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
        /// Creates an image filled with tooltip data (icon, title, message, border, etc.).
        /// </summary>
        /// <returns></returns>
        public virtual Image CreateToolTipImage()
        {
            data.Title = Title;
            data.Text = Text;
            data.Font = RealFont;
            data.MaxWidth = DefaultMaxWidth;
            data.ScaleFactor = ScaleFactor;

            var result = CreateToolTipImage(template, data);
            return result;
        }

        /// <summary>
        /// Shows the tooltip at the specified location inside the.
        /// Location coordinates are in device-independent units.
        /// </summary>
        public virtual IRichToolTip ShowToolTip(PointD? location = null)
        {
            HideToolTip();

            var image = CreateToolTipImage();

            if (location is not null)
                ToolTipLocation = location.Value;

            drawable.Image = image;

            if (showDelayInMilliseconds > 0)
            {
                var timer = TimerForShow;
                timer.Stop();
                timer.Interval = showDelayInMilliseconds;
                timer.TickAction = ShowAction;
                timer.Start();
            }
            else
                ShowAction();

            void ShowAction()
            {
                if (DisposingOrDisposed)
                    return;
                drawable.Visible = true;
                LayoutMaxSize = drawable.GetPreferredSize(this);
                RaiseToolTipVisibleChanged(EventArgs.Empty);
                Invalidate();

                var timer = TimerForHide;
                timer.Stop();

                if (timeoutInMilliseconds <= 0)
                    return;

                timer.Interval = timeoutInMilliseconds ?? TimerUtils.DefaultToolTipTimeout;
                timer.TickAction = () =>
                {
                    if (IsDisposed)
                        return;
                    HideToolTip();
                };
                timer.Start();
            }

            return this;
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
        /// default timeout value is used. If 0 is specified, tooltip will not be
        /// hidden after timeout.
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
        /// Shows tooltip with error on the screen.
        /// </summary>
        /// <param name="title">Tooltip title. Optional. If not specified "Error" is used. </param>
        /// <param name="timeoutMilliseconds">
        /// Timeout in milliseconds after which tooltip will be hidden. Optional. If not specified,
        /// default timeout value is used. If 0 is specified, tooltip will not be
        /// hidden after timeout.
        /// </param>
        /// <param name="e">Exception.</param>
        /// <param name="location">Location where tooltip will be shown.</param>
        public virtual IRichToolTip ShowToolTipWithError(
            object? title,
            Exception e,
            uint? timeoutMilliseconds = null,
            PointD? location = null)
        {
            return ShowToolTip(
                        title ?? ErrorMessages.Default.ErrorTitle,
                        e.Message,
                        MessageBoxIcon.Error,
                        timeoutMilliseconds,
                        location);
        }

        /// <summary>
        /// Raises <see cref="ToolTipVisibleChanged"/> event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        public virtual void RaiseToolTipVisibleChanged(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            ToolTipVisibleChanged?.Invoke(this, e);
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

        /// <inheritdoc/>
        public override void DefaultPaint(PaintEventArgs e)
        {
            DrawDefaultBackground(e);

            if (drawable.Visible && drawable.Image != null)
            {
                var containerBounds = ClientRectangle.DeflatedWithPadding(Padding);
                RectD imageBounds = (PointD.Empty, drawable.GetPreferredSize(this));

                var alignedRect = AlignUtils.AlignRectInRect(
                    imageBounds,
                    containerBounds,
                    ToolTipHorizontalAlignment,
                    ToolTipVerticalAlignment,
                    false);

                drawable.VisualState = Enabled
                    ? VisualControlState.Normal : VisualControlState.Disabled;
                alignedRect.Location += LayoutOffset;
                drawable.Bounds = alignedRect;
                drawable.Draw(this, e.Graphics);
            }

            DefaultPaintDebug(e);
        }

        IRichToolTip? IToolTipProvider.Get(object? sender)
        {
            return this;
        }

        /// <inheritdoc/>
        protected override void OnScroll(ScrollEventArgs e)
        {
            base.OnScroll(e);
            Invalidate();
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
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            SafeDisposeObject(ref showTimer);
            SafeDisposeObject(ref hideTimer);
            HideToolTip();
            base.DisposeManaged();
        }

        [Conditional("DEBUG")]
        private void DefaultPaintDebug(PaintEventArgs e)
        {
            if (ShowDebugRectangleAtCenter)
            {
                e.Graphics.FillRectangleAtCenter(
                    LightDarkColors.Red.AsBrush,
                    ClientRectangle,
                    3);
            }
        }
    }
}