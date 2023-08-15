#include "AuiNotebook.h"

namespace Alternet::UI
{
    class wxAuiNotebook2 : public wxAuiNotebook
    {
    public:
        wxAuiNotebook2(wxWindow* parent,
            wxWindowID id = wxID_ANY,
            const wxPoint& pos = wxDefaultPosition,
            const wxSize& size = wxDefaultSize,
            long style = wxAUI_NB_DEFAULT_STYLE)
        {
            Create(parent, id, pos, size, style);
        }
    };

	AuiNotebook::AuiNotebook()
	{

	}

	AuiNotebook::~AuiNotebook()
	{
	}
}
