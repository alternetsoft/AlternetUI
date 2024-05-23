using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;
using Alternet.UI.Localization;

namespace Alternet.Drawing
{
    /// <summary>
    /// Implements base class that pass calls to it's handler.
    /// and other graphics objects.
    /// </summary>
    public abstract class HandledObject<T> : DisposableObject
        where T : class
    {
        private bool immutable;
        private T? handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="HandledObject"/> class.
        /// </summary>
        /// <param name="immutable">Whether this object is immutable (properties are readonly).</param>
        protected HandledObject(bool immutable)
        {
            this.immutable = immutable;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HandledObject"/> class.
        /// </summary>
        protected HandledObject()
        {
        }

        /// <summary>
        /// Gets whether object is readonly.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsReadOnly => Immutable;

        /// <summary>
        /// Gets whether this object is immutable (properties are readonly).
        /// </summary>
        [Browsable(false)]
        public bool Immutable
        {
            get => immutable;
            protected set => immutable = value;
        }

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
        protected bool UpdateRequired { get; set; }

        /// <summary>
        /// Checks whether object is readonly
        /// </summary>
        /// <exception cref="InvalidOperationException">Raised if object is readonly.</exception>
        protected void CheckReadOnly()
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
            (handler as IDisposable)?.Dispose();
            handler = null;
        }
    }
}
