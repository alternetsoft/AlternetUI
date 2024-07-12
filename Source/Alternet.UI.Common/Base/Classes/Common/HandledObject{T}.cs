using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Implements base class that pass calls to it's handler.
    /// and other graphics objects.
    /// </summary>
    public abstract class HandledObject<T> : ImmutableObject
        where T : class, IDisposable
    {
        private T? handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="HandledObject{T}"/> class.
        /// </summary>
        /// <param name="immutable">Whether this object is immutable (properties are readonly).</param>
        public HandledObject(bool immutable)
        {
            Immutable = immutable;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HandledObject{T}"/> class.
        /// </summary>
        public HandledObject()
        {
        }

        /// <summary>
        /// Gets whether object is readonly.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsReadOnly => Immutable;

        /// <summary>
        /// Gets whether handler was created.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsHandlerCreated => handler is not null;

        /// <summary>
        /// Gets handler.
        /// </summary>
        [Browsable(false)]
        public virtual T Handler
        {
            get
            {
                if (handler is null)
                {
                    handler = CreateHandler();
                    UpdateHandler();
                }
                else
                if (UpdateRequired)
                {
                    UpdateRequired = false;
                    UpdateHandler();
                }

                return handler;
            }

            protected set
            {
                handler = value;
            }
        }

        /// <summary>
        /// Gets whether handler update is required.
        /// </summary>
        protected virtual bool UpdateRequired { get; set; }

        /// <summary>
        /// Checks whether object is readonly
        /// </summary>
        /// <exception cref="InvalidOperationException">Raised if object is readonly.</exception>
        protected virtual void CheckReadOnly()
        {
            if (IsReadOnly)
            {
                throw new InvalidOperationException(
                    ErrorMessages.Default.CannotChangeReadOnlyObject);
            }
        }

        /// <summary>
        /// Creates handler.
        /// </summary>
        /// <returns></returns>
        protected abstract T CreateHandler();

        /// <summary>
        /// Updates handler.
        /// </summary>
        protected virtual void UpdateHandler()
        {
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            SafeDispose(ref handler);
        }
    }
}
