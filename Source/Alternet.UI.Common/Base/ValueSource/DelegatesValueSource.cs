using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    internal class DelegatesValueSource<T> : IValueSource<object>
    {
        private readonly Func<T> getValue;
        private readonly Action<T> setValue;

        public DelegatesValueSource(
            Func<T> getValue,
            Action<T>? setValue)
        {
            setValue ??= (value) => { };
            this.getValue = getValue;
            this.setValue = setValue;
        }

        public event EventHandler? ValueChanged;

        public virtual object? Value
        {
            get => getValue();
            set
            {
                if (Value == value)
                    return;

                if(value is null)
                {
                    setValue(default!);
                }
                else
                {
                    setValue((T)value);
                }

                RaiseValueChanged();
            }
        }

        private void RaiseValueChanged()
        {
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
