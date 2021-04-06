#pragma once

#include "Common.h"
#include "Control.h"

namespace Alternet::UI
{
    class Button : public Control
    {
#include "Api/Button.inc"
    public:
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;
        void OnLeftUp(wxMouseEvent& event);

    private:

        wxButton* GetButton();

        DelayedValue<Button, string> _text;
        DelayedValues _delayedValues;

        string RetrieveText();
        void ApplyText(const string& value);
    };
}
