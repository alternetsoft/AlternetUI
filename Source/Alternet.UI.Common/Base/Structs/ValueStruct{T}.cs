using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    internal struct ValueStruct<T>
        where T : notnull, IEquatable<T>
    {
#pragma warning disable
        public Action? Changed;
#pragma warning restore

        private T value;

        public readonly T GetValue()
        {
            return value;
        }

        public void SetValue(T value)
        {
            if (((IEquatable<T>)this.value).Equals(value))
                return;
            this.value = value;
            RaiseChanged();
        }

        public readonly void RaiseChanged()
        {
            Changed?.Invoke();
        }
    }
}
