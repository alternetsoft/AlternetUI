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
        
        int w = 0, h = 0;
        _button->GetBestSize(&w, &h);
        _button->SetSize(w, h);
        _button->SetPosition(_frame->FromDIP(wxPoint(20, 20)));
        _button->Bind(wxEVT_PAINT, &Window::OnPaint, this);

        _textBox = new wxTextCtrl(_panel, wxID_ANY, "heelo", _frame->FromDIP(wxPoint(100, 100)), _frame->FromDIP(wxSize(50, 20)));
    }

    void Window::OnPaint(wxPaintEvent& event)
    {
        wxPaintDC dc(_button);
        dc.SetPen(*wxBLACK_PEN);
        dc.SetBrush(*wxRED_BRUSH);

        wxSize sz = _button->GetClientSize();
        wxCoord w = 10, h = 10;

        int x = wxMax(0, (sz.x - w) / 2);
        int y = wxMax(0, (sz.y - h) / 2);
        wxRect rectToDraw(x, y, w, h);

        if (_button->IsExposed(rectToDraw))
            dc.DrawRectangle(rectToDraw);
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