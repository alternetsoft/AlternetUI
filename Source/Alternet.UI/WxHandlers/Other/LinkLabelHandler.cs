using System.ComponentModel;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class LinkLabelHandler
        : WxControlHandler<LinkLabel, Native.LinkLabel>, ILinkLabelHandler
    {
        public LinkLabelHandler()
        {
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
    }
}