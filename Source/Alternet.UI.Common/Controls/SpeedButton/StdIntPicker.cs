using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a text editor with spinner buttons that allow the user to increment or decrement the value.
    /// </summary>
    public class StdIntPicker : ToolBar
    {
        /// <summary>
        /// Gets or sets default image for the 'Plus' button.
        /// </summary>
        public static KnownButton DefaultBtnPlusImage = UI.KnownButton.TextBoxPlus;

        /// <summary>
        /// Gets or sets default image for the 'Minus' button.
        /// </summary>
        public static KnownButton DefaultBtnMinusImage = UI.KnownButton.TextBoxMinus;

        /// <summary>
        /// Gets or sets whether minus button is shown before plus button. Default value is True.
        /// </summary>
        /// <remarks>
        /// This property affects all <see cref="StdIntPicker"/> descendants created after
        /// it was changed.
        /// </remarks>
        public static bool IsMinusButtonFirst = true;

        private readonly TextPicker textPicker = new ();

        private int smallChange = 1;
        private int largeChange = 5;
        private int val = 0;
        private int textUpdateSuppressed;
        private int minValue = 0;
        private int maxValue = 100;

        private ObjectUniqueId? idButtonPlus;
        private ObjectUniqueId? idButtonMinus;

        /// <summary>
        /// Initializes a new instance of the <see cref="StdIntPicker"/> class.
        /// </summary>
        public StdIntPicker()
        {
            textPicker.IsEditable = true;
            textPicker.MinWidth = 100;
            textPicker.ValueChanged += OnTextPickerValueChanged;
            textPicker.HorizontalAlignment = HorizontalAlignment.Fill;
            AddControl(textPicker);
            DoubleClickAsClick = false;
            IsBtnClickRepeated = true;
            HasBtnPlusMinus = true;
            UpdateTextFromValue();
            UpdateButtonsEnabled();
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
        /// Occurs when button is clicked.
        /// </summary>
        public event EventHandler<ControlAndButtonClickEventArgs>? ButtonClick;

        /// <summary>
        /// Gets or sets action that is called when button is clicked.
        /// This is a simplified alternative to <see cref="ButtonClick"/> event.
        /// </summary>
        public Action<ControlAndButtonClickEventArgs>? ButtonClickAction;

        /// <summary>
        /// Gets 'Plus' button if it is available.
        /// </summary>
        [Browsable(false)]
        public SpeedButton ButtonPlus => FindTool(IdButtonPlus)!;

        /// <summary>
        /// Gets 'Minus' button if it is available.
        /// </summary>
        [Browsable(false)]
        public SpeedButton ButtonMinus => FindTool(IdButtonMinus)!;

        /// <summary>
        /// Gets <see cref="TextPicker"/> control used to display and edit the value.
        /// </summary>
        [Browsable(false)]
        public TextPicker TextPicker => textPicker;

        /// <summary>
        /// Gets or sets 'Plus' button image as <see cref="SvgImage"/>.
        /// Default is null, which means that <see cref="BtnPlusImage"/> is used.
        /// </summary>
        [Browsable(false)]
        public virtual SvgImage? BtnPlusImageSvg { get; set; }

        /// <summary>
        /// Gets or sets 'Plus' button image as <see cref="UI.KnownButton"/>.
        /// Default is null, which means that <see cref="DefaultBtnPlusImage"/> is used.
        /// </summary>
        [Browsable(false)]
        public virtual KnownButton? BtnPlusImage { get; set; }

        /// <summary>
        /// Gets or sets 'Minus' button image as <see cref="SvgImage"/>.
        /// Default is null, which means that <see cref="BtnMinusImage"/> is used.
        /// </summary>
        [Browsable(false)]
        public virtual SvgImage? BtnMinusImageSvg { get; set; }

        /// <summary>
        /// Gets or sets 'Minus' button image as <see cref="UI.KnownButton"/>.
        /// Default is null, which means that <see cref="DefaultBtnMinusImage"/> is used.
        /// </summary>
        [Browsable(false)]
        public virtual KnownButton? BtnMinusImage { get; set; }

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
        /// Gets id of the plus button.
        /// </summary>
        [Browsable(false)]
        public ObjectUniqueId? IdButtonPlus => idButtonPlus;

        /// <summary>
        /// Gets id of the minus button.
        /// </summary>
        [Browsable(false)]
        public ObjectUniqueId? IdButtonMinus => idButtonMinus;

        /// <summary>
        /// Gets or sets whether 'Plus' and 'Minus' buttons are visible.
        /// </summary>
        public virtual bool HasBtnPlusMinus
        {
            get
            {
                return HasButton(IdButtonPlus) && HasButton(IdButtonMinus);
            }

            set
            {
                if (IsMinusButtonFirst)
                {
                    ToggleMinusButton();
                    TogglePlusButton();
                }
                else
                {
                    TogglePlusButton();
                    ToggleMinusButton();
                }

                void TogglePlusButton()
                {
                    OverrideButtonType(GetBtnPlusMinusType(), () =>
                    {
                        SetHasButton(
                            BtnPlusImage ?? DefaultBtnPlusImage,
                            ref idButtonPlus,
                            value,
                            BtnPlusImageSvg);
                    });
                }

                void ToggleMinusButton()
                {
                    OverrideButtonType(GetBtnPlusMinusType(), () =>
                    {
                        SetHasButton(
                            BtnMinusImage ?? DefaultBtnMinusImage,
                            ref idButtonMinus,
                            value,
                            BtnMinusImageSvg);
                    });
                }
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
        /// Gets or sets 'IsClickRepeated' property of the buttons.
        /// </summary>
        public virtual bool IsBtnClickRepeated
        {
            get
            {
                return GetToolCount() > 0 && IsToolClickRepeated;
            }

            set
            {
                IsToolClickRepeated = value;
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
                var result = minValue;
                return result;
            }

            set
            {
                if (value > Maximum)
                    value = Maximum;
                if (Minimum == value)
                    return;
                minValue = value;
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
                var result = maxValue;
                return result;
            }

            set
            {
                if (value < Minimum)
                    value = Minimum;
                if (Maximum == value)
                    return;
                maxValue = value;
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

        /// <summary>
        /// Increments or decrements value by <see cref="SmallChange"/> or <see cref="LargeChange"/>
        /// depending on whether the Ctrl key is pressed.
        /// </summary>
        /// <param name="e">An <see cref="ControlAndButtonClickEventArgs"/> that contains the event data.</param>
        public virtual void OnButtonClick(ControlAndButtonClickEventArgs e)
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
        /// Gets whether button with the specified id is visible.
        /// </summary>
        /// <param name="id">Button id.</param>
        /// <returns></returns>
        public virtual bool HasButton(ObjectUniqueId? id)
        {
            if (id is null)
                return false;
            var result = GetToolVisible(id.Value);
            return result;
        }

        /// <summary>
        /// Sets the SVG image and/or known image for the 'Minus' button.
        /// </summary>
        /// <param name="svg">The SVG image for the 'Minus' button.</param>
        /// <param name="knownImage">The known image for the 'Minus' button.</param>
        public virtual void SetMinusImage(SvgImage? svg, KnownButton? knownImage)
        {
            BtnMinusImage = knownImage;
            BtnMinusImageSvg = svg;

            if (IdButtonMinus is not null)
            {
                if (svg is not null)
                {
                    SetToolSvg(IdButtonMinus.Value, BtnMinusImageSvg);
                }
                else
                {
                    SetToolImage(IdButtonMinus.Value, BtnMinusImage ?? DefaultBtnMinusImage);
                }
            }
        }

        /// <summary>
        /// Raises <see cref="ButtonClick"/> event and calls <see cref="OnButtonClick"/> method.
        /// </summary>
        public void RaiseButtonClick(ControlAndButtonClickEventArgs e)
        {
            ButtonClick?.Invoke(this, e);
            ButtonClickAction?.Invoke(e);
            if (e.Handled)
                return;
            OnButtonClick(e);
        }

        /// <summary>
        /// Sets the SVG image and/or known image for the 'Plus' button.
        /// </summary>
        /// <param name="svg">The SVG image for the 'Plus' button.</param>
        /// <param name="knownImage">The known image for the 'Plus' button.</param>
        public virtual void SetPlusImage(SvgImage? svg, KnownButton? knownImage)
        {
            BtnPlusImage = knownImage;
            BtnPlusImageSvg = svg;

            if (IdButtonPlus is not null)
            {
                if (svg is not null)
                {
                    SetToolSvg(IdButtonPlus.Value, BtnPlusImageSvg);
                }
                else
                {
                    SetToolImage(
                    IdButtonPlus.Value,
                    BtnPlusImage ?? DefaultBtnPlusImage);
                }
            }
        }

        /// <summary>
        /// Sets images for the plus and minus buttons.
        /// </summary>
        /// <param name="plusImage">The image for the plus button.</param>
        /// <param name="minusImage">The image for the minus button.</param>
        public virtual void SetPlusMinusImages(
            SvgImage? plusImage,
            SvgImage? minusImage)
        {
            SetMinusImage(minusImage, null);
            SetPlusImage(plusImage, null);
        }

        /// <summary>
        /// Sets images for the plus and minus buttons.
        /// </summary>
        /// <param name="plusImage">The image for the plus button.</param>
        /// <param name="minusImage">The image for the minus button.</param>
        public virtual void SetPlusMinusImages(
            KnownButton? plusImage,
            KnownButton? minusImage)
        {
            SetMinusImage(null, minusImage);
            SetPlusImage(null, plusImage);
        }

        /// <summary>
        /// Sets whether button with the specified id is visible.
        /// </summary>
        /// <param name="btn">Known button.</param>
        /// <param name="svg">SVG image for the button.</param>
        /// <param name="id">Id of the created button.</param>
        /// <param name="value">Whether button is visible.</param>
        private void SetHasButton(
            KnownButton btn,
            ref ObjectUniqueId? id,
            bool value,
            SvgImage? svg = null)
        {
            if (HasButton(id) == value)
            {
                return;
            }

            if (value)
            {
                if (id is null)
                {
                    var isClickRepeat = IsBtnClickRepeated;

                    void ClickAction(object? s, EventArgs e)
                    {
                        ControlAndButtonClickEventArgs args = new();
                        args.ButtonId = (s as AbstractControl)?.UniqueId;
                        RaiseButtonClick(args);
                    }

                    id = AddSpeedBtn(btn, svg, ClickAction);

                    SetToolAlignRight(id.Value, true);

                    if (isClickRepeat)
                    {
                        SetToolIsClickRepeated(id.Value, true);
                    }
                }
                else
                {
                    SetToolVisible(id.Value, true);
                }
            }
            else
            {
                if (id is null)
                    return;
                SetToolVisible(id.Value, false);
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

        /// <summary>
        /// Called when the value is changed in the text picker.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnTextPickerValueChanged(object? sender, EventArgs e)
        {
            Text = TextPicker.Text;
            UpdateValueFromText();
        }

        /// <summary>
        /// Updates <see cref="Value"/> with the text.
        /// </summary>
        protected virtual void UpdateValueFromText()
        {
            textUpdateSuppressed++;
            try
            {
                if(int.TryParse(Text, out var newValue))
                {
                    Value = newValue;
                }
            }
            finally
            {
                textUpdateSuppressed--;
            }
        }

        /// <summary>
        /// Gets type of the <see cref="SpeedButton"/> ancestor class used
        /// as 'Plus' and 'Minus' buttons.
        /// </summary>
        /// <returns></returns>
        protected virtual Type GetBtnPlusMinusType()
        {
            return typeof(SpeedButton);
        }

        /// <summary>
        /// Updates text with the <see cref="Value"/>.
        /// </summary>
        protected virtual void UpdateTextFromValue()
        {
            if (textUpdateSuppressed > 0)
                return;
            var newText = Value.ToString();
            Text = newText;
            textPicker.Value = newText;
        }
    }
}
