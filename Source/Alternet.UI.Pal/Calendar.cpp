#include "Calendar.h"

namespace Alternet::UI
{
    Calendar::Calendar()
    {

    }

    class wxCalendarCtrl2 : public wxCalendarCtrl
    {
    public:
        wxCalendarCtrl2(){}
        wxCalendarCtrl2(wxWindow* parent,
            wxWindowID id,
            const wxDateTime& date = wxDefaultDateTime,
            const wxPoint& pos = wxDefaultPosition,
            const wxSize& size = wxDefaultSize,
            long style = wxCAL_SHOW_HOLIDAYS,
            const wxString& name = wxASCII_STR(wxCalendarNameStr))
            : wxCalendarCtrl(parent, id, date, pos, size, style, name)
        {
        }
    };

    class wxGenericCalendarCtrl2 : public wxGenericCalendarCtrl
    {
    public:
        wxGenericCalendarCtrl2(){}
        wxGenericCalendarCtrl2(wxWindow* parent,
            wxWindowID id,
            const wxDateTime& date = wxDefaultDateTime,
            const wxPoint& pos = wxDefaultPosition,
            const wxSize& size = wxDefaultSize,
            long style = wxCAL_SHOW_HOLIDAYS,
            const wxString& name = wxASCII_STR(wxCalendarNameStr))
            : wxGenericCalendarCtrl(parent, id, date, pos, size, style, name)
        {
        }
    };

    bool Calendar::GetHasBorder()
    {
        return _hasBorder;
    }

    void Calendar::SetHasBorder(bool value)
    {
        if (_hasBorder == value)
            return;
        _hasBorder = value;
        RecreateWxWindowIfNeeded();
    }

    void Calendar::RecreateWxWindowIfNeeded()
    {
        Control::RecreateWxWindowIfNeeded();
    }

    wxWindow* Calendar::CreateWxWindowUnparented()
    {
        if (_useGeneric)
        {
            return new wxGenericCalendarCtrl2();
        }
        else
        {
            return new wxCalendarCtrl2();
        }
    }

    wxWindow* Calendar::CreateWxWindowCore(wxWindow* parent)
    {
        long style = 0;

        if (!_hasBorder)
            style = style | wxBORDER_NONE;

        if (_showHolidays)
            style |= wxCAL_SHOW_HOLIDAYS;
        
        if (_sundayFirst)
            style |= wxCAL_SUNDAY_FIRST;
        else
        if (_mondayFirst)
            style |= wxCAL_MONDAY_FIRST;
        
        if (_noMonthChange)
            style |= wxCAL_NO_MONTH_CHANGE;
        if (_sequentalMonthSelect)
            style |= wxCAL_SEQUENTIAL_MONTH_SELECTION;
        if (_showSurroundWeeks)
            style |= wxCAL_SHOW_SURROUNDING_WEEKS;
        if (_showWeekNumbers)
            style |= wxCAL_SHOW_WEEK_NUMBERS;

        wxWindow* window;

        if (_useGeneric)
        {
            window = new wxGenericCalendarCtrl2(parent,
                wxID_ANY,
                wxDefaultDateTime,
                wxDefaultPosition,
                wxDefaultSize,
                style);
        }
        else
        {
            window = new wxCalendarCtrl2(parent,
                wxID_ANY,
                wxDefaultDateTime,
                wxDefaultPosition,
                wxDefaultSize,
                style);
        }

        window->Bind(wxEVT_CALENDAR_DOUBLECLICKED, &Calendar::OnEventDoubleClick, this);
        window->Bind(wxEVT_CALENDAR_SEL_CHANGED, &Calendar::OnEventSelChanged, this);
        window->Bind(wxEVT_CALENDAR_PAGE_CHANGED, &Calendar::OnEventPageChanged, this);
        window->Bind(wxEVT_CALENDAR_WEEKDAY_CLICKED, &Calendar::OnEventDayHeaderClick, this);
        window->Bind(wxEVT_CALENDAR_WEEK_CLICKED, &Calendar::OnEventWeekNumberClick, this);
        return window;
    }

