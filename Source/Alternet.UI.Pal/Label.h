#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Control.h"

namespace Alternet::UI
{
    class Label : public Control
    {
#include "Api/Label.inc"
    public:
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;
        wxWindow* CreateWxWindowUnparented() override;

    private:
    
        DelayedValue<Label, string> _text;

        wxStaticText* GetStaticText();

        string RetrieveText();
        
        void ApplyText(const string& value);
    };
}
