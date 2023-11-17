using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implements object unique id.
    /// </summary>
    public readonly struct ObjectUniqueId : IEquatable<ObjectUniqueId>
    {
        private readonly Guid guid;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectUniqueId"/> struct.
        /// </summary>
        public ObjectUniqueId()
        {
            guid = Guid.NewGuid();
        }

        /// <summary>
        /// Tests whether two <see cref='ObjectUniqueId'/> objects are not equal.
        /// </summary>
        public static bool operator !=(ObjectUniqueId left, ObjectUniqueId right)
            => left.guid != right.guid;

        /// <summary>
        /// Tests whether two <see cref='ObjectUniqueId'/> objects are equal.
        /// </summary>
        public static bool operator ==(ObjectUniqueId left, ObjectUniqueId right) =>
            left.guid == right.guid;

        /// <summary>
        /// Indicates whether the current object is equal to another object.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to other; otherwise,
        /// <c>false</c>.</returns>
        public override readonly bool Equals([NotNullWhen(true)] object? other) =>
            other is ObjectUniqueId id && Equals(id);

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to other; otherwise,
        /// <c>false</c>.</returns>
        public readonly bool Equals(ObjectUniqueId other) => this == other;

        /// <summary>
        /// Returns the hash code of this instance.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return guid.GetHashCode();
        }
    }
}
