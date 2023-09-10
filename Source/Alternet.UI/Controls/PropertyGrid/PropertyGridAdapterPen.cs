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
                return color;
            }

            set
            {
                color = value;
                Save();
            }
        }

        /// <inheritdoc cref="Pen.DashStyle"/>
        public PenDashStyle DashStyle
        {
            get
            {
                return dashStyle;
            }

            set
            {
                dashStyle = value;
                Save();
            }
        }

        /// <inheritdoc cref="Pen.LineCap"/>
        public LineCap LineCap
        {
            get
            {
                return lineCap;
            }

            set
            {
                lineCap = value;
                Save();
            }
        }

        /// <inheritdoc cref="Pen.LineJoin"/>
        public LineJoin LineJoin
        {
            get
            {
                return lineJoin;
            }

            set
            {
                lineJoin = value;
                Save();
            }
        }

        /// <inheritdoc cref="Pen.Width"/>
        public double Width
        {
            get
            {
                return width;
            }

            set
            {
                if (value < 0)
                    value = 1;
                width = value;
                Save();
            }
        }

        /// <inheritdoc/>
        public override IEnumerable<IPropertyGridItem> CreateProps(IPropertyGrid propGrid)
        {
            List<IPropertyGridItem> list = new()
            {
                propGrid.CreateProperty(this, nameof(Color))!,
                propGrid.CreateProperty(this, nameof(DashStyle))!,
                propGrid.CreateProperty(this, nameof(LineCap))!,
                propGrid.CreateProperty(this, nameof(LineJoin))!,
                propGrid.CreateProperty(this, nameof(Width))!,
            };
            return list;
        }

        /// <inheritdoc/>
        protected override void Load()
        {
            if (Pen == null)
                return;
            color = Pen.Color;
            dashStyle = Pen.DashStyle;
            lineCap = Pen.LineCap;
            lineJoin = Pen.LineJoin;
            width = Pen.Width;
        }

        /// <inheritdoc/>
        protected override void Save()
        {
            Pen = new(color, width, dashStyle, lineCap, lineJoin);
        }
    }
}