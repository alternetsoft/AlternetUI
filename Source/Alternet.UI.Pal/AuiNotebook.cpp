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

    void AuiNotebook::SetArtProvider(void* art)
    {
    }

    void* AuiNotebook::GetArtProvider()
    {
        return nullptr;
    }

    void AuiNotebook::SetUniformBitmapSize(int width, int height)
    {
    }

    void AuiNotebook::SetTabCtrlHeight(int height)
    {
    }

    bool AuiNotebook::AddPage(void* page, const string& caption,
        bool select, ImageSet* bitmap)
    {
        return false;
    }

    bool AuiNotebook::InsertPage(uint64_t pageIdx, void* page, const string& caption,
        bool select, ImageSet* bitmap)
    {
        return false;
    }

    bool AuiNotebook::DeletePage(uint64_t page)
    {
        return false;
    }

    bool AuiNotebook::RemovePage(uint64_t page)
    {
        return false;
    }

    uint64_t AuiNotebook::GetPageCount()
    {
        return 0;
    }

    void* AuiNotebook::GetPage(uint64_t pageIdx)
    {
        return nullptr;
    }

    int AuiNotebook::FindPage(void* page)
    {
        return 0;
    }

    bool AuiNotebook::SetPageText(uint64_t page, const string& text)
    {
        return false;
    }

    string AuiNotebook::GetPageText(uint64_t pageIdx)
    {
        return wxStr(wxEmptyString);
    }

    bool AuiNotebook::SetPageToolTip(uint64_t page, const string& text)
    {
        return false;
    }

    string AuiNotebook::GetPageToolTip(uint64_t pageIdx)
    {
        return wxStr(wxEmptyString);
    }

    bool AuiNotebook::SetPageBitmap(uint64_t page, ImageSet* bitmap)
    {
        return false;
    }

    int AuiNotebook::SetSelection(uint64_t newPage)
    {
        return 0;
    }

    int AuiNotebook::GetSelection()
    {
        return 0;
    }

    void AuiNotebook::Split(uint64_t page, int direction)
    {
    }

    int AuiNotebook::GetTabCtrlHeight()
    {
        return 0;
    }

    int AuiNotebook::GetHeightForPageHeight(int pageHeight)
    {
        return 0;
    }

    bool AuiNotebook::ShowWindowMenu()
    {
        return false;
    }

    bool AuiNotebook::DeleteAllPages()
    {
        return false;
    }
}
