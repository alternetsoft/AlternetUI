using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class NativeDateTimePickerHandler : NativeControlHandler<DateTimePicker, Native.DateTimePicker>
    {
        public DateTimePickerKind Kind
        {
            get
            {
                return (DateTimePickerKind)Enum.ToObject(
                    typeof(DateTimePickerKind), NativeControl.ValueKind);
            }

            set
            {
                NativeControl.ValueKind = (int)value;
            }
        }

        internal override Native.Control CreateNativeControl()
        {
            return new Native.DateTimePicker();
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            NativeControl.Value = (DateTime)Control.Value;

            Control.ValueChanged += Control_ValueChanged;

            NativeControl.ValueChanged += NativeControl_ValueChanged;
        }

        private void NativeControl_ValueChanged(object? sender, EventArgs e)
        {
            Control.Value = NativeControl.Value;
        }

        protected override void OnDetach()
        {
            base.OnDetach();

            Control.ValueChanged -= Control_ValueChanged;

            NativeControl.ValueChanged -= NativeControl_ValueChanged;
        }

        private void Control_ValueChanged(object? sender, System.EventArgs e)
        {
            NativeControl.Value = Control.Value;
        }
    }
}
