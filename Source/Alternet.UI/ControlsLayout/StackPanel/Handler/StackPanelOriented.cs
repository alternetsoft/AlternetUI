using Alternet.Drawing;

namespace Alternet.UI
{
    internal partial class StackPanelHandler
    {
        internal abstract class OrientedLayout
        {
            protected OrientedLayout(ControlHandler handler)
            {
                Handler = handler;
                Control = handler.Control;
            }

            public Control Control { get; }

            public ControlHandler Handler { get; }

            public abstract void Layout();

            public abstract SizeD GetPreferredSize(SizeD availableSize);
        }
    }
}