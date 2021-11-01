#pragma once
#include "Common.h"
#include "Control.h"

namespace Alternet::UI
{
    class TabControl : public Control
    {
#include "Api/TabControl.inc"
    public:
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;

    private:

        void OnSelectedPageChanged(wxBookCtrlEvent& event);

        wxNotebook* GetNotebook();
    };
}
