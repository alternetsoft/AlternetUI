using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties related to the event arguments.
    /// </summary>
    public static class EventArgsUtils
    {
        private static PropertyChangedEventArgs? defaultPropertyChangedEventArgs;

        /// <summary>
        /// Gets default <see cref="PropertyChangedEventArgs"/> instance with an empty
        /// <see cref="PropertyChangedEventArgs.PropertyName"/>.
        /// </summary>
        public static PropertyChangedEventArgs DefaultPropertyChangedEventArgs
        {
            get
            {
                return defaultPropertyChangedEventArgs ??= new(string.Empty);
            }
        }

        /// <summary>
        /// Gets <see cref="PropertyChangedEventArgs"/> instance for the specified property name.
        /// </summary>
        /// <param name="propName"></param>
        /// <returns></returns>
        public static PropertyChangedEventArgs GetPropertyChangedEventArgs(string? propName)
        {
            if (string.IsNullOrEmpty(propName))
                return DefaultPropertyChangedEventArgs;
            else
                return new(propName);
        }
    }
}
