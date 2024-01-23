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
        /// <summary>
        /// Gets or sets a value indicating whether the control has a border.
        /// </summary>
        public bool HasBorder
        {
            get => NativeControl.HasBorder;
            set => NativeControl.HasBorder = value;
        }

        public DateTimePickerPopupKind PopupKind
        {
            get
            {
                return (DateTimePickerPopupKind)Enum.ToObject(
                    typeof(DateTimePickerPopupKind), NativeControl.PopupKind);
            }

            set
            {
                NativeControl.PopupKind = (int)value;
            }
        }

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

        public void SetRange(DateTime min, DateTime max, bool useMin, bool useMax)
        {
            NativeControl.MinValue = min;
            NativeControl.MaxValue = max;
            NativeControl.SetRange(useMin, useMax);
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

        protected override void OnDetach()
        {
            base.OnDetach();

            Control.ValueChanged -= Control_ValueChanged;

            NativeControl.ValueChanged -= NativeControl_ValueChanged;
        }

        private void NativeControl_ValueChanged(object? sender, EventArgs e)
        {
            Control.Value = NativeControl.Value;
        }

        private void Control_ValueChanged(object? sender, System.EventArgs e)
        {
            NativeControl.Value = Control.Value;
        }
    }
}
