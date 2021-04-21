using System;
using System.ComponentModel;

namespace Alternet.UI
{
    internal class NativeWindowHandler : NativeControlHandler<Window, Native.Window>
    {
        internal override Native.Control CreateNativeControl()
        {
            return new Native.Window();
        }

        protected override void OnAttach()
        {
            base.OnAttach();
            Control.TitleChanged += Control_TitleChanged;
            NativeControl.Closing += Control_Closing;
        }

        private void Control_Closing(object? sender, CancelEventArgs? e)
        {
            if (e is null)
                throw new System.ArgumentNullException(nameof(e));

            // todo: add close reason/force parameter (see wxCloseEvent.CanVeto()).
            Control.InvokeClosing(e);
            if (e.Cancel)
                return;

            Control.InvokeClosed(EventArgs.Empty);
            Control.Dispose();
        }

        protected override void OnDetach()
        {
            NativeControl.Closing -= Control_Closing;
            Control.TitleChanged -= Control_TitleChanged;

            base.OnDetach();
        }

        private void Control_TitleChanged(object? sender, System.EventArgs? e)
        {
            if (e is null)
                throw new System.ArgumentNullException(nameof(e));

            NativeControl.Title = Control.Title;
        }
    }
}