using System;
using System.ComponentModel;

using Alternet.UI;
using Alternet.UI.Localization;

using SkiaSharp;

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
    public class Brush : HandledObject<IBrushHandler>, IEquatable<Brush>
    {
        /// <summary>
        /// Gets transparent brush.
        /// </summary>
        public static readonly Brush Transparent = new();

        private static Brush? defaultBrush;

        private Pen? asPen;
        private SKPaint? paint;

        /// <summary>
        /// Initializes a new instance of the <see cref="Brush"/> class.
        /// </summary>
        /// <param name="immutable">Whether this brush is immutable.</param>
        protected Brush(bool immutable)
            : base(immutable)
        {
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
        /// Default brush is <see cref="SolidBrush"/> with black color.
        /// </value>
        public static Brush Default => defaultBrush ??= Brushes.Black;

        /// <summary>
        /// Gets or sets <see cref="SKPaint"/> for this brush.
        /// </summary>
        public virtual SKPaint SkiaPaint
        {
            get
            {
                paint ??= CreateSkiaPaint();
                return paint;
            }

            set
            {
                paint = value;
            }
        }

        /// <summary>
        /// Creates <see cref="Pen"/> with this brush as a parameter.
        /// </summary>
        /// <remarks>
        /// Pen created only once and saved internally in the brush instance.
        /// </remarks>
        public virtual Pen AsPen
        {
            get
            {
                if (asPen == null)
                    asPen = new Pen(AsColor);
                return asPen;
            }
        }

        /// <summary>
        /// Gets type of the brush.
        /// </summary>
        public virtual BrushType BrushType => BrushType.None;

        /// <summary>
        /// Gets color of the brush.
        /// </summary>
        public virtual Color AsColor => Color.Black;

        /// <inheritdoc/>
        protected override bool UpdateRequired
        {
            get => base.UpdateRequired;

            set
            {
                if (UpdateRequired == value)
                    return;
                base.UpdateRequired = value;
                if (value)
                {
                    SafeDispose(ref paint);
                    asPen = null;
                }
            }
        }

        /// <summary>
        /// Converts the specified <see cref='Pen'/> to a <see cref='SKPaint'/>.
        /// </summary>
        public static implicit operator SKPaint(Brush value)
        {
            value ??= Brush.Default;
            return value.SkiaPaint;
        }

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
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return "Transparent";
        }

        bool IEquatable<Brush>.Equals(Brush? other)
        {
            return Equals(other as Brush);
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
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() => base.GetHashCode();

        /// <inheritdoc/>
        protected override IBrushHandler CreateHandler()
        {
            return GraphicsFactory.Handler.CreateTransparentBrushHandler(this);
        }

        /// <summary>
        /// Creates <see cref="SKPaint"/> for this brush.
        /// </summary>
        /// <returns></returns>
        protected virtual SKPaint CreateSkiaPaint()
        {
            SKPaint result = GraphicsFactory.CreateFillPaint(AsColor);
            return result;
        }

        /// <inheritdoc/>
        protected override void UpdateHandler()
        {
        }
    }
}