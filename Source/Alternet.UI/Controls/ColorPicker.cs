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
                    "Value", // Property name
                    typeof(Color), // Property type
                    typeof(ColorPicker), // Property owner
                    new FrameworkPropertyMetadata(
                            Color.Black, // default value
                            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsPaint,
                            new PropertyChangedCallback(OnValuePropertyChanged),    // property changed callback
                            null, // CoerseValueCallback
                            true, // IsAnimationProhibited
                            UpdateSourceTrigger.PropertyChanged
                            // UpdateSourceTrigger.LostFocus   // DefaultUpdateSourceTrigger
                            ));

        /// <summary>
        /// Occurs when the <see cref="Value"/> property has been changed in some way.
        /// </summary>
        /// <remarks>For the <see cref="ValueChanged"/> event to occur, the <see cref="Value"/> property can be changed in code,
        /// by clicking the up or down button, or by the user entering a new value that is read by the control.</remarks>
        public event EventHandler? ValueChanged;

        /// <summary>
        /// Gets or sets the value assigned to the color picker as a selected color.
        /// </summary>
        public Color Value
        {
            get { return (Color)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Raises the <see cref="ValueChanged"/> event and calls <see cref="OnValueChanged(EventArgs)"/>.
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
        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColorPicker control = (ColorPicker)d;
            control.OnValuePropertyChanged((Color)e.OldValue, (Color)e.NewValue);
        }

        private void OnValuePropertyChanged(Color oldValue, Color newValue)
        {
            RaiseValueChanged(EventArgs.Empty);
        }
    }
}