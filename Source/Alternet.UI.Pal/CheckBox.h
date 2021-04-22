#pragma once
#include "Common.h"
#include "Control.h"

namespace Alternet::UI
{
    class CheckBox : public Control
    {
#include "Api/CheckBox.inc"

        wxWindow* CreateWxWindowCore(wxWindow* parent) override;
        void OnCheckedChanged(wxCommandEvent& event);

    public:
    
    private:

        wxCheckBox* GetCheckBox();

        DelayedValue<CheckBox, string> _text;

        string RetrieveText();
        void ApplyText(const string& value);
    };
}
