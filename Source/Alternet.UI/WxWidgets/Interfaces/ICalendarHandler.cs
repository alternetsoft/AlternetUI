using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to work with calendar control.
    /// </summary>
    internal interface ICalendarHandler
    {
        /// <summary>
        /// Gets or sets whether or not the first day of week is Sunday.
        /// </summary>
        bool SundayFirst { get; set; }

        /// <summary>
        /// Gets or sets whether or not the first day of week is Monday.
        /// </summary>
        bool MondayFirst { get; set; }

        bool ShowHolidays { get; set; }

        /// <summary>
        /// Gets or sets whether or not current year can be changed.
        /// </summary>
        bool NoYearChange { get; set; }

        bool NoMonthChange { get; set; }

        bool SequentialMonthSelect { get; set; }

        bool ShowSurroundWeeks { get; set; }

        /// <inheritdoc cref="Calendar.ShowWeekNumbers"/>
        bool ShowWeekNumbers { get; set; }

        bool UseGeneric { get; set; }

        DateTime Value { get; set; }

        /// <inheritdoc cref="CustomDateEdit.MinDate"/>
        DateTime MinValue { get; set; }

        /// <inheritdoc cref="CustomDateEdit.MaxDate"/>
        DateTime MaxValue { get; set; }

        ICalendarDateAttr? MarkDateAttr { get; set; }

        ICalendarDateAttr? GetAttr(int day);

        void SetAttr(int day, ICalendarDateAttr? dateAttr);

        ICalendarDateAttr CreateDateAttr(CalendarDateBorder border = 0);

        bool SetRange(bool useMinValue, bool useMaxValue);

        void SetHolidayColors(Alternet.Drawing.Color colorFg, Alternet.Drawing.Color colorBg);

        /// <summary>
        /// Gets holidays foreground color.
        /// </summary>
        /// <returns></returns>
        Alternet.Drawing.Color GetHolidayColorFg();

        /// <summary>
        /// Gets holidays background color.
        /// </summary>
        /// <returns></returns>
        Alternet.Drawing.Color GetHolidayColorBg();

        Calendar.HitTestResult HitTest(Alternet.Drawing.PointD point);

        void SetHeaderColors(Alternet.Drawing.Color colorFg, Alternet.Drawing.Color colorBg);

        /// <summary>
        /// Get header foreground color.
        /// </summary>
        /// <returns></returns>
        Alternet.Drawing.Color GetHeaderColorFg();

        /// <summary>
        /// Get header background color.
        /// </summary>
        /// <returns></returns>
        Alternet.Drawing.Color GetHeaderColorBg();

        /// <inheritdoc cref="Calendar.SetHighlightColors"/>
        void SetHighlightColors(Alternet.Drawing.Color colorFg, Alternet.Drawing.Color colorBg);

        /// <summary>
        /// Get highlight foreground color.
        /// </summary>
        /// <returns></returns>
        Alternet.Drawing.Color GetHighlightColorFg();

        /// <summary>
        /// Get highlight background color.
        /// </summary>
        /// <returns></returns>
        Alternet.Drawing.Color GetHighlightColorBg();

        bool AllowMonthChange();

        /// Sets value of <see cref="NoMonthChange"/> property.
        bool SetNoMonthChange(bool enable);

        void Mark(int day, bool mark);

        void ResetAttr(int day);

        void EnableHolidayDisplay(bool display);

        void SetHoliday(int day);
    }
}
