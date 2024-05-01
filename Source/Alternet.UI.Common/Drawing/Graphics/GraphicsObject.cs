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
    /// Implements base class for <see cref="Brush"/>, <see cref="Pen"/>
    /// and other graphics objects.
    /// </summary>
    public abstract class GraphicsObject : DisposableObject
    {
        private bool immutable;
        private object? nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsObject"/> class.
        /// </summary>
        /// <param name="immutable">Whether this object is immutable (properties are readonly).</param>
        protected GraphicsObject(bool immutable)
        {
            this.immutable = immutable;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsObject"/> class.
        /// </summary>
        protected GraphicsObject()
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
        /// Gets native graphics object.
        /// </summary>
        [Browsable(false)]
        public virtual object NativeObject
        {
            get
            {
                if (nativeObject is null)
                {
                    nativeObject = CreateNativeObject();
                    UpdateNativeObject();
                }
                else
                if (UpdateRequired)
                {
                    UpdateRequired = false;
                    UpdateNativeObject();
                }

                return nativeObject;
            }

            protected set
            {
                nativeObject = value;
            }
        }

        /// <summary>
        /// Gets whether native object update is required.
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
        /// Creates native object.
        /// </summary>
        /// <returns></returns>
        protected abstract object CreateNativeObject();

        /// <summary>
        /// Dispose native object.
        /// </summary>
        protected virtual void DisposeNativeObject()
        {
            if(nativeObject is IDisposable disposable)
                disposable.Dispose();
            nativeObject = null;
        }

        /// <summary>
        /// Updates native object.
        /// </summary>
        protected virtual void UpdateNativeObject()
        {
        }

        /// <inheritdoc/>
        protected override void DisposeManagedResources()
        {
            if (nativeObject is not null)
            {
                DisposeNativeObject();
            }
        }
    }
}
