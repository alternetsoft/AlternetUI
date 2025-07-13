using System;
using System.ComponentModel;

using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a generic slider control (also known as track bar).
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is a scrollable control similar to the scroll bar control.
    /// You can configure ranges through which the value of the <see cref="Value"/> property of a
    /// slider scrolls by setting the <see cref="Minimum"/> property to specify the lower end
    /// of the range and the <see cref="Maximum"/> property to specify the upper end of the range.
    /// </para>
    /// <para>
    /// The slider can be displayed horizontally or vertically.
    /// </para>
    /// <para>
    /// You can use this control to input numeric data obtained through the
    /// <see cref="Value"/> property.
    /// You can display this numeric data in a control or use it in code.
    /// </para>
    /// </remarks>
    [DefaultProperty("Value")]
    [DefaultEvent("ValueChanged")]
    [DefaultBindingProperty("Value")]
    [ControlCategory("Common")]
    public partial class Slider : Border, ISliderScaleContainer
    {
        /// <summary>
        /// Gets or sets default slider thumb border color.
        /// </summary>
        public static Color? DefaultThumbBorderColor = SystemColors.WindowText;

        /// <summary>
        /// Gets or sets whether to use default spacer color.
        /// </summary>
        public static bool UseDefaultSpacerColor = false;

        /// <summary>
        /// Gets or sets whether slider thumb has border by default.
        /// </summary>
        public static bool DefaultThumbHasBorder = true;

        /// <summary>
        /// Gets or sets a value indicating whether the slider control uses the
        /// <see cref="DefaultColors.ControlBackColor"/> and
        /// <see cref="DefaultColors.ControlForeColor"/>. Default is <c>true</c>.
        /// </summary>
        public static bool DefaultUseControlColors = true;

        /// <summary>
        /// Represents the default size of the left/top and right/bottom indicators.
        /// </summary>
        public static Coord DefaultScaleSize = 5;

        /// <summary>
        /// Represents the default minimum size for a slider control.
        /// </summary>
        /// <remarks>This value is used to define the smallest allowable size for a slider.
        /// It can be used
        /// as a baseline for UI layout calculations or constraints.</remarks>
        public static Coord DefaultSliderMinimumSize = 20;

        /// <summary>
        /// Represents the default width of the slider's thumb.
        /// </summary>
        public static Coord DefaultSliderThumbWidth = 15;

        /// <summary>
        /// Represents the default tick style for a slider control.
        /// </summary>
        public static SliderTickStyle DefaultTickStyle = SliderTickStyle.None;

        private static Color? defaultSpacerColor;

        private readonly Spacer leftTopSpacer;
        private readonly Spacer rightBottomSpacer;
        private readonly SliderScale leftTopScale;
        private readonly SliderScale rightBottomScale;
        private readonly SliderThumb thumb;

        private int maximum = 10;
        private int minimum = 0;
        private int val = 0;
        private int smallChange = 1;
        private int largeChange = 5;
        private int tickFrequency = 1;
        private SliderOrientation orientation;
        private SliderTickStyle tickStyle;
        private WeakReferenceValue<AbstractControl> valueDisplay = new();
        private string? valueFormat;
        private EventHandler<FormatValueEventArgs<int>>? formatValueForDisplay;
        private bool isFirstTickVisible = true;
        private bool isLastTickVisible = true;
        private bool autoSize = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="Slider"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public Slider(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Slider"/> class.
        /// </summary>
        public Slider()
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

            leftTopSpacer = CreateSpacer();
            leftTopSpacer.ParentBackColor = true;
            leftTopSpacer.Dock = DockStyle.Left;
            leftTopSpacer.BackgroundPadding = 4;

            thumb = CreateSliderThumb();

            rightBottomSpacer = CreateSpacer();
            rightBottomSpacer.ParentBackColor = true;
            rightBottomSpacer.Dock = DockStyle.Fill;
            rightBottomSpacer.Margin = 1;
            rightBottomSpacer.BackgroundPadding = 4;

            SuggestedSize = 150;

            tickStyle = DefaultTickStyle;
            Padding = 1;

            thumb.Margin = (0, 0, 1, 1);
            thumb.MinSize = 0;
            thumb.MinExtra = 0;
            thumb.SizeDelta = 0;

            MinimumSize = DefaultSliderMinimumSize;

            UseControlColors(DefaultUseControlColors);

            Layout = LayoutStyle.Dock;
            leftTopSpacer.Width = 0;

            rightBottomSpacer.Parent = this;
            thumb.Parent = this;
            leftTopSpacer.Parent = this;

            thumb.SplitterMoved += OnThumbSplitterMoved;
            thumb.SplitterMoving += OnThumbSplitterMoving;

            leftTopSpacer.MouseLeftButtonDown += OnLeftTopSpacerMouseDown;
            leftTopSpacer.MouseDoubleClick += OnLeftTopSpacerMouseDown;
            rightBottomSpacer.MouseLeftButtonDown += OnRightBottomSpacerMouseDown;
            rightBottomSpacer.MouseDoubleClick += OnRightBottomSpacerMouseDown;

            if (UseDefaultSpacerColor)
            {
                SetSpacerColor(DefaultSpacerColor);
            }

            thumb.Bounds = thumb.Bounds;
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
        /// Occurs when the value of the <see cref="SmallChange"/> property changes.
        /// </summary>
        public event EventHandler? SmallChangeChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="LargeChange"/> property changes.
        /// </summary>
        public event EventHandler? LargeChangeChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="TickFrequency"/> property changes.
        /// </summary>
        public event EventHandler? TickFrequencyChanged;

        /// <summary>
        /// Gets default spacer color of the slider.
        /// This value is used when <see cref="UseDefaultSpacerColor"/> is True.
        /// </summary>
        public static Color DefaultSpacerColor
        {
            get => defaultSpacerColor ?? DefaultColors.DefaultCheckBoxColor;
            set => defaultSpacerColor = value;
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.Slider;

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
                thumb.Bounds = thumb.Bounds;
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
        /// Gets the thumb control of the slider.
        /// </summary>
        [Browsable(false)]
        public SliderThumb ThumbControl => thumb;

        /// <summary>
        /// Gets the size of the thumb control.
        /// </summary>
        [Browsable(false)]
        public SizeD ThumbSize => thumb.Size;

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

        /// <summary>
        /// Gets the left/top spacer control of the slider.
        /// </summary>
        [Browsable(false)]
        public AbstractControl LeftTopSpacer => leftTopSpacer;

        /// <summary>
        /// Gets the right/bottom spacer control of the slider.
        /// </summary>
        [Browsable(false)]
        public AbstractControl RightBottomSpacer => rightBottomSpacer;

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
                    if (ThumbControl.Parent is not null)
                        result = result - ThumbControl.Width - ThumbControl.Margin.Horizontal;
                }
                else
                {
                    result = ClientSize.Height - Padding.Vertical;
                    if (ThumbControl.Parent is not null)
                        result = result - ThumbControl.Height - ThumbControl.Margin.Vertical;
                }

                return result;
            }
        }

        /// <summary>
        /// Gets the current size of the left/top spacer control.
        /// </summary>
        [Browsable(false)]
        public virtual Coord LeftTopSpacerSize
        {
            get
            {
                if (IsHorizontal)
                    return LeftTopSpacer.Width;
                else
                    return LeftTopSpacer.Height;
            }

            set
            {
                if (IsHorizontal)
                    LeftTopSpacer.Width = value;
                else
                    LeftTopSpacer.Height = value;
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
                this.val = value;
                UpdateThumbPositionFromValue();
                RaiseValueChanged();
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

                    LeftTopSpacer.Dock = dockStyle;
                    ThumbControl.Dock = dockStyle;

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
                    leftTopSpacer.Size = 0;
                    thumb.Size = 0;
                    Size = GetPreferredSize(Size);
                    UpdateThumbPositionFromValue();
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
                Value = Value;
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
                Value = Value;
            }
        }

        /// <summary>
        /// Gets or sets the value added to or subtracted from the <see cref="Value"/> property
        /// when the thumb is moved a small distance.
        /// </summary>
        /// <value>A numeric value. The default value is 1.</value>
        public virtual int SmallChange
        {
            get
            {
                return smallChange;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                if (value < 0)
                    value = 0;
                if (smallChange == value)
                    return;
                smallChange = value;

                SmallChangeChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets a value to be added to or subtracted from the <see cref="Value"/> property
        /// when the thumb is moved a large distance.
        /// </summary>
        /// <value>A numeric value. The default is 5.</value>
        public virtual int LargeChange
        {
            get
            {
                return largeChange;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                if (value < 0)
                    value = 0;
                if (largeChange == value)
                    return;
                largeChange = value;
                LargeChangeChanged?.Invoke(this, EventArgs.Empty);
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
        /// Decreases the <see cref="Value"/> property by the <see cref="LargeChange"/> value.
        /// </summary>
        public virtual void DecValueLarge()
        {
            if (DisposingOrDisposed)
                return;
            Value -= LargeChange;
        }

        /// <summary>
        /// Decreases the <see cref="Value"/> property by the <see cref="SmallChange"/> value.
        /// </summary>
        public virtual void DecValue()
        {
            if (DisposingOrDisposed)
                return;
            Value -= SmallChange;
        }

        /// <summary>
        /// Increases the <see cref="Value"/> property by the <see cref="LargeChange"/> value.
        /// </summary>
        public virtual void IncValueLarge()
        {
            if (DisposingOrDisposed)
                return;
            Value += LargeChange;
        }

        /// <summary>
        /// Increases the <see cref="Value"/> property by the <see cref="SmallChange"/> value.
        /// </summary>
        public virtual void IncValue()
        {
            if (DisposingOrDisposed)
                return;
            Value += SmallChange;
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
            ValueChanged?.Invoke(this, EventArgs.Empty);
            Designer?.RaisePropertyChanged(this, nameof(Value));
            UpdateValueDisplay();
        }

        /// <inheritdoc/>
        public override SizeD GetPreferredSize(SizeD availableSize)
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
                        if (CoordUtils.IsInfinityOrNanOrMax(availableSize.Width))
                            specifiedWidth = defaultAutoWidth;
                        else
                            specifiedWidth = Math.Min(defaultAutoWidth, availableSize.Width);
                    }
                    else
                    {
                    }

                    if (Coord.IsNaN(specifiedHeight) || AutoSize)
                    {
                        specifiedHeight = MathUtils.Max(
                            MinimumSize.Height,
                            DefaultSliderMinimumSize);
                        specifiedHeight = Math.Ceiling(specifiedHeight);

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
                        specifiedWidth = MathUtils.Max(
                            MinimumSize.Width,
                            DefaultSliderMinimumSize);

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
                        if (CoordUtils.IsInfinityOrNanOrMax(availableSize.Height))
                            specifiedHeight = defaultAutoWidth;
                        else
                            specifiedHeight = Math.Min(defaultAutoWidth, availableSize.Height);
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
        /// Sets left/top spacer color to default.
        /// </summary>
        public virtual void SetSpacerColorToDefault()
        {
            SetSpacerColor(DefaultSpacerColor);
        }

        /// <summary>
        /// Sets the colors of the left/top spacer.
        /// </summary>
        /// <param name="leftTopSpacerColor">The color of the left/top spacer.
        /// If <c>null</c>, the left/top spacer will use the parent's background color.</param>
        public virtual void SetSpacerColor(Color? leftTopSpacerColor)
        {
            leftTopSpacer.ParentBackColor = leftTopSpacerColor is null;
            leftTopSpacer.BackgroundColor = leftTopSpacerColor;
            leftTopSpacer.HasBackground = leftTopSpacerColor is not null;
            leftTopSpacer.HasBackground = leftTopSpacerColor is not null;
        }

        /// <summary>
        /// Sets the colors of the right/bottom spacer.
        /// </summary>
        /// <param name="spacerColor">The color of the right/bottom spacer.
        /// If <c>null</c>, the right/bottom spacer will use the parent's background color.</param>
        public virtual void SetFarSpacerColor(Color? spacerColor)
        {
            rightBottomSpacer.ParentBackColor = spacerColor is null;
            rightBottomSpacer.BackgroundColor = spacerColor;
            rightBottomSpacer.HasBackground = spacerColor is not null;
            rightBottomSpacer.HasBackground = spacerColor is not null;
        }

        /// <summary>
        /// Sets right/bottom spacer color to default.
        /// </summary>
        public virtual void SetFarSpacerColorToDefault()
        {
            SetFarSpacerColor(DefaultSpacerColor);
        }

        internal void SetDebugColors()
        {
            SetSpacerColor(LightDarkColors.Green);
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

        /// <summary>
        /// Updates the <see cref="Value"/> property based on the current
        /// position of the thumb control.
        /// </summary>
        protected virtual void UpdateValueFromThumbPosition()
        {
            if (LeftTopSpacerSize <= 0)
            {
                Value = Minimum;
                return;
            }

            if (LeftTopSpacerSize >= MaxLeftTopSpacerSize)
            {
                Value = Maximum;
                return;
            }

            var computedValue = ((Maximum - Minimum) * LeftTopSpacerSize) / MaxLeftTopSpacerSize;
            var asInt = Convert.ToInt32(computedValue);
            Value = Minimum + asInt;
        }

        /// <inheritdoc/>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateThumbPositionFromValue();
        }

        /// <summary>
        /// Updates the position of the thumb control based on the current
        /// state of the control.
        /// </summary>
        protected virtual void UpdateThumbPositionFromValue()
        {
            if (DisposingOrDisposed)
                return;

            LeftTopSpacerSize = ScaleValueToPosition(Value);
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
        /// Handles the mouse down event on the left/top spacer.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The mouse event data.</param>
        protected virtual void OnLeftTopSpacerMouseDown(object? sender, MouseEventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            if (IsHorizontal)
                Value -= LargeChange;
            else
                Value -= LargeChange;
        }

        /// <summary>
        /// Handles the mouse down event on the right/bottom spacer.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The mouse event data.</param>
        protected virtual void OnRightBottomSpacerMouseDown(object? sender, MouseEventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            if (IsHorizontal)
                Value += LargeChange;
            else
                Value += LargeChange;
        }

        /// <summary>
        /// Handles the event triggered when the thumb splitter is moved.
        /// </summary>
        /// <remarks>This method is intended to be overridden in derived classes
        /// to provide custom
        /// handling for the thumb splitter movement event.</remarks>
        /// <param name="sender">The source of the event, typically the control
        /// that raised the event. Can be <see langword="null"/>.</param>
        /// <param name="e">An instance of <see cref="SplitterEventArgs"/> containing
        /// event data related to the splitter movement.</param>
        protected virtual void OnThumbSplitterMoved(object? sender, SplitterEventArgs e)
        {
            if (DisposingOrDisposed)
                return;
        }

        /// <summary>
        /// Handles the event triggered when the thumb splitter is moving.
        /// </summary>
        /// <remarks>This method is intended to be overridden in derived classes
        /// to provide custom
        /// handling for the thumb splitter movement event.</remarks>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An instance of <see cref="SplitterEventArgs"/> containing event
        /// data related to the splitter movement.</param>
        protected virtual void OnThumbSplitterMoving(object? sender, SplitterEventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            UpdateValueFromThumbPosition();
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
        /// Creates a spacer control for the slider.
        /// </summary>
        /// <returns>A new spacer control instance.</returns>
        protected virtual Spacer CreateSpacer()
        {
            return new Spacer();
        }

        /// <summary>
        /// Creates slider thumb control.
        /// </summary>
        /// <returns>New instance of <see cref="SliderThumb"/> or its descendant.</returns>
        protected virtual SliderThumb CreateSliderThumb()
        {
            return new SliderThumb();
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

            if(formatValueForDisplay is not null)
            {
                var formatEventArgs = new FormatValueEventArgs<int>(Value);
                formatValueForDisplay(this, formatEventArgs);
                ValueDisplay.Text = formatEventArgs.FormattedValue ?? DefaultValueToString();
                return;
            }

            ValueDisplay.Text = DefaultValueToString();

            string DefaultValueToString()
            {
                if(ValueFormat is not null)
                {
                    return string.Format(ValueFormat, Value);
                }

                return Value.ToString() ?? string.Empty;
            }
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

        private void InvalidateScales()
        {
            if (TickStyle != SliderTickStyle.None)
            {
                leftTopScale.Invalidate();
                rightBottomScale.Invalidate();
            }
        }

        /// <summary>
        /// Represents the slider thumb control.
        /// </summary>
        public class SliderThumb : Splitter
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="SliderThumb"/> class.
            /// </summary>
            public SliderThumb()
            {
                DefaultCursor = Cursors.Default;
                HasBorder = DefaultThumbHasBorder;
                BorderColor = DefaultThumbBorderColor;
            }

            /// <inheritdoc/>
            public override PointD? MinimumLocation
            {
                get
                {
                    var baseLocation = base.MinimumLocation;

                    if(baseLocation is null)
                    {
                        if (Parent is not null && Parent.HasBorder)
                            return (2, 2);
                        return (1, 1);
                    }

                    return baseLocation;
                }

                set => base.MinimumLocation = value;
            }

            /// <summary>
            /// Gets the container that holds the slider scale.
            /// In the default implementation, this property returns the parent control
            /// that implements the <see cref="ISliderScaleContainer"/> interface.
            /// </summary>
            [Browsable(false)]
            public virtual ISliderScaleContainer? Container
            {
                get
                {
                    return Parent as ISliderScaleContainer;
                }
            }

            /// <summary>
            /// Gets a value indicating whether the slider's orientation is horizontal.
            /// </summary>
            public bool IsHorizontal
            {
                get
                {
                    return Orientation == SliderOrientation.Horizontal;
                }
            }

            /// <summary>
            /// Gets the orientation of the slider.
            /// </summary>
            [Browsable(false)]
            public SliderOrientation Orientation
            {
                get
                {
                    return Container?.Orientation ?? SliderOrientation.Horizontal;
                }
            }

            /// <inheritdoc/>
            public override RectD Bounds
            {
                get
                {
                    return base.Bounds;
                }

                set
                {
                    base.Bounds = value;
                }
            }

            /// <inheritdoc/>
            protected override Coord GetDefaultWidth()
            {
                return DefaultSliderThumbWidth;
            }
        }
    }
}