using Alternet.Drawing;

namespace Alternet.UI
{
    internal partial class StackPanelHandler
    {
        private abstract class OrientedLayout
        {
            protected OrientedLayout(StackPanelHandler handler)
            {
                Handler = handler;
                Control = handler.Control;
            }

            public StackPanel Control { get; }

            public StackPanelHandler Handler { get; }

            public abstract void Layout();

            public abstract Size GetPreferredSize(Size availableSize);

            public class AlignedPosition
            {
                public AlignedPosition(double origin, double size)
                {
                    Origin = origin;
                    Size = size;
                }

                public double Origin { get; }

                public double Size { get; }
            }
        }
    }
}