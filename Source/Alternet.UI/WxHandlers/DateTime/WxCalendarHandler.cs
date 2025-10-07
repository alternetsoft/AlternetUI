using System;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal class WxCalendarHandler : WxControlHandler, ICalendarHandler
    {
        static WxCalendarHandler()
        {
            Application.BeforeNativeLogMessage += Application_BeforeNativeLogMessage;

            static void Application_BeforeNativeLogMessage(object? sender, LogMessageEventArgs e)
            {
                const string s1 = "'MonthCal_SetDayState'";

                if (e.Message?.Contains(s1) ?? false)
                    e.Cancel = true;
            }
        }

        public WxCalendarHandler()
        {
        }

        public ICalendarDateAttr? MarkDateAttr
        {
            get
            {
                var result = Native.Calendar.GetMarkDateAttr();
                if (result == default)
                    return null;
                var resultManaged = new WxCalendarDateAttr(result, true);
                resultManaged.SetImmutable(true);
                return resultManaged;
            }

            set
            {
                if (value is null)
                    Native.Calendar.SetMarkDateAttr(default);
                else
                    Native.Calendar.SetMarkDateAttr(((WxCalendarDateAttr)value).Handle);
            }
        }

        public bool SundayFirst
        {
            get
            {
                return NativeControl.SundayFirst;
            }

            set
            {
                NativeControl.SundayFirst = value;
            }
        }

        public bool MondayFirst
        {
            get
            {
                return NativeControl.MondayFirst;
            }

            set
            {
                NativeControl.MondayFirst = value;
            }
        }

        public bool ShowHolidays
        {
            get
            {
                return NativeControl.ShowHolidays;
            }

            set
            {
                NativeControl.ShowHolidays = value;
            }
        }

        public bool NoYearChange
        {
            get
            {
                return NativeControl.NoYearChange;
            }

            set
            {
                NativeControl.NoYearChange = value;
            }
        }

        public bool NoMonthChange
        {
            get
            {
                return NativeControl.NoMonthChange;
            }

            set
            {
                NativeControl.NoMonthChange = value;
            }
        }

        public bool SequentialMonthSelect
        {
            get
            {
                return NativeControl.SequentalMonthSelect;
            }

            set
            {
                NativeControl.SequentalMonthSelect = value;
            }
        }

        public bool ShowSurroundWeeks
        {
            get
            {
                return NativeControl.ShowSurroundWeeks;
            }

            set
            {
                NativeControl.ShowSurroundWeeks = value;
            }
        }

        public bool ShowWeekNumbers
        {
            get
            {
                return NativeControl.ShowWeekNumbers;
            }

            set
            {
                NativeControl.ShowWeekNumbers = value;
            }
        }

        public bool UseGeneric
        {
            get
            {
                return NativeControl.UseGeneric;
            }

            set
            {
                NativeControl.UseGeneric = value;
            }
        }

        public override bool HasBorder
        {
            get
            {
                return NativeControl.HasBorder;
            }

            set
            {
                NativeControl.HasBorder = value;
            }
        }

        public DateTime Value
        {
            get
            {
                return NativeControl.Value;
            }

            set
            {
                NativeControl.Value = value;
            }
        }

        public DateTime MinValue
        {
            get
            {
                return NativeControl.MinValue;
            }

            set
            {
                NativeControl.MinValue = value;
            }
        }

        public DateTime MaxValue
        {
            get
            {
                return NativeControl.MaxValue;
            }

            set
            {
                NativeControl.MaxValue = value;
            }
        }

        public new Native.Calendar NativeControl => (Native.Calendar)base.NativeControl!;

        public new Calendar? Control => (Calendar?)base.Control;

        public bool SetRange(bool useMinValue, bool useMaxValue)
        {
            return NativeControl.SetRange(useMinValue, useMaxValue);
        }

        public void SetHolidayColors(Color colorFg, Color colorBg)
        {
            NativeControl.SetHolidayColors(colorFg, colorBg);
        }

        public Color GetHolidayColorFg()
        {
            return NativeControl.GetHolidayColorFg();
        }

        public Color GetHolidayColorBg()
        {
            return NativeControl.GetHolidayColorBg();
        }

        public Calendar.HitTestResult HitTest(PointD point)
        {
            if (App.IsLinuxOS || Control is null)
                return Calendar.HitTestResult.None;

            var pointi = Control.PixelFromDip(point);
            var result = NativeControl.HitTest(pointi);
            return (Calendar.HitTestResult)result;
        }

        public void SetHeaderColors(Color colorFg, Color colorBg)
        {
            NativeControl.SetHeaderColors(colorFg, colorBg);
        }

        public Color GetHeaderColorFg()
        {
            return NativeControl.GetHeaderColorFg();
        }

        public Color GetHeaderColorBg()
        {
            return NativeControl.GetHeaderColorBg();
        }

        public void SetHighlightColors(Color colorFg, Color colorBg)
        {
            NativeControl.SetHighlightColors(colorFg, colorBg);
        }

        public Color GetHighlightColorFg()
        {
            return NativeControl.GetHighlightColorFg();
        }

        public Color GetHighlightColorBg()
        {
            return NativeControl.GetHighlightColorBg();
        }

        public bool AllowMonthChange()
        {
            return NativeControl.AllowMonthChange();
        }

        public bool SetNoMonthChange(bool enable)
        {
            return NativeControl.EnableMonthChange(!enable);
        }

        public void Mark(int day, bool mark)
        {
            NativeControl.Mark(day, mark);
        }

        public void ResetAttr(int day)
        {
            NativeControl.ResetAttr(day);
        }

        public void EnableHolidayDisplay(bool display)
        {
            NativeControl.EnableHolidayDisplay(display);
        }

        public void SetHoliday(int day)
        {
            NativeControl.SetHoliday(day);
        }

        public ICalendarDateAttr? GetAttr(int day)
        {
            var result = NativeControl.GetAttr(day);
            if (result == default)
                return null;
            return new WxCalendarDateAttr(result, false);
        }

        public void SetAttr(int day, ICalendarDateAttr? dateAttr)
        {
            if (dateAttr is null)
                NativeControl.ResetAttr(day);
            else
                NativeControl.SetAttr(day, ((WxCalendarDateAttr)dateAttr).Handle);
        }

        public ICalendarDateAttr CreateDateAttr(CalendarDateBorder border = 0)
        {
            var result = Native.Calendar.CreateDateAttr((int)border);
            return new WxCalendarDateAttr(result, true);
        }

        internal override Native.Control CreateNativeControl()
        {
            return new Native.Calendar();
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            if (App.IsWindowsOS)
                UserPaint = true;
        }
    }
}