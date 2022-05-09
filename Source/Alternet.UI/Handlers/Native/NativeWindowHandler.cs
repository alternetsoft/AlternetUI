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

            ApplyTitle();
            ApplyShowInTaskbar();
            ApplyOwner();
            ApplyMaximizeEnabled();
            ApplyMinimizeEnabled();
            ApplyCloseEnabled();

            Control.TitleChanged += Control_TitleChanged;
            Control.ShowInTaskbarChanged += Control_ShowInTaskbarChanged;
            Control.OwnerChanged += Control_OwnerChanged;
            Control.MinimizeEnabledChanged += Control_MinimizeEnabledChanged;
            Control.MaximizeEnabledChanged += Control_MaximizeEnabledChanged;
            Control.CloseEnabledChanged += Control_CloseEnabledChanged;
            
            NativeControl.Closing += Control_Closing;
            NativeControl.SizeChanged += NativeControl_SizeChanged;
        }

        private void Control_CloseEnabledChanged(object sender, EventArgs e)
        {
            ApplyCloseEnabled();
        }

        private void Control_MaximizeEnabledChanged(object sender, EventArgs e)
        {
            ApplyMaximizeEnabled();
        }

        private void Control_MinimizeEnabledChanged(object sender, EventArgs e)
        {
            ApplyMinimizeEnabled();
        }

        private void ApplyOwner()
        {
            var newOwner = Control.Owner?.Handler?.NativeControl;
            var oldOwner = NativeControl.ParentRefCounted;
            if (newOwner == oldOwner)
                return;

            if (oldOwner != null)
                oldOwner.RemoveChild(NativeControl);

            if (newOwner == null)
                return;

            newOwner.AddChild(NativeControl);
        }

        private void Control_OwnerChanged(object sender, EventArgs e)
        {
            ApplyOwner();
        }

        private void Control_ShowInTaskbarChanged(object? sender, EventArgs e)
        {
            if (e is null)
                throw new System.ArgumentNullException(nameof(e));

            ApplyShowInTaskbar();
        }

        private void ApplyShowInTaskbar()
        {
            NativeControl.ShowInTaskbar = Control.ShowInTaskbar;
        }

        private void ApplyMinimizeEnabled()
        {
            NativeControl.MinimizeEnabled = Control.MinimizeEnabled;
        }

        private void ApplyMaximizeEnabled()
        {
            NativeControl.MaximizeEnabled = Control.MaximizeEnabled;
        }

        private void ApplyCloseEnabled()
        {
            NativeControl.CloseEnabled = Control.CloseEnabled;
        }

        private void NativeControl_SizeChanged(object? sender, EventArgs e)
        {
            PerformLayout();
        }

        private void Control_Closing(object? sender, CancelEventArgs? e)
        {
            if (e is null)
                throw new System.ArgumentNullException(nameof(e));

            // todo: add close reason/force parameter (see wxCloseEvent.CanVeto()).
            var closingEventArgs = new WindowClosingEventArgs(e.Cancel);
            Control.RaiseClosing(closingEventArgs);
            if (closingEventArgs.Cancel)
                return;

            Control.RaiseClosed(new WindowClosedEventArgs());
            Control.Dispose();
        }

        protected override void OnDetach()
        {
            NativeControl.SizeChanged -= NativeControl_SizeChanged;
            NativeControl.Closing -= Control_Closing;
            
            Control.OwnerChanged -= Control_OwnerChanged;
            Control.TitleChanged -= Control_TitleChanged;
            Control.ShowInTaskbarChanged -= Control_ShowInTaskbarChanged;
            Control.MinimizeEnabledChanged -= Control_MinimizeEnabledChanged;
            Control.MaximizeEnabledChanged -= Control_MaximizeEnabledChanged;
            Control.CloseEnabledChanged -= Control_CloseEnabledChanged;

            base.OnDetach();
        }

        private void Control_TitleChanged(object? sender, System.EventArgs? e)
        {
            if (e is null)
                throw new System.ArgumentNullException(nameof(e));

            ApplyTitle();
        }

        private void ApplyTitle()
        {
            NativeControl.Title = Control.Title;
        }
    }
}