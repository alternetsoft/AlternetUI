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
        void OnTextEnter(wxCommandEvent& event);
        void OnTextUrl(wxTextUrlEvent& event);
        void OnTextMaxLength(wxCommandEvent& event);
        TextBox(void* validator);
    protected:

    private:
        bool _editControlOnly = false;
        void* _validator = nullptr;
        bool _readOnly = false;
        bool _multiline = false;
        bool _isRichEdit = false;
        bool _processTab = false;
        bool _password = false;
        bool _processEnter = false;
        bool _noVScroll = false;
        bool _autoUrl = false;
        bool _noHideSel = false;
        int _textAlign = 0;
        int _textWrap = 0;
        string _eventUrl = wxStr(wxEmptyString);

        int _alignment = 0;
        //wxTE_LEFT           0x0000                    
        //wxTE_CENTER         0x0100
        //wxTE_RIGHT          0x0200

        DelayedValue<TextBox, string> _text;

        wxTextCtrl* GetTextCtrl();

        string RetrieveText();
        void ApplyText(const string& value);

        long GetCreateStyle();

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
                return wxTextCtrl::AcceptsFocusFromKeyboard()
                    && wxControl::AcceptsFocusFromKeyboard();
            }
        };
    };
}
