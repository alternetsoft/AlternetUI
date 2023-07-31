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

    private:
        bool _editControlOnly = false;
        bool _readOnly = false;
        bool _multiline = false;
        bool _isRichEdit = false;

        DelayedValue<TextBox, string> _text;

        wxTextCtrl* GetTextCtrl();
        wxTextAttr _textAttr;

        string RetrieveText();
        void ApplyText(const string& value);

        long GetStyle();

        class TextCtrlEx : public wxTextCtrl, public wxWidgetExtender
        {
        public:
            TextCtrlEx(wxWindow* parent, wxWindowID id,
                const wxString& value = wxEmptyString,
                const wxPoint& pos = wxDefaultPosition,
                const wxSize& size = wxDefaultSize,
                long style = 0,
                const wxValidator& validator = wxDefaultValidator,
                const wxString& name = wxASCII_STR(wxTextCtrlNameStr)):
                wxTextCtrl(parent, id, value, pos, size, style, validator, name)
            {
            }

            virtual bool AcceptsFocusFromKeyboard() const override
            {
                return wxTextCtrl::AcceptsFocusFromKeyboard() && wxControl::AcceptsFocusFromKeyboard();
            }
        };
    };
}
