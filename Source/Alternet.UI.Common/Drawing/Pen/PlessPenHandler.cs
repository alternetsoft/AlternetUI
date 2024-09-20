using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Platformless pen handler.
    /// </summary>
    public class PlessPenHandler : DisposableObject, IPenHandler
    {
        private Color color = Color.Black;
        private DashStyle dashStyle;
        private LineCap lineCap;
        private LineJoin lineJoin;
        private Coord width;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlessPenHandler"/> class.
        /// </summary>
        /// <param name="pen">Owner.</param>
        public PlessPenHandler(Pen pen)
        {
        }

        /// <inheritdoc cref="Pen.Color"/>
        public Color Color => color;

        /// <inheritdoc cref="Pen.DashStyle"/>
        public DashStyle DashStyle => dashStyle;

        /// <inheritdoc cref="Pen.LineCap"/>
        public LineCap LineCap => lineCap;

        /// <inheritdoc cref="Pen.LineJoin"/>
        public LineJoin LineJoin => lineJoin;

        /// <inheritdoc cref="Pen.Width"/>
        public Coord Width => width;

        /// <inheritdoc/>
        public virtual void Update(Pen pen)
        {
            color = pen.Color;
            dashStyle = pen.DashStyle;
            lineCap = pen.LineCap;
            lineJoin = pen.LineJoin;
            width = pen.Width;
        }
    }
}
