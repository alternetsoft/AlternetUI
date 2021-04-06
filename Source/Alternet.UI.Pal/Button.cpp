#include "Button.h"

namespace Alternet::UI
{
    Button::Button()
    {
    }

    Button::~Button()
    {
    }

    string Button::GetText()
    {
        auto button = GetButton();

        if (button == nullptr)
            return _text;
        else
            return wxStr(button->GetLabel());
    }

    void Button::SetText(const string& value)
    {
        auto button = GetButton();

        if (button == nullptr)
            _text = value;
        else
            button->SetLabel(wxStr(value));
    }

    wxWindow* Button::CreateWxWindowCore(wxWindow* parent)
    {
        auto button = new wxButton(parent, wxID_ANY, wxStr(_text));
        button->Bind(wxEVT_LEFT_UP, &Button::OnLeftUp, this);
        _text = u"";
        return button;
    }

    wxButton* Button::GetButton()
    {
        return dynamic_cast<wxButton*>(GetWxWindow());
    }

    void Button::OnLeftUp(wxMouseEvent& event)
    {
        RaiseEvent(ButtonEvent::Click);
    }
}