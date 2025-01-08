using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implements object unique id.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public readonly struct ObjectUniqueId : IEquatable<ObjectUniqueId>
    {
        private static ulong globalCounter;

        [FieldOffset(0)]
        private readonly State state;

        [FieldOffset(1)]
        private readonly int hashCode;

        [FieldOffset(1 + 4)]
        private readonly Guid guid;

        [FieldOffset(1 + 4)]
        private readonly ulong id;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectUniqueId"/> struct.
        /// </summary>
        public ObjectUniqueId()
            : this(ref globalCounter)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectUniqueId"/> struct
        /// using the specified reference to the counter variable.
        /// </summary>
        public ObjectUniqueId(ref ulong counter)
        {
            if (counter == long.MaxValue)
            {
                state = State.Guid;
                guid = Guid.NewGuid();
                hashCode = guid.GetHashCode();
            }
            else
            {
                state = State.Long;
                id = counter++;
                hashCode = id.GetHashCode();
            }
        }

        private enum State : byte
        {
            Long,

            Guid,
        }

        /// <summary>
        /// Tests whether two <see cref='ObjectUniqueId'/> objects are not equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ObjectUniqueId left, ObjectUniqueId right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Tests whether two <see cref='ObjectUniqueId'/> objects are equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ObjectUniqueId left, ObjectUniqueId right)
        {
            if (left.state != right.state)
                return false;

            if(left.state == State.Guid)
                return left.guid == right.guid;
            return left.id == right.id;
        }

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(ObjectUniqueId other) => this == other;

        /// <summary>
        /// Returns the hash code of this instance.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return hashCode;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            if(state == State.Guid)
                return $"A{guid:N}";
            return id.ToString();
        }
    }
}
