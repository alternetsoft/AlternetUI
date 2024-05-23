using System;

namespace Alternet.UI
{
    internal class CheckBoxHandler : NativeControlHandler<CheckBox, Native.CheckBox>, ICheckBoxHandler
    {
        public CheckBoxHandler()
        {
        }

        public CheckState CheckState
        {
            get
            {
                return (CheckState)NativeControl.CheckState;
            }

            set
            {
                NativeControl.CheckState = (int)value;
            }
        }

        public bool AllowAllStatesForUser
        {
            get
            {
                return NativeControl.AllowAllStatesForUser;
            }

            set
            {
                NativeControl.AllowAllStatesForUser = value;
            }
        }

        public bool AlignRight
        {
            get
            {
                return NativeControl.AlignRight;
            }

            set
            {
                NativeControl.AlignRight = value;
            }
        }

        public bool ThreeState
        {
            get
            {
                return NativeControl.ThreeState;
            }

            set
            {
                NativeControl.ThreeState = value;
            }
        }

        internal override Native.Control CreateNativeControl()
        {
            return new Native.CheckBox();
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            NativeControl.Text = Control.Text;
            NativeControl.IsChecked = Control.IsChecked;

            Control.TextChanged += Control_TextChanged;
            Control.CheckedChanged += Control_CheckedChanged;
            NativeControl.CheckedChanged = NativeControl_CheckedChanged;
        }

        protected override void OnDetach()
        {
            base.OnDetach();

            Control.TextChanged -= Control_TextChanged;
            Control.CheckedChanged -= Control_CheckedChanged;
            NativeControl.CheckedChanged = null;
        }

        private void Control_CheckedChanged(object? sender, System.EventArgs? e)
        {
        }

        private void NativeControl_CheckedChanged()
        {
            Control.RaiseCheckedChanged(EventArgs.Empty);
        }

        private void Control_TextChanged(object? sender, System.EventArgs? e)
        {
            NativeControl.Text = Control.Text;
        }
    }
}