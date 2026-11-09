using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for the cancelable property change event.
    /// </summary>
    public class PropertyChangeRequestedEventArgs : BaseCancelEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyChangeRequestedEventArgs"/>
        /// class with the specified property name.
        /// </summary>
        /// <param name="propertyName">The name of the property that is changing.</param>
        public PropertyChangeRequestedEventArgs(string propertyName)
        {
            PropertyName = propertyName;
        }

        /// <summary>
        /// Gets or sets the name of the property that is changing.
        /// </summary>
        public virtual string PropertyName { get; set; }
    }
}
