using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    internal struct ValueContainer<T>
    {
        public T? Value;

        public Action? Changed;

        public ValueContainer()
        {
        }

        public ValueContainer(Action changed)
        {
            Changed = changed;
        }

        public ValueContainer(T? value)
        {
            Value = value;
        }

        public readonly bool IsNull => Value == null;

        public readonly T? GetValue()
        {
            return Value;
        }

        public readonly bool ValueEquals(T? value)
        {
            if(Value is null)
            {
                if (value is null)
                    return true;
                return false;
            }
            else
            {
                if (value is null)
                    return false;

                if (Value is IEquatable<T> equatable)
                {
                    if (equatable.Equals(value))
                        return true;
                }

                if (Value.Equals(value))
                    return true;
            }

            return false;
        }

        public bool SetValue(T? value)
        {
            if (ValueEquals(value))
                return false;
            Value = value;
            RaiseChanged();
            return true;
        }

        public bool SetValueForDisposable(T? value)
        {
            if (ValueEquals(value))
                return false;
            if (Value is IDisposableObject disposable)
                disposable.Disposed -= ValueDisposedHandler;
            Value = value;
            if (value is IDisposableObject disposable2)
                disposable2.Disposed += ValueDisposedHandler;
            RaiseChanged();
            return true;
        }

        public readonly void RaiseChanged()
        {
            Changed?.Invoke();
        }

        private void ValueDisposedHandler(object? sender, EventArgs e)
        {
            SetValueForDisposable(default);
        }
    }
}
