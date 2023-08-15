#include "AuiNotebook.h"

namespace Alternet::UI
{
    class wxAuiNotebook2 : public wxAuiNotebook, public wxWidgetExtender
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
        if (IsWxWindowCreated())
        {
            auto window = GetWxWindow();
            if (window != nullptr)
            {
                //window->Unbind(wxEVT_BUTTON, &Button::OnButtonClick, this);
            }
        }
    }

    wxWindow* AuiNotebook::CreateWxWindowCore(wxWindow* parent)
    {
        long style = wxAUI_NB_DEFAULT_STYLE;

        auto toolbar = new wxAuiNotebook2(parent,
            wxID_ANY,
            wxDefaultPosition,
            wxDefaultSize,
            style);

        //toolbar->Bind(wxEVT_BUTTON, &Button::OnButtonClick, this);
        return toolbar;
    }

    wxAuiNotebook* AuiNotebook::GetNotebook()
    {
        return dynamic_cast<wxAuiNotebook*>(GetWxWindow());
    }

}
