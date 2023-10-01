using System.ComponentModel;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class NativeLinkLabelHandler : NativeControlHandler<LinkLabel, Native.LinkLabel>
    {
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

        public string Url
        {
            get
            {
                return NativeControl.Url;
            }

            set
            {
                if (NativeControl.Url != value)
                    NativeControl.Url = value;
            }
        }

        internal override Native.Control CreateNativeControl()
        {
            return new Native.LinkLabel()
            {
                Text = Control.Text,
            };
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            NativeControl.Text = Control.Text;

            Control.TextChanged += Control_TextChanged;
            NativeControl.HyperlinkClick += NativeControl_HyperlinkClick;
        }

        protected override void OnDetach()
        {
            base.OnDetach();
            Control.TextChanged -= Control_TextChanged;
            NativeControl.HyperlinkClick -= NativeControl_HyperlinkClick;
        }

        private void NativeControl_HyperlinkClick(
            object? sender,
            System.ComponentModel.CancelEventArgs e)
        {
            Control.RaiseLinkClicked(e);
        }

        private void Control_TextChanged(object? sender, System.EventArgs? e)
        {
            if (e is null)
                throw new System.ArgumentNullException(nameof(e));

            if (NativeControl.Text != Control.Text)
                NativeControl.Text = Control.Text;
        }
    }
}