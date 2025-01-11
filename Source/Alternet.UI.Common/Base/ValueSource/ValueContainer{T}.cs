using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Value container structure that implements <see cref="IValueSource{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">Type of the value.</typeparam>
    public struct ValueContainer<T> : IValueSource<T>
    {
        private readonly Action? changed;

        private T? value;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueContainer{T}"/> struct.
        /// </summary>
        public ValueContainer()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueContainer{T}"/> struct
        /// with the specified action which is executed when value is changed.
        /// </summary>
        /// <param name="changed">Action to call when value is changed.</param>
        public ValueContainer(Action changed)
        {
            this.changed = changed;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueContainer{T}"/> struct
        /// with the specified default value.
        /// </summary>
        /// <param name="value">Default value.</param>
        public ValueContainer(T? value)
        {
            this.value = value;
        }

        /// <inheritdoc/>
        public event EventHandler? ValueChanged;

        /// <inheritdoc/>
        public T? Value
        {
            readonly get
            {
                return GetValue();
            }

            set
            {
                SetValue(value);
            }
        }

        /// <summary>
        /// Gets whether value is null.
        /// </summary>
        public readonly bool IsNull => value == null;

        /// <summary>
        /// Compares this value with the another value.
        /// </summary>
        /// <param name="value">Another value to compare with.</param>
        /// <returns></returns>
        public readonly bool ValueEquals(T? value)
        {
            if(this.value is null)
            {
                if (value is null)
                    return true;
                return false;
            }
            else
            {
                if (value is null)
                    return false;

                if (this.value.Equals(value))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Raises events after value is changed.
        /// </summary>
        public readonly void RaiseChanged()
        {
            changed?.Invoke();
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }

        internal readonly T? GetValue()
        {
            return value;
        }

        internal bool SetValue(T? value)
        {
            if (ValueEquals(value))
                return false;
            this.value = value;
            RaiseChanged();
            return true;
        }

        /// <summary>
        /// Sets value as <see cref="IDisposableObject"/>.
        /// </summary>
        /// <param name="value">Value to set.</param>
        /// <returns></returns>
        internal bool SetValueAsDisposable(T? value)
        {
            if (ValueEquals(value))
                return false;
            if (this.value is IDisposableObject disposable)
                disposable.Disposed -= ValueDisposedHandler;
            this.value = value;
            if (value is IDisposableObject disposable2)
                disposable2.Disposed += ValueDisposedHandler;
            RaiseChanged();
            return true;
        }

        /// <summary>
        /// Sets value as <see cref="AbstractControl"/>.
        /// </summary>
        /// <param name="value">Value to set.</param>
        /// <returns></returns>
        internal bool SetValueAsControl(T? value)
        {
            var oldValue = this.value;

            var result = SetValueAsDisposable(value);

            if (result)
            {
                if (oldValue is AbstractControl oldControl)
                {
                    oldControl.HandleCreated -= HandleControlHandleCreated;
                }

                if (value is AbstractControl newControl)
                {
                    newControl.HandleCreated += HandleControlHandleCreated;
                }
            }

            return result;
        }

        private readonly void HandleControlHandleCreated(object? sender, EventArgs e)
        {
            RaiseChanged();
        }

        private void ValueDisposedHandler(object? sender, EventArgs e)
        {
            SetValueAsDisposable(default);
        }
    }
}
