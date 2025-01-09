using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the method that will handle the <see cref="App.ThreadException"/>
    /// event of an application.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">A <see cref="BaseThreadExceptionEventArgs"/>
    /// that contains the event data.</param>
    public delegate void BaseThreadExceptionEventHandler(
        object sender,
        BaseThreadExceptionEventArgs e);

    /// <summary>
    /// Provides data for the <see cref="App.ThreadException"/> event.
    /// </summary>
    public class BaseThreadExceptionEventArgs : HandledEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseThreadExceptionEventArgs"/> class.
        /// </summary>
        /// <param name="t">The <see cref="Exception"/> that occurred.</param>
        public BaseThreadExceptionEventArgs(Exception t)
        {
            Exception = t;
        }

        /// <summary>
        /// Gets or sets the <see cref="Exception"/> that occurred.
        /// </summary>
        public Exception Exception { get; set; }
    }
}
