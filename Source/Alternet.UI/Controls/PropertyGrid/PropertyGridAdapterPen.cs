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
                color = value;
                UpdateInstanceProperty();
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
                dashStyle = value;
                UpdateInstanceProperty();
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
                lineCap = value;
                UpdateInstanceProperty();
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
                lineJoin = value;
                UpdateInstanceProperty();
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
                width = value;
                UpdateInstanceProperty();
            }
        }

        /// <inheritdoc/>
        protected override void UpdateInstanceProperty()
        {
            Pen = new(color, width, dashStyle, lineCap, lineJoin);
        }
    }
}