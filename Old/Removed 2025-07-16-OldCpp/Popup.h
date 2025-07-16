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
        wxWindow* CreateWxWindowUnparented() override;

    protected:
        /*void UpdateWxWindowParent() override;*/

    private:
        /*static std::vector<wxWindow*> GetVisiblePopupWindows();*/

        wxPopupWindow* GetWxPopup();

        wxPopupTransientWindow* GetWxTransientPopup();

        bool _isTransient = true;
        bool _puContainsControls = true;

        /*inline static std::vector<wxWindow*> _popupWindows;*/
    };
}
