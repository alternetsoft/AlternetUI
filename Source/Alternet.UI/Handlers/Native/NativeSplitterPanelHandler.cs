using System;
using System.Collections.Generic;
using System.Linq;

namespace Alternet.UI
{
    internal class NativeSplitterPanelHandler : ControlHandler
    {
        public new Native.SplitterPanel NativeControl =>
            (Native.SplitterPanel)base.NativeControl!;

        public override IEnumerable<Control> AllChildrenIncludedInLayout
            => Enumerable.Empty<Control>();

        internal override Native.Control CreateNativeControl()
        {
            return new Native.SplitterPanel();
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            NativeControl.SplitterSashPosChanging +=
                NativeControl_SplitterSashPosChanging;
            NativeControl.SplitterSashPosChanged +=
                NativeControl_SplitterSashPosChanged;
            NativeControl.Unsplit += NativeControl_Unsplit;
            NativeControl.SplitterDoubleClick += NativeControl_SplitterDoubleClick;
        }

        protected override void OnDetach()
        {
            base.OnDetach();

            NativeControl.SplitterSashPosChanging -=
               NativeControl_SplitterSashPosChanging;
            NativeControl.SplitterSashPosChanged -=
                NativeControl_SplitterSashPosChanged;
            NativeControl.Unsplit -= NativeControl_Unsplit;
            NativeControl.SplitterDoubleClick -= NativeControl_SplitterDoubleClick;
        }

        private void NativeControl_SplitterDoubleClick(object sender, EventArgs e)
        {
        }

        private void NativeControl_Unsplit(object sender, EventArgs e)
        {
        }

        private void NativeControl_SplitterSashPosChanged(object sender, EventArgs e)
        {
        }

        private void NativeControl_SplitterSashPosChanging(
            object sender,
            EventArgs e)
        {
        }
    }
}