using System;
using System.ComponentModel;

using Alternet.Drawing;
using Alternet.UI.Localization;

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
    [DefaultProperty("Value")]
    [DefaultEvent("ValueChanged")]
    [DefaultBindingProperty("Value")]
    [ControlCategory("Common")]
    public partial class Slider : Control
    {
        private int maximum = 10;
        private int minimum = 0;
        private int value = 0;
        private int smallChange = 1;
        private int largeChange = 5;
        private int tickFrequency = 1;
        private SliderOrientation orientation;
        private SliderTickStyle tickStyle;

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
                if (orientation == value)
                    return;
                orientation = value;
                OrientationChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating how to display the tick marks on the slider.
        /// </summary>
        /// <value>
        /// One of the <see cref="SliderTickStyle"/> values. The default is
        /// <see cref="SliderTickStyle.BottomRight"/>.
        /// </value>
        public virtual SliderTickStyle TickStyle
        {
            get => tickStyle;
            set
            {
                if (tickStyle == value)
                    return;
                tickStyle = value;
                TickStyleChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets a numeric value that represents the current position of the scroll box
        /// on the slider.
        /// </summary>
        /// <value>A numeric value that is within the <see cref="Minimum"/> and
        /// <see cref="Maximum"/> range. The default value is 0.</value>
        /// <remarks>The <see cref="Value"/> property contains the number that represents
        /// the current position of the scroll box on the slider.</remarks>
        public virtual int Value
        {
            get
            {
                return value;
            }

            set
            {
                value = CoerceValue(value);
                if (this.value == value)
                    return;
                this.value = value;
                RaiseValueChanged();
            }
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
        public virtual int Minimum
        {
            get
            {
                return minimum;
            }

            set
            {
                if (minimum == value)
                    return;
                minimum = value;
                RaiseMinimumChanged(EventArgs.Empty);
                Maximum = maximum;
            }
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
        public virtual int Maximum
        {
            get
            {
                return maximum;
            }

            set
            {
                value = CoerceMaximum(value);
                if (maximum == value)
                    return;
                maximum = value;
                RaiseMaximumChanged(EventArgs.Empty);
                Value = value;
            }
        }

        /// <summary>
        /// Gets or sets the value added to or subtracted from the <see cref="Value"/> property
        /// when the scroll box is moved a small distance.
        /// </summary>
        /// <value>A numeric value. The default value is 1.</value>
        /// <exception cref="ArgumentException">The assigned value is less than 0.</exception>
        public virtual int SmallChange
        {
            get
            {
                return smallChange;
            }

            set
            {
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
        /// when the scroll box is moved a large distance.
        /// </summary>
        /// <value>A numeric value. The default is 5.</value>
        /// <exception cref="ArgumentException">The assigned value is less than 0.</exception>
        public virtual int LargeChange
        {
            get
            {
                return largeChange;
            }

            set
            {
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
                if (tickFrequency == value)
                    return;
                tickFrequency = value;
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

        internal new ISliderHandler Handler => (ISliderHandler)base.Handler;

        /// <summary>
        /// Clears the ticks.
        /// </summary>
        /// <remarks>
        /// Availability: only available for the Windows, Linux ports.
        /// </remarks>
        public virtual void ClearTicks()
        {
            Handler.ClearTicks();
        }

        /// <summary>
        /// Raises the <see cref="ValueChanged"/> event and calls
        /// <see cref="OnValueChanged(EventArgs)"/>.
        /// </summary>
        public void RaiseValueChanged()
        {
            Designer?.RaisePropertyChanged(this, nameof(Value));
            OnValueChanged(EventArgs.Empty);
            ValueChanged?.Invoke(this, EventArgs.Empty);
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
        protected override IControlHandler CreateHandler()
        {
            return ControlFactory.Handler.CreateSliderHandler(this);
        }

        /// <summary>
        /// Raises the <see cref="MinimumChanged"/> event and calls
        /// <see cref="OnMinimumChanged(EventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        private void RaiseMinimumChanged(EventArgs e)
        {
            OnMinimumChanged(e);
            MinimumChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="MaximumChanged"/> event and calls
        /// <see cref="OnMaximumChanged(EventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        private void RaiseMaximumChanged(EventArgs e)
        {
            OnMaximumChanged(e);
            MaximumChanged?.Invoke(this, e);
        }

        private int CoerceValue(int value)
        {
            if (value < Minimum)
                return Minimum;

            if (value > Maximum)
                return Maximum;

            return value;
        }

        private int CoerceMaximum(int value)
        {
            int min = Minimum;
            if (value < min)
                return min;
            return value;
        }
    }
}