using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains properties and methods which allow to get and set immutable state of the object.
    /// </summary>
    public interface IImmutableObject : IDisposableObject
    {
        /// <summary>
        /// Gets whether object is immutable (properties can not be changed).
        /// </summary>
        bool Immutable { get; }

        /// <summary>
        /// <inheritdoc cref="ImmutableObject.SetImmutable()"/>
        /// </summary>
        void SetImmutable();
    }
}