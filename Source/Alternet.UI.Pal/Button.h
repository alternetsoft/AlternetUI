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
    public:
        Button();
        virtual ~Button();

        wxWindowBase* GetControl() override;

        string GetText() const;
        void SetText(const string& value);

        static void SetEventCallback(ButtonEventCallbackType value);

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
