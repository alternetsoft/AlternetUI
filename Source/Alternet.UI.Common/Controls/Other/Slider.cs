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
    public partial class Slider : Border
    {
        /// <summary>
        /// Represents the default size of the left/top and right/bottom indicators.
        /// </summary>
        public static Coord DefaultIndicatorSize = 6;

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

        /// <summary>
        /// Gets or sets a default value of the
        /// <see cref="AbstractControl.ParentBackColor"/> property.
        /// </summary>
        public static bool? DefaultParentBackColor;

        /// <summary>
        /// Gets or sets a default value of the
        /// <see cref="AbstractControl.ParentForeColor"/> property.
        /// </summary>
        public static bool? DefaultParentForeColor;

        private readonly AbstractControl leftTopSpacer;
        private readonly AbstractControl leftTopIndicator;
        private readonly AbstractControl rightBottomIndicator;
        private readonly SliderThumb thumb;

        private int maximum = 10;
        private int minimum = 0;
        private int val = 0;
        private int smallChange = 1;
        private int largeChange = 5;
        private int tickFrequency = 1;
        private SliderOrientation orientation;
        private SliderTickStyle tickStyle;

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
            leftTopIndicator = CreateIndicator(isLeftTop: true);
            leftTopIndicator.Dock = DockStyle.Top;
            leftTopIndicator.MinimumSize = DefaultIndicatorSize;
            rightBottomIndicator = CreateIndicator(isLeftTop: false);
            rightBottomIndicator.Dock = DockStyle.Bottom;
            rightBottomIndicator.MinimumSize = DefaultIndicatorSize;

            leftTopSpacer = CreateSpacer();
            leftTopSpacer.ParentBackColor = true;
            leftTopSpacer.Dock = DockStyle.Left;

            thumb = CreateSliderThumb();

            tickStyle = DefaultTickStyle;
            Padding = 1;
            thumb.Margin = 1;
            thumb.MinSize = 0;
            thumb.MinExtra = 2;
            thumb.SizeDelta = 0;

            MinimumSize = DefaultSliderMinimumSize;
            SuggestedSize = (200, Coord.NaN);

            if (DefaultParentBackColor is not null)
                ParentBackColor = DefaultParentBackColor.Value;
            if (DefaultParentForeColor is not null)
                ParentForeColor = DefaultParentForeColor.Value;

            UseControlColors(true);

            Layout = LayoutStyle.Dock;
            leftTopSpacer.Width = 0;

            UpdateIndicatorVisibility();

            thumb.Parent = this;
            leftTopSpacer.Parent = this;
            leftTopIndicator.Parent = this;
            rightBottomIndicator.Parent = this;

            thumb.SplitterMoved += OnThumbSplitterMoved;
            thumb.SplitterMoving += OnThumbSplitterMoving;
            leftTopSpacer.MouseLeftButtonDown += OnLeftTopSpacerMouseDown;
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

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.Slider;

        /// <summary>
        /// Gets the thumb control of the slider.
        /// </summary>
        [Browsable(false)]
        public SliderThumb ThumbControl => thumb;

        /// <summary>
        /// Gets the left/top spacer control of the slider.
        /// </summary>
        [Browsable(false)]
        public AbstractControl LeftTopSpacer => leftTopSpacer;

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
                    result = ClientSize.Width - ThumbControl.Width
                    - ThumbControl.Margin.Horizontal - Padding.Horizontal;
                }
                else
                {
                    result = ClientSize.Height - ThumbControl.Height
                    - ThumbControl.Margin.Vertical - Padding.Vertical;
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
                    if (SuggestedSize.IsNanWidthOrHeight)
                        SuggestedSize = SuggestedSize.WithSwappedWidthAndHeight();

                    var isHorizontal = value == SliderOrientation.Horizontal;
                    var dockStyle = isHorizontal ? DockStyle.Left : DockStyle.Top;
                    LeftTopSpacer.Dock = dockStyle;
                    ThumbControl.Dock = dockStyle;

                    if (isHorizontal)
                    {
                        leftTopIndicator.Dock = DockStyle.Top;
                        rightBottomIndicator.Dock = DockStyle.Bottom;
                    }
                    else
                    {
                        leftTopIndicator.Dock = DockStyle.Left;
                        rightBottomIndicator.Dock = DockStyle.Right;
                    }
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
                DoInsideLayout(() =>
                {
                    UpdateIndicatorVisibility();
                });

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
        internal new Color? BackgroundColor
        {
            get => base.BackgroundColor;
            set => base.BackgroundColor = value;
        }

        [Browsable(false)]
        internal new Color? ForegroundColor
        {
            get => base.ForegroundColor;
            set => base.ForegroundColor = value;
        }

        [Browsable(false)]
        internal new Thickness? MinChildMargin
        {
            get => base.MinChildMargin;
            set => base.MinChildMargin = value;
        }

        [Browsable(false)]
        internal new Thickness Padding
        {
            get => base.Padding;
            set => base.Padding = value;
        }

        [Browsable(false)]
        internal new bool ParentForeColor
        {
            get => base.ParentForeColor;
            set => base.ParentForeColor = value;
        }

        [Browsable(false)]
        internal new bool ParentBackColor
        {
            get => base.ParentBackColor;
            set => base.ParentBackColor = value;
        }

        [Browsable(false)]
        internal new LayoutStyle? Layout
        {
            get => base.Layout;
            set => base.Layout = value;
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
        }

        /// <inheritdoc/>
        public override SizeD GetPreferredSize(SizeD availableSize)
        {
            Coord WidthFromParent()
            {
                if (Parent is not null)
                    return Parent.ClientSize.Width - Parent.Padding.Horizontal - Margin.Horizontal;
                return 150;
            }

            Coord HeightFromParent()
            {
                if (Parent is not null)
                    return Parent.ClientSize.Height - Parent.Padding.Vertical - Margin.Vertical;
                return 150;
            }

            Coord? minimumHeight = null;

            Coord GetMinimumHeight()
            {
                minimumHeight ??= MeasureCanvas.GetTextExtent("Wg", RealFont).Height;
                return minimumHeight.Value;
            }

            var specifiedWidth = SuggestedWidth;
            var specifiedHeight = SuggestedHeight;

            if (IsHorizontal)
            {
                if (Coord.IsNaN(specifiedWidth))
                {
                    if (CoordUtils.IsInfinityOrNanOrMax(availableSize.Width))
                        specifiedWidth = WidthFromParent();
                    else
                        specifiedWidth = Math.Min(WidthFromParent(), availableSize.Width);
                }

                if (Coord.IsNaN(specifiedHeight))
                {
                    specifiedHeight = MathUtils.Max(
                        MinimumSize.Height,
                        DefaultSliderMinimumSize,
                        GetMinimumHeight() + Padding.Vertical + 1);

                    if (leftTopIndicator.IsVisible)
                    {
                        specifiedHeight += leftTopIndicator.Height;
                    }

                    if (rightBottomIndicator.IsVisible)
                    {
                        specifiedHeight += rightBottomIndicator.Height;
                    }
                }
            }
            else
            {
                if (Coord.IsNaN(specifiedWidth))
                {
                    specifiedWidth = Math.Max(MinimumSize.Width, DefaultSliderMinimumSize);

                    if (leftTopIndicator.IsVisible)
                    {
                        specifiedWidth += leftTopIndicator.Width;
                    }

                    if (rightBottomIndicator.IsVisible)
                    {
                        specifiedWidth += rightBottomIndicator.Width;
                    }
                }

                if (Coord.IsNaN(specifiedHeight))
                {
                    if (CoordUtils.IsInfinityOrNanOrMax(availableSize.Height))
                        specifiedHeight = HeightFromParent();
                    else
                        specifiedHeight = Math.Min(HeightFromParent(), availableSize.Height);
                }
            }

            return (specifiedWidth, specifiedHeight);
        }

        /// <summary>
        /// Sets the colors of the left/top and right/bottom spacers.
        /// </summary>
        /// <param name="leftTopSpacerColor">The color of the left/top spacer.
        /// If <c>null</c>, the left/top spacer will use the parent's background color.</param>
        public virtual void SetSpacerColor(Color? leftTopSpacerColor)
        {
            leftTopSpacer.ParentBackColor = leftTopSpacerColor is null;
            leftTopSpacer.BackgroundColor = leftTopSpacerColor;
            leftTopSpacer.Update();
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

            if (Value <= Minimum)
                LeftTopSpacerSize = 0;
            else
            if (Value >= Maximum)
                LeftTopSpacerSize = MaxLeftTopSpacerSize;
            else
            {
                var v = Value - Minimum;
                var maxV = Maximum - Minimum;

                var newSpacerSize = (v * MaxLeftTopSpacerSize) / maxV;

                LeftTopSpacerSize = Math.Min(Math.Max(0, newSpacerSize), MaxLeftTopSpacerSize);
            }
        }

        /// <summary>
        /// Coerces minimal value the have the valid range.
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
                Value -= LargeChange;
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
        /// Creates an indicator control for the slider.
        /// </summary>
        /// <param name="isLeftTop">Whether to create a red or green indicator
        /// based on the position.</param>
        /// <returns>A new instance of <see cref="AbstractControl"/>
        /// representing the indicator.</returns>
        protected virtual AbstractControl CreateIndicator(bool isLeftTop)
        {
            var indicator = new Spacer();
            return indicator;
        }

        /// <summary>
        /// Creates a spacer control for the slider.
        /// </summary>
        /// <returns>A new spacer control instance.</returns>
        protected virtual AbstractControl CreateSpacer()
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
        /// Updates the visibility of the indicators based on the current
        /// state of the <see cref="TickStyle"/> property.
        /// </summary>
        protected virtual void UpdateIndicatorVisibility()
        {
            switch (TickStyle)
            {
                case SliderTickStyle.None:
                    leftTopIndicator.IsVisible = false;
                    rightBottomIndicator.IsVisible = false;
                    break;
                case SliderTickStyle.Both:
                    leftTopIndicator.IsVisible = true;
                    rightBottomIndicator.IsVisible = true;
                    break;
                case SliderTickStyle.TopLeft:
                    leftTopIndicator.IsVisible = true;
                    rightBottomIndicator.IsVisible = false;
                    break;
                case SliderTickStyle.BottomRight:
                    leftTopIndicator.IsVisible = false;
                    rightBottomIndicator.IsVisible = true;
                    break;
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
                HasBorder = true;
            }

            /// <inheritdoc/>
            protected override Coord GetDefaultWidth()
            {
                return DefaultSliderThumbWidth;
            }
        }
    }
}