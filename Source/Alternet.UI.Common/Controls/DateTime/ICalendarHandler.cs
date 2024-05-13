using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public interface ICalendarHandler : IControlHandler
    {
        bool SundayFirst { get; set; }

        bool MondayFirst { get; set; }

        bool ShowHolidays { get; set; }

        bool NoYearChange { get; set; }

        bool NoMonthChange { get; set; }

        bool SequentalMonthSelect { get; set; }

        bool ShowSurroundWeeks { get; set; }

        bool ShowWeekNumbers { get; set; }

        bool UseGeneric { get; set; }

        bool HasBorder { get; set; }

        DateTime Value { get; set; }

        DateTime MinValue { get; set; }

        DateTime MaxValue { get; set; }

        ICalendarDateAttr? MarkDateAttr { get; set; }

        ICalendarDateAttr? GetAttr(int day);

        void SetAttr(int day, ICalendarDateAttr? dateAttr);

        ICalendarDateAttr CreateDateAttr(CalendarDateBorder border = 0);

        bool SetRange(bool useMinValue, bool useMaxValue);

        void SetHolidayColors(Alternet.Drawing.Color colorFg, Alternet.Drawing.Color colorBg);

        Alternet.Drawing.Color GetHolidayColorFg();

        Alternet.Drawing.Color GetHolidayColorBg();

        Calendar.HitTestResult HitTest(Alternet.Drawing.PointD point);

        void SetHeaderColors(Alternet.Drawing.Color colorFg, Alternet.Drawing.Color colorBg);

        Alternet.Drawing.Color GetHeaderColorFg();

        Alternet.Drawing.Color GetHeaderColorBg();

        void SetHighlightColors(Alternet.Drawing.Color colorFg, Alternet.Drawing.Color colorBg);

        Alternet.Drawing.Color GetHighlightColorFg();

        Alternet.Drawing.Color GetHighlightColorBg();

        bool AllowMonthChange();

        bool EnableMonthChange(bool enable);

        void Mark(int day, bool mark);

        void ResetAttr(int day);

        void EnableHolidayDisplay(bool display);

        void SetHoliday(int day);
    }
}
