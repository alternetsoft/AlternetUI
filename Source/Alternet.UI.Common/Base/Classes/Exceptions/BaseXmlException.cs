using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Alternet.UI
{
    internal class BaseXmlException : XmlException
    {
        public BaseXmlException()
        {
            BaseException.RaiseExceptionCreated(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see langword="BaseXmlException" />
        /// class with a specified error message.
        /// </summary>
        /// <param name="message">The error description. </param>
        public BaseXmlException(string message)
            : base(message)
        {
            BaseException.RaiseExceptionCreated(this, message);
        }

        /// <summary>
        /// Initializes a new instance of the <see langword="BaseXmlException" /> class.
        /// </summary>
        /// <param name="message">The description of the error condition. </param>
        /// <param name="innerException">The <see cref="Exception" /> that threw
        /// the <see langword="BaseXmlException" />, if any.
        /// This value can be <see langword="null" />. </param>
        public BaseXmlException(string message, Exception innerException)
            : base(message, innerException)
        {
            BaseException.RaiseExceptionCreated(this, message, innerException);
        }

        /// <summary>
        /// Initializes a new instance of the <see langword="BaseXmlException" />
        /// class with the specified message, inner exception, line number, and line position.
        /// </summary>
        /// <param name="message">The error description. </param>
        /// <param name="innerException">The exception that is the cause of the current exception.
        /// This value can be <see langword="null" />. </param>
        /// <param name="lineNumber">The line number indicating where the error occurred. </param>
        /// <param name="linePosition">The line position indicating where the error occurred. </param>
        public BaseXmlException(string message, Exception innerException, int lineNumber, int linePosition)
            : base(message, innerException, lineNumber, linePosition)
        {
            var attr = Factory.CreateAttributes();
            attr["LineNumber"] = lineNumber;
            attr["LinePosition"] = linePosition;
            BaseException.RaiseExceptionCreated(this, message, innerException, attr);
        }
    }
}
