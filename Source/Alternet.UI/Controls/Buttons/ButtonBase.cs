using System;
using System.ComponentModel;

namespace Alternet.UI
{
    /// <summary>
    /// Implements the basic functionality common to button controls.
    /// </summary>
    public abstract class ButtonBase : CustomButton
    {
        private string text = string.Empty;
        private Action? clickAction;

        /// <summary>
        /// Gets or sets the text displayed on this button.
        /// </summary>
        public override string Text
        {
            get
            {
                return text;
            }

            set
            {
                if (text == value)
                    return;
                CheckDisposed();

                text = value;
                OnTextChanged(EventArgs.Empty);
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets <see cref="Action"/> which will be executed when
        /// this <see cref="MenuItem"/> is clicked by the user.
        /// </summary>
        [Browsable(false)]
        public Action? ClickAction
        {
            get => clickAction;
            set
            {
                if (clickAction != null)
                    Click -= OnClickAction;
                clickAction = value;
                if (clickAction != null)
                    Click += OnClickAction;
            }
        }

        private void OnClickAction(object? sender, EventArgs? e)
        {
            clickAction?.Invoke();
        }
    }
}