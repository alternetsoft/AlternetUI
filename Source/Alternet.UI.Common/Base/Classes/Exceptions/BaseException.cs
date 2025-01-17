﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Base class for the exception descendants in the library.
    /// </summary>
    public class BaseException : Exception
    {
        private FlagsAndAttributesStruct attr = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseException"/> class.
        /// </summary>
        public BaseException()
        {
            RaiseExceptionCreated(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseException" /> class with
        /// a specified error message.</summary>
        /// <param name="message">The message that describes the error.</param>
        public BaseException(string message)
            : base(message)
        {
            RaiseExceptionCreated(this, message);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseException" /> class with
        /// a specified error message and a reference to the inner exception that is
        /// the cause of this exception.</summary>
        /// <param name="message">The error message that explains the
        /// reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current
        /// exception, or a null reference if no inner exception is specified.</param>
        public BaseException(string message, Exception? innerException)
            : base(message, innerException)
        {
            RaiseExceptionCreated(this, message, innerException);
        }

        /// <summary>
        /// Occurs when new <see cref="BaseException"/> descendant is created.
        /// </summary>
        public static event EventHandler<ExceptionCreatedEventArgs>? ExceptionCreated;

        /// <inheritdoc cref="BaseObjectWithAttr.Tag"/>
        [Browsable(false)]
        public virtual object? Tag { get; set; }

        /// <inheritdoc cref="BaseObjectWithAttr.IntFlags"/>
        [Browsable(false)]
        public ICustomIntFlags IntFlags
        {
            get
            {
                return IntFlagsAndAttributes;
            }
        }

        /// <inheritdoc cref="BaseObjectWithAttr.IntFlagsAndAttributes"/>
        [Browsable(false)]
        public virtual IIntFlagsAndAttributes IntFlagsAndAttributes
        {
            get
            {
                return attr.IntFlagsAndAttributes;
            }

            set
            {
                attr.IntFlagsAndAttributes = value;
            }
        }

        /// <inheritdoc cref="BaseObjectWithAttr.FlagsAndAttributes"/>
        [Browsable(false)]
        public virtual IFlagsAndAttributes FlagsAndAttributes
        {
            get
            {
                return attr.FlagsAndAttributes;
            }

            set
            {
                attr.FlagsAndAttributes = value;
            }
        }

        /// <summary>
        /// <inheritdoc cref="BaseObjectWithAttr.CustomFlags"/>
        /// </summary>
        [Browsable(false)]
        public ICustomFlags<string> CustomFlags => FlagsAndAttributes.Flags;

        /// <inheritdoc cref="BaseObjectWithAttr.CustomAttr"/>
        [Browsable(false)]
        public ICustomAttributes<string, object> CustomAttr => FlagsAndAttributes.Attr;

        /// <summary>
        /// Gets or sets an additional information for the exception.
        /// This is used in the <see cref="ThreadExceptionWindow"/>.
        /// </summary>
        public virtual string? AdditionalInformation { get; set; }

        /// <summary>
        /// Raises <see cref="ExceptionCreated"/> event.
        /// </summary>
        /// <param name="sender">Exception object to pass as sender parameter.</param>
        /// <param name="message">The error message that explains the
        /// reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current
        /// exception, or a null reference if no inner exception is specified.</param>
        /// <param name="attr">Custom atrributes of the exception.</param>
        public static void RaiseExceptionCreated(
            Exception sender,
            string? message = null,
            Exception? innerException = null,
            ICustomAttributes? attr = null)
        {
            ExceptionCreated?.Invoke(
                sender,
                new ExceptionCreatedEventArgs(message, innerException, attr));
        }
    }
}
