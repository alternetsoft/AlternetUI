using System;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal partial class StackPanelHandler : ControlHandler<StackPanel>
    {
        private OrientedLayout? layout = null;

        OrientedLayout Layout => layout ??= CreateLayout();

        public override void OnLayout() => Layout.Layout();

        public override SizeF GetPreferredSize(SizeF availableSize) => Layout.GetPreferredSize(availableSize);

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
            layout = null;
        }
    }
}