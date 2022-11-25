#include "Button.h"
#include "Window.h"

#ifdef __WXOSX_COCOA__
#include <wx/osx/core/private.h>
#endif

namespace Alternet::UI
{
#ifdef __WXOSX_COCOA__            
    static const bool ButtonImagesEnabled = false;
#else
    static const bool ButtonImagesEnabled = true;
#endif

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
            {
#ifdef __WXOSX_COCOA__
                button->GetPeer()->SetDefaultButton(false);
#endif
                ButtonDefaultStyleSetter::SetDefaultStyle(button, false);
                topLevelWindow->SetDefaultItem(nullptr);
            }
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

    Image* Button::GetNormalImage()
    {
        if (_normalImage == nullptr)
            return nullptr;

        _normalImage->AddRef();
        return _normalImage;
    }

    void Button::SetNormalImage(Image* value)
    {
        if (_normalImage != nullptr)
            _normalImage->Release();

        _normalImage = value;

        
        auto button = GetButton();
        if (_normalImage != nullptr)
        {
            _normalImage->AddRef();
            if(ButtonImagesEnabled)
                button->SetBitmapLabel(_normalImage->GetBitmap());
        }
        else
        {
            if (ButtonImagesEnabled)
                button->SetBitmapLabel(wxBitmap());
        }
    }

    Image* Button::GetHoveredImage()
    {
        if (_hoveredImage == nullptr)
            return nullptr;

        _hoveredImage->AddRef();
        return _hoveredImage;
    }

    void Button::SetHoveredImage(Image* value)
    {
        if (_hoveredImage != nullptr)
            _hoveredImage->Release();

        _hoveredImage = value;

        auto button = GetButton();
        if (_hoveredImage != nullptr)
        {
            _hoveredImage->AddRef();
            if (ButtonImagesEnabled)
                button->SetBitmapCurrent(_hoveredImage->GetBitmap());
        }
        else
        {
            if (ButtonImagesEnabled)
                button->SetBitmapCurrent(wxBitmap());
        }
    }

    Image* Button::GetPressedImage()
    {
        if (_pressedImage == nullptr)
            return nullptr;

        _pressedImage->AddRef();
        return _pressedImage;
    }

    void Button::SetPressedImage(Image* value)
    {
        if (_pressedImage != nullptr)
            _pressedImage->Release();

        _pressedImage = value;

        auto button = GetButton();
        if (_pressedImage != nullptr)
        {
            _pressedImage->AddRef();
            if (ButtonImagesEnabled)
                button->SetBitmapPressed(_pressedImage->GetBitmap());
        }
        else
        {
            if (ButtonImagesEnabled)
                button->SetBitmapCurrent(wxBitmap());
        }
    }

    Image* Button::GetDisabledImage()
    {
        if (_disabledImage == nullptr)
            return nullptr;

        _disabledImage->AddRef();
        return _disabledImage;
    }

    void Button::SetDisabledImage(Image* value)
    {
        if (_disabledImage != nullptr)
            _disabledImage->Release();

        _disabledImage = value;

        auto button = GetButton();
        if (_disabledImage != nullptr)
        {
            _disabledImage->AddRef();
            if (ButtonImagesEnabled)
                button->SetBitmapDisabled(_disabledImage->GetBitmap());
        }
        else
        {
            if (ButtonImagesEnabled)
                button->SetBitmapCurrent(wxBitmap());
        }
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