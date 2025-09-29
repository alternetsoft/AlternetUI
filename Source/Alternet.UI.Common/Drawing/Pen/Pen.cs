using System;

using Alternet.UI;
using Alternet.UI.Extensions;

using SkiaSharp;

namespace Alternet.Drawing
{
    /// <summary>
    /// Defines an object used to draw lines and curves.
    /// </summary>
    /// <remarks>
    /// A <see cref="Pen"/> draws a line of specified width and style.
    /// Use the <see cref="DashStyle"/> property to draw several varieties of dashed lines.
    /// </remarks>
    public class Pen : HandledObject<IPenHandler>, IEquatable<Pen>
    {
        private static Pen? defaultPen;

        private Color color;
        private DashStyle dashStyle;
        private LineCap lineCap;
        private LineJoin lineJoin;
        private Coord width;
        private SKPaint? paint;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pen"/> class with the specified
        /// <see cref="Color"/>, <see cref="Width"/> and <see cref="DashStyle"/> properties.
        /// </summary>
        /// <param name="color">A <see cref="Color"/> structure that indicates the color of this
        /// <see cref="Pen"/>.</param>
        /// <param name="width">A value indicating the width of this <see cref="Pen"/>, in
        /// device-independent units.</param>
        /// <param name="dashStyle">A style used for dashed lines drawn with this
        /// <see cref="Pen"/>.</param>
        public Pen(Color color, Coord width, DashStyle dashStyle)
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
        /// device-independent units.</param>
        /// <param name="dashStyle">A style used for dashed lines drawn with this
        /// <see cref="Pen"/>.</param>
        /// <param name="lineCap">Specifies the available cap styles with which a
        /// <see cref="Pen"/> object can end a line.</param>
        /// <param name="lineJoin">Specifies how to join consecutive line or curve segments.</param>
        public Pen(
            Color color,
            Coord width,
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
        /// device-independent units.</param>
        public Pen(Color color, Coord width)
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
            : this(brush.AsColor)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pen"/> class with
        /// the specified color, width, dash style, line cap, and line join settings.
        /// </summary>
        /// <param name="color">The color of the pen used for drawing.</param>
        /// <param name="width">The width of the pen, typically measured
        /// in device-independent units.</param>
        /// <param name="dashStyle">The dash style of the pen, which determines
        /// the pattern of dashes and gaps.</param>
        /// <param name="lineCap">The style of the ends of lines drawn with the pen.</param>
        /// <param name="lineJoin">The style of the joints between connected lines or segments.</param>
        /// <param name="immutable">A value indicating whether the pen is immutable.
        /// If <see langword="true"/>, the pen cannot be modified
        /// after creation; otherwise, it can be modified.</param>
        public Pen(
            Color color,
            Coord width,
            DashStyle dashStyle,
            LineCap lineCap,
            LineJoin lineJoin,
            bool immutable)
            : base(immutable)
        {
            this.color = color;
            this.width = width;
            this.dashStyle = dashStyle;
            this.lineCap = lineCap;
            this.lineJoin = lineJoin;
        }

        /// <summary>
        /// Gets the default pen.
        /// </summary>
        /// <value>
        /// Default pen has width equal to 1 and Black color.
        /// </value>
        public static Pen Default => defaultPen ??= new(Color.Black);

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
        public virtual Color Color
        {
            get => color;

            set
            {
                if (color == value || Immutable)
                    return;
                color = value;
                UpdateRequired = true;
            }
        }

        /// <summary>
        /// Gets this pen as <see cref="SKPaint"/>.
        /// </summary>
        /// <returns></returns>
        public virtual SKPaint SkiaPaint
        {
            get
            {
                return paint ??= GraphicsFactory.PenToPaint(this);
            }

            set
            {
                paint = value;
            }
        }

        /// <summary>
        /// Gets <see cref="SolidBrush"/> with <see cref="Color"/> of this pen.
        /// Uses <see cref="Color.AsBrush"/> internally.
        /// </summary>
        public virtual SolidBrush AsBrush => Color.AsBrush;

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
        public virtual DashStyle DashStyle
        {
            get => dashStyle;

            set
            {
                if (dashStyle == value || Immutable)
                    return;
                dashStyle = value;
                UpdateRequired = true;
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
        public virtual LineCap LineCap
        {
            get => lineCap;

            set
            {
                if (lineCap == value || Immutable)
                    return;
                lineCap = value;
                UpdateRequired = true;
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
        public virtual LineJoin LineJoin
        {
            get => lineJoin;

            set
            {
                if (lineJoin == value || Immutable)
                    return;
                lineJoin = value;
                UpdateRequired = true;
            }
        }

        /// <summary>
        /// Gets or sets the width of this <see cref="Pen"/>, in device-independent units.
        /// </summary>
        /// <value>The width of this <see cref="Pen"/>, in device-independent units.</value>
        /// <remarks>
        /// If <see cref="Pen"/> is immutable (for example <see cref="Pens.Black"/>),
        /// this property is readonly.
        /// </remarks>
        /// <remarks>
        /// Default value is 1.
        /// </remarks>
        public virtual Coord Width
        {
            get => width;

            set
            {
                if (width == value || Immutable)
                    return;
                width = value;
                UpdateRequired = true;
            }
        }

        /// <inheritdoc/>
        protected override bool UpdateRequired
        {
            get
            {
                return base.UpdateRequired;
            }

            set
            {
                if (UpdateRequired == value)
                    return;
                base.UpdateRequired = value;
                if (value)
                {
                    SafeDispose(ref paint);
                }
            }
        }

        /// <summary>
        /// Converts the specified <see cref='Pen'/> to a <see cref='SKPaint'/>.
        /// </summary>
        public static implicit operator SKPaint(Pen value)
        {
            value ??= Pen.Default;
            return value.SkiaPaint;
        }

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
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return (Color, DashStyle, Width).GetHashCode();
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        public bool Equals(Pen? o)
        {
            if (o == null)
                return false;
            return Color == o.Color && DashStyle == o.DashStyle && Width == o.Width;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
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

        /// <summary>
        /// Creates native pen.
        /// </summary>
        /// <returns></returns>
        protected override IPenHandler CreateHandler()
        {
            return GraphicsFactory.Handler.CreatePenHandler(this);
        }

        /// <summary>
        /// Updates native pen.
        /// </summary>
        protected override void UpdateHandler()
        {
            Handler.Update(this);
        }
    }
}