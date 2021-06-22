using System;

namespace Alternet.UI
{
    public class ProgressBar : Control
    {
        private int value;

        public int Value
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
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler? ValueChanged;

        private int minimum;

        public int Minimum
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

        private int maximum = 100;

        public int Maximum
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