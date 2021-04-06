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
        button->Bind(wxEVT_LEFT_UP, &Button::OnLeftUp, this);
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

    void Button::OnLeftUp(wxMouseEvent& event)
    {
        RaiseEvent(ButtonEvent::Click);
    }
    
    SizeF Button::GetPreferredSize(const SizeF& availableSize)
    {
        if (IsWxWindowCreated())
            return FromWxSize(GetButton()->GetBestSize());
        
        return FromWxSize(wxButton::GetDefaultSize());
    }
}