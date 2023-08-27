using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Helper class for using <see cref="Pen"/> properties in the <see cref="PropertyGrid"/>.
    /// </summary>
    public class PropertyGridAdapterPen : PropertyGridAdapterGeneric
    {
        private Color color = Color.Black;
        private PenDashStyle dashStyle = PenDashStyle.Solid;
        private LineCap lineCap = LineCap.Flat;
        private LineJoin lineJoin = LineJoin.Miter;
        private double width = 1;

        /// <summary>
        /// Returns <see cref="PropertyGridAdapterGeneric.Value"/> as <see cref="Pen"/>.
        /// </summary>
        public Pen? Pen
        {
            get => Value as Pen;
            set => Value = value;
        }

        /// <inheritdoc cref="Pen.Color"/>
        public Color Color
        {
            get
            {
                if(Pen == null)
                    return color;
                return Pen.Color;
            }

            set
            {
                if (color == value)
                    return;
                color = value;
                OnInstancePropertyChanged();
            }
        }

        /// <inheritdoc cref="Pen.DashStyle"/>
        public PenDashStyle DashStyle
        {
            get
            {
                if (Pen == null)
                    return dashStyle;
                return Pen.DashStyle;
            }

            set
            {
                if (dashStyle == value)
                    return;
                dashStyle = value;
                OnInstancePropertyChanged();
            }
        }

        /// <inheritdoc cref="Pen.LineCap"/>
        public LineCap LineCap
        {
            get
            {
                if (Pen == null)
                    return lineCap;
                return Pen.LineCap;
            }

            set
            {
                if (lineCap == value)
                    return;
                lineCap = value;
                OnInstancePropertyChanged();
            }
        }

        /// <inheritdoc cref="Pen.LineJoin"/>
        public LineJoin LineJoin
        {
            get
            {
                if (Pen == null)
                    return lineJoin;
                return Pen.LineJoin;
            }

            set
            {
                if (lineJoin == value)
                    return;
                lineJoin = value;
                OnInstancePropertyChanged();
            }
        }

        /// <inheritdoc cref="Pen.Width"/>
        public double Width
        {
            get
            {
                if (Pen == null)
                    return width;
                return Pen.Width;
            }

            set
            {
                if (value < 0)
                    value = 1;
                if (width == value)
                    return;
                width = value;
                OnInstancePropertyChanged();
            }
        }

        private void OnInstancePropertyChanged()
        {
            Pen = new(Color, Width, DashStyle, LineCap, LineJoin);
        }
    }
}