#include "Calendar.h"

namespace Alternet::UI
{
    Calendar::Calendar()
    {

    }

    class wxCalendarCtrl2 : public wxCalendarCtrl
    {
    public:
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

    wxWindow* Calendar::CreateWxWindowCore(wxWindow* parent)
    {
        long style = wxCAL_SHOW_HOLIDAYS;

        auto control = new wxCalendarCtrl2(parent,
            wxID_ANY,
            wxDefaultDateTime,
            wxDefaultPosition,
            wxDefaultSize,
            style);

        return control;
    }

    wxCalendarCtrl* Calendar::GetCalendar()
    {
        return dynamic_cast<wxCalendarCtrl*>(GetWxWindow());
    }

    Calendar::~Calendar()
    {
    }

    bool Calendar::GetSundayFirst()
    {
        return false;
    }

    void Calendar::SetSundayFirst(bool value)
    {

    }

    bool Calendar::GetShowHolidays()
    {
        return false;
    }

    void Calendar::SetShowHolidays(bool value)
    {

    }

    bool Calendar::GetNoYearChange()
    {
        return false;
    }

    void Calendar::SetNoYearChange(bool value)
    {

    }

    bool Calendar::GetNoMonthChange()
    {
        return false;
    }

    void Calendar::SetNoMonthChange(bool value)
    {

    }

    bool Calendar::GetSequentalMonthSelect()
    {
        return false;
    }

    void Calendar::SetSequentalMonthSelect(bool value)
    {

    }

    bool Calendar::GetShowSurroundWeeks()
    {
        return false;
    }

    void Calendar::SetShowSurroundWeeks(bool value)
    {

    }

    bool Calendar::GetShowWeekNumbers()
    {
        return false;
    }

    void Calendar::SetShowWeekNumbers(bool value)
    {

    }

    DateTime Calendar::GetValue()
    {
        return DateTime();
    }

    void Calendar::SetValue(const DateTime& value)
    {

    }

    DateTime Calendar::GetMinValue()
    {
        return DateTime();
    }

    void Calendar::SetMinValue(const DateTime& value)
    {

    }

    DateTime Calendar::GetMaxValue()
    {
        return DateTime();
    }

    void Calendar::SetMaxValue(const DateTime& value)
    {

    }

    void Calendar::SetRange(bool useMinValue, bool useMaxValue)
    {

    }

    void Calendar::SetHolidayColours(const Color& colorFg, const Color& colorBg)
    {

    }

    Color Calendar::GetHolidayColourFg()
    {
        return Color();
    }

    Color Calendar::GetHolidayColourBg()
    {
        return Color();
    }

    void Calendar::SetHeaderColours(const Color& colorFg, const Color& colorBg)
    {

    }

    Color Calendar::GetHeaderColourFg()
    {
        return Color();
    }

    Color Calendar::GetHeaderColourBg()
    {
        return Color();
    }

    void Calendar::SetHighlightColours(const Color& colorFg, const Color& colorBg)
    {

    }

    Color Calendar::GetHighlightColourFg()
    {
        return Color();
    }

    Color Calendar::GetHighlightColourBg()
    {
        return Color();
    }

    bool Calendar::AllowMonthChange()
    {
        return false;
    }

    bool Calendar::EnableMonthChange(bool enable)
    {
        return false;
    }

    void Calendar::Mark(int day, bool mark)
    {

    }

    void* Calendar::GetAttr(int day)
    {
        return nullptr;
    }

    void Calendar::SetAttr(int day, void* calendarDateAttr)
    {

    }

    void Calendar::ResetAttr(int day)
    {

    }

    void Calendar::EnableHolidayDisplay(bool display)
    {

    }

    void Calendar::SetHoliday(int day)
    {

    }

    void* Calendar::GetMarkDateAttr()
    {
        return nullptr;
    }

    void Calendar::SetMarkDateAttr(void* dateAttr)
    {

    }

    void* Calendar::CreateDateAttr(int border)
    {
        return nullptr;
    }

    void Calendar::DateAttrSetTextColour(void* handle, const Color& colText)
    {

    }

    void Calendar::DateAttrSetBackgroundColour(void* handle, const Color& colBack)
    {

    }

    void Calendar::DateAttrSetBorderColour(void* handle, const Color& color)
    {

    }

    void Calendar::DateAttrSetFont(void* handle, void* font)
    {

    }

    void Calendar::DateAttrSetBorder(void* handle, int border)
    {

    }

    void Calendar::DateAttrSetHoliday(void* handle, bool holiday)
    {

    }

    bool Calendar::DateAttrHasTextColor(void* handle)
    {
        return false;
    }

    bool Calendar::DateAttrHasBackgroundColor(void* handle)
    {
        return false;
    }

    bool Calendar::DateAttrHasBorderColor(void* handle)
    {
        return false;
    }

    bool Calendar::DateAttrHasFont(void* handle)
    {
        return false;
    }

    bool Calendar::DateAttrHasBorder(void* handle)
    {
        return false;
    }

    bool Calendar::DateAttrIsHoliday(void* handle)
    {
        return false;
    }

    Color Calendar::DateAttrGetTextColor(void* handle)
    {
        return Color();
    }

    Color Calendar::DateAttrGetBackgroundColor(void* handle)
    {
        return Color();
    }

    Color Calendar::DateAttrGetBorderColor(void* handle)
    {
        return Color();
    }

    void* Calendar::DateAttrGetFont(void* handle)
    {
        return nullptr;
    }

    int Calendar::DateAttrGetBorder(void* handle)
    {
        return 0;
    }
}
