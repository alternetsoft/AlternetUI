using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class CalendarDateAttr : DisposableObject, ICalendarDateAttr
    {
        public CalendarDateAttr(IntPtr handle, bool disposeHandle)
            : base(handle, disposeHandle)
        {
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
                if(value is null)
                    Native.Calendar.DateAttrSetTextColor(Handle, Color.Empty);
                else
                    Native.Calendar.DateAttrSetTextColor(Handle, value.Value);
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
                if (value is null)
                    Native.Calendar.DateAttrSetBackgroundColor(Handle, Color.Empty);
                else
                    Native.Calendar.DateAttrSetBackgroundColor(Handle, value.Value);
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
                if (value is null)
                    Native.Calendar.DateAttrSetBorderColor(Handle, Color.Empty);
                else
                    Native.Calendar.DateAttrSetBorderColor(Handle, value.Value);
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
    }
}

