using System;

namespace Alternet.UI
{
    internal class LabelHandler
        : WxControlHandler<Label, Native.Label>
    {
        internal override Native.Control CreateNativeControl()
        {
            return new Native.Label();
        }

        public override void BindEvents()
        {
            base.BindEvents();
        }

        public override void UnbindEvents()
        {
            base.UnbindEvents();
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            if (App.IsWindowsOS)
                UserPaint = true;
        }
    }
}