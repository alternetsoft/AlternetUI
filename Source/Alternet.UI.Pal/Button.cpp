#include "Button.h"

namespace Alternet::UI
{
    Button::Button()
    {
    }

    Button::~Button()
    {
    }

    wxWindowBase* Button::GetControl()
    {
        return _button;
    }

    string Button::GetText()
    {
        if (_button == nullptr)
            return _text;
        else
            return wxStr(_button->GetLabel());
    }

    void Button::SetText(const string& value)
    {
        if (_button == nullptr)
            _text = value;
        else
            _button->SetLabel(wxStr(value));
    }

    wxWindow* Button::CreateWxWindow(wxWindow* parent)
    {
        _button = new wxButton(parent, wxID_ANY, wxStr(_text), wxDefaultPosition, wxSize(100, 20));
        _button->Bind(wxEVT_LEFT_UP, &Button::OnLeftUp, this);
        parent->GetSizer()->Add(_button, wxALIGN_TOP);
        _text = u"";
        return _button;
    }

    void Button::OnLeftUp(wxMouseEvent& event)
    {
        RaiseEvent(ButtonEvent::Click);
    }
}