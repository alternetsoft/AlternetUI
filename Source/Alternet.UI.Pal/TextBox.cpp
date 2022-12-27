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
        if (IsWxWindowCreated())
        {
            auto window = GetWxWindow();
            if (window != nullptr)
            {
                window->Unbind(wxEVT_TEXT, &TextBox::OnTextChanged, this);
            }
        }
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
        auto textCtrl = new TextCtrlEx(
            parent, wxID_ANY, wxEmptyString, wxDefaultPosition, wxDefaultSize, GetStyle());

#ifdef __WXOSX__
        // todo: port all platforms to the latest wx version, and then the ifdef can be removed.
        // EnableVisibleFocus is implemented only on macOS at the moment through.
        if (_editControlOnly)
            textCtrl->EnableVisibleFocus(false);
#endif
            
        textCtrl->Bind(wxEVT_TEXT, &TextBox::OnTextChanged, this);
        return textCtrl;
    }

    long TextBox::GetStyle()
    {
        long style = _editControlOnly ? wxNO_BORDER : 0;

        if (_readOnly)
            style |= wxTE_READONLY;

        if (_multiline)
            style |= wxTE_MULTILINE;

        return style;
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

    bool TextBox::GetReadOnly()
    {
        return _readOnly;
    }

    void TextBox::SetReadOnly(bool value)
    {
        if (_readOnly == value)
            return;

        _readOnly = value;
        RecreateWxWindowIfNeeded();
    }

    bool TextBox::GetMultiline()
    {
        return _multiline;
    }

    void TextBox::SetMultiline(bool value)
    {
        if (_multiline == value)
            return;

        _multiline = value;
        RecreateWxWindowIfNeeded();
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
