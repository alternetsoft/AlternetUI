using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for the log events.
    /// </summary>
    public class LogMessageEventArgs : EventArgs
    {
        private readonly string message;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyChangeEventArgs"/> class.
        /// </summary>
        /// <param name="message">Message that needs to be logged.</param>
        public LogMessageEventArgs(string message)
        {
            this.message = message;
        }

        /// <summary>
        /// Message that needs to be logged.
        /// </summary>
        public string Message => message;
    }
}
