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

        public string Text
        {
            get => NativeControl.Text;
            set => NativeControl.Text = value;
        }

        public string Url
        {
            get
            {
                return NativeControl.Url;
            }

            set
            {
                NativeControl.Url = value;
            }
        }

        public Color HoverColor
        {
            get => NativeControl.HoverColor;
            set => NativeControl.HoverColor = value;
        }

        public Color NormalColor
        {
            get => NativeControl.NormalColor;
            set => NativeControl.NormalColor = value;
        }

        public Color VisitedColor
        {
            get => NativeControl.VisitedColor;
            set => NativeControl.VisitedColor = value;
        }

        public bool Visited
        {
            get => NativeControl.Visited;
            set => NativeControl.Visited = value;
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