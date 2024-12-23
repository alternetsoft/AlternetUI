using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains <see cref="WeakReference{T}"/> field, methods and properties to manage it.
    /// </summary>
    /// <typeparam name="T">The type of the value to store in the weak reference object.</typeparam>
    public struct WeakReferenceValue<T>
         where T : class
    {
        /// <summary>
        /// Gets or sets weak reference object.
        /// </summary>
        public WeakReference<T>? Reference;

        /// <summary>
        /// Occurs when <see cref="Value"/> is changed.
        /// </summary>
        public Action? Changed;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakReferenceValue{T}"/> struct.
        /// </summary>
        public WeakReferenceValue()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakReferenceValue{T}"/> struct
        /// with the specified value.
        /// </summary>
        /// <param name="value">Initial value stored in the weak reference object.</param>
        public WeakReferenceValue(T value)
        {
            Reference = new(value);
        }

        /// <summary>
        /// Gets value.
        /// </summary>
        public T? Value
        {
            get
            {
                if (Reference is null)
                    return null;
                if (Reference.TryGetTarget(out var result))
                    return result;
                else
                {
                    Reference = null;
                    RaiseValueChanged();
                    return null;
                }
            }

            set
            {
                if (Value == value)
                    return;

                if(value is null)
                {
                    Reference = null;
                }
                else
                {
                    if (Reference is null)
                    {
                        Reference = new(value);
                    }
                    else
                    {
                        Reference.SetTarget(value);
                    }
                }

                RaiseValueChanged();
            }
        }

        /// <summary>
        /// Sets <see cref="Value"/> to Null.
        /// </summary>
        public void Reset()
        {
            Value = null;
        }

        /// <summary>
        /// Raises <see cref="Changed"/> event.
        /// </summary>
        public readonly void RaiseValueChanged()
        {
            Changed?.Invoke();
        }
    }
}
