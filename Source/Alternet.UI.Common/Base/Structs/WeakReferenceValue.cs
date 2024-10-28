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
        /// Gets control where caret is located.
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
                    return null;
                }
            }

            set
            {
                if(value is null)
                {
                    Reference = null;
                    return;
                }

                if(Reference is null)
                {
                    Reference = new(value);
                    return;
                }

                Reference.SetTarget(value);
            }
        }
    }
}
