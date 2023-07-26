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
        wxPanel2(wxWindow* parent,
            wxWindowID winid = wxID_ANY,
            const wxPoint& pos = wxDefaultPosition,
            const wxSize& size = wxDefaultSize,
            long style = wxTAB_TRAVERSAL | wxNO_BORDER,
            const wxString& name = wxASCII_STR(wxPanelNameStr))
        {
            Create(parent, winid, pos, size, style, name);
        }
    };
    
    wxWindow* Panel::CreateWxWindowCore(wxWindow* parent)
    {
        long style = wxNO_BORDER;

        if (GetIsScrollable())
            style |= wxHSCROLL | wxVSCROLL;

        auto p = new wxPanel2(
            parent, wxID_ANY, wxDefaultPosition, wxDefaultSize, style);
        return p;
    }
}