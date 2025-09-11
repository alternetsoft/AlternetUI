using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties related to the event arguments.
    /// </summary>
    public static class EventArgsUtils
    {
        /// <summary>
        /// Provides a pre-initialized <see cref="PropertyChangedEventArgs"/>
        /// instance for the "Count" property.
        /// </summary>
        /// <remarks>This static field can be used to avoid repeatedly creating
        /// new <see cref="PropertyChangedEventArgs"/> instances for the
        /// "Count" property, improving performance in scenarios
        /// where property change notifications for "Count" are frequent.</remarks>
        public static readonly PropertyChangedEventArgs CountPropertyChangedArgs
            = new("Count");

        /// <summary>
        /// Provides a pre-initialized <see cref="PropertyChangedEventArgs"/>
        /// instance for the "Items[]" property.
        /// </summary>
        /// <remarks>This static field can be used to avoid repeatedly creating
        /// new <see cref="PropertyChangedEventArgs"/> instances for the
        /// "Items[]" property, improving performance in scenarios
        /// where property change notifications for "Items[]" are frequent.</remarks>
        public static readonly PropertyChangedEventArgs IndexerPropertyChangedArgs
            = new("Item[]");

        /// <summary>
        /// Provides a pre-initialized instance
        /// of <see cref="NotifyCollectionChangedEventArgs"/>  with the
        /// <see cref="NotifyCollectionChangedAction.Reset"/> action.
        /// </summary>
        /// <remarks>This static field can be used to represent a reset action
        /// in collection change notifications,  avoiding the need to create
        /// a new instance of <see cref="NotifyCollectionChangedEventArgs"/>
        /// each time a reset action is required.</remarks>
        public static readonly NotifyCollectionChangedEventArgs ResetCollectionChangedArgs
            = new(NotifyCollectionChangedAction.Reset);

        private static CancelEventArgs? defaultCancelEventArgs;
        private static PropertyChangedEventArgs? defaultPropertyChangedEventArgs;

        /// <summary>
        /// Provides a pre-initialized <see cref="PropertyChangedEventArgs"/>
        /// instance with an empty
        /// <see cref="PropertyChangedEventArgs.PropertyName"/>.
        /// </summary>
        /// <remarks>This static field can be used to avoid repeatedly creating
        /// new <see cref="PropertyChangedEventArgs"/> instances, improving performance.</remarks>

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
        /// Gets a default instance of <see cref="CancelEventArgs"/>
        /// with the <see cref="CancelEventArgs.Cancel"/>
        /// property set to <see langword="false"/>.
        /// </summary>
        /// <remarks>This property provides a shared instance of <see cref="CancelEventArgs"/> to reduce
        /// object allocations. The <see cref="CancelEventArgs.Cancel"/> property
        /// is reset to <see langword="false"/>
        /// each time the instance is accessed.</remarks>
        public static CancelEventArgs DefaultCancelEventArgs
        {
            get
            {
                defaultCancelEventArgs ??= new(false);
                defaultCancelEventArgs.Cancel = false;
                return defaultCancelEventArgs;
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
