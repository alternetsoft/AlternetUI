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
            var c = color ?? defaultColor;

            brush ??= c.AsBrush;
            return brush;
        }

        /// <summary>
        /// Gets <see cref="Pen"/> which can be used to draw the border.
        /// </summary>
        public virtual Pen GetPen(Color defaultColor)
        {
            var c = color ?? defaultColor;

            pen ??= c.GetAsPen(Math.Max(1, width));
            return pen;
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
