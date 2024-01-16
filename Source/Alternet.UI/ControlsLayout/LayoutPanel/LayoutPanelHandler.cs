using System.Collections.Generic;
using System.Linq;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class LayoutPanelHandler : ControlHandler
    {
        private StackPanelHandler.VerticalLayout? vertLayout;
        private StackPanelHandler.HorizontalLayout? horzLayout;

        public new LayoutPanel Control => (LayoutPanel)base.Control;

        /*public override IEnumerable<Control> AllChildrenIncludedInLayout
            => Enumerable.Empty<Control>();*/

        public StackPanelHandler.VerticalLayout VertLayout
        {
            get
            {
                return vertLayout ??= new StackPanelHandler.VerticalLayout(this);
            }
        }

        public StackPanelHandler.HorizontalLayout HorzLayout
        {
            get
            {
                return horzLayout ??= new StackPanelHandler.HorizontalLayout(this);
            }
        }

        public override void OnLayout()
        {
            switch (Control.Layout)
            {
                case GenericLayoutStyle.Default:
                case GenericLayoutStyle.DockStyle:
                default:
                    LayoutPanel.PerformDockStyleLayout(Control);
                    break;
                case GenericLayoutStyle.Native:
                    break;
                case GenericLayoutStyle.Control:
                    UI.Control.PerformDefaultLayout(Control);
                    break;
                case GenericLayoutStyle.VerticalStack:
                    VertLayout.Layout();
                    break;
                case GenericLayoutStyle.HorizontalStack:
                    HorzLayout.Layout();
                    break;
            }
        }

        public override SizeD GetPreferredSize(SizeD availableSize)
        {
            switch (Control.Layout)
            {
                case GenericLayoutStyle.Default:
                case GenericLayoutStyle.DockStyle:
                case GenericLayoutStyle.Native:
                default:
                    return base.GetPreferredSize(availableSize);
                case GenericLayoutStyle.Control:
                    return UI.Control.GetPreferredSizeDefaultLayout(Control, availableSize);
                case GenericLayoutStyle.VerticalStack:
                    return VertLayout.GetPreferredSize(availableSize);
                case GenericLayoutStyle.HorizontalStack:
                    return HorzLayout.GetPreferredSize(availableSize);
            }
        }

        internal override Native.Control CreateNativeControl()
        {
            return new Native.Panel();
        }
    }
}