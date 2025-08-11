using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for the property change events.
    /// </summary>
    /// <typeparam name="T">The type of the object whose property changed.</typeparam>
    public class ObjectPropertyChangedEventArgs<T> : BaseEventArgs
    {
        private T instance;
        private string? propName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectPropertyChangedEventArgs"/> class.
        /// </summary>
        /// <param name="instance">Object instance which property was changed.</param>
        /// <param name="propName">Property name. Optional. If <c>null</c>, more than one
        /// property were changed.</param>
        public ObjectPropertyChangedEventArgs(T instance, string? propName)
        {
            this.instance = instance;
            this.propName = propName;
        }

        /// <summary>
        /// Object instance which property was changed.
        /// </summary>
        public virtual T Instance
        {
            get
            {
                return instance;
            }

            set
            {
                instance = value;
            }
        }

        /// <summary>
        /// Property name. If <c>null</c>, more than one property were changed.
        /// </summary>
        public virtual string? PropName
        {
            get
            {
                return propName;
            }

            set
            {
                propName = value;
            }
        }
    }

#pragma warning disable
    /// <summary>
    /// Provides data for the property change events.
    /// </summary>
    public class ObjectPropertyChangedEventArgs : ObjectPropertyChangedEventArgs<object?>
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ObjectPropertyChangedEventArgs"/> class
        /// with the specified instance and property name.
        /// </summary>
        /// <param name="instance">The object instance which property was changed.</param>
        /// <param name="propName">The name of the property that was changed.</param>
        public ObjectPropertyChangedEventArgs(object? instance, string? propName)
            : base(instance, propName)
        {
        }
    }
#pragma warning restore
}
