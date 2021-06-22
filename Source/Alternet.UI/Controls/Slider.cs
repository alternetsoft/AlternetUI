using System;

namespace Alternet.UI
{
    public class Slider : Control
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

        private int maximum = 10;

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

        private int smallChange = 1;

        public int SmallChange
        {
            get
            {
                CheckDisposed();
                return smallChange;
            }

            set
            {
                CheckDisposed();
                if (smallChange == value)
                    return;

                smallChange = value;
                SmallChangeChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler? SmallChangeChanged;
        
        private int largeChange = 5;

        public int LargeChange
        {
            get
            {
                CheckDisposed();
                return largeChange;
            }

            set
            {
                CheckDisposed();
                if (largeChange == value)
                    return;

                largeChange = value;
                LargeChangeChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler? LargeChangeChanged;

        private int tickFrequency = 1;

        public int TickFrequency
        {
            get
            {
                CheckDisposed();
                return tickFrequency;
            }

            set
            {
                CheckDisposed();
                if (tickFrequency == value)
                    return;

                tickFrequency = value;
                TickFrequencyChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler? TickFrequencyChanged;
    }
}