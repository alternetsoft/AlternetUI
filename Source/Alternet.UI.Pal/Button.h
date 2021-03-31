#pragma once

#include "Common.h"
#include "Control.h"

namespace Alternet::UI
{
    class Button : public Control
    {
#include "Api/Button.inc"
    public:

        wxWindowBase* GetControl() override;
        wxWindow* CreateWxWindow(wxWindow* parent) override;
        void OnLeftUp(wxMouseEvent& event);

    private:

        wxButton* _button = nullptr;
        string _text;
    };
}
