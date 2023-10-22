#pragma warning disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace NativeApi.Api
{
    // https://docs.wxwidgets.org/3.2/classwx_calendar_ctrl.html
    // #include <wx/calctrl.h>
    public class Calendar : Control
    {
        public bool SundayFirst { get; set; }
        public bool ShowHolidays { get; set; }
        public bool NoYearChange { get; set; }
        public bool NoMonthChange { get; set; }
        public bool SequentalMonthSelect { get; set; }
        public bool ShowSurroundWeeks { get; set; }
        public bool ShowWeekNumbers { get; set; }

        public event EventHandler? ValueChanged;

        public Alternet.UI.DateTime Value { get; set; }
        public Alternet.UI.DateTime MinValue { get; set; }
        public Alternet.UI.DateTime MaxValue { get; set; }

        public void SetRange(bool useMinValue, bool useMaxValue) { }

        public void SetHolidayColours(Color colorFg, Color colorBg) { }

        public Color GetHolidayColourFg() => default;
        public Color GetHolidayColourBg() => default;

        public void SetHeaderColours(Color colorFg, Color colorBg) { }

        public Color GetHeaderColourFg() => default;
        public Color GetHeaderColourBg() => default;

        public void SetHighlightColours(Color colorFg, Color colorBg) { }

        public Color GetHighlightColourFg() => default;
        public Color GetHighlightColourBg() => default;

        public bool AllowMonthChange() => default;
        public bool EnableMonthChange(bool enable = true) => default;

        public void Mark(int day, bool mark) { }
        public IntPtr GetAttr(int day) => default;
        public void SetAttr(int day, IntPtr calendarDateAttr) { }
        public void ResetAttr(int day) { }

        public void EnableHolidayDisplay(bool display = true) { }
        public void SetHoliday(int day) { }

        public static IntPtr GetMarkDateAttr() => default; /* real static */
        public static void SetMarkDateAttr(IntPtr dateAttr) { } /* real static */
        public static IntPtr CreateDateAttr(int border = 0) => default;

        public static void DateAttrSetTextColour(IntPtr handle, Color colText) { }
        public static void DateAttrSetBackgroundColour(IntPtr handle, Color colBack) { }
        public static void DateAttrSetBorderColour(IntPtr handle, Color color) { }
        public static void DateAttrSetFont(IntPtr handle, IntPtr font) { }
        public static void DateAttrSetBorder(IntPtr handle, int border) { }
        public static void DateAttrSetHoliday(IntPtr handle, bool holiday) { }

        public static bool DateAttrHasTextColor(IntPtr handle) => default;
        public static bool DateAttrHasBackgroundColor(IntPtr handle) => default;
        public static bool DateAttrHasBorderColor(IntPtr handle) => default;
        public static bool DateAttrHasFont(IntPtr handle) => default;
        public static bool DateAttrHasBorder(IntPtr handle) => default;

        public static bool DateAttrIsHoliday(IntPtr handle) => default;

        public static Color DateAttrGetTextColor(IntPtr handle) => default;
        public static Color DateAttrGetBackgroundColor(IntPtr handle) => default;
        public static Color DateAttrGetBorderColor(IntPtr handle) => default;
        public static IntPtr DateAttrGetFont(IntPtr handle) => default;
        public static int DateAttrGetBorder(IntPtr handle) => default;
    }
}