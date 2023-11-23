#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"
#include "Control.h"

#include <wx/calctrl.h>
#include <wx/generic/calctrlg.h>

namespace Alternet::UI
{
    class Calendar : public Control
    {
#include "Api/Calendar.inc"
    public:
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;
        virtual void RecreateWxWindowIfNeeded() override;
        static wxCalendarDateAttr* CloneDateAttr(const wxCalendarDateAttr& attr);

    private:
        wxCalendarCtrlBase* GetCalendar();

        bool _showHolidays = false;
        bool _sundayFirst = false;
        bool _mondayFirst = false;
        bool _noYearChange = false;
        bool _noMonthChange = false;
        bool _sequentalMonthSelect = false;
        bool _showSurroundWeeks = true;
        bool _showWeekNumbers = false;
        bool _useGeneric = false;
        bool _hasBorder = false;
        DateTime _minValue = DateTime();
        DateTime _maxValue = DateTime();

        void OnEventDoubleClick(wxCalendarEvent& event);
        void OnEventSelChanged(wxCalendarEvent& event);
        void OnEventPageChanged(wxCalendarEvent& event);
        void OnEventDayHeaderClick(wxCalendarEvent& event);
        void OnEventWeekNumberClick(wxCalendarEvent& event);
    };
}
