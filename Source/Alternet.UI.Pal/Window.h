#pragma once

#include "Common.h"

namespace Alternet::UI
{
    class Frame : public wxFrame
    {
    public:
        Frame() : wxFrame(NULL, wxID_ANY, "")
        {
        }

    private:

        BYREF_ONLY(Frame);
    };

    class MessageBox_
    {
    public:

        static void Show(string text, string caption)
        {
            wxMessageBox(wxStr(text), wxStr(caption));
        }

    private:
        MessageBox_()
        {
        }

        virtual ~MessageBox_()
        {
        }

        BYREF_ONLY(MessageBox_);
    };

    class Control
    {
    public:
        Control()
        {
        }

        virtual ~Control()
        {
            //delete GetControl();
        }

        virtual wxWindowBase* GetControl() = 0;

        virtual wxWindow* CreateWxWindow(wxWindow* parent) { return nullptr; };

    private:

        BYREF_ONLY(Control);
    };

    enum class ButtonEvent
    {
        Click,
    };

    typedef int(*ButtonEventCallbackType)(void* button, ButtonEvent event, void* param);

    class Button : public Control
    {
    public:
        Button()
        {
        }

        virtual ~Button()
        {
        }

        wxWindowBase* GetControl() override
        {
            return _button;
        }

        string GetText() const
        {
            if (_button == nullptr)
                return _text;
            else
                return wxStr(_button->GetLabel());
        }

        void SetText(const string& value)
        {
            if (_button == nullptr)
                _text = value;
            else
                _button->SetLabel(wxStr(value));
        }

        static void SetEventCallback(ButtonEventCallbackType value)
        {
            eventCallback = value;
        }

        wxWindow* CreateWxWindow(wxWindow* parent) override
        {
            _button = new wxButton(parent, wxID_ANY, wxStr(_text), wxDefaultPosition, wxSize(100, 20));
            _button->Bind(wxEVT_LEFT_UP, &Button::OnLeftUp, this);
            parent->GetSizer()->Add(_button, wxALIGN_TOP);
            _text = u"";
            return _button;
        }

        void OnLeftUp(wxMouseEvent& event)
        {
            RaiseEvent(ButtonEvent::Click);
        }

    private:

        int RaiseEvent(ButtonEvent event, void* param = nullptr)
        {
            if (eventCallback != nullptr)
                return eventCallback(this, event, param);
            return 0;
        }

        inline static ButtonEventCallbackType eventCallback = nullptr;

        wxButton* _button = nullptr;
        string _text;

        BYREF_ONLY(Button);
    };

    class Window
    {
    public:
        Window() : _frame(new Frame())
        {
            _frame->SetSizer(new wxBoxSizer(wxVERTICAL));
#ifdef __WXMSW__
            _frame->SetBackgroundColour(wxColor(0xffffff));
#endif
        }

        virtual ~Window()
        {
            //delete _frame;
            _frame = nullptr;
        }

        string GetTitle() const
        {
            return wxStr(_frame->GetTitle());
        }

        void SetTitle(const string& value)
        {
            _frame->SetTitle(wxStr(value));
        }

        void Show()
        {
            _frame->Show();
        }

        void AddChildControl(Control* value)
        {
            value->CreateWxWindow(_frame);
        }

    private:

        Frame* _frame;


        BYREF_ONLY(Window);
    };
}
