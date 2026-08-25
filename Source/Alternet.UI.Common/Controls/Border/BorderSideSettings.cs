using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies <see cref="Border"/> drawing settings for the individual side.
    /// </summary>
    public class BorderSideSettings : ImmutableObject
    {
        /// <summary>
        /// Gets or sets default border width. This value is used when border is creatded.
        /// </summary>
        public static Coord DefaultBorderWidth = 1;

        private Pen? pen;
        private Brush? brush;
        private Coord width = DefaultBorderWidth;
        private Color? color;
        private bool incStartPoint;
        private bool decLength;

        /// <summary>
        /// Gets or sets whether to draw border from the start point.
        /// </summary>
        public virtual bool IncStartPoint
        {
            get
            {
                return incStartPoint;
            }

            internal set
            {
                if (incStartPoint == value || Immutable)
                    return;
                incStartPoint = value;
                RaisePropertyChanged(nameof(IncStartPoint));
            }
        }

        /// <summary>
        /// Gets or sets the brush used to paint the border.
        /// </summary>
        public virtual Brush? Brush
        {
            get
            {
                return brush;
            }
            set
            {
                if (brush == value)
                    return;
                brush = value;
                RaisePropertyChanged(nameof(Brush));
            }
        }

        /// <summary>
        /// Gets or sets the pen used to draw the border.
        /// </summary>
        /// <remarks>Changing this property raises a property changed notification. The pen determines the
        /// color, width, and style of the outline when rendering the element.</remarks>
        public virtual Pen? Pen
        {
            get
            {
                return pen;
            }

            set
            {
                if (pen == value)
                    return;
                pen = value;
                RaisePropertyChanged(nameof(Pen));
            }
        }

        /// <summary>
        /// Gets or sets whether to draw border with full length.
        /// </summary>
        public virtual bool DecLength
        {
            get
            {
                return decLength;
            }

            internal set
            {
                if (decLength == value || Immutable)
                    return;
                decLength = value;
                RaisePropertyChanged(nameof(DecLength));
            }
        }

        /// <summary>
        /// Gets or sets border color.
        /// </summary>
        public virtual Color? Color
        {
            get
            {
                return color;
            }

            set
            {
                if (color == value || Immutable)
                    return;
                color = value;
                pen = null;
                brush = null;
                RaisePropertyChanged(nameof(Color));
            }
        }

        /// <summary>
        /// Gets or sets border width.
        /// </summary>
        public virtual Coord Width
        {
            get
            {
                return width;
            }

            set
            {
                if (width == value || Immutable)
                    return;
                pen = null;
                width = value;
                RaisePropertyChanged(nameof(Width));
            }
        }

        /// <summary>
        /// Gets <see cref="Brush"/> which can be used to draw the border.
        /// </summary>
        public virtual Brush GetBrush(Color defaultColor)
        {
            if (brush is not null)
                return brush;

            var c = color ?? defaultColor;
            var result = c.AsBrush;
            return result;
        }

        /// <summary>
        /// Gets <see cref="Pen"/> which can be used to draw the border.
        /// </summary>
        public virtual Pen GetPen(Color defaultColor)
        {
            if (pen is not null)
                return pen;

            var c = color ?? defaultColor;
            var result = c.GetAsPen(Math.Max(1, width));
            return result;
        }

        /// <summary>
        /// Assign properties from another object.
        /// </summary>
        /// <param name="value">Source of the properties to assign.</param>
        public virtual void Assign(BorderSideSettings value)
        {
            if (Immutable)
                return;
            Width = value.width;
            Color = value.color;
        }
    }
}
