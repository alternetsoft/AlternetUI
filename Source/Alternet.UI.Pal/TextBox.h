#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Control.h"

namespace Alternet::UI
{
    class TextBox : public Control
    {
#include "Api/TextBox.inc"
    public:
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;
        void OnTextChanged(wxCommandEvent& event);

    protected:

        bool DoNotBindPaintEvent() override;

    private:
        DelayedValue<TextBox, string> _text;

        wxTextCtrl* GetTextCtrl();

        string RetrieveText();
        void ApplyText(const string& value);
    };
}
