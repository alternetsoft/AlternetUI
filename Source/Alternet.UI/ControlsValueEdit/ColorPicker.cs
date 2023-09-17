using System;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents control that displays a selected color and allows to change it.
    /// </summary>
    public class ColorPicker : Control
    {
        /// <summary>
        /// Identifies the <see cref="Value"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                    "Value",
                    typeof(Color),
                    typeof(ColorPicker),
                    new FrameworkPropertyMetadata(
                            Color.Black,
                            PropMetadataOption.BindsTwoWayByDefault | PropMetadataOption.AffectsPaint,
                            new PropertyChangedCallback(OnValuePropertyChanged),
                            null,
                            isAnimationProhibited: true,
                            UpdateSourceTrigger.PropertyChanged));

        /// <summary>
        /// Occurs when the <see cref="Value"/> property has been changed in some way.
        /// </summary>
        /// <remarks>For the <see cref="ValueChanged"/> event to occur, the
        /// <see cref="Value"/> property can be changed in code,
        /// by clicking the up or down button, or by the user entering a new
        /// value that is read by the control.</remarks>
        public event EventHandler? ValueChanged;

        /// <inheritdoc/>
        public override ControlId ControlKind => ControlId.ColorPicker;

        /// <summary>
        /// Gets or sets the value assigned to the color picker as a selected color.
        /// </summary>
        public Color Value
        {
            get
            {
                return (Color)GetValue(ValueProperty);
            }

            set
            {
                SetValue(ValueProperty, value);
            }
        }

        /// <summary>
        /// Raises the <see cref="ValueChanged"/> event and calls
        /// <see cref="OnValueChanged(EventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        public void RaiseValueChanged(EventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            OnValueChanged(e);
            ValueChanged?.Invoke(this, e);
        }

        /// <inheritdoc/>
        protected override ControlHandler CreateHandler()
        {
            return GetEffectiveControlHandlerHactory().
                CreateColorPickerHandler(this);
        }

        /// <summary>
        /// Called when the value of the <see cref="Value"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
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
            ColorPicker control = (ColorPicker)d;
            control.OnValuePropertyChanged((Color)e.OldValue, (Color)e.NewValue);
        }

#pragma warning disable IDE0060 // Remove unused parameter
        private void OnValuePropertyChanged(Color oldValue, Color newValue)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            RaiseValueChanged(EventArgs.Empty);
        }
    }
}