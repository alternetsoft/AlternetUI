#pragma once

#include "Common.h"
#include "Control.h"

namespace Alternet::UI
{
    class Button : public Control
    {
#include "Api/Button.inc"
    public:
        wxWindow* CreateWxWindowCore() override;
        void OnLeftUp(wxMouseEvent& event);

    private:

        wxButton* GetButton();

        string _text;
    };
}
