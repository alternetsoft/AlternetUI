using System;

namespace Alternet.Drawing
{
    /// <summary>
    /// Defines an object used to draw lines and curves.
    /// </summary>
    /// <remarks>
    /// A <see cref="Pen"/> draws a line of specified width and style.
    /// Use the <see cref="DashStyle"/> property to draw several varieties of dashed lines.
    /// </remarks>
    public sealed class Pen : IDisposable, IEquatable<Pen>
    {
        private bool isDisposed;
        private Color color;
        private PenDashStyle dashStyle;
        private double width;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pen"/> class with the specified
        /// <see cref="Color"/>, <see cref="Width"/> and <see cref="DashStyle"/> properties.
        /// </summary>
        /// <param name="color">A <see cref="Color"/> structure that indicates the color of this <see cref="Pen"/>.</param>
        /// <param name="width">A value indicating the width of this <see cref="Pen"/>, in device-independent units (1/96th inch per unit).</param>
        /// <param name="dashStyle">A style used for dashed lines drawn with this <see cref="Pen"/>.</param>
        public Pen(Color color, double width, PenDashStyle dashStyle) : this(color, width, dashStyle, immutable: false)
        {
        }

        internal Pen(Color color, double width, PenDashStyle dashStyle, bool immutable) : this(new UI.Native.Pen())
        {
            DashStyle = dashStyle;
            Color = color;
            Width = width;

            ReinitializeNativePen();
            this.immutable = true;
        }

        private bool immutable;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pen"/> class with the specified
        /// <see cref="Color"/> and <see cref="Width"/>.
        /// </summary>
        /// <param name="color">A <see cref="Color"/> structure that indicates the color of this <see cref="Pen"/>.</param>
        /// <param name="width">A value indicating the width of this <see cref="Pen"/>, in device-independent units (1/96th inch per unit).</param>
        public Pen(Color color, double width) : this(color, width, PenDashStyle.Solid)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pen"/> class with the specified <see cref="Color"/>.
        /// </summary>
        /// <param name="color">A <see cref="Color"/> structure that indicates the color of this <see cref="Pen"/>.</param>
        public Pen(Color color) : this(color, 1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pen"/> class with the specified <see cref="Brush"/>.
        /// </summary>
        /// <param name="brush">A <see cref="Brush"/> that indicates the color of this <see cref="Pen"/>.</param>
        public Pen(Brush brush) :
            this(
                brush as SolidBrush != null ?
                    ((SolidBrush)brush).Color :
                    throw new ArgumentException("Only SolidBrush instances are supported for Pen initialization."))
        {
        }

        private Pen(UI.Native.Pen nativePen)
        {
            NativePen = nativePen;
        }

        /// <summary>
        /// Gets or sets the color of this <see cref="Pen"/>.
        /// </summary>
        /// <value>A <see cref="Drawing.Color"/> structure that represents the color for this <see cref="Pen"/>.</value>
        public Color Color
        {
            get => color;

            set
            {
                CheckModificationOfImmutableObject();
                if (color == value)
                    return;

                color = value;
                ReinitializeNativePen();
            }
        }

        /// <summary>
        /// Gets or sets the style used for dashed lines drawn with this <see cref="Pen"/>.
        /// </summary>
        /// <value>One of the <see cref="PenDashStyle"/> values that represents the dash style of this <see cref="Pen"/>.</value>
        public PenDashStyle DashStyle
        {
            get => dashStyle;

            set
            {
                CheckModificationOfImmutableObject();
                if (dashStyle == value)
                    return;

                dashStyle = value;
                ReinitializeNativePen();
            }
        }

        /// <summary>
        /// Gets or sets the width of this <see cref="Pen"/>, in device-independent units (1/96th inch per unit).
        /// </summary>
        /// <value>The width of this <see cref="Pen"/>, in device-independent units (1/96th inch per unit).</value>
        public double Width
        {
            get => width;

            set
            {
                CheckModificationOfImmutableObject();
                if (width == value)
                    return;

                width = value;
                ReinitializeNativePen();
            }
        }

        private void CheckModificationOfImmutableObject()
        {
            if (immutable)
                throw new InvalidOperationException("Cannot change an immutable pen.");
        }

        internal UI.Native.Pen NativePen { get; private set; }

        /// <summary>
        /// Returns a value that indicates whether the two objects are equal.
        /// </summary>
        public static bool operator ==(Pen? a, Pen? b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        /// <summary>
        /// Returns a value that indicates whether the two objects are not equal.
        /// </summary>
        public static bool operator !=(Pen? a, Pen? b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Releases all resources used by this <see cref="Pen"/>.
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
            return HashCode.Combine(Color, DashStyle, Width);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        public bool Equals(Pen? o)
        {
            if (o == null)
                return false;

            CheckDisposed();
            return Color == o.Color && DashStyle == o.DashStyle && Width == o.Width;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            CheckDisposed();
            return $"Pen ({Color}, {Width}, {DashStyle})";
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified object is equal to the current object; otherwise, <c>false</c>.</returns>
        public override bool Equals(object? obj)
        {
            var pen = obj as Pen;

            if (ReferenceEquals(pen, null))
                return false;

            if (GetType() != obj?.GetType())
                return false;

            return Equals(pen);
        }

        private void ReinitializeNativePen()
        {
            NativePen.Initialize((UI.Native.PenDashStyle)DashStyle, Color, Width);
        }

        /// <summary>
        /// Throws <see cref="ObjectDisposedException"/> if the object has been disposed.
        /// </summary>
        private void CheckDisposed()
        {
            if (isDisposed)
                throw new ObjectDisposedException(null);
        }

        /// <inheritdoc/>
        private void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (immutable)
                    throw new InvalidOperationException("Cannot dispose an immutable pen.");

                if (disposing)
                {
                    NativePen.Dispose();
                    NativePen = null!;
                }

                isDisposed = true;
            }
        }
    }
}