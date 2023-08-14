#pragma warning disable
using ApiCommon;
using System;
using System.Collections.Generic;
using System.Text;

namespace NativeApi.Api
{
	//https://docs.wxwidgets.org/3.2/classwx_aui_tool_bar.html
	public class AuiToolBar	: Control
	{

	}
}

/*
    void SetArtProvider(wxAuiToolBarArt* art);
    wxAuiToolBarArt* GetArtProvider() const;

    bool SetFont(const wxFont& font) wxOVERRIDE;


    wxAuiToolBarItem* AddTool(int toolId,
                 const wxString& label,
                 const wxBitmapBundle& bitmap,
                 const wxString& shortHelpString = wxEmptyString,
                 wxItemKind kind = wxITEM_NORMAL);

    wxAuiToolBarItem* AddTool(int toolId,
                 const wxString& label,
                 const wxBitmapBundle& bitmap,
                 const wxBitmapBundle& disabledBitmap,
                 wxItemKind kind,
                 const wxString& shortHelpString,
                 const wxString& longHelpString,
                 wxObject* clientData);

    wxAuiToolBarItem* AddTool(int toolId,
                 const wxBitmapBundle& bitmap,
                 const wxBitmapBundle& disabledBitmap,
                 bool toggle = false,
                 wxObject* clientData = NULL,
                 const wxString& shortHelpString = wxEmptyString,
                 const wxString& longHelpString = wxEmptyString)
    {
        return AddTool(toolId,
                wxEmptyString,
                bitmap,
                disabledBitmap,
                toggle ? wxITEM_CHECK : wxITEM_NORMAL,
                shortHelpString,
                longHelpString,
                clientData);
    }

    wxAuiToolBarItem* AddLabel(int toolId,
                  const wxString& label = wxEmptyString,
                  const int width = -1);
    wxAuiToolBarItem* AddControl(wxControl* control,
                    const wxString& label = wxEmptyString);
    wxAuiToolBarItem* AddSeparator();
    wxAuiToolBarItem* AddSpacer(int pixels);
    wxAuiToolBarItem* AddStretchSpacer(int proportion = 1);

    bool Realize();

    wxControl* FindControl(int windowId);
    wxAuiToolBarItem* FindToolByPosition(wxCoord x, wxCoord y) const;
    wxAuiToolBarItem* FindToolByIndex(int idx) const;
    wxAuiToolBarItem* FindTool(int toolId) const;

    void ClearTools() { Clear() ; }
    void Clear();

    bool DestroyTool(int toolId);
    bool DestroyToolByIndex(int idx);

    // Note that these methods do _not_ delete the associated control, if any.
    // Use DestroyTool() or DestroyToolByIndex() if this is wanted.
    bool DeleteTool(int toolId);
    bool DeleteByIndex(int toolId);

    size_t GetToolCount() const;
    int GetToolPos(int toolId) const { return GetToolIndex(toolId); }
    int GetToolIndex(int toolId) const;
    bool GetToolFits(int toolId) const;
    wxRect GetToolRect(int toolId) const;
    bool GetToolFitsByIndex(int toolId) const;
    bool GetToolBarFits() const;

    void SetMargins(const wxSize& size) { SetMargins(size.x, size.x, size.y, size.y); }
    void SetMargins(int x, int y) { SetMargins(x, x, y, y); }
    void SetMargins(int left, int right, int top, int bottom);

    void SetToolBitmapSize(const wxSize& size);
    wxSize GetToolBitmapSize() const;

    bool GetOverflowVisible() const;
    void SetOverflowVisible(bool visible);

    bool GetGripperVisible() const;
    void SetGripperVisible(bool visible);

    void ToggleTool(int toolId, bool state);
    bool GetToolToggled(int toolId) const;

    void EnableTool(int toolId, bool state);
    bool GetToolEnabled(int toolId) const;

    void SetToolDropDown(int toolId, bool dropdown);
    bool GetToolDropDown(int toolId) const;

    void SetToolBorderPadding(int padding);
    int  GetToolBorderPadding() const;

    void SetToolTextOrientation(int orientation);
    int  GetToolTextOrientation() const;

    void SetToolPacking(int packing);
    int  GetToolPacking() const;

    void SetToolProportion(int toolId, int proportion);
    int  GetToolProportion(int toolId) const;

    void SetToolSeparation(int separation);
    int GetToolSeparation() const;

    void SetToolSticky(int toolId, bool sticky);
    bool GetToolSticky(int toolId) const;

    wxString GetToolLabel(int toolId) const;
    void SetToolLabel(int toolId, const wxString& label);

    wxBitmap GetToolBitmap(int toolId) const;
    void SetToolBitmap(int toolId, const wxBitmapBundle& bitmap);

    wxString GetToolShortHelp(int toolId) const;
    void SetToolShortHelp(int toolId, const wxString& helpString);

    wxString GetToolLongHelp(int toolId) const;
    void SetToolLongHelp(int toolId, const wxString& helpString);

    void SetCustomOverflowItems(const wxAuiToolBarItemArray& prepend,
                                const wxAuiToolBarItemArray& append);

    // get size of hint rectangle for a particular dock location
    wxSize GetHintSize(int dockDirection) const;
    bool IsPaneValid(const wxAuiPaneInfo& pane) const;
 */