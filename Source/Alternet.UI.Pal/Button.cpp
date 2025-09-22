#include "Button.h"
#include "Window.h"

#ifdef __WXOSX_COCOA__
#include <wx/osx/core/private.h>
#endif

namespace Alternet::UI
{
    bool wxButton2::AcceptsFocus() const
    {
        if (_owner == nullptr)
            return wxButton::AcceptsFocus();
        else
            return _owner->_acceptsFocus;
    }

    bool wxButton2::AcceptsFocusFromKeyboard() const
    {
        if (_owner == nullptr)
            return wxButton::AcceptsFocusFromKeyboard();
        else
            return _owner->_acceptsFocusFromKeyboard;
    }

    bool wxButton2::AcceptsFocusRecursively() const
    {
        if (_owner == nullptr)
            return wxButton::AcceptsFocusRecursively();
        else
            return _owner->_acceptsFocusRecursively;
    }

#ifdef __WXOSX_COCOA__            
    static bool ButtonImagesEnabled = true;
#else
    static bool ButtonImagesEnabled = true;
#endif

    bool Button::GetImagesEnabled()
    {
        return ButtonImagesEnabled;
    }

    void Button::SetImagesEnabled(bool value)
    {
        ButtonImagesEnabled = value;
    }

    bool Button::GetTextVisible() 
    {
        return _textVisible;
    }
    
    void Button::SetTextVisible(bool value)
    {
        if (_textVisible == value)
            return;
        _textVisible = value;
        RecreateWxWindowIfNeeded();
    }

    int Button::GetTextAlign()
    {
        return _textAlign;
    }

    void Button::SetTextAlign(int value)
    {
        if (_textAlign == value)
            return;
        _textAlign = (wxDirection)value;
        RecreateWxWindowIfNeeded();
    }

    void Button::SetImagePosition(int dir)
    {
        GetButton()->SetBitmapPosition((wxDirection)dir);
    }

    void Button::SetImageMargins(Coord x, Coord y)
    {
        GetButton()->SetBitmapMargins(x, y);
    }

    Button::Button():
        _text(*this, u"", &Control::IsWxWindowCreated, 
            &Button::RetrieveText, &Button::ApplyText)
    {
        GetDelayedValues().Add(&_text);
    }

    bool Button::GetHasBorder()
    {
        return _hasBorder;
    }

    void Button::SetHasBorder(bool value)
    {
        if (_hasBorder == value)
            return;
        _hasBorder = value;
        RecreateWxWindowIfNeeded();
    }

    Button::~Button()
    {
        if (IsWxWindowCreated())
        {
            auto window = GetWxWindow();
            if (window != nullptr)
            {
                window->Unbind(wxEVT_BUTTON, &Button::OnButtonClick, this);
            }
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

    bool Button::GetExactFit()
    {
        return _exactFit;
    }

    void Button::SetExactFit(bool value)
    {
        if (_exactFit == value)
            return;
        _exactFit = value;
        RecreateWxWindowIfNeeded();
    }

    wxWindow* Button::CreateWxWindowUnparented()
    {
        return new wxButton2();
    }

    wxWindow* Button::CreateWxWindowCore(wxWindow* parent)
    {
        long style = 0;

        if (!_hasBorder)
            style |= wxBORDER_NONE;

        if (!_textVisible)
            style |= wxBU_NOTEXT;

        bool isLeft = (_textAlign & wxLEFT) == wxLEFT;
        bool isRight = (_textAlign & wxRIGHT) == wxRIGHT;
        bool isTop = (_textAlign & wxTOP) == wxTOP;
        bool isBottom = (_textAlign & wxBOTTOM) == wxBOTTOM;

        if(isLeft)
            style |= wxBU_LEFT;
        else
        if(isRight)
            style |= wxBU_RIGHT;

        if (isTop)
            style |= wxBU_TOP;
        else
        if (isBottom)
            style |= wxBU_BOTTOM;

        if (_exactFit)
            style |= wxBU_EXACTFIT;

        auto button = new wxButton2(this, parent,
            wxID_ANY,
            wxEmptyString,
            wxDefaultPosition,
            wxDefaultSize,
            style,
            wxDefaultValidator,
            wxASCII_STR(wxButtonNameStr));

        button->Bind(wxEVT_BUTTON, &Button::OnButtonClick, this);
        return button;
    }

    wxButton* Button::GetButton()
    {
        return dynamic_cast<wxButton*>(GetWxWindow());
    }

    wxButton2* Button::GetButton2()
    {
        return dynamic_cast<wxButton2*>(GetWxWindow());
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
        event.Skip();
        RaiseClick();
    }

    void Button::RaiseClick()
    {
        RaiseEvent(ButtonEvent::Click);
    }

    Image* Button::GetFocusedImage()
    {
        if (_focusedImage == nullptr)
            return nullptr;

        _focusedImage->AddRef();
        return _focusedImage;
    }

    void Button::SetFocusedImage(Image* value)
    {
        if (_focusedImage != nullptr)
            _focusedImage->Release();

        _focusedImage = value;

        auto button = GetButton();
        if (_focusedImage != nullptr)
        {
            _focusedImage->AddRef();
            if (ButtonImagesEnabled)
                button->SetBitmapFocus(_focusedImage->GetBitmap());
        }
        else
        {
            if (ButtonImagesEnabled)
                button->SetBitmapFocus(wxBitmap());
        }
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
                button->SetBitmap(_normalImage->GetBitmap());
        }
        else
        {
            if (ButtonImagesEnabled)
                button->SetBitmap(wxBitmap());
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
                button->SetBitmapPressed(wxBitmap());
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
                button->SetBitmapDisabled(wxBitmap());
        }
    }

    void Button::OnWxWindowCreated()
    {
        Control::OnWxWindowCreated();
        
        auto button = GetButton();

        if (ButtonImagesEnabled)
        {
                button->SetBitmap(Image::GetWxBitmap(_normalImage));
                if (_hoveredImage != nullptr)
                    button->SetBitmapCurrent(Image::GetWxBitmap(_hoveredImage));
                if (_pressedImage != nullptr)
                    button->SetBitmapPressed(Image::GetWxBitmap(_pressedImage));
                if (_disabledImage != nullptr)
                    button->SetBitmapDisabled(Image::GetWxBitmap(_disabledImage));
                if (_focusedImage != nullptr)
                    button->SetBitmapFocus(Image::GetWxBitmap(_focusedImage));
        }
    }

    void Button::OnParentChanged()
    {
        Control::OnParentChanged();
    }

    void Button::OnAnyParentChanged()
    {
        Control::OnAnyParentChanged();
    }    
}