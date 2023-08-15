#include "AuiToolBar.h"

namespace Alternet::UI
{

    class wxAuiToolBar2 : public wxAuiToolBar, public wxWidgetExtender
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

    wxWindow* AuiToolBar::CreateWxWindowCore(wxWindow* parent)
    {
        long style = wxAUI_TB_DEFAULT_STYLE;

        auto toolbar = new wxAuiToolBar2(parent,
            wxID_ANY,
            wxDefaultPosition,
            wxDefaultSize,
            style);

        //toolbar->Bind(wxEVT_BUTTON, &Button::OnButtonClick, this);
        return toolbar;
    }

    wxAuiToolBar* AuiToolBar::GetToolbar()
    {
        return dynamic_cast<wxAuiToolBar2*>(GetWxWindow());
    }

	AuiToolBar::~AuiToolBar()
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

    void AuiToolBar::SetArtProvider(void* art)
    {
    }

    void* AuiToolBar::GetArtProvider()
    {
        return nullptr;
    }

    void* AuiToolBar::AddTool(int toolId, const string& label, void* bitmapBundle,
        const string& shortHelpString, int itemKind)
    {
        return nullptr;
    }

    void* AuiToolBar::AddTool2(int toolId, const string& label, void* bitmapBundle,
        void* disabledBitmapBundle, int itemKind, const string& shortHelpString,
        const string& longHelpString, void* clientData)
    {
        return nullptr;
    }

    void* AuiToolBar::AddTool3(int toolId, void* bitmapBundle,
        void* disabledBitmapBundle, bool toggle, void* clientData,
        const string& shortHelpString, const string& longHelpString)
    {
        return nullptr;
    }

    void* AuiToolBar::AddLabel(int toolId, const string& label, int width)
    {
        return nullptr;
    }

    void* AuiToolBar::AddControl(void* control, const string& label)
    {
        return nullptr;
    }

    void* AuiToolBar::AddSeparator()
    {
        return nullptr;
    }

    void* AuiToolBar::AddSpacer(int pixels)
    {
        return nullptr;
    }

    void* AuiToolBar::AddStretchSpacer(int proportion)
    {
        return nullptr;
    }

    bool AuiToolBar::Realize()
    {
        return false;
    }

    void* AuiToolBar::FindControl(int windowId)
    {
        return nullptr;
    }

    void* AuiToolBar::FindToolByPosition(int x, int y)
    {
        return nullptr;
    }

    void* AuiToolBar::FindToolByIndex(int idx)
    {
        return nullptr;
    }

    void* AuiToolBar::FindTool(int toolId)
    {
        return nullptr;
    }

    void AuiToolBar::Clear()
    {
    }

    bool AuiToolBar::DestroyTool(int toolId)
    {
        return false;
    }

    bool AuiToolBar::DestroyToolByIndex(int idx)
    {
        return false;
    }

    bool AuiToolBar::DeleteTool(int toolId)
    {
        return false;
    }

    bool AuiToolBar::DeleteByIndex(int toolId)
    {
        return false;
    }

    int AuiToolBar::GetToolIndex(int toolId)
    {
        return 0;
    }

    bool AuiToolBar::GetToolFits(int toolId)
    {
        return false;
    }

    Rect AuiToolBar::GetToolRect(int toolId)
    {
        return Rect();
    }

    bool AuiToolBar::GetToolFitsByIndex(int toolId)
    {
        return false;
    }

    bool AuiToolBar::GetToolBarFits()
    {
        return false;
    }

    void AuiToolBar::SetToolBitmapSize(const Size& size)
    {
    }

    Size AuiToolBar::GetToolBitmapSize()
    {
        return Size();
    }

    bool AuiToolBar::GetOverflowVisible()
    {
        return false;
    }

    void AuiToolBar::SetOverflowVisible(bool visible)
    {
    }

    bool AuiToolBar::GetGripperVisible()
    {
        return false;
    }

    void AuiToolBar::SetGripperVisible(bool visible)
    {
    }

    void AuiToolBar::ToggleTool(int toolId, bool state)
    {
    }

    bool AuiToolBar::GetToolToggled(int toolId)
    {
        return false;
    }

    void AuiToolBar::SetMargins(int left, int right, int top, int bottom)
    {
    }

    void AuiToolBar::EnableTool(int toolId, bool state)
    {
    }

    bool AuiToolBar::GetToolEnabled(int toolId)
    {
        return false;
    }

    void AuiToolBar::SetToolDropDown(int toolId, bool dropdown)
    {
    }

    bool AuiToolBar::GetToolDropDown(int toolId)
    {
        return false;
    }

    void AuiToolBar::SetToolBorderPadding(int padding)
    {
    }

    int AuiToolBar::GetToolBorderPadding()
    {
        return 0;
    }

    void AuiToolBar::SetToolTextOrientation(int orientation)
    {
    }

    int AuiToolBar::GetToolTextOrientation()
    {
        return 0;
    }

    void AuiToolBar::SetToolPacking(int packing)
    {
    }

    int AuiToolBar::GetToolPacking()
    {
        return 0;
    }

    void AuiToolBar::SetToolProportion(int toolId, int proportion)
    {
    }

    int AuiToolBar::GetToolProportion(int toolId)
    {
        return 0;
    }

    void AuiToolBar::SetToolSeparation(int separation)
    {
    }

    int AuiToolBar::GetToolSeparation()
    {
        return 0;
    }

    void AuiToolBar::SetToolSticky(int toolId, bool sticky)
    {
    }

    bool AuiToolBar::GetToolSticky(int toolId)
    {
        return false;
    }

    string AuiToolBar::GetToolLabel(int toolId)
    {
        return wxStr(wxEmptyString);
    }

    void AuiToolBar::SetToolLabel(int toolId, const string& label)
    {
    }

    void* AuiToolBar::GetToolBitmap(int toolId)
    {
        return nullptr;
    }

    void AuiToolBar::SetToolBitmap(int toolId, void* bitmapBundle)
    {
    }

    string AuiToolBar::GetToolShortHelp(int toolId)
    {
        return wxStr(wxEmptyString);
    }

    void AuiToolBar::SetToolShortHelp(int toolId, const string& helpString)
    {
    }

    string AuiToolBar::GetToolLongHelp(int toolId)
    {
        return wxStr(wxEmptyString);
    }

    void AuiToolBar::SetToolLongHelp(int toolId, const string& helpString)
    {
    }

    uint64_t AuiToolBar::GetToolCount()
    {
        return 0;
    }
}
