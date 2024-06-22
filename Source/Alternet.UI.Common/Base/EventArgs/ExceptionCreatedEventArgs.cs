using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for the <see cref="BaseException.ExceptionCreated"/> event.
    /// </summary>
    public class ExceptionCreatedEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionCreatedEventArgs" /> class with
        /// a specified error message and a reference to the inner exception that is
        /// the cause of this exception.</summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current
        /// exception, or a null reference if no inner exception is specified.</param>
        /// <param name="attr">Custom atrributes of the exception.</param>
        public ExceptionCreatedEventArgs(
            string? message,
            Exception? innerException,
            ICustomAttributes? attr = null)
        {
            Message = message;
            InnerException = innerException;
            Attributes = attr;
        }

        /// <summary>
        /// Gets or sets custom atrributes of the exception.
        /// </summary>
        public ICustomAttributes? Attributes { get; set; }

        /// <summary>
        /// Gets or sets the error message that explains the reason for the exception.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Gets or sets the exception that is the cause of the current
        /// exception, or a null reference if no inner exception is specified.
        /// </summary>
        public Exception? InnerException { get; set; }
    }
}