    wxCalendarCtrlBase* Calendar::GetCalendar()
    {
        return dynamic_cast<wxCalendarCtrlBase*>(GetWxWindow());
    }

    Calendar::~Calendar()
    {
        if (IsWxWindowCreated())
        {
            auto window = GetWxWindow();
            if (window != nullptr)
            {
                window->Unbind(wxEVT_CALENDAR_DOUBLECLICKED, &Calendar::OnEventDoubleClick, this);
                window->Unbind(wxEVT_CALENDAR_SEL_CHANGED, &Calendar::OnEventSelChanged, this);
                window->Unbind(wxEVT_CALENDAR_PAGE_CHANGED, &Calendar::OnEventPageChanged, this);
                window->Unbind(wxEVT_CALENDAR_WEEKDAY_CLICKED, &Calendar::OnEventDayHeaderClick, this);
                window->Unbind(wxEVT_CALENDAR_WEEK_CLICKED, &Calendar::OnEventWeekNumberClick, this);
            }
        }
    }

    void Calendar::OnEventDoubleClick(wxCalendarEvent& event)
    {
        event.Skip();
        RaiseEvent(CalendarEvent::DayDoubleClick);
    }

    void Calendar::OnEventSelChanged(wxCalendarEvent& event)
    {
        event.Skip();
        RaiseEvent(CalendarEvent::SelectionChanged);
    }

    void Calendar::OnEventPageChanged(wxCalendarEvent& event)
    {
        event.Skip();
        RaiseEvent(CalendarEvent::PageChanged);
    }

    void Calendar::OnEventWeekNumberClick(wxCalendarEvent& event)
    {
        event.Skip();
        RaiseEvent(CalendarEvent::WeekNumberClick);
    }

    void Calendar::OnEventDayHeaderClick(wxCalendarEvent& event)
    {
        event.Skip();
        RaiseEvent(CalendarEvent::DayHeaderClick);
    }

    bool Calendar::GetUseGeneric()
    {
        return _useGeneric;
    }

    void Calendar::SetUseGeneric(bool value)
    {
        if (_useGeneric == value)
            return;
        _useGeneric = value;
        RecreateWxWindowIfNeeded();
    }

    bool Calendar::GetMondayFirst()
    {
        return _mondayFirst;
    }

    void Calendar::SetMondayFirst(bool value)
    {
        if (_mondayFirst == value)
            return;
        _mondayFirst = value;
        RecreateWxWindowIfNeeded();
    }

    bool Calendar::GetSundayFirst()
    {
        return _sundayFirst;
    }

    void Calendar::SetSundayFirst(bool value)
    {
        if (_sundayFirst == value)
            return;
        _sundayFirst = value;
        RecreateWxWindowIfNeeded();
    }

    bool Calendar::GetShowHolidays()
    {
        return _showHolidays;
    }

    void Calendar::SetShowHolidays(bool value)
    {
        if (_showHolidays == value)
            return;
        _showHolidays = value;
        RecreateWxWindowIfNeeded();
    }

    bool Calendar::GetNoYearChange()
    {
        return _noYearChange;
    }

    void Calendar::SetNoYearChange(bool value)
    {
        if (_noYearChange == value)
            return;
        _noYearChange = value;
        RecreateWxWindowIfNeeded();
    }

    bool Calendar::GetNoMonthChange()
    {
        return _noMonthChange;
    }

    void Calendar::SetNoMonthChange(bool value)
    {
        if (_noMonthChange == value)
            return;
        _noMonthChange = value;
        RecreateWxWindowIfNeeded();
    }

    bool Calendar::GetSequentalMonthSelect()
    {
        return _sequentalMonthSelect;
    }

    void Calendar::SetSequentalMonthSelect(bool value)
    {
        if (_sequentalMonthSelect == value)
            return;
        _sequentalMonthSelect = value;
        RecreateWxWindowIfNeeded();
    }

