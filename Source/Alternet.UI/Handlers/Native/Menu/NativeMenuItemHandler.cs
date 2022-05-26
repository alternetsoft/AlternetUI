using System;

namespace Alternet.UI
{
    internal class NativeMenuItemHandler : NativeControlHandler<MenuItem, Native.MenuItem>
    {
        internal override Native.Control CreateNativeControl()
        {
            return new Native.MenuItem();
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            //NativeControl.Text = Control.Text;

            //Control.TextChanged += Control_TextChanged;
            //NativeControl.Click += NativeControl_Click;
        }

        protected override void OnDetach()
        {
            base.OnDetach();

            //Control.TextChanged -= Control_TextChanged;
            //NativeControl.Click -= NativeControl_Click;
        }

        internal void EnsureNativeSubmenuCreated()
        {
            throw new NotImplementedException();
        }

        //private void NativeControl_Click(object? sender, System.EventArgs? e)
        //{
        //    if (e is null)
        //        throw new System.ArgumentNullException(nameof(e));

        //    Control.RaiseClick(e);
        //}

        //private void Control_TextChanged(object? sender, System.EventArgs? e)
        //{
        //    if (e is null)
        //        throw new System.ArgumentNullException(nameof(e));

        //    NativeControl.Text = Control.Text;
        //}
    }
}