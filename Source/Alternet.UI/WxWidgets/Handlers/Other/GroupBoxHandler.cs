using System;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class GroupBoxHandler : NativeControlHandler<GroupBox, Native.GroupBox>, IGroupBoxHandler
    {
        public GroupBoxHandler()
        {
        }

        public int GetTopBorderForSizer()
        {
            return NativeControl.GetTopBorderForSizer();
        }

        public int GetOtherBorderForSizer()
        {
            return NativeControl.GetOtherBorderForSizer();
        }

        internal override Native.Control CreateNativeControl()
        {
            return new Native.GroupBox();
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            NativeControl.Title = Control.Title;

            Control.TitleChanged += Control_TitleChanged;
        }

        protected override void OnDetach()
        {
            base.OnDetach();

            Control.TitleChanged -= Control_TitleChanged;
        }

        private void Control_TitleChanged(object? sender, EventArgs e)
        {
            NativeControl.Title = Control.Title;
        }
    }
}