    bool Calendar::GetShowSurroundWeeks()
    {
        return _showSurroundWeeks;
    }

    void Calendar::SetShowSurroundWeeks(bool value)
    {
        if (_showSurroundWeeks == value)
            return;
        _showSurroundWeeks = value;
        RecreateWxWindowIfNeeded();
    }

    bool Calendar::GetShowWeekNumbers()
    {
        return _showWeekNumbers;
    }

    void Calendar::SetShowWeekNumbers(bool value)
    {
        if (_showWeekNumbers == value)
            return;
        _showWeekNumbers = value;
        RecreateWxWindowIfNeeded();
    }

    DateTime Calendar::GetValue()
    {
        return GetCalendar()->GetDate();
    }

    void Calendar::SetValue(const DateTime& value)
    {
        GetCalendar()->SetDate(value);
    }

    DateTime Calendar::GetMinValue()
    {
        return _minValue;
    }

    void Calendar::SetMinValue(const DateTime& value)
    {
        _minValue = value;
    }

    DateTime Calendar::GetMaxValue()
    {
        return _maxValue;
    }

    void Calendar::SetMaxValue(const DateTime& value)
    {
        _maxValue = value;
    }

    bool Calendar::SetRange(bool useMinValue, bool useMaxValue)
    {
        wxDateTime wxdt1 = wxDefaultDateTime;
        if (useMinValue)
            wxdt1 = _minValue;

        wxDateTime wxdt2 = wxDefaultDateTime;
        if (useMaxValue)
            wxdt2 = _maxValue;

        return GetCalendar()->SetDateRange(wxdt1, wxdt2);
    }

    Color Calendar::GetHolidayColorFg()
    {
        return GetCalendar()->GetHolidayColourFg();
    }

    Color Calendar::GetHolidayColorBg()
    {
        return GetCalendar()->GetHolidayColourBg();
    }

    Color Calendar::GetHeaderColorFg()
    {
        return GetCalendar()->GetHeaderColourFg();
    }

    Color Calendar::GetHeaderColorBg()
    {
        return GetCalendar()->GetHeaderColourBg();
    }

    Color Calendar::GetHighlightColorFg()
    {
        return GetCalendar()->GetHighlightColourFg();
    }

    Color Calendar::GetHighlightColorBg()
    {
        return GetCalendar()->GetHighlightColourBg();
    }

    bool Calendar::AllowMonthChange()
    {
        return GetCalendar()->AllowMonthChange();
    }

    bool Calendar::EnableMonthChange(bool enable)
    {
        return GetCalendar()->EnableMonthChange(enable);
    }

    void Calendar::Mark(int day, bool mark)
    {
        GetCalendar()->Mark(day, mark);
    }

    void Calendar::SetHighlightColors(const Color& colorFg, const Color& colorBg)
    {
        GetCalendar()->SetHighlightColours(colorFg, colorBg);
    }

    void Calendar::SetHolidayColors(const Color& colorFg, const Color& colorBg)
    {
        GetCalendar()->SetHolidayColours(colorFg, colorBg);
    }

    void Calendar::SetHeaderColors(const Color& colorFg, const Color& colorBg)
    {
        GetCalendar()->SetHeaderColours(colorFg, colorBg);
    }

    void* Calendar::GetAttr(int day)
    {
        return GetCalendar()->GetAttr(day);
    }

    void Calendar::SetAttr(int day, void* calendarDateAttr)
    {
        if (calendarDateAttr == nullptr)
        {
            ResetAttr(day);
            return;
        }

        auto cloned = CloneDateAttr(*(wxCalendarDateAttr*)calendarDateAttr);

        GetCalendar()->SetAttr(day, cloned);
    }

    void Calendar::ResetAttr(int day)
    {
        GetCalendar()->ResetAttr(day);
    }

    void Calendar::EnableHolidayDisplay(bool display)
    {
        GetCalendar()->EnableHolidayDisplay(display);
    }

