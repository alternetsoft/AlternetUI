using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Base class with properties and methods common to all Alternet.UI <see cref="CancelEventArgs"/>
    /// descendants.
    /// </summary>
    public class BaseCancelEventArgs : CancelEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCancelEventArgs"/> class.
        /// </summary>
        public BaseCancelEventArgs()
                : base(cancel: false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCancelEventArgs"/> class.
        /// </summary>
        /// <param name="cancel"><c>true</c> to cancel the event; otherwise, <c>false</c>.</param>
        public BaseCancelEventArgs(bool cancel)
                : base(cancel)
        {
        }
    }
}