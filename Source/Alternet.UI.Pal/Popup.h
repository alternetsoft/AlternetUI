#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Control.h"

namespace Alternet::UI
{
    class Popup : public Control
    {
#include "Api/Popup.inc"
    public:
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;

    protected:
        void UpdateWxWindowParent() override;

    private:
        wxPopupTransientWindow* GetWxPopup();

        wxPopupTransientWindow* _popup = nullptr;
    };
}
