using System;
using System.ComponentModel;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents control that displays a selected color and allows to change it.
    /// </summary>
    [ControlCategory("Other")]
    public partial class ColorPicker : Control
    {
        private Color color = Color.Black;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorPicker"/> class.
        /// </summary>
        public ColorPicker()
        {
        }

        /// <summary>
        /// Occurs when the <see cref="Value"/> property has been changed in some way.
        /// </summary>
        /// <remarks>For the <see cref="ValueChanged"/> event to occur, the
        /// <see cref="Value"/> property can be changed in code,
        /// by clicking the up or down button, or by the user entering a new
        /// value that is read by the control.</remarks>
        public event EventHandler? ValueChanged;

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.ColorPicker;

        /// <summary>
        /// Gets or sets the value assigned to the color picker as a selected color.
        /// </summary>
        public Color Value
        {
            get
            {
                return color;
            }

            set
            {
                value ??= Color.Black;
                if (color == value)
                    return;
                color = value;
                RaiseValueChanged(EventArgs.Empty);
            }
        }

        [Browsable(false)]
        internal new string Text
        {
            get => base.Text;
            set => base.Text = value;
        }

        /// <summary>
        /// Raises the <see cref="ValueChanged"/> event and calls
        /// <see cref="OnValueChanged(EventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        public void RaiseValueChanged(EventArgs e)
        {
            OnValueChanged(e);
            ValueChanged?.Invoke(this, e);
        }

        /// <inheritdoc/>
        protected override IControlHandler CreateHandler()
        {
            return ControlFactory.Handler.CreateColorPickerHandler(this);
        }

        /// <summary>
        /// Called when the value of the <see cref="Value"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
        protected virtual void OnValueChanged(EventArgs e)
        {
        }
    }
}