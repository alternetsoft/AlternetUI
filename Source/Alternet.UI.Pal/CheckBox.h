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
        DelayedValue<CheckBox, wxCheckBoxState> _state;
        bool _threeState = false;
        bool _alignRight = false;
        bool _allowAllStatesForUser = false;

        string RetrieveText();
        void ApplyText(const string& value);

        wxCheckBoxState RetrieveState();
        void ApplyState(const wxCheckBoxState& value);
    };
}
