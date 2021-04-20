#include "TextBox.h"

namespace Alternet::UI
{
    TextBox::TextBox():
        _text(*this, u"", &Control::IsWxWindowCreated, &TextBox::RetrieveText, &TextBox::ApplyText)
    {
        GetDelayedValues().Add(&_text);
    }

    TextBox::~TextBox()
    {
    }

    string TextBox::GetText()
    {
        return _text.Get();
    }

    void TextBox::SetText(const string& value)
    {
        _text.Set(value);
    }

    wxWindow* TextBox::CreateWxWindowCore(wxWindow* parent)
    {
        auto textCtrl = new wxTextCtrl(
            parent, wxID_ANY, wxEmptyString, wxDefaultPosition, wxDefaultSize, _editControlOnly ? wxNO_BORDER : 0);

#ifdef __WXOSX__
        // todo: port all platforms to the latest wx version, and then the ifdef can be removed.
        // EnableVisibleFocus is implemented only on macOS at the moment through.
        if (_editControlOnly)
            textCtrl->EnableVisibleFocus(false);
#endif
            
        textCtrl->Bind(wxEVT_TEXT, &TextBox::OnTextChanged, this);
        return textCtrl;
    }

    void TextBox::OnTextChanged(wxCommandEvent& event)
    {
        RaiseEvent(TextBoxEvent::TextChanged);
    }

    bool TextBox::GetEditControlOnly()
    {
        return _editControlOnly;
    }

    void TextBox::SetEditControlOnly(bool value)
    {
        _editControlOnly = value;
        assert(!IsWxWindowCreated()); // todo: recreate window
    }

    wxTextCtrl* TextBox::GetTextCtrl()
    {
        return dynamic_cast<wxTextCtrl*>(GetWxWindow());
    }

    string TextBox::RetrieveText()
    {
        return wxStr(GetTextCtrl()->GetValue());
    }

    void TextBox::ApplyText(const string& value)
    {
        GetTextCtrl()->SetValue(wxStr(value));
    }
}
