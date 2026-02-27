using System;
using System.ComponentModel;

using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a standard progress bar control that visually indicates the progress of a task.
    /// </summary>
    [DefaultProperty("Value")]
    [DefaultEvent("ValueChanged")]
    [DefaultBindingProperty("Value")]
    [ControlCategory("Common")]
    public partial class StdProgressBar : Border
    {
        /// <summary>
        /// Gets or sets a value indicating whether the progress bar control uses the
        /// <see cref="DefaultColors.ControlBackColor"/> and
        /// <see cref="DefaultColors.ControlForeColor"/>. Default is <c>true</c>.
        /// </summary>
        public static bool DefaultUseControlColors = false;

        /// <summary>
        /// Gets or sets whether control has border by default.
        /// </summary>
        public static bool DefaultHasBorder = true;

        /// <summary>
        /// Gets or sets whether top border is visible by default.
        /// </summary>
        public static bool DefaultTopBorderVisible = false;

        /// <summary>
        /// Gets or sets whether bottom border is visible by default.
        /// </summary>
        public static bool DefaultBottomBorderVisible = false;

        /// <summary>
        /// Gets or sets whether left border is visible by default.
        /// </summary>
        public static bool DefaultLeftBorderVisible = false;

        /// <summary>
        /// Gets or sets whether right border is visible by default.
        /// </summary>
        public static bool DefaultRightBorderVisible = false;

        /// <summary>
        /// Represents the default size of the left/top and right/bottom indicators.
        /// </summary>
        public static Coord DefaultScaleSize = 7;

        /// <summary>
        /// Represents the default minimum size for a control.
        /// </summary>
        /// <remarks>This value is used to define the smallest allowable size for a control.
        /// It can be used
        /// as a baseline for UI layout calculations or constraints.</remarks>
        public static Coord DefaultControlMinimumSize = 20;

        /// <summary>
        /// Specifies the default interval, in milliseconds, used for animation timer updates.
        /// </summary>
        /// <remarks>This value determines how frequently animation updates occur when no custom interval
        /// is set. Adjusting this value can affect the smoothness and performance of animations that rely on
        /// timer-based updates.</remarks>
        public static int DefaultAnimationTimerInterval = 30;

        /// <summary>
        /// Represents the default tick style for a progress bar control.
        /// </summary>
        public static SliderTickStyle DefaultTickStyle = SliderTickStyle.None;

        private static Color? defaultSpacerColor;
        private static Color? defaultSecondarySpacerColor;

        private readonly SliderScale leftTopScale;
        private readonly SliderScale rightBottomScale;

        private Timer? timer;
        private int maximum = 100;
        private int minimum = 0;
        private int val = 0;
        private int tickFrequency = 1;
        private SliderOrientation orientation;
        private SliderTickStyle tickStyle;
        private WeakReferenceValue<AbstractControl> valueDisplay = new();
        private string? valueFormat;
        private EventHandler<FormatValueEventArgs<int>>? formatValueForDisplay;
        private bool isFirstTickVisible = true;
        private bool isLastTickVisible = true;
        private bool autoSize = true;
        private Color? spacerColor;
        private Color? secondarySpacerColor;
        private bool goingForward = true;
        private bool isIndeterminate;

        /// <summary>
        /// Initializes a new instance of the <see cref="StdProgressBar"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public StdProgressBar(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StdProgressBar"/> class.
        /// </summary>
        public StdProgressBar()
        {
            AutoPadding = false;

            leftTopScale = CreateScale(isLeftTop: true);
            leftTopScale.Dock = DockStyle.Top;
            leftTopScale.MinimumSize = DefaultScaleSize;
            leftTopScale.Height = DefaultScaleSize;
            leftTopScale.Padding = 0;
            leftTopScale.Margin = 0;

            rightBottomScale = CreateScale(isLeftTop: false);
            rightBottomScale.Dock = DockStyle.Bottom;
            rightBottomScale.MinimumSize = DefaultScaleSize;
            rightBottomScale.Height = DefaultScaleSize;
            rightBottomScale.Padding = 0;
            rightBottomScale.Margin = 0;

            tickStyle = DefaultTickStyle;
            Padding = 1;

            MinimumSize = DefaultControlMinimumSize;

            UseControlColors(DefaultUseControlColors);

            Layout = LayoutStyle.Dock;

            SetVisibleBorders(
                DefaultLeftBorderVisible,
                DefaultTopBorderVisible,
                DefaultRightBorderVisible,
                DefaultBottomBorderVisible);
        }

        /// <summary>
        /// Occurs when the <see cref="Value"/> property is formatted for display
        /// before it is assigned to display control specified in
        /// the <see cref="ValueDisplay"/> property.
        /// </summary>
        public event EventHandler<FormatValueEventArgs<int>>? FormatValueForDisplay
        {
            add
            {
                formatValueForDisplay += value;
                UpdateValueDisplay();
            }

            remove
            {
                formatValueForDisplay -= value;
                UpdateValueDisplay();
            }
        }

        /// <summary>
        /// Occurs when the <see cref="Value"/> property of a slider changes,
        /// either by movement of the scroll box or by manipulation in code.
        /// </summary>
        /// <remarks>You can use this event to update other controls when the value represented
        /// in the slider changes.</remarks>
        public event EventHandler? ValueChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Minimum"/> property changes.
        /// </summary>
        public event EventHandler? MinimumChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Orientation"/> property changes.
        /// </summary>
        public event EventHandler? OrientationChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="TickStyle"/> property changes.
        /// </summary>
        public event EventHandler? TickStyleChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Maximum"/> property changes.
        /// </summary>
        public event EventHandler? MaximumChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="TickFrequency"/> property changes.
        /// </summary>
        public event EventHandler? TickFrequencyChanged;

        /// <summary>
        /// Gets default spacer color of the control.
        /// </summary>
        public static Color DefaultSpacerColor
        {
            get => defaultSpacerColor ?? LightDarkColors.Green;
            set => defaultSpacerColor = value;
        }

        /// <summary>
        /// Gets default secondary spacer color of the control.
        /// </summary>
        public static Color DefaultSecondarySpacerColor
        {
            get
            {
                return defaultSecondarySpacerColor ?? DefaultColors.BorderColor;
            }

            set => defaultSecondarySpacerColor = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the progress bar operates in an indeterminate state.
        /// </summary>
        /// <remarks>When set to <see langword="true"/>, the progress bar displays a continuous animation
        /// to indicate ongoing activity without specifying the amount of progress completed. Setting this property to
        /// <see langword="false"/> returns the progress bar to its normal, determinate mode. Use the indeterminate
        /// state when the duration or progress of an operation cannot be determined.</remarks>
        public virtual bool IsIndeterminate
        {
            get
            {
                return isIndeterminate;
            }

            set
            {
                if (isIndeterminate == value)
                    return;
                isIndeterminate = value;
                if (isIndeterminate)
                {
                    goingForward = true;
                    Timer.Start();
                }
                else
                {
                    Timer.Stop();
                    Value = Minimum;
                    Invalidate();
                }
            }
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.ProgressBar;

        /// <summary>
        /// Gets or sets a value indicating whether the slider control automatically sizes itself.
        /// If slider is horizontal and is set to auto-size, it will adjust its height automatically.
        /// If slider is vertical and is set to auto-size, it will adjust its width automatically.
        /// Default is <c>true</c>.
        /// </summary>
        public virtual bool AutoSize
        {
            get => autoSize;
            set
            {
                if (autoSize == value)
                    return;
                autoSize = value;
                PerformLayoutAndInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets the color of the top or left spacer element of the slider control.
        /// If not specified, default spacer color is used, which is defined by the
        /// <see cref="DefaultSpacerColor"/> property.
        /// </summary>
        public virtual Color? SpacerColor
        {
            get
            {
                return spacerColor;
            }

            set
            {
                if (spacerColor == value)
                    return;
                spacerColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the color of the secondary spacer element.
        /// If not specified, default secondary spacer color is used, which is defined by the
        /// <see cref="DefaultSecondarySpacerColor"/> property.
        /// </summary>
        public virtual Color? SecondarySpacerColor
        {
            get
            {
                return secondarySpacerColor;
            }

            set
            {
                if (secondarySpacerColor == value)
                    return;
                secondarySpacerColor = value;
                Invalidate();
            }
        }

        /// <inheritdoc/>
        public override bool HasBorder
        {
            get
            {
                return base.HasBorder;
            }

            set
            {
                base.HasBorder = value;
            }
        }

        /// <inheritdoc/>
        public override Color? BorderColor
        {
            get
            {
                return base.BorderColor;
            }

            set => base.BorderColor = value;
        }

        /// <inheritdoc/>
        public override SizeD SuggestedSize
        {
            get
            {
                return base.SuggestedSize;
            }

            set
            {
                base.SuggestedSize = value;
            }
        }

        /// <summary>
        /// Gets or sets whether the first tick mark is visible on the slider scale.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsFirstTickVisible
        {
            get => isFirstTickVisible;
            set
            {
                if (isFirstTickVisible == value)
                    return;
                isFirstTickVisible = value;
                InvalidateScales();
            }
        }

        /// <summary>
        /// Gets or sets whether the last tick mark is visible on the slider scale.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsLastTickVisible
        {
            get => isLastTickVisible;
            set
            {
                if (isLastTickVisible == value)
                    return;
                isLastTickVisible = value;
                InvalidateScales();
            }
        }

        /// <summary>
        /// Gets or sets the format string used to display the value of the slider.
        /// </summary>
        public virtual string? ValueFormat
        {
            get => valueFormat;
            set
            {
                if (valueFormat != value)
                {
                    valueFormat = value;
                    UpdateValueDisplay();
                }
            }
        }

        /// <summary>
        /// Gets or sets the display control for the slider value.
        /// </summary>
        public AbstractControl? ValueDisplay
        {
            get => valueDisplay.Value;

            set
            {
                valueDisplay.Value = value;
                UpdateValueDisplay();
            }
        }

        /// <summary>
        /// Gets the left/top scale control of the slider.
        /// </summary>
        [Browsable(false)]
        public SliderScale LeftTopScale => leftTopScale;

        /// <summary>
        /// Gets the right/bottom scale control of the slider.
        /// </summary>
        [Browsable(false)]
        public SliderScale RightBottomScale => rightBottomScale;

        /// <inheritdoc/>
        public override HorizontalAlignment HorizontalAlignment
        {
            get
            {
                return HorizontalAlignment.Left;
            }

            set
            {
                base.HorizontalAlignment = value;
            }
        }

        /// <inheritdoc/>
        public override VerticalAlignment VerticalAlignment
        {
            get
            {
                return base.VerticalAlignment;
            }

            set
            {
                base.VerticalAlignment = value;
            }
        }

        /// <summary>
        /// Gets the maximum possible size of the left/top spacer control.
        /// </summary>
        [Browsable(false)]
        public virtual Coord MaxLeftTopSpacerSize
        {
            get
            {
                Coord result;

                if (IsHorizontal)
                {
                    result = ClientSize.Width - Padding.Horizontal;
                }
                else
                {
                    result = ClientSize.Height - Padding.Vertical;
                }

                return result;
            }
        }

        /// <summary>
        /// Gets or sets a numeric value that represents the current position of the thumb
        /// on the slider.
        /// </summary>
        /// <value>A numeric value that is within the <see cref="Minimum"/> and
        /// <see cref="Maximum"/> range. The default value is 0.</value>
        /// <remarks>The <see cref="Value"/> property contains the number that represents
        /// the current position of the thumb on the slider.</remarks>
        public virtual int Value
        {
            get
            {
                return val;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                value = CoerceValue(value);
                if (this.val == value)
                    return;
                try
                {
                    SuppressInvalidate();
                    this.val = value;
                    RaiseValueChanged();
                }
                finally
                {
                    EndInvalidateSuppression(true);
                }
            }
        }

        /// <inheritdoc/>
        public override Thickness Padding
        {
            get
            {
                var result = base.Padding;
                result.ApplyMin(1);
                return result;
            }

            set
            {
                value.ApplyMin(1);
                base.Padding = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the horizontal or vertical orientation of the slider.
        /// </summary>
        /// <value>One of the <see cref="SliderOrientation"/> values.</value>
        /// <remarks>
        /// When the <see cref="Orientation"/> property is set to
        /// <see cref="SliderOrientation.Horizontal"/>, the scroll
        /// box moves from left to right as the <see cref="Value"/> increases. When the
        /// <see cref="Orientation"/>
        /// property is set to <see cref="SliderOrientation.Vertical"/>, the scroll box moves
        /// from bottom to top as the
        /// <see cref="Value"/> increases.
        /// </remarks>
        public virtual SliderOrientation Orientation
        {
            get => orientation;
            set
            {
                if (DisposingOrDisposed)
                    return;
                if (orientation == value)
                    return;
                orientation = value;

                PerformLayoutAndInvalidate(() =>
                {
                    var isHorizontal = value == SliderOrientation.Horizontal;
                    var dockStyle = isHorizontal ? DockStyle.Left : DockStyle.Top;

                    if (isHorizontal)
                    {
                        leftTopScale.Dock = DockStyle.Top;
                        rightBottomScale.Dock = DockStyle.Bottom;
                    }
                    else
                    {
                        leftTopScale.Dock = DockStyle.Left;
                        rightBottomScale.Dock = DockStyle.Right;
                    }

                    leftTopScale.Size = 0;
                    rightBottomScale.Size = 0;
                    Size = GetPreferredSize(Size);
                });

                OrientationChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the slider is horizontal.
        /// </summary>
        [Browsable(false)]
        public bool IsHorizontal
        {
            get => Orientation == SliderOrientation.Horizontal;
            set
            {
                Orientation = value ? SliderOrientation.Horizontal : SliderOrientation.Vertical;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the slider is vertical.
        /// </summary>
        [Browsable(false)]
        public bool IsVertical
        {
            get => Orientation == SliderOrientation.Vertical;
            set
            {
                Orientation = value ? SliderOrientation.Vertical : SliderOrientation.Horizontal;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating how to display the tick marks on the slider.
        /// </summary>
        /// <value>
        /// One of the <see cref="SliderTickStyle"/> values. The default is
        /// <see cref="DefaultTickStyle"/>.
        /// </value>
        public virtual SliderTickStyle TickStyle
        {
            get => tickStyle;
            set
            {
                if (DisposingOrDisposed)
                    return;
                if (tickStyle == value)
                    return;
                tickStyle = value;
                UpdateScaleVisibility();
                TickStyleChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the lower limit of the range this control is working with.
        /// </summary>
        /// <value>The minimum value for the Value property. The default is 0.</value>
        /// <remarks>
        /// The minimum and maximum values of the Value property are specified by the
        /// <see cref="Minimum"/> and <see cref="Maximum"/> properties.
        /// If the new <see cref="Minimum"/> property value is greater than the
        /// <see cref="Maximum"/> property value,
        /// the <see cref="Maximum"/> value is set equal to the <see cref="Minimum"/> value.
        /// If the <see cref="Value"/> is less than the new <see cref="Minimum"/> value,
        /// the <see cref="Value"/> property
        /// is also set equal to the <see cref="Minimum"/> value.
        /// </remarks>
        public virtual int Minimum
        {
            get
            {
                return minimum;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                if (minimum == value)
                    return;
                minimum = value;
                RaiseMinimumChanged(EventArgs.Empty);
                Maximum = maximum;
                var v = Value;
                Value = v;
            }
        }

        /// <summary>
        /// Gets or sets the upper limit of the range this control is working with.
        /// </summary>
        /// <value>The maximum value for the Value property. The default is 10.</value>
        /// <remarks>
        /// The minimum and maximum values of the Value property are specified by the
        /// <see cref="Minimum"/> and <see cref="Maximum"/> properties.
        /// If the new <see cref="Minimum"/> property value is greater than the
        /// <see cref="Maximum"/> property value,
        /// the <see cref="Maximum"/> value is set equal to the <see cref="Minimum"/> value.
        /// If the <see cref="Value"/> is less than the new <see cref="Minimum"/> value, the
        /// <see cref="Value"/> property
        /// is also set equal to the <see cref="Minimum"/> value.
        /// </remarks>
        public virtual int Maximum
        {
            get
            {
                return maximum;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                value = CoerceMaximum(value);
                if (maximum == value)
                    return;
                maximum = value;
                RaiseMaximumChanged(EventArgs.Empty);
                var v = Value;
                Value = v;
            }
        }

        /// <summary>
        /// Gets or sets a value that specifies the delta between ticks drawn on the control.
        /// </summary>
        /// <value>The numeric value representing the delta between ticks. The default is 1.</value>
        public virtual int TickFrequency
        {
            get
            {
                return tickFrequency;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                if (value < 1)
                    value = 1;
                if (tickFrequency == value)
                    return;
                tickFrequency = value;

                if (TickStyle != SliderTickStyle.None)
                {
                    Invalidate();
                }

                TickFrequencyChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        [Browsable(false)]
        internal new string Title
        {
            get => base.Title;
            set => base.Title = value;
        }

        [Browsable(false)]
        internal new string Text
        {
            get => base.Text;
            set => base.Text = value;
        }

        [Browsable(false)]
        internal new Font? Font
        {
            get => base.Font;
            set => base.Font = value;
        }

        [Browsable(false)]
        internal new bool IsBold
        {
            get => base.IsBold;
            set => base.IsBold = value;
        }

        [Browsable(false)]
        internal new bool ParentFont
        {
            get => base.ParentFont;
            set => base.ParentFont = value;
        }

        [Browsable(false)]
        internal new Thickness? MinChildMargin
        {
            get => base.MinChildMargin;
            set => base.MinChildMargin = value;
        }

        [Browsable(false)]
        internal new LayoutStyle? Layout
        {
            get => base.Layout;
            set => base.Layout = value;
        }

        /// <inheritdoc/>
        protected override Thickness MinPadding
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// Gets the timer used for the animation.
        /// </summary>
        protected Timer Timer
        {
            get
            {
                if (timer == null)
                {
                    timer = new Timer();
                    timer.Interval = DefaultAnimationTimerInterval;
                    timer.Tick += OnTimerTick;
                }

                return timer;
            }
        }

        /// <summary>
        /// Clears the ticks.
        /// </summary>
        public virtual void ClearTicks()
        {
            TickStyle = SliderTickStyle.None;
        }

        /// <summary>
        /// Raises the <see cref="MinimumChanged"/> event and calls
        /// <see cref="OnMinimumChanged(EventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        public void RaiseMinimumChanged(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnMinimumChanged(e);
            MinimumChanged?.Invoke(this, e);
        }

        /// <inheritdoc/>
        public override void DefaultPaint(PaintEventArgs e)
        {
            const float defaultBarSize = 15;

            base.DefaultPaint(e);

            var sc1 = (spacerColor ?? DefaultSpacerColor).AsBrush;
            var sc2 = (secondarySpacerColor ?? DefaultSecondarySpacerColor).AsBrush;

            var r = e.ClipRectangle;
            r = r.DeflatedWithPadding(Padding);

            if (r.SizeIsEmpty)
                return;

            var g = e.Graphics;

            if (IsHorizontal)
            {
                r.Height = defaultBarSize;
                r.CenterVert = e.ClipRectangle.CenterVert;
            }
            else
            {
                r.Width = defaultBarSize;
                r.CenterHorz = e.ClipRectangle.CenterHorz;
            }

            if (Value <= Minimum)
            {
                g.FillRectangle(sc2, r);
            }
            else
            if(Value >= Maximum)
            {
                g.FillRectangle(sc1, r);
            }
            else
            {
                var pos = ScaleValueToPosition(Value);

                RectD leftTop = new();
                RectD rightBottom = new();

                if (IsHorizontal)
                {
                    leftTop.Left = r.Left;
                    leftTop.Width = r.Left + pos;
                    leftTop.Top = r.Top;
                    leftTop.Height = r.Height;

                    rightBottom.Left = leftTop.Right;
                    rightBottom.Width = r.Right - rightBottom.Left;
                    rightBottom.Top = r.Top;
                    rightBottom.Height = r.Height;
                }
                else
                {
                }

                if (!leftTop.SizeIsEmpty)
                    g.FillRectangle(sc1, leftTop);
                if (!rightBottom.SizeIsEmpty)
                    g.FillRectangle(sc2, rightBottom);
            }
        }

        /// <summary>
        /// Raises the <see cref="MaximumChanged"/> event and calls
        /// <see cref="OnMaximumChanged(EventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        public void RaiseMaximumChanged(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnMaximumChanged(e);
            MaximumChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="ValueChanged"/> event and calls
        /// <see cref="OnValueChanged(EventArgs)"/>.
        /// </summary>
        public void RaiseValueChanged()
        {
            if (DisposingOrDisposed)
                return;
            OnValueChanged(EventArgs.Empty);
            UpdateValueDisplay();

            Invoke(() =>
            {
                ValueChanged?.Invoke(this, EventArgs.Empty);
                Designer?.RaisePropertyChanged(this, nameof(Value));
            });
        }

        /// <inheritdoc/>
        public override SizeD GetPreferredSize(PreferredSizeContext context)
        {
            var width = NormalBorder.Width;
            return Internal() + (width.Horizontal, width.Vertical);

            SizeD Internal()
            {
                const Coord defaultAutoWidth = 150;

                var specifiedWidth = SuggestedWidth;
                var specifiedHeight = SuggestedHeight;

                if (IsHorizontal)
                {
                    if (Coord.IsNaN(specifiedWidth))
                    {
                        if (MathUtils.IsInfinityOrNanOrMax(context.AvailableSize.Width))
                            specifiedWidth = defaultAutoWidth;
                        else
                            specifiedWidth = Math.Min(defaultAutoWidth, context.AvailableSize.Width);
                    }
                    else
                    {
                    }

                    if (Coord.IsNaN(specifiedHeight) || AutoSize)
                    {
                        specifiedHeight = MathUtils.Max(
                            MinimumSize.Height,
                            DefaultControlMinimumSize);
                        specifiedHeight = MathUtils.Ceiling(specifiedHeight);

                        if (leftTopScale.Parent is not null)
                        {
                            specifiedHeight += leftTopScale.Height + leftTopScale.Margin.Vertical;
                        }

                        if (rightBottomScale.Parent is not null)
                        {
                            specifiedHeight
                                += rightBottomScale.Height + rightBottomScale.Margin.Vertical;
                        }
                    }
                    else
                    {
                    }
                }
                else
                {
                    if (Coord.IsNaN(specifiedWidth) || AutoSize)
                    {
                        specifiedWidth = MathUtils.Max(MinimumSize.Width, DefaultControlMinimumSize);

                        if (leftTopScale.Parent is not null)
                        {
                            specifiedWidth += leftTopScale.Width + leftTopScale.Margin.Horizontal;
                        }

                        if (rightBottomScale.Parent is not null)
                        {
                            specifiedWidth
                                += rightBottomScale.Width + rightBottomScale.Margin.Horizontal;
                        }
                    }

                    if (Coord.IsNaN(specifiedHeight))
                    {
                        if (MathUtils.IsInfinityOrNanOrMax(context.AvailableSize.Height))
                            specifiedHeight = defaultAutoWidth;
                        else
                            specifiedHeight = Math.Min(defaultAutoWidth, context.AvailableSize.Height);
                    }
                }

                return (specifiedWidth, specifiedHeight);
            }
        }

        /// <summary>
        /// Scales a numeric value to a position on the slider.
        /// </summary>
        /// <param name="val">The numeric value to be scaled.</param>
        /// <returns>The position on the slider corresponding to the specified value.</returns>
        public virtual Coord ScaleValueToPosition(int val)
        {
            if (val <= Minimum)
                return 0;
            else
                if (val >= Maximum)
                    return MaxLeftTopSpacerSize;
                else
                {
                    var v = val - Minimum;
                    var maxV = Maximum - Minimum;

                    var newSpacerSize = (v * MaxLeftTopSpacerSize) / maxV;

                    var result = Math.Min(Math.Max(0, newSpacerSize), MaxLeftTopSpacerSize);
                    return result;
                }
        }

        /// <summary>
        /// Called when the value of the <see cref="Value"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnValueChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the minimum of the <see cref="Minimum"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnMinimumChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the maximum of the <see cref="Maximum"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnMaximumChanged(EventArgs e)
        {
        }

        /// <inheritdoc/>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            Invalidate();
        }

        /// <summary>
        /// Coerces minimal value to have the valid range.
        /// </summary>
        /// <param name="value">Value to coerce.</param>
        /// <returns></returns>
        protected virtual int CoerceMaximum(int value)
        {
            int min = Minimum;
            if (value < min)
                return min;
            return value;
        }

        /// <summary>
        /// Coerces value the have the valid range.
        /// </summary>
        /// <param name="value">Value to coerce.</param>
        /// <returns></returns>
        protected virtual int CoerceValue(int value)
        {
            if (value < Minimum)
                return Minimum;

            if (value > Maximum)
                return Maximum;

            return value;
        }

        /// <inheritdoc/>
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
        }

        /// <summary>
        /// Creates an scale control for the slider.
        /// </summary>
        /// <param name="isLeftTop">Whether to create a left or top slider scale.</param>
        /// <returns>A new instance of <see cref="SliderScale"/>
        /// representing the slider scale with ticks and labels.</returns>
        protected virtual SliderScale CreateScale(bool isLeftTop)
        {
            var indicator = new SliderScale();
            return indicator;
        }

        /// <summary>
        /// Updates the display of the current value in the
        /// <see cref="ValueDisplay"/> control.
        /// </summary>
        protected virtual void UpdateValueDisplay()
        {
            if (DisposingOrDisposed)
                return;
            if (ValueDisplay is null || ValueDisplay.DisposingOrDisposed)
                return;

            if (formatValueForDisplay is not null)
            {
                var formatEventArgs = new FormatValueEventArgs<int>(Value);
                formatValueForDisplay(this, formatEventArgs);
                ValueDisplay.Text = formatEventArgs.FormattedValue ?? DefaultValueToString();
                return;
            }

            ValueDisplay.Text = DefaultValueToString();

            string DefaultValueToString()
            {
                if (ValueFormat is not null)
                {
                    return string.Format(ValueFormat, Value);
                }

                return Value.ToString() ?? string.Empty;
            }
        }

        /// <inheritdoc/>
        protected override bool GetDefaultHasBorder()
        {
            return DefaultHasBorder;
        }

        /// <summary>
        /// Updates the visibility of the indicators based on the current
        /// state of the <see cref="TickStyle"/> property.
        /// </summary>
        protected virtual void UpdateScaleVisibility()
        {
            DoInsideLayout(() =>
            {
                switch (TickStyle)
                {
                    case SliderTickStyle.None:
                        leftTopScale.Parent = null;
                        rightBottomScale.Parent = null;
                        break;
                    case SliderTickStyle.Both:
                        leftTopScale.Parent = this;
                        rightBottomScale.Parent = this;
                        break;
                    case SliderTickStyle.TopLeft:
                        leftTopScale.Parent = this;
                        rightBottomScale.Parent = null;
                        break;
                    case SliderTickStyle.BottomRight:
                        leftTopScale.Parent = null;
                        rightBottomScale.Parent = this;
                        break;
                }
            });
        }

        /// <summary>
        /// Invoked when the timer interval elapses to allow implementation of animated value changes.
        /// </summary>
        /// <param name="sender">The source of the event, typically the timer that raised the event.</param>
        /// <param name="e">An object that contains the event data.</param>
        protected virtual void OnTimerTick(object? sender, EventArgs e)
        {
            if (goingForward)
            {
                if (Value < Maximum)
                    Value++;
                else
                    goingForward = false;
            }
            else
            {
                if (Value > Minimum)
                    Value--;
                else
                    goingForward = true;
            }
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            timer?.Stop();
            SafeDispose(ref timer);
            base.DisposeManaged();
        }

        private void InvalidateScales()
        {
            if (TickStyle != SliderTickStyle.None)
            {
                leftTopScale.Invalidate();
                rightBottomScale.Invalidate();
            }
        }

    }
}
