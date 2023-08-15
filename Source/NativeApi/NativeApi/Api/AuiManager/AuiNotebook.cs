#pragma warning disable
using ApiCommon;
using System;
using System.Collections.Generic;
using System.Text;

namespace NativeApi.Api
{
    // https://docs.wxwidgets.org/3.2/classwx_aui_notebook.html
    public class AuiNotebook : Control
	{

	}
}


/*
    void SetArtProvider(wxAuiTabArt* art);
    wxAuiTabArt* GetArtProvider() const;

    virtual void SetUniformBitmapSize(const wxSize& size);
    virtual void SetTabCtrlHeight(int height);

    bool AddPage(wxWindow* page,
                 const wxString& caption,
                 bool select = false,
                 const wxBitmapBundle& bitmap = wxBitmapBundle());

    bool InsertPage(size_t pageIdx,
                    wxWindow* page,
                    const wxString& caption,
                    bool select = false,
                    const wxBitmapBundle& bitmap = wxBitmapBundle());

    bool DeletePage(size_t page) wxOVERRIDE;
    bool RemovePage(size_t page) wxOVERRIDE;

    virtual size_t GetPageCount() const wxOVERRIDE;
    virtual wxWindow* GetPage(size_t pageIdx) const wxOVERRIDE;
    virtual int FindPage(const wxWindow* page) const wxOVERRIDE;

    // This is wxAUI-specific equivalent of FindPage(), prefer to use the other
    // function.
    int GetPageIndex(wxWindow* pageWnd) const { return FindPage(pageWnd); }

    bool SetPageText(size_t page, const wxString& text) wxOVERRIDE;
    wxString GetPageText(size_t pageIdx) const wxOVERRIDE;

    bool SetPageToolTip(size_t page, const wxString& text);
    wxString GetPageToolTip(size_t pageIdx) const;

    bool SetPageBitmap(size_t page, const wxBitmapBundle& bitmap);
    wxBitmap GetPageBitmap(size_t pageIdx) const;

    int SetSelection(size_t newPage) wxOVERRIDE;
    int GetSelection() const wxOVERRIDE;

    virtual void Split(size_t page, int direction);

    const wxAuiManager& GetAuiManager() const { return m_mgr; }

    // Sets the normal font
    void SetNormalFont(const wxFont& font);

    // Sets the selected tab font
    void SetSelectedFont(const wxFont& font);

    // Sets the measuring font
    void SetMeasuringFont(const wxFont& font);

    // Sets the tab font
    virtual bool SetFont(const wxFont& font) wxOVERRIDE;

    // Gets the tab control height
    int GetTabCtrlHeight() const;

    // Gets the height of the notebook for a given page height
    int GetHeightForPageHeight(int pageHeight);

    // Shows the window menu
    bool ShowWindowMenu();

    // we do have multiple pages
    virtual bool HasMultiplePages() const wxOVERRIDE { return true; }

    // we don't want focus for ourselves
    // virtual bool AcceptsFocus() const { return false; }

    //wxBookCtrlBase functions

    virtual void SetPageSize (const wxSize &size) wxOVERRIDE;
    virtual int  HitTest (const wxPoint &pt, long *flags=NULL) const wxOVERRIDE;

    virtual int GetPageImage(size_t n) const wxOVERRIDE;
    virtual bool SetPageImage(size_t n, int imageId) wxOVERRIDE;

    virtual int ChangeSelection(size_t n) wxOVERRIDE;

    virtual bool AddPage(wxWindow *page, const wxString &text, bool select,
                         int imageId) wxOVERRIDE;
    virtual bool DeleteAllPages() wxOVERRIDE;
    virtual bool InsertPage(size_t index, wxWindow *page, const wxString &text,
                            bool select, int imageId) wxOVERRIDE;

    virtual wxSize DoGetBestSize() const wxOVERRIDE;

    wxAuiTabCtrl* GetTabCtrlFromPoint(const wxPoint& pt);
    wxAuiTabCtrl* GetActiveTabCtrl();
    bool FindTab(wxWindow* page, wxAuiTabCtrl** ctrl, int* idx);
 
 
 
 */