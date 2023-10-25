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

        static std::vector<wxWindow*> GetVisiblePopupWindows();

    protected:
        void UpdateWxWindowParent() override;

    private:
        wxPopupWindow* GetWxPopup();

        bool _isTransient = true;
        bool _puContainsControls = false;

        inline static std::vector<wxWindow*> _popupWindows;
    };
}
