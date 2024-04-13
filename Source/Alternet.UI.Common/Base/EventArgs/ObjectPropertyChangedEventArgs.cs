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
    public class ObjectPropertyChangedEventArgs : BaseEventArgs
    {
        private readonly object? instance;
        private readonly string? propName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectPropertyChangedEventArgs"/> class.
        /// </summary>
        /// <param name="instance">Object instance which property was changed.</param>
        /// <param name="propName">Property name. Optional. If <c>null</c>, more than one
        /// property were changed.</param>
        public ObjectPropertyChangedEventArgs(object? instance, string? propName)
        {
            this.instance = instance;
            this.propName = propName;
        }

        /// <summary>
        /// Object instance which property was changed.
        /// </summary>
        public object? Instance => instance;

        /// <summary>
        /// Property name. If <c>null</c>, more than one property were changed.
        /// </summary>
        public string? PropName => propName;
    }
}
