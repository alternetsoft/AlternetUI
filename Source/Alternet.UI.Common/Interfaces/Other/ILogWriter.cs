using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Defines methods for logging with indentation and other features.
    /// </summary>
    public interface ILogWriter : IDisposableObject
    {
        /// <summary>
        /// Increases the indentation level for subsequent log messages.
        /// </summary>
        ILogWriter Indent();

        /// <summary>
        /// Decreases the indentation level for subsequent log messages.
        /// </summary>
        ILogWriter Unindent();

        /// <summary>
        /// Writes a message to the log, followed by a line terminator.
        /// </summary>
        /// <param name="message">The message to write.</param>
        ILogWriter WriteLine(string message);
    }
}