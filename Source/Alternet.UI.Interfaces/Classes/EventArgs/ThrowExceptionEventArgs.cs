using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>Provides data for the exception related events.</summary>
    public class ThrowExceptionEventArgs : BaseEventArgs
    {
        private readonly Exception exception;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThrowExceptionEventArgs" /> class.
        /// </summary>
        /// <param name="errorException">The <see cref="Exception" /> that occurred.</param>
        public ThrowExceptionEventArgs(Exception errorException)
        {
            exception = errorException ?? throw new ArgumentNullException(nameof(errorException));
        }

        /// <summary>
        /// Gets the <see cref="Exception" /> that occurred.
        /// </summary>
        /// <returns>The <see cref="Exception" /> that occurred.</returns>
        public Exception ErrorException => exception;

        /// <summary>
        /// Gets the inner exception of <see cref="ErrorException"/> or
        /// <see cref="ErrorException"/> itself.
        /// </summary>
        public Exception InnerException
        {
            get
            {
                return exception.InnerException ?? exception;
            }
        }

        /// <summary>
        /// Gets or sets whether to throw an exception after event handler executed.
        /// </summary>
        public bool ThrowIt { get; set; }
    }
}