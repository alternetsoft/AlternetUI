using System;
using System.Collections.Generic;
using System.Linq;

namespace Alternet.UI
{
    internal class NativeLayoutPanelHandler : ControlHandler
    {
        public new Native.Panel NativeControl =>
            (Native.Panel)base.NativeControl!;

        public override IEnumerable<Control> AllChildrenIncludedInLayout
            => Enumerable.Empty<Control>();

        public override void OnLayout()
        {
            DefaultLayout.Layout(Control);
        }

        internal override Native.Control CreateNativeControl()
        {
            return new Native.Panel();
        }

        protected override void OnAttach()
        {
            base.OnAttach();
        }

        protected override void OnDetach()
        {
            base.OnDetach();
        }
    }
}