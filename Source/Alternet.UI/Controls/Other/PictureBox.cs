using Alternet.Drawing;
using System;
using System.ComponentModel;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a picture box control for displaying an image.
    /// </summary>
    /// <remarks>
    /// Set the <see cref="Image"/> property to the Image you want to display.
    /// </remarks>
    [DefaultProperty("Image")]
    [DefaultBindingProperty("Image")]
    public class PictureBox : Control
    {
        /// <summary>
        /// Identifies the <see cref="Image"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register(
                    "Image",
                    typeof(Image),
                    typeof(PictureBox),
                    new FrameworkPropertyMetadata(
                            null,
                            PropMetadataOption.AffectsPaint | PropMetadataOption.AffectsLayout,
                            new PropertyChangedCallback(OnImagePropertyChanged),
                            new CoerceValueCallback(CoerceImage),
                            isAnimationProhibited: true,
                            UpdateSourceTrigger.PropertyChanged
                            ));

        /// <summary>
        /// Occurs when the <see cref="Image"/> property changes.
        /// </summary>
        public event EventHandler? ImageChanged;

        /// <summary>
        /// Gets or sets the image that is displayed by <see cref="PictureBox"/>.
        /// </summary>
        public Image? Image
        {
            get
            {
                return (Image?)GetValue(ImageProperty);
            }

            set
            {
                SetValue(ImageProperty, value);
            }
        }

        /// <inheritdoc/>
        public override ControlId ControlKind => ControlId.PictureBox;

        /// <summary>
        /// Raises the <see cref="ImageChanged"/> event and calls
        /// <see cref="OnImageChanged(EventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
        public void RaiseImageChanged(EventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            OnImageChanged(e);
            ImageChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Called when the value of the <see cref="Image"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
        protected virtual void OnImageChanged(EventArgs e)
        {
        }

        /// <inheritdoc/>
        protected override ControlHandler CreateHandler()
        {
            return GetEffectiveControlHandlerHactory().CreatePictureBoxHandler(this);
        }

        private static object CoerceImage(DependencyObject d, object value)
        {
            // var o = (PictureBox)d;
            return value;
        }

        private static void OnImagePropertyChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            PictureBox control = (PictureBox)d;
            control.OnValuePropertyChanged((Image)e.OldValue, (Image)e.NewValue);
        }

#pragma warning disable IDE0060 // Remove unused parameter
        private void OnValuePropertyChanged(Image oldValue, Image newValue)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            RaiseImageChanged(EventArgs.Empty);
        }
    }
}