using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// This class represents a label with plus and minus buttons
    /// which allow users to increment and decrement an integer value.
    /// </summary>
    public class IntPicker : TextBoxAndButton
    {
        /// <summary>
        /// Gets or sets whether to use char validator to limit unwanted chars in the input.
        /// Default is False.
        /// </summary>
        public static bool DefaultUseCharValidator = false;

        private int smallChange = 1;
        private int largeChange = 5;
        private int val = 0;
        private int textUpdateSuppressed;

        /// <summary>
        /// Initializes a new instance of the <see cref="IntPicker"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public IntPicker(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntPicker"/> class.
        /// </summary>
        public IntPicker()
        {
            TextBox.MinValue = 0;
            TextBox.MaxValue = 100;
            TextBox.Text = "0";
            Buttons.DoubleClickAsClick = false;
            IsBtnClickRepeated = true;
            HasBtnPlusMinus = true;
            HasBtnComboBox = false;
            UpdateButtonsEnabled();
            InnerOuterBorder = InnerOuterSelector.Outer;
            InitAsNumericEdit(
                NumericTypeCode.Int32,
                InitAsNumericEditParams.WithCharValidator(DefaultUseCharValidator));
        }

        /// <summary>
        /// Occurs when the <see cref="Value"/> property has been changed in some way.
        /// </summary>
        /// <remarks>For the <see cref="ValueChanged"/> event to occur, the
        /// <see cref="Value"/> property can be changed in code,
        /// by clicking the up or down button, or by the user entering a new value
        /// that is read by the control.</remarks>
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
        /// Gets or sets the value added to or subtracted from the <see cref="Value"/> property
        /// when the plus/minus buttons are pressed.
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
        /// when the plus/minus buttons are pressed while holding Ctrl key.
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
        /// Gets or sets the value assigned to control.
        /// </summary>
        /// <value>The numeric value of the <see cref="IntPicker"/>
        /// control.</value>
        /// <remarks>When the <see cref="Value"/> property is set, the new
        /// value is validated
        /// to be between the <see cref="Minimum"/> and <see cref="Maximum"/>
        /// values.</remarks>
        public virtual int Value
        {
            get
            {
                return val;
            }

            set
            {
                value = CoerceValue(value);
                if (this.val == value)
                    return;
                this.val = value;
                UpdateTextFromValue();
                RaiseValueChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the minimum allowed value for the control.
        /// </summary>
        /// <value>The minimum allowed value for the control.
        /// The default value is 0.</value>
        /// <remarks>
        /// The minimum and maximum values of the <see cref="Value"/> property are specified
        /// by the <see cref="Minimum"/> and <see cref="Maximum"/> properties.
        /// If the new <see cref="Minimum"/> property value is greater than the
        /// <see cref="Maximum"/> property value,
        /// the <see cref="Maximum"/> value is set equal to the
        /// <see cref="Minimum"/> value.
        /// If the <see cref="Value"/> is less than the new
        /// <see cref="Minimum"/> value, the <see cref="Value"/> property
        /// is also set equal to the <see cref="Minimum"/> value.
        /// </remarks>
        public virtual int Minimum
        {
            get
            {
                var result = TextBox.GetRealMinValue();

                if (result is null)
                    return default;

                return Convert.ToInt32(result);
            }

            set
            {
                if (value > Maximum)
                    value = Maximum;
                if (Minimum == value)
                    return;
                TextBox.MinValue = value;
                UpdateErrorText();
                if (Value < value)
                    Value = value;
                RaiseMinimumChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the maximum allowed value for the control.
        /// </summary>
        /// <value>The maximum allowed value for the control.
        /// The default value is 100.</value>
        /// <remarks>
        ///  If the <see cref="Minimum"/> property is greater than the new
        ///  <see cref="Maximum"/> property, the <see cref="Minimum"/>
        ///  property value is set equal to the <see cref="Maximum"/> value.
        ///  If the current <see cref="Value"/> is greater than the new
        ///  <see cref="Maximum"/> value, the <see cref="Value"/> property
        ///  value is set equal to the <see cref="Maximum"/> value.
        /// </remarks>
        public virtual int Maximum
        {
            get
            {
                var result = TextBox.GetRealMaxValue();

                if (result is null)
                    return 100;

                return Convert.ToInt32(result);
            }

            set
            {
                if (value < Minimum)
                    value = Minimum;
                if (Maximum == value)
                    return;
                TextBox.MaxValue = value;
                UpdateErrorText();
                if (Value > value)
                    Value = value;
                RaiseMaximumChanged(EventArgs.Empty);
            }
        }

        [Browsable(false)]
        internal new string Title
        {
            get => base.Title;
            set => base.Title = value;
        }

        /// <summary>
        /// Raises the <see cref="ValueChanged"/> event and calls
        /// <see cref="OnValueChanged(EventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
        public void RaiseValueChanged(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnValueChanged(e);
            ValueChanged?.Invoke(this, e);
            UpdateButtonsEnabled();
            Refresh();
            Update();
        }

        /// <summary>
        /// Increments or decrements value.
        /// </summary>
        /// <param name="incValue">Delta to add to the value.</param>
        public virtual void IncrementValue(int incValue = 1)
        {
            var newValue = Value + incValue;

            if (newValue < Minimum)
                Value = Minimum;
            else
            if (newValue > Maximum)
                Value = Maximum;
            else
                Value = newValue;
        }

        /// <inheritdoc/>
        public override void OnButtonClick(ControlAndButtonClickEventArgs e)
        {
            var change = Keyboard.IsControlPressed ? LargeChange : SmallChange;

            if (e.IsButtonMinus(this))
            {
                IncrementValue(-change);
            }
            else
            if (e.IsButtonPlus(this))
            {
                IncrementValue(change);
            }
        }

        /// <summary>
        /// Called when the value of the <see cref="Value"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
        protected virtual void OnValueChanged(EventArgs e)
        {
        }

        /// <inheritdoc/>
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            UpdateTextFromValue();
        }

        /// <summary>
        /// Called when the minimum of the <see cref="Minimum"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
        protected virtual void OnMinimumChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the maximum of the <see cref="Maximum"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
        protected virtual void OnMaximumChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Coerces value to fit in the allowed bounds.
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

        /// <summary>
        /// Updates the enabled state of the plus and minus buttons.
        /// </summary>
        protected virtual void UpdateButtonsEnabled()
        {
            if (DisposingOrDisposed)
                return;
            ButtonMinus?.SetEnabled(Value > Minimum);
            ButtonPlus?.SetEnabled(Value < Maximum);
        }

        /// <summary>
        /// Raises the <see cref="MinimumChanged"/> event and calls
        /// <see cref="OnMinimumChanged(EventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        protected void RaiseMinimumChanged(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnMinimumChanged(e);
            MinimumChanged?.Invoke(this, e);
            UpdateButtonsEnabled();
        }

        /// <summary>
        /// Raises the <see cref="MaximumChanged"/> event and calls
        /// <see cref="OnMaximumChanged(EventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        protected void RaiseMaximumChanged(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnMaximumChanged(e);
            MaximumChanged?.Invoke(this, e);
            UpdateButtonsEnabled();
        }

        /// <inheritdoc/>
        protected override void MainControlTextChanged()
        {
            base.MainControlTextChanged();
            UpdateValueFromText();
        }

        /// <summary>
        /// Updates <see cref="Value"/> with text entered in the inner
        /// <see cref="TextBox"/> control.
        /// </summary>
        protected virtual void UpdateValueFromText()
        {
            textUpdateSuppressed++;
            try
            {
                Value = TextBox.TextAsInt32;
            }
            finally
            {
                textUpdateSuppressed--;
            }
        }

        /// <summary>
        /// Sets appropriate error text using <see cref="Minimum"/>, <see cref="Maximum"/>
        /// and other properties.
        /// </summary>
        protected virtual void UpdateErrorText()
        {
            SetErrorText(
                NumericTypeCode.Int32,
                InitAsNumericEditParams.WithCharValidator(DefaultUseCharValidator));
        }

        /// <summary>
        /// Updates text of the inner <see cref="TextBox"/> control with the <see cref="Value"/>.
        /// </summary>
        protected virtual void UpdateTextFromValue()
        {
            if(textUpdateSuppressed > 0)
                return;
            TextBox.SetTextAsNumber(NumericTypeCode.Int32, Value);
        }
    }
}
