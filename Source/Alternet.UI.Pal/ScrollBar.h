#pragma once
#include "Common.h"
#include "Control.h"
#include "ApiTypes.h"
#include "Object.h"

#include <wx/scrolbar.h>

namespace Alternet::UI
{
    class ScrollBar : public Control
    {
#include "Api/ScrollBar.inc"
    public:
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;
        wxWindow* CreateWxWindowUnparented() override;

    private:
        wxScrollBarBase* GetScrollBar();

        bool _isVertical = false;
    };
}
