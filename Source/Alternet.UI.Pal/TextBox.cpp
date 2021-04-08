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

    bool TextBox::DoNotBindPaintEvent()
    {
#if defined(__WXOSX_COCOA__)
        // Binding the paint event of TextCtrl on macOS leads to a blank control.
        return true;
#else
        return false;
#endif
    }


    wxWindow* TextBox::CreateWxWindowCore(wxWindow* parent)
    {
        auto textCtrl = new wxTextCtrl(parent, wxID_ANY);
        textCtrl->Bind(wxEVT_TEXT, &TextBox::OnTextChanged, this);
        return textCtrl;
    }

    void TextBox::OnTextChanged(wxCommandEvent& event)
    {
        RaiseEvent(TextBoxEvent::TextChanged);
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
