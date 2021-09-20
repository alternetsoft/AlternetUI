using Alternet.Drawing;

namespace Alternet.UI
{
    internal partial class StackPanelHandler
    {
        abstract class OrientedLayout
        {
            protected OrientedLayout(StackPanelHandler handler)
            {
                Handler = handler;
                Control = handler.Control;
            }

            public class AlignedPosition
            {
                public AlignedPosition(float origin, float size)
                {
                    Origin = origin;
                    Size = size;
                }

                public float Origin { get; }
                public float Size { get; }
            }

            public StackPanel Control { get; }
            public StackPanelHandler Handler { get; }

            public abstract void Layout();
            public abstract SizeF GetPreferredSize(SizeF availableSize);
        }
    }
}