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
        /// <summary>
        /// Gets <see cref="HandledEventArgs"/> instance which <see cref="Handled"/>
        /// property is always <c>false</c>.
        /// </summary>
        public static readonly HandledEventArgs NotHandled = new NotHandledEventArgs();

        private bool handled;

        /// <summary>
        /// Gets or sets a value indicating whether the event was handled.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> to bypass the control's default handling;
        /// otherwise, <see langword="false" /> to also pass the event along to the
        /// default control handler.
        /// </returns>
        public virtual bool Handled
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

        private class NotHandledEventArgs : HandledEventArgs
        {
            public override bool Handled { get => false; }
        }
    }
}
