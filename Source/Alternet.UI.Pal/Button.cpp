#include "Button.h"

namespace Alternet::UI
{
    Button::Button():
        _text(*this, u"", &Control::IsWxWindowCreated, &Button::RetrieveText, &Button::ApplyText)
    {
        GetDelayedValues().Add(&_text);
    }

    Button::~Button()
    {
    }

    string Button::GetText()
    {
        return _text.Get();
    }

    void Button::SetText(const string& value)
    {
        _text.Set(value);
    }

    wxWindow* Button::CreateWxWindowCore(wxWindow* parent)
    {
        auto button = new wxButton(parent, wxID_ANY);
        button->Bind(wxEVT_BUTTON, &Button::OnButtonClick, this);
        return button;
    }

    wxButton* Button::GetButton()
    {
        return dynamic_cast<wxButton*>(GetWxWindow());
    }

    string Button::RetrieveText()
    {
        return wxStr(GetButton()->GetLabel());
    }

    void Button::ApplyText(const string& value)
    {
        GetButton()->SetLabel(wxStr(value));
    }

    void Button::OnButtonClick(wxCommandEvent& event)
    {
        RaiseEvent(ButtonEvent::Click);
    }
    
    SizeF Button::GetDefaultSize()
    {
        return toDip(wxButton::GetDefaultSize(), nullptr);
    }
}