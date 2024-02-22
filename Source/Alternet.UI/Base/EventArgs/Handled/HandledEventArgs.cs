using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for the events which require <see cref="Handled"/> property.
    /// </summary>
    public class HandledEventArgs : BaseEventArgs
    {
        private bool handled;

        /// <summary>
        /// Gets or sets a value indicating whether the event was handled.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> to bypass the control's default handling;
        /// otherwise, <see langword="false" /> to also pass the event along to the
        /// default control handler.
        /// </returns>
        public bool Handled
        {
            get
            {
                return handled;
            }

            set
            {
                handled = value;
            }
        }
    }
}
