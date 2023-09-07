using System;
using System.ComponentModel;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a slider control (also known as track bar).
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="Slider"/> is a scrollable control similar to the scroll bar control.
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
    public class Slider : Control
    {
        /// <summary>
        /// Identifies the <see cref="Value"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                    "Value",
                    typeof(int),
                    typeof(Slider),
                    new FrameworkPropertyMetadata(
                            0,
                            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsPaint,
                            new PropertyChangedCallback(OnValuePropertyChanged),
                            new CoerceValueCallback(CoerceValue),
                            true,
                            UpdateSourceTrigger.PropertyChanged));

        /// <summary>
        /// Identifies the <see cref="Minimum"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register(
                    "Minimum",
                    typeof(int),
                    typeof(Slider),
                    new FrameworkPropertyMetadata(
                            0,
                            FrameworkPropertyMetadataOptions.AffectsPaint,
                            new PropertyChangedCallback(OnMinimumPropertyChanged),
                            null,
                            true,
                            UpdateSourceTrigger.PropertyChanged));

        /// <summary>
        /// Identifies the <see cref="Maximum"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register(
                    "Maximum",
                    typeof(int),
                    typeof(Slider),
                    new FrameworkPropertyMetadata(
                            10,
                            FrameworkPropertyMetadataOptions.AffectsPaint,
                            new PropertyChangedCallback(OnMaximumPropertyChanged),
                            CoerceMaximum,
                            true,
                            UpdateSourceTrigger.PropertyChanged));

        private int smallChange = 1;
        private int largeChange = 5;
        private int tickFrequency = 1;

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
        /// Gets a <see cref="SliderHandler"/> associated with this class.
        /// </summary>
        [Browsable(false)]
        public new SliderHandler Handler
        {
            get
            {
                CheckDisposed();
                return (SliderHandler)base.Handler;
            }
        }

        /// <inheritdoc/>
        public override ControlId ControlKind => ControlId.Slider;

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
        public SliderOrientation Orientation
        {
            get => Handler.Orientation;
            set => Handler.Orientation = value;
        }

        /// <summary>
        /// Gets or sets a value indicating how to display the tick marks on the slider.
        /// </summary>
        /// <value>
        /// One of the <see cref="SliderTickStyle"/> values. The default is
        /// <see cref="SliderTickStyle.BottomRight"/>.
        /// </value>
        public SliderTickStyle TickStyle
        {
            get => Handler.TickStyle;
            set => Handler.TickStyle = value;
        }

        /// <summary>
        /// Gets or sets a numeric value that represents the current position of the scroll box
        /// on the slider.
        /// </summary>
        /// <value>A numeric value that is within the <see cref="Minimum"/> and
        /// <see cref="Maximum"/> range. The default value is 0.</value>
        /// <remarks>The <see cref="Value"/> property contains the number that represents
        /// the current position of the scroll box on the slider.</remarks>
        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Gets or sets the lower limit of the range this <see cref="Slider"/> is working with.
        /// </summary>
        /// <value>The minimum value for the <see cref="Slider"/>. The default is 0.</value>
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
        public int Minimum
        {
            get { return (int)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        /// <summary>
        /// Gets or sets the upper limit of the range this <see cref="Slider"/> is working with.
        /// </summary>
        /// <value>The maximum value for the <see cref="Slider"/>. The default is 10.</value>
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
        public int Maximum
        {
            get { return (int)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value added to or subtracted from the <see cref="Value"/> property
        /// when the scroll box is moved a small distance.
        /// </summary>
        /// <value>A numeric value. The default value is 1.</value>
        /// <exception cref="ArgumentException">The assigned value is less than 0.</exception>
        public int SmallChange
        {
            get
            {
                CheckDisposed();
                return smallChange;
            }

            set
            {
                CheckDisposed();

                if (value < 0)
                    throw new ArgumentException(nameof(SmallChange));

                if (smallChange == value)
                    return;

                smallChange = value;
                SmallChangeChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets a value to be added to or subtracted from the <see cref="Value"/> property
        /// when the scroll box is moved a large distance.
        /// </summary>
        /// <value>A numeric value. The default is 5.</value>
        /// <exception cref="ArgumentException">The assigned value is less than 0.</exception>
        public int LargeChange
        {
            get
            {
                CheckDisposed();
                return largeChange;
            }

            set
            {
                CheckDisposed();

                if (value < 0)
                {
                    throw new ArgumentException(
                        ErrorMessages.Default.InvalidParameter, nameof(LargeChange));
                }

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
        public int TickFrequency
        {
            get
            {
                CheckDisposed();
                return tickFrequency;
            }

            set
            {
                CheckDisposed();
                if (tickFrequency == value)
                    return;

                tickFrequency = value;
                TickFrequencyChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the <see cref="ValueChanged"/> event and calls
        /// <see cref="OnValueChanged(EventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        public void RaiseValueChanged(EventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            OnValueChanged(e);
            ValueChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Called when the value of the <see cref="Value"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnValueChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Callback for changes to the Value property
        /// </summary>
        private static void OnValuePropertyChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            Slider control = (Slider)d;
            control.OnValuePropertyChanged((int)e.OldValue, (int)e.NewValue);
        }

        private static object CoerceValue(DependencyObject d, object value)
        {
            var o = (Slider)d;

            var intValue = (int)value;
            if (intValue < o.Minimum)
                return o.Minimum;

            if (intValue > o.Maximum)
                return o.Maximum;

            return value;
        }

#pragma warning disable
        private void OnValuePropertyChanged(int oldValue, int newValue)
#pragma warning enable
        {
            RaiseValueChanged(EventArgs.Empty);
            Designer?.PropertyChanged(this, nameof(Value));
        }

        /// <summary>
        /// Called when the minimum of the <see cref="Minimum"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnMinimumChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Callback for changes to the Minimum property
        /// </summary>
        private static void OnMinimumPropertyChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            Slider control = (Slider)d;
            control.OnMinimumPropertyChanged((int)e.OldValue, (int)e.NewValue);
        }

        /// <summary>
        /// Raises the <see cref="MinimumChanged"/> event and calls
        /// <see cref="OnMinimumChanged(EventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        void RaiseMinimumChanged(EventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            OnMinimumChanged(e);
            MinimumChanged?.Invoke(this, e);
        }

        private void OnMinimumPropertyChanged(int oldValue, int newValue)
        {
            RaiseMinimumChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Called when the maximum of the <see cref="Maximum"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnMaximumChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Callback for changes to the Maximum property
        /// </summary>
        private static void OnMaximumPropertyChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            Slider control = (Slider)d;
            control.OnMaximumPropertyChanged((int)e.OldValue, (int)e.NewValue);
        }

        /// <summary>
        /// Raises the <see cref="MaximumChanged"/> event and calls
        /// <see cref="OnMaximumChanged(EventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        private void RaiseMaximumChanged(EventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            OnMaximumChanged(e);
            MaximumChanged?.Invoke(this, e);
        }

        private void OnMaximumPropertyChanged(int oldValue, int newValue)
        {
            RaiseMaximumChanged(EventArgs.Empty);
        }

        /// <inheritdoc/>
        protected override ControlHandler CreateHandler()
        {
            return GetEffectiveControlHandlerHactory().CreateSliderHandler(this);
        }

        private static object CoerceMaximum(DependencyObject d, object value)
        {
            var o = (Slider)d;

            int min = o.Minimum;
            if ((int)value < min)
            {
                return min;
            }

            return value;
        }
    }
}