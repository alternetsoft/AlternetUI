#pragma warning disable
using System;

namespace Alternet.UI.Data.Core
{
    /// <summary>
    /// Exception thrown when ExpressionObserver could not parse the provided
    /// expression string.
    /// </summary>
    class ExpressionParseException : BaseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionParseException"/> class.
        /// </summary>
        /// <param name="column">The column position of the error.</param>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The exception that caused the parsing failure.</param>
        public ExpressionParseException(int column, string message, Exception? innerException = null)
            : base(message, innerException)
        {
            Column = column;
        }

        /// <summary>
        /// Gets the column position at which the error occurred.
        /// </summary>
        public int Column { get; }
    }
}
