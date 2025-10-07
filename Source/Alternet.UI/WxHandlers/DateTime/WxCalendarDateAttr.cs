using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class WxCalendarDateAttr : DisposableObject<IntPtr>, ICalendarDateAttr
    {
        private bool immutable;

        public WxCalendarDateAttr(IntPtr handle, bool disposeHandle)
            : base(handle, disposeHandle)
        {
        }

        public override bool Immutable
        {
            get => immutable;
        }

        public Color? TextColor
        {
            get
            {
                if (HasTextColor)
                    return Native.Calendar.DateAttrGetTextColor(Handle);
                return null;
            }

            set
            {
                if (immutable)
                    return;
                if(value is null)
                    Native.Calendar.DateAttrSetTextColor(Handle, Color.Empty);
                else
                    Native.Calendar.DateAttrSetTextColor(Handle, value);
            }
        }

        public Color? BackgroundColor
        {
            get
            {
                if (HasBackgroundColor)
                    return Native.Calendar.DateAttrGetBackgroundColor(Handle);
                return null;
            }

            set
            {
                if (immutable)
                    return;
                if (value is null)
                    Native.Calendar.DateAttrSetBackgroundColor(Handle, Color.Empty);
                else
                    Native.Calendar.DateAttrSetBackgroundColor(Handle, value);
            }
        }

        public Color? BorderColor
        {
            get
            {
                if (HasBorderColor)
                    return Native.Calendar.DateAttrGetBorderColor(Handle);
                return null;
            }

            set
            {
                if (immutable)
                    return;
                if (value is null)
                    Native.Calendar.DateAttrSetBorderColor(Handle, Color.Empty);
                else
                    Native.Calendar.DateAttrSetBorderColor(Handle, value);
            }
        }

        public bool IsHoliday
        {
            get
            {
                return Native.Calendar.DateAttrIsHoliday(Handle);
            }

            set
            {
                if (immutable)
                    return;
                Native.Calendar.DateAttrSetHoliday(Handle, value);
            }
        }

        public CalendarDateBorder Border
        {
            get
            {
                return (CalendarDateBorder)Native.Calendar.DateAttrGetBorder(Handle);
            }

            set
            {
                if (immutable)
                    return;
                Native.Calendar.DateAttrSetBorder(Handle, (int)value);
            }
        }

        public bool HasTextColor
        {
            get
            {
                return Native.Calendar.DateAttrHasTextColor(Handle);
            }
        }

        public bool HasBackgroundColor
        {
            get
            {
                return Native.Calendar.DateAttrHasBackgroundColor(Handle);
            }
        }

        public bool HasBorderColor
        {
            get
            {
                return Native.Calendar.DateAttrHasBorderColor(Handle);
            }
        }

        public bool HasFont
        {
            get
            {
                return Native.Calendar.DateAttrHasFont(Handle);
            }
        }

        public bool HasBorder
        {
            get
            {
                return Native.Calendar.DateAttrHasBorder(Handle);
            }
        }

        public void SetImmutable(bool value)
        {
            immutable = value;
        }

        /// <inheritdoc/>
        protected override void DisposeUnmanaged()
        {
            Native.Calendar.DeleteDateAttr(Handle);
        }
    }
}