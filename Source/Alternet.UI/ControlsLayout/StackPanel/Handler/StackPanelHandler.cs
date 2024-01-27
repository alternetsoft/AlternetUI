using System;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal partial class StackPanelHandler : ControlHandler<StackPanel>
    {
        private OrientedLayout? layout = null;

        private OrientedLayout Layout => layout ??= CreateLayout();

        public override void OnLayout() => Layout.Layout();

        public override SizeD GetPreferredSize(SizeD availableSize) =>
            Layout.GetPreferredSize(availableSize);

        protected override void OnAttach()
        {
            base.OnAttach();

            Control.OrientationChanged += Control_OrientationChanged;
        }

        protected override void OnDetach()
        {
            Control.OrientationChanged -= Control_OrientationChanged;

            base.OnDetach();
        }

        private OrientedLayout CreateLayout()
        {
            var orientation = Control.Orientation;

            if (orientation == StackPanelOrientation.Vertical)
                return new VerticalLayout(this);
            else if (orientation == StackPanelOrientation.Horizontal)
                return new HorizontalLayout(this);
            else
                throw new InvalidOperationException();
        }

        private void Control_OrientationChanged(object? sender, EventArgs e)
        {
            Control.RaiseLayoutChanged();
            layout = null;
        }

        internal abstract class OrientedLayout
        {
            protected OrientedLayout(ControlHandler handler)
            {
                Handler = handler;
                Control = handler.Control;
            }

            public bool AllowStretch
            {
                get
                {
                    if (Control is StackPanel stackPanel)
                        return stackPanel.AllowStretch;
                    return true;
                }
            }

            public Control Control { get; }

            public ControlHandler Handler { get; }

            public abstract void Layout();

            public abstract SizeD GetPreferredSize(SizeD availableSize);
        }
    }
}