using System;

namespace Alternet.UI
{
    public class NumericUpDown : Control
    {
        private decimal value;

        public decimal Value
        {
            get
            {
                CheckDisposed();
                return value;
            }

            set
            {
                CheckDisposed();
                if (this.value == value)
                    return;

                this.value = value;
                InvokeValueChanged(EventArgs.Empty);
            }
        }

        public void InvokeValueChanged(EventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            OnValueChanged(e);
            ValueChanged?.Invoke(this, e);
        }

        protected virtual void OnValueChanged(EventArgs e)
        {
        }

        public event EventHandler? ValueChanged;

        private decimal minimum;

        public decimal Minimum
        {
            get
            {
                CheckDisposed();
                return minimum;
            }

            set
            {
                CheckDisposed();
                if (minimum == value)
                    return;

                minimum = value;
                MinimumChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler? MinimumChanged;

        private decimal maximum = 100;

        public decimal Maximum
        {
            get
            {
                CheckDisposed();
                return maximum;
            }

            set
            {
                CheckDisposed();
                if (maximum == value)
                    return;

                maximum = value;
                MaximumChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler? MaximumChanged;
    }
}