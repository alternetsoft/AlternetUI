using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a writer that supports structured text output, allowing for indentation and writing lines of text.
    /// </summary>
    public interface IStructuredTextWriter : IDisposableObject
    {
        /// <summary>
        /// Gets the current indentation level for the structured text output.
        /// </summary>
        int IndentLevel { get; set; }

        /// <summary>
        /// Increases the indentation level for subsequent log messages.
        /// </summary>
        void Indent();

        /// <summary>
        /// Decreases the indentation level for subsequent log messages.
        /// </summary>
        void Unindent();

        /// <summary>
        /// Writes a message to the log, followed by a line terminator.
        /// </summary>
        /// <param name="message">The message to write.</param>
        void WriteLine(string message);
    }

    /// <summary>
    /// Represents a log writer that supports structured text output.
    /// </summary>
    public interface ILogWriter : IStructuredTextWriter
    {
    }
}