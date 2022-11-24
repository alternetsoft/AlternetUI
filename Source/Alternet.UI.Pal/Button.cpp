#include "Button.h"
#include "Window.h"

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

        auto window = GetParentWindow();
        if (window != nullptr)
        {
            if (_isDefault)
                window->SetAcceptButton(this);
            else if (window->GetAcceptButton() == this)
                window->SetAcceptButton(nullptr);
        }
    }

    void Button::ApplyIsCancel()
    {
        auto button = GetButton();

        auto window = GetParentWindow();
        if (window != nullptr)
        {
            if (_isCancel)
                window->SetCancelButton(this);
            else if (window->GetCancelButton() == this)
                window->SetCancelButton(nullptr);
        }
    }

    void Button::OnButtonClick(wxCommandEvent& event)
    {
        RaiseClick();
    }

    void Button::RaiseClick()
    {
        RaiseEvent(ButtonEvent::Click);
    }

    void Button::OnWxWindowCreated()
    {
        Control::OnWxWindowCreated();
        ApplyIsDefault();
        ApplyIsCancel();
    }

    void Button::OnParentChanged()
    {
        Control::OnParentChanged();
        ApplyIsDefault();
        ApplyIsCancel();
    }

    void Button::OnAnyParentChanged()
    {
        Control::OnAnyParentChanged();
        ApplyIsDefault();
        ApplyIsCancel();
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
        return _isCancel;
    }
    
    void Button::SetIsCancel(bool value)
    {
        if (_isCancel == value)
            return;

        _isCancel = value;
        ApplyIsCancel();
    }
}