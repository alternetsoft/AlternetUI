using System;

namespace Alternet.UI
{
    internal class NativeSplitterPanelHandler : ControlHandler
    {
        public new Native.SplitterPanel NativeControl =>
            (Native.SplitterPanel)base.NativeControl!;

        internal override Native.Control CreateNativeControl()
        {
            return new Native.SplitterPanel();
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            /*NativeControl.Minimum = Control.Minimum;
            NativeControl.Maximum = Control.Maximum;
            Control.TickFrequencyChanged += Control_TickFrequencyChanged;

            NativeControl.ValueChanged += NativeControl_ValueChanged;*/
        }

        protected override void OnDetach()
        {
            base.OnDetach();

        }

    }
}