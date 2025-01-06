using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to work with calendar control.
    /// </summary>
    public interface ICalendarHandler
    {
        /// <summary>
        /// Gets or sets whether or not the first day of week is sunday.
        /// </summary>
        bool SundayFirst { get; set; }

        /// <summary>
        /// Gets or sets whether or not the first day of week is monday.
        /// </summary>
        bool MondayFirst { get; set; }

        /// <inheritdoc cref="Calendar.ShowHolidays"/>
        bool ShowHolidays { get; set; }

        /// <summary>
        /// Gets or sets whether or not current year can be changed.
        /// </summary>
        bool NoYearChange { get; set; }

        /// <inheritdoc cref="Calendar.NoMonthChange"/>
        bool NoMonthChange { get; set; }

        /// <inheritdoc cref="Calendar.SequentalMonthSelect"/>
        bool SequentalMonthSelect { get; set; }

        /// <inheritdoc cref="Calendar.ShowSurroundWeeks"/>
        bool ShowSurroundWeeks { get; set; }

        /// <inheritdoc cref="Calendar.ShowWeekNumbers"/>
        bool ShowWeekNumbers { get; set; }

        /// <inheritdoc cref="Calendar.UseGeneric"/>
        bool UseGeneric { get; set; }

        /// <inheritdoc cref="Calendar.Value"/>
        DateTime Value { get; set; }

        /// <inheritdoc cref="CustomDateEdit.MinDate"/>
        DateTime MinValue { get; set; }

        /// <inheritdoc cref="CustomDateEdit.MaxDate"/>
        DateTime MaxValue { get; set; }

        /// <inheritdoc cref="Calendar.MarkDateAttr"/>
        ICalendarDateAttr? MarkDateAttr { get; set; }

        /// <inheritdoc cref="Calendar.GetAttr"/>
        ICalendarDateAttr? GetAttr(int day);

        /// <inheritdoc cref="Calendar.SetAttr"/>
        void SetAttr(int day, ICalendarDateAttr? dateAttr);

        /// <inheritdoc cref="Calendar.CreateDateAttr"/>
        ICalendarDateAttr CreateDateAttr(CalendarDateBorder border = 0);

        /// <inheritdoc cref="Calendar.SetRange"/>
        bool SetRange(bool useMinValue, bool useMaxValue);

        /// <inheritdoc cref="Calendar.SetHolidayColors"/>
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

        /// <inheritdoc cref="Calendar.HitTest"/>
        Calendar.HitTestResult HitTest(Alternet.Drawing.PointD point);

        /// <inheritdoc cref="Calendar.SetHeaderColors"/>
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

        /// <inheritdoc cref="Calendar.AllowMonthChange"/>
        bool AllowMonthChange();

        /// <inheritdoc cref="Calendar.EnableMonthChange"/>
        bool EnableMonthChange(bool enable);

        /// <inheritdoc cref="Calendar.Mark"/>
        void Mark(int day, bool mark);

        /// <inheritdoc cref="Calendar.ResetAttr"/>
        void ResetAttr(int day);

        /// <inheritdoc cref="Calendar.EnableHolidayDisplay"/>
        void EnableHolidayDisplay(bool display);

        /// <inheritdoc cref="Calendar.SetHoliday"/>
        void SetHoliday(int day);
    }
}