    void Calendar::SetHoliday(int day)
    {
        GetCalendar()->SetHoliday(day);
    }

    void* Calendar::GetMarkDateAttr()
    {
        auto& mark = wxCalendarDateAttr::GetMark();
        return CloneDateAttr(mark);
    }

    wxCalendarDateAttr* Calendar::CloneDateAttr(const wxCalendarDateAttr& attr)
    {
        return new wxCalendarDateAttr(
            attr.GetTextColour(),
            attr.GetBackgroundColour(),
            attr.GetBorderColour(),
            attr.GetFont(),
            attr.GetBorder());
    }

    wxCalendarDateAttr markDateAttr;

    void Calendar::SetMarkDateAttr(void* dateAttr)
    {
        auto attr = (wxCalendarDateAttr*)dateAttr;

        markDateAttr = wxCalendarDateAttr(
            attr->GetTextColour(),
            attr->GetBackgroundColour(),
            attr->GetBorderColour(),
            attr->GetFont(),
            attr->GetBorder());

        wxCalendarDateAttr::SetMark(markDateAttr);
    }

    void* Calendar::CreateDateAttr(int border)
    {
        return new wxCalendarDateAttr(border);
    }

    void Calendar::DeleteDateAttr(void* handle)
    {
        delete (wxCalendarDateAttr*)handle;
    }

    void Calendar::DateAttrSetTextColor(void* handle, const Color& colText)
    {
        ((wxCalendarDateAttr*)handle)->SetTextColour(colText);
    }

    void Calendar::DateAttrSetBackgroundColor(void* handle, const Color& colBack)
    {
        ((wxCalendarDateAttr*)handle)->SetBackgroundColour(colBack);
    }

    void Calendar::DateAttrSetBorderColor(void* handle, const Color& color)
    {
        ((wxCalendarDateAttr*)handle)->SetBorderColour(color);
    }

    void Calendar::DateAttrSetBorder(void* handle, int border)
    {
        ((wxCalendarDateAttr*)handle)->SetBorder((wxCalendarDateBorder)border);
    }

    void Calendar::DateAttrSetHoliday(void* handle, bool holiday)
    {
        ((wxCalendarDateAttr*)handle)->SetHoliday(holiday);
    }

    bool Calendar::DateAttrHasTextColor(void* handle)
    {
        return ((wxCalendarDateAttr*)handle)->HasTextColour();
    }

    bool Calendar::DateAttrHasBackgroundColor(void* handle)
    {
        return ((wxCalendarDateAttr*)handle)->HasBackgroundColour();
    }

    bool Calendar::DateAttrHasBorderColor(void* handle)
    {
        return ((wxCalendarDateAttr*)handle)->HasBorderColour();
    }

    bool Calendar::DateAttrHasFont(void* handle)
    {
        return ((wxCalendarDateAttr*)handle)->HasFont();
    }

    bool Calendar::DateAttrHasBorder(void* handle)
    {
        return ((wxCalendarDateAttr*)handle)->HasBorder();
    }

    bool Calendar::DateAttrIsHoliday(void* handle)
    {
        return ((wxCalendarDateAttr*)handle)->IsHoliday();
    }

    Color Calendar::DateAttrGetTextColor(void* handle)
    {
        return ((wxCalendarDateAttr*)handle)->GetTextColour();
    }

    Color Calendar::DateAttrGetBackgroundColor(void* handle)
    {
        return ((wxCalendarDateAttr*)handle)->GetBackgroundColour();
    }

    Color Calendar::DateAttrGetBorderColor(void* handle)
    {
        return ((wxCalendarDateAttr*)handle)->GetBorderColour();
    }

    void* Calendar::DateAttrGetFont(void* handle)
    {
        return nullptr;
    }

    void Calendar::DateAttrSetFont(void* handle, void* font)
    {
    }

    int Calendar::DateAttrGetBorder(void* handle)
    {
        return ((wxCalendarDateAttr*)handle)->GetBorder();
    }
}
