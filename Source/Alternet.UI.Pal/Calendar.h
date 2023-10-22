#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"
#include "Control.h"
#include "wx/calctrl.h"

namespace Alternet::UI
{
    class Calendar : public Control
    {
#include "Api/Calendar.inc"
    public:
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;

    private:
        wxCalendarCtrl* GetCalendar();

    };
}
