using System.ComponentModel;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class LinkLabelHandler
        : NativeControlHandler<LinkLabel, Native.LinkLabel>, ILinkLabelHandler
    {
        public LinkLabelHandler()
        {
        }

        internal override Native.Control CreateNativeControl()
        {
            return new Native.LinkLabel();
        }

        protected override void OnAttach()
        {
            base.OnAttach();
            NativeControl.HyperlinkClick += NativeControl_HyperlinkClick;
        }

        protected override void OnDetach()
        {
            base.OnDetach();
            NativeControl.HyperlinkClick -= NativeControl_HyperlinkClick;
        }

        private void NativeControl_HyperlinkClick(
            object? sender,
            System.ComponentModel.CancelEventArgs e)
        {
            Control.RaiseLinkClicked(e);
        }
    }
}