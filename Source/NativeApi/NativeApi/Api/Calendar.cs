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
    public class Calendar
    {
        public Alternet.UI.DateTime Value { get; set; }
        public Alternet.UI.DateTime MinValue { get; set; }
        public Alternet.UI.DateTime MaxValue { get; set; }

        public void SetRange(bool useMinValue, bool useMaxValue) { }

        public void SetHolidayColours(Color colFg, Color colBg) { }

        public Color GetHolidayColourFg() => default;
        public Color GetHolidayColourBg() => default;

        public void SetHeaderColours(Color colFg, Color colBg) { }

        public Color GetHeaderColourFg() => default;
        public Color GetHeaderColourBg() => default;

        public void SetHighlightColours(Color colFg, Color colBg) { }

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
    }
}

/*

    wxCalendarDateAttr(const wxColour& colText = wxNullColour,
                       const wxColour& colBack = wxNullColour,
                       const wxColour& colBorder = wxNullColour,
                       const wxFont& font = wxNullFont,
                       wxCalendarDateBorder border = wxCAL_BORDER_NONE)
        : m_colText(colText), m_colBack(colBack),
          m_colBorder(colBorder), m_font(font)
    {
        Init(border);
    }
    wxCalendarDateAttr(wxCalendarDateBorder border,
                       const wxColour& colBorder = wxNullColour)
        : m_colBorder(colBorder)
    {
        Init(border);
    }

    // setters
    void SetTextColour(const wxColour& colText) { m_colText = colText; }
    void SetBackgroundColour(const wxColour& colBack) { m_colBack = colBack; }
    void SetBorderColour(const wxColour& col) { m_colBorder = col; }
    void SetFont(const wxFont& font) { m_font = font; }
    void SetBorder(wxCalendarDateBorder border) { m_border = border; }
    void SetHoliday(bool holiday) { m_holiday = holiday; }

    // accessors
    bool HasTextColour() const { return m_colText.IsOk(); }
    bool HasBackgroundColour() const { return m_colBack.IsOk(); }
    bool HasBorderColour() const { return m_colBorder.IsOk(); }
    bool HasFont() const { return m_font.IsOk(); }
    bool HasBorder() const { return m_border != wxCAL_BORDER_NONE; }

    bool IsHoliday() const { return m_holiday; }

    const wxColour& GetTextColour() const { return m_colText; }
    const wxColour& GetBackgroundColour() const { return m_colBack; }
    const wxColour& GetBorderColour() const { return m_colBorder; }
    const wxFont& GetFont() const { return m_font; }
    wxCalendarDateBorder GetBorder() const { return m_border; }

    // get or change the "mark" attribute, i.e. the one used for the items
    // marked with wxCalendarCtrl::Mark()
    static const wxCalendarDateAttr& GetMark() { return m_mark; }
    static void SetMark(wxCalendarDateAttr const& m) { m_mark = m; }
 
 
 */
