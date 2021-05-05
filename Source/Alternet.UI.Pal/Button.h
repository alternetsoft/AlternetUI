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
        void OnButtonClick(wxCommandEvent& event);

    protected:

    private:

        wxButton* GetButton();

        DelayedValue<Button, string> _text;

        string RetrieveText();
        void ApplyText(const string& value);
    };
}
