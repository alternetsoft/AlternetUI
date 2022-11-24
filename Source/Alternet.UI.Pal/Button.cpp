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
        auto window = GetWxWindow();
        if (window != nullptr)
        {
            window->Unbind(wxEVT_BUTTON, &Button::OnButtonClick, this);
        }
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

    void Button::ApplyIsDefault()
    {
        auto button = GetButton();
        auto topLevelWindow = dynamic_cast<wxTopLevelWindow*>(wxGetTopLevelParent(button));
        if (topLevelWindow != nullptr)
        {
            if (_isDefault)
                button->SetDefault();
            else if (topLevelWindow->GetDefaultItem() == button)
                topLevelWindow->SetDefaultItem(nullptr);
        }
    }

    void Button::OnButtonClick(wxCommandEvent& event)
    {
        RaiseEvent(ButtonEvent::Click);
    }

    void Button::OnWxWindowCreated()
    {
        Control::OnWxWindowCreated();
        ApplyIsDefault();
    }
    
    bool Button::GetIsDefault()
    {
        return _isDefault;
    }
    
    void Button::SetIsDefault(bool value)
    {
        if (_isDefault == value)
            return;

        _isDefault = value;
        ApplyIsDefault();
    }
    
    bool Button::GetIsCancel()
    {
        return false;
    }
    
    void Button::SetIsCancel(bool value)
    {
    }
}