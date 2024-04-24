using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Implements base class for <see cref="Brush"/>, <see cref="Pen"/>
    /// and other graphics objects.
    /// </summary>
    public abstract class GraphicsObject : DisposableObject
    {
        private readonly bool immutable;
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
        /// Gets whether this object is immutable (properties are readonly).
        /// </summary>
        [Browsable(false)]
        public bool Immutable => immutable;

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
        }

        /// <summary>
        /// Gets whether native object update is required.
        /// </summary>
        protected bool UpdateRequired { get; set; }

        /// <summary>
        /// Creates native object.
        /// </summary>
        /// <returns></returns>
        protected abstract object CreateNativeObject();

        /// <summary>
        /// Updates native object.
        /// </summary>
        protected abstract void UpdateNativeObject();

        /// <inheritdoc/>
        protected override void DisposeManagedResources()
        {
            if (nativeObject is not null)
            {
                ((IDisposable)nativeObject).Dispose();
                nativeObject = null;
            }
        }
    }
}
