using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    public class PlessPenHandler : DisposableObject, IPenHandler
    {
        private Color color = Color.Black;
        private DashStyle dashStyle;
        private LineCap lineCap;
        private LineJoin lineJoin;
        private double width;

        public PlessPenHandler(Pen pen)
        {
        }

        public Color Color => color;

        public DashStyle DashStyle => dashStyle;

        public LineCap LineCap => lineCap;

        public LineJoin LineJoin => lineJoin;

        public double Width => width;

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
