using System.ComponentModel;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class NativeLinkLabelHandler : NativeControlHandler<LinkLabel, Native.LinkLabel>
    {
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