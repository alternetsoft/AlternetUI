#include "AuiToolBar.h"

namespace Alternet::UI
{

    class wxAuiToolBar2 : public wxAuiToolBar
    {
    public:
        wxAuiToolBar2(wxWindow* parent,
            wxWindowID id = wxID_ANY,
            const wxPoint& pos = wxDefaultPosition,
            const wxSize& size = wxDefaultSize,
            long style = wxAUI_TB_DEFAULT_STYLE)
        {
            Create(parent, id, pos, size, style);
        }
    };

	AuiToolBar::AuiToolBar()
	{

	}

	AuiToolBar::~AuiToolBar()
	{
	}

}
