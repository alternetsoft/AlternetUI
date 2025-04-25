using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Extends <see cref="DisposableObject"/> with <see cref="Immutable"/> property
    /// and other features.
    /// Allows to implement immutable objects with properties that can not be changed.
    /// </summary>
    public class ImmutableObject : DisposableObject, IImmutableObject
    {
        private bool immutable;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImmutableObject"/> class.
        /// </summary>
        public ImmutableObject()
        {
        }

        /// <inheritdoc/>
        public override bool Immutable
        {
            get
            {
                return immutable;
            }
        }

        /// <summary>
        /// Marks the object as immutable.
        /// </summary>
        /// <remarks>
        /// Marks this object as immutable, meaning that the contents of its properties
        /// will not change
        /// for the lifetime of the object. This state can be set, but it cannot be cleared once
        /// it is set.
        /// </remarks>
        [Browsable(false)]
        public virtual void SetImmutable()
        {
            immutable = true;
        }

        /// <summary>
        /// Sets value of the <see cref="Immutable"/> property.
        /// </summary>
        /// <param name="value"></param>
        protected virtual void SetImmutable(bool value)
        {
            immutable = value;
        }
    }
}
