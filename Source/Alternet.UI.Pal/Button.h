#pragma once

#include "Common.h"
#include "Control.h"

namespace Alternet::UI
{
    enum class ButtonEvent
    {
        Click,
    };

    typedef int(*ButtonEventCallbackType)(void* button, ButtonEvent event, void* param);

    class Button : public Control
    {
#include "Api/Button.inc"
    public:

        wxWindowBase* GetControl() override;
        wxWindow* CreateWxWindow(wxWindow* parent) override;
        void OnLeftUp(wxMouseEvent& event);

    private:

        int RaiseEvent(ButtonEvent event, void* param = nullptr);

        inline static ButtonEventCallbackType eventCallback = nullptr;

        wxButton* _button = nullptr;
        string _text;

        BYREF_ONLY(Button);
    };
}
