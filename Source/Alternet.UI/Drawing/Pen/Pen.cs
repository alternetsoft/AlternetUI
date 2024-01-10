using System;
using System.ComponentModel;
using Alternet.UI;
using Alternet.UI.Localization;

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
        private static Pen? defaultPen;

        private readonly bool immutable;
        private bool isDisposed;
        private Color color;
        private DashStyle dashStyle;
        private LineCap lineCap;
        private LineJoin lineJoin;
        private double width;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pen"/> class with the specified
        /// <see cref="Color"/>, <see cref="Width"/> and <see cref="DashStyle"/> properties.
        /// </summary>
        /// <param name="color">A <see cref="Color"/> structure that indicates the color of this
        /// <see cref="Pen"/>.</param>
        /// <param name="width">A value indicating the width of this <see cref="Pen"/>, in
        /// device-independent units (1/96th inch per unit).</param>
        /// <param name="dashStyle">A style used for dashed lines drawn with this
        /// <see cref="Pen"/>.</param>
        public Pen(Color color, double width, DashStyle dashStyle)
            : this(color, width, dashStyle, LineCap.Flat, LineJoin.Miter)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pen"/> class with the specified
        /// <see cref="Color"/>, <see cref="Width"/> and <see cref="DashStyle"/> properties.
        /// </summary>
        /// <param name="color">A <see cref="Color"/> structure that indicates the color of
        /// this <see cref="Pen"/>.</param>
        /// <param name="width">A value indicating the width of this <see cref="Pen"/>, in
        /// device-independent units (1/96th inch per unit).</param>
        /// <param name="dashStyle">A style used for dashed lines drawn with this
        /// <see cref="Pen"/>.</param>
        /// <param name="lineCap">Specifies the available cap styles with which a
        /// <see cref="Pen"/> object can end a line.</param>
        /// <param name="lineJoin">Specifies how to join consecutive line or curve segments.</param>
        public Pen(
            Color color,
            double width,
            DashStyle dashStyle,
            LineCap lineCap,
            LineJoin lineJoin)
            : this(color, width, dashStyle, lineCap, lineJoin, immutable: false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pen"/> class with the specified
        /// <see cref="Color"/> and <see cref="Width"/>.
        /// </summary>
        /// <param name="color">A <see cref="Color"/> structure that indicates the color of this
        /// <see cref="Pen"/>.</param>
        /// <param name="width">A value indicating the width of this <see cref="Pen"/>, in
        /// device-independent units (1/96th inch per unit).</param>
        public Pen(Color color, double width)
            : this(color, width, DashStyle.Solid)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pen"/> class with the specified
        /// <see cref="Color"/>.
        /// </summary>
        /// <param name="color">A <see cref="Color"/> structure that indicates the color
        /// of this <see cref="Pen"/>.</param>
        public Pen(Color color)
            : this(color, 1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pen"/> class with the specified
        /// <see cref="Brush"/>.
        /// </summary>
        /// <param name="brush">A <see cref="Brush"/> that indicates the color of this
        /// <see cref="Pen"/>.</param>
        public Pen(Brush brush)
            : this(brush as SolidBrush != null ?
                    ((SolidBrush)brush).Color :
                    throw new ArgumentException(
                        ErrorMessages.Default.OnlySolidBrushInstancesSupported))
        {
        }

        internal Pen(
            Color color,
            double width,
            DashStyle dashStyle,
            LineCap lineCap,
            LineJoin lineJoin,
            bool immutable)
        {
            NativePen = new UI.Native.Pen();
            this.color = color;
            this.width = width;
            this.dashStyle = dashStyle;
            this.lineCap = lineCap;
            this.lineJoin = lineJoin;

            this.immutable = immutable;

            ReinitializeNativePen();
        }

        /// <summary>
        /// Gets the default pen.
        /// </summary>
        /// <value>
        /// Default pen has width equal to 1 and Black color.
        /// </value>
        public static Pen Default => defaultPen ??= new(Color.Black);

        /// <summary>
        /// Gets whether this object is immutable (properties are readonly).
        /// </summary>
        [Browsable(false)]
        public bool Immutable => immutable;

        /// <summary>
        /// Gets or sets the color of this <see cref="Pen"/>.
        /// </summary>
        /// <value>A <see cref="Drawing.Color"/> structure that represents the color for this
        /// <see cref="Pen"/>.</value>
        /// <remarks>
        /// If <see cref="Pen"/> is immutable (for example <see cref="Pens.Black"/>),
        /// this property is readonly.
        /// </remarks>
        /// <remarks>
        /// Default value is <see cref="Color.Black"/>.
        /// </remarks>
        public Color Color
        {
            get => color;

            set
            {
                if (color == value || immutable)
                    return;
                color = value;
                ReinitializeNativePen();
            }
        }

        /// <summary>
        /// Creates <see cref="SolidBrush"/> with <see cref="Color"/> of this pen.
        /// </summary>
        public SolidBrush AsBrush => new(Color);

        /// <summary>
        /// Gets or sets the style used for dashed lines drawn with this <see cref="Pen"/>.
        /// </summary>
        /// <value>One of the <see cref="Drawing.DashStyle"/> values that represents the dash style
        /// of this <see cref="Pen"/>.</value>
        /// <remarks>
        /// If <see cref="Pen"/> is immutable (for example <see cref="Pens.Black"/>),
        /// this property is readonly.
        /// </remarks>
        /// <remarks>
        /// Default value is <see cref="DashStyle.Solid"/>.
        /// </remarks>
        public DashStyle DashStyle
        {
            get => dashStyle;

            set
            {
                if (dashStyle == value || immutable)
                    return;
                dashStyle = value;
                ReinitializeNativePen();
            }
        }

        /// <summary>
        /// Specifies the available cap styles with which a <see cref="Pen"/> object can end a line.
        /// </summary>
        /// <remarks>
        /// If <see cref="Pen"/> is immutable (for example <see cref="Pens.Black"/>),
        /// this property is readonly.
        /// </remarks>
        /// <remarks>
        /// Default value is <see cref="LineCap.Flat"/>.
        /// </remarks>
        public LineCap LineCap
        {
            get => lineCap;

            set
            {
                if (lineCap == value || immutable)
                    return;
                lineCap = value;
                ReinitializeNativePen();
            }
        }

        /// <summary>
        /// Specifies how to join consecutive line or curve segments.
        /// </summary>
        /// <remarks>
        /// If <see cref="Pen"/> is immutable (for example <see cref="Pens.Black"/>),
        /// this property is readonly.
        /// </remarks>
        /// <remarks>
        /// Default value is <see cref="LineJoin.Miter"/>.
        /// </remarks>
        public LineJoin LineJoin
        {
            get => lineJoin;

            set
            {
                if (lineJoin == value || immutable)
                    return;
                lineJoin = value;
                ReinitializeNativePen();
            }
        }

        /// <summary>
        /// Gets or sets the width of this <see cref="Pen"/>, in device-independent units
        /// (1/96th inch per unit).
        /// </summary>
        /// <value>The width of this <see cref="Pen"/>, in device-independent units
        /// (1/96th inch per unit).</value>
        /// <remarks>
        /// If <see cref="Pen"/> is immutable (for example <see cref="Pens.Black"/>),
        /// this property is readonly.
        /// </remarks>
        /// <remarks>
        /// Default value is 1.
        /// </remarks>
        public double Width
        {
            get => width;

            set
            {
                if (width == value || immutable)
                    return;
                width = value;
                ReinitializeNativePen();
            }
        }

        internal UI.Native.Pen NativePen { get; private set; }

        /// <summary>
        /// Returns a value that indicates whether the two objects are equal.
        /// </summary>
        public static bool operator ==(Pen? a, Pen? b)
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
        /// <returns><c>true</c> if the specified object is equal to the current object;
        /// otherwise, <c>false</c>.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is not Pen pen)
                return false;

            if (GetType() != obj?.GetType())
                return false;

            return Equals(pen);
        }

        private void ReinitializeNativePen()
        {
            NativePen.Initialize(
                (UI.Native.PenDashStyle)DashStyle,
                Color,
                Width,
                (UI.Native.LineCap)LineCap,
                (UI.Native.LineJoin)LineJoin);
        }

        /// <summary>
        /// Throws <see cref="ObjectDisposedException"/> if the object has been disposed.
        /// </summary>
        private void CheckDisposed()
        {
            if (isDisposed)
                throw new ObjectDisposedException(null);
        }

        private void Dispose(bool disposing)
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
                    NativePen.Dispose();
                    NativePen = null!;
                }

                isDisposed = true;
            }
        }
    }
}