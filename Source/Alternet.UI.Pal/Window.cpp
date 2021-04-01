#include "Window.h"

namespace Alternet::UI
{
    Frame::Frame() : wxFrame(NULL, wxID_ANY, "")
    {
    }

    // ------------

    Window::Window() : _frame(new Frame())
    {
        //_frame->SetSizer(new wxBoxSizer(wxVERTICAL));
#ifdef __WXMSW__
//        _frame->SetBackgroundColour(wxColor(0xffffff));
#endif

        _frame->SetSize(_frame->FromDIP(wxSize(200, 200)));

        _panel = new wxPanel(_frame);
        _button = new wxButton(_panel, wxID_ANY, "HI", _frame->FromDIP(wxPoint(10, 10)), _frame->FromDIP(wxSize(20, 20)));
        _textBox = new wxTextCtrl(_panel, wxID_ANY, "heelo", _frame->FromDIP(wxPoint(100, 100)), _frame->FromDIP(wxSize(50, 20)));

    }

    Window::~Window()
    {
        //delete _frame;
        _frame = nullptr;
    }

    string Window::GetTitle()
    {
        return wxStr(_frame->GetTitle());
    }

    void Window::SetTitle(const string& value)
    {
        _frame->SetTitle(wxStr(value));
    }

    void Window::Show()
    {
        _frame->Show();
    }

    void Window::AddControl(Control& value)
    {
        //value.CreateWxWindow(_frame);
    }
}