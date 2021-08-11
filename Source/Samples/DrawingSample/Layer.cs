using Alternet.UI;
using System.Drawing;

namespace DrawingSample
{
    public abstract class Layer
    {
        public abstract string Name { get; }

        public abstract void Draw(DrawingContext dc, RectangleF bounds);
    }
}