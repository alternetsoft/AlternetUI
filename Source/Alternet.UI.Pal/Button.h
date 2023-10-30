#pragma once

#include "Common.h"
#include "Control.h"
#include "wx/defs.h"

namespace Alternet::UI
{
    class Button;

    class wxButton2 : public wxButton, public wxWidgetExtender
    {
    public:
        Button* _owner;

        virtual bool AcceptsFocus() const override;
        virtual bool AcceptsFocusFromKeyboard() const override;
        virtual bool AcceptsFocusRecursively() const override;

        wxButton2(
            Button* owner,
            wxWindow* parent,
            wxWindowID id,
            const wxString& label = wxEmptyString,
            const wxPoint& pos = wxDefaultPosition,
            const wxSize& size = wxDefaultSize,
            long style = 0,
            const wxValidator& validator = wxDefaultValidator,
            const wxString& name = wxASCII_STR(wxButtonNameStr))
        {
            _owner = owner;
            Create(parent, id, label, pos, size, style, validator, name);
        }
    };

    class Button : public Control
    {
#include "Api/Button.inc"
    public:
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;
        void OnButtonClick(wxCommandEvent& event);

        void RaiseClick();

    protected:
        virtual void OnWxWindowCreated() override;
        virtual void OnParentChanged() override;
        virtual void OnAnyParentChanged() override;

    private:
        wxButton* GetButton();
        wxButton2* GetButton2();

        DelayedValue<Button, string> _text;

        string RetrieveText();
        void ApplyText(const string& value);

        bool _isDefault = false;
        bool _isCancel = false;
        bool _hasBorder = true;
        bool _textVisible = true;
        bool _exactFit = false;
        wxDirection _textAlign = (wxDirection)0;

        void ApplyIsDefault();
        void ApplyIsCancel();

        Image* _normalImage = nullptr;
        Image* _hoveredImage = nullptr;
        Image* _pressedImage = nullptr;
        Image* _disabledImage = nullptr;
        Image* _focusedImage = nullptr;

        class ButtonDefaultStyleSetter : public wxButton
        {
        public:
            static void SetDefaultStyle(wxButton* button, bool on)
            {
#ifdef  __WXMSW__
                wxButton::SetDefaultStyle(button, on);
#endif
            }
        };
    };

}
