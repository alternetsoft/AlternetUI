#include "PropertyGrid.h"

namespace Alternet::UI
{
    class wxPropertyGrid2 : public wxPropertyGrid, public wxWidgetExtender
    {
    public:
        wxPropertyGrid2(wxWindow* parent, wxWindowID id = wxID_ANY,
            const wxPoint& pos = wxDefaultPosition,
            const wxSize& size = wxDefaultSize,
            long style = wxPG_DEFAULT_STYLE,
            const wxString& name = wxASCII_STR(wxPropertyGridNameStr))
            : wxPropertyGrid(parent, id, pos, size, style, name)
        {
        }
    };

    PropertyGrid::PropertyGrid(long styles)
    {
        _createStyle = styles;
    }

    /*static*/ void* PropertyGrid::CreateEx(int64_t styles)
    {
        return new PropertyGrid(styles);
    }

    int64_t PropertyGrid::GetCreateStyle()
    {
        return _createStyle;
    }

    void PropertyGrid::SetCreateStyle(int64_t value)
    {
        if (_createStyle == value)
            return;
        _createStyle = value;
        RecreateWxWindowIfNeeded();
    }

    wxWindow* PropertyGrid::CreateWxWindowCore(wxWindow* parent)
    {
        long style = _createStyle;

        auto result = new wxPropertyGrid2(parent, wxID_ANY,
            wxDefaultPosition,
            wxDefaultSize,
            style);

        //toolbar->Bind(wxEVT_LEFT_DOWN, &AuiToolBar::OnLeftDown, this);
        return result;
    }

	PropertyGrid::PropertyGrid()
	{
	}

    wxPropertyGrid* PropertyGrid::GetPropGrid()
    {
        return dynamic_cast<wxPropertyGrid2*>(GetWxWindow());
    }

	PropertyGrid::~PropertyGrid()
	{
        if (IsWxWindowCreated())
        {
            auto window = GetWxWindow();
            if (window != nullptr)
            {
                //window->Unbind(wxEVT_LEFT_DOWN, &AuiToolBar::OnLeftDown, this);    
            }
        }
    }

}
