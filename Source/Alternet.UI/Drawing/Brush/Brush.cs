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
    public class Brush : DisposableObject, IEquatable<Brush>
    {
        /// <summary>
        /// Gets transparent brush.
        /// </summary>
        public static readonly Brush Transparent = new();

        private static Brush? defaultBrush;
        private readonly bool immutable;
        private Pen? asPen;
        private object? nativeBrush;

        /// <summary>
        /// Initializes a new instance of the <see cref="Brush"/> class.
        /// </summary>
        /// <param name="immutable">Whether this brush is immutable.</param>
        protected Brush(bool immutable)
        {
            this.immutable = immutable;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Brush"/> class.
        /// </summary>
        protected Brush()
            : this(true)
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

        /// <summary>
        /// Gets native brush.
        /// </summary>
        public virtual object NativeBrush
        {
            get
            {
                if(nativeBrush is null)
                {
                    nativeBrush = CreateNativeBrush();
                    UpdateNativeBrush();
                }
                else
                if (UpdateRequired)
                {
                    UpdateRequired = false;
                    UpdateNativeBrush();
                }

                return nativeBrush;
            }
        }

        /// <summary>
        /// Gets color of the brush.
        /// </summary>
        public virtual Color BrushColor => Color.Black;

        /// <summary>
        /// Gets whether native brush update is required.
        /// </summary>
        protected bool UpdateRequired { get; set; }

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
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            CheckDisposed();
            return GetHashCodeCore();
        }

        /// <summary>
        /// Creates native brush.
        /// </summary>
        /// <returns></returns>
        public virtual object CreateNativeBrush()
        {
            return NativeDrawing.Default.CreateTransparentBrush();
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

        /// <inheritdoc/>
        protected override void DisposeManagedResources()
        {
            if (nativeBrush is not null)
            {
                ((IDisposable)NativeBrush).Dispose();
                nativeBrush = null;
            }
        }

        /// <summary>
        /// Updates native brush.
        /// </summary>
        protected virtual void UpdateNativeBrush()
        {
        }

        private protected virtual int GetHashCodeCore() => base.GetHashCode();

        private protected virtual bool EqualsCore(Brush other) => other == this;

        private protected virtual string ToStringCore() => "Transparent";
    }
}