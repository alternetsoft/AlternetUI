using System;
using System.ComponentModel;
using Alternet.UI;
using Alternet.UI.Localization;

namespace Alternet.Drawing
{
    /// <summary>
    /// Defines objects used to fill the interiors of graphical shapes such as rectangles,
    /// ellipses, pies, polygons, and paths.
    /// </summary>
    /// <remarks>
    /// This is an abstract base class and cannot be instantiated. To create a brush object,
    /// use classes derived from <see cref="Brush" />, such as <see cref="SolidBrush" /> or
    /// <see cref="HatchBrush" />.
    /// </remarks>
    [TypeConverter(typeof(BrushConverter))]
    public class Brush : IDisposable, IEquatable<Brush>
    {
        /// <summary>
        /// Gets transparent brush.
        /// </summary>
        public static readonly Brush Transparent = new();

        private static Brush? defaultBrush;
        private readonly bool immutable;
        private bool isDisposed;
        private Pen? asPen;

        internal Brush(UI.Native.Brush nativeBrush, bool immutable)
        {
            NativeBrush = nativeBrush;
            this.immutable = immutable;
        }

        internal Brush()
            : this(new UI.Native.Brush(), true)
        {
        }

        /// <summary>
        /// Gets the default brush.
        /// </summary>
        /// <value>
        /// Default brush is <see cref="SolidBrush"/> with Black color.
        /// </value>
        public static Brush Default => defaultBrush ??= Brushes.Black;

        /// <summary>
        /// Gets whether this object is immutable (properties are readonly).
        /// </summary>
        [Browsable(false)]
        public bool Immutable => immutable;

        /// <summary>
        /// Gets whether object is disposed.
        /// </summary>
        [Browsable(false)]
        public bool IsDisposed => isDisposed;

        /// <summary>
        /// Creates <see cref="Pen"/> with this brush as a parameter.
        /// </summary>
        /// <remarks>
        /// Pen created only once and saved internally in the brush instance.
        /// </remarks>
        public Pen AsPen
        {
            get
            {
                if (asPen == null)
                    asPen = new Pen(BrushColor);
                return asPen;
            }
        }

        /// <summary>
        /// Gets type of the brush.
        /// </summary>
        public virtual BrushType BrushType => BrushType.None;

        internal virtual Color BrushColor => Color.Black;

        internal UI.Native.Brush NativeBrush { get; private set; }

        /// <summary>
        /// Returns a value that indicates whether the two objects are equal.
        /// </summary>
        public static bool operator ==(Brush? a, Brush? b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        /// <summary>
        /// Returns a value that indicates whether the two objects are not equal.
        /// </summary>
        public static bool operator !=(Brush? a, Brush? b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Releases all resources used by this <see cref="Brush"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            CheckDisposed();
            return GetHashCodeCore();
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        public bool Equals(Brush? other)
        {
            if (other == null)
                return false;

            CheckDisposed();
            return EqualsCore(other);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            CheckDisposed();
            return ToStringCore();
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified object is equal to the current object;
        /// otherwise, <c>false</c>.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is not Brush brush)
                return false;

            if (GetType() != obj?.GetType())
                return false;

            return Equals(brush);
        }

        /// <summary>
        /// Throws <see cref="ObjectDisposedException"/> if the object has been disposed.
        /// </summary>
        protected void CheckDisposed()
        {
            if (isDisposed)
                throw new ObjectDisposedException(null);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the System.ComponentModel.Component
        /// and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources; false<c></c> to release
        /// only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (immutable)
                {
                    throw new InvalidOperationException(
                        ErrorMessages.Default.CannotDisposeImmutableObject);
                }

                if (disposing)
                {
                    NativeBrush.Dispose();
                    NativeBrush = null!;
                }

                isDisposed = true;
            }
        }

        private protected virtual int GetHashCodeCore() => base.GetHashCode();

        private protected virtual bool EqualsCore(Brush other) => other == this;

        private protected virtual string ToStringCore() => "Transparent";
    }
}