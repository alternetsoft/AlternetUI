using System;

namespace Alternet.UI
{
    internal class WxCheckBoxHandler : WxControlHandler<CheckBox, Native.CheckBox>, ICheckBoxHandler
    {
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

            if (App.IsWindowsOS)
                UserPaint = true;
        }
    }
}