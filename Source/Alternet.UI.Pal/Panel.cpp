#include "Panel.h"

namespace Alternet::UI
{
    Panel::Panel()
    {
    }

    Panel::~Panel()
    {
    }

    class wxPanel2 : public wxPanel, public wxWidgetExtender
    {
    public:
        Panel* _owner;
            
        virtual bool AcceptsFocus() const override;
        virtual bool AcceptsFocusFromKeyboard() const override;
        virtual bool AcceptsFocusRecursively() const override;

        wxPanel2(
            Panel* owner,
            wxWindow* parent,
            wxWindowID winid = wxID_ANY,
            const wxPoint& pos = wxDefaultPosition,
            const wxSize& size = wxDefaultSize,
            long style = wxTAB_TRAVERSAL | wxNO_BORDER,
            const wxString& name = wxASCII_STR(wxPanelNameStr))
        {
            _owner = owner;
            Create(parent, winid, pos, size, style, name);
        }
    };
    
    bool wxPanel2::AcceptsFocus() const
    {
        if (_owner == nullptr)
            return wxPanel::AcceptsFocus();
        else
            return _owner->_acceptsFocus;
    }

    bool wxPanel2::AcceptsFocusFromKeyboard() const
    {
        if (_owner == nullptr)
            return wxPanel::AcceptsFocusFromKeyboard();
        else
            return _owner->_acceptsFocusFromKeyboard;
    }

    bool wxPanel2::AcceptsFocusRecursively() const
    {
        if (_owner == nullptr)
            return wxPanel::AcceptsFocusRecursively();
        else
            return _owner->_acceptsFocusRecursively;
    }

    wxWindow* Panel::CreateWxWindowCore(wxWindow* parent)
    {
        long style = wxNO_BORDER;

        if (GetIsScrollable())
            style |= wxHSCROLL | wxVSCROLL;

        auto p = new wxPanel2(this, parent, wxID_ANY, wxDefaultPosition, wxDefaultSize, style);
        return p;
    }
}