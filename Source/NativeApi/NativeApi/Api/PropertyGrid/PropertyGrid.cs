#pragma warning disable
using ApiCommon;
using System;
using System.Collections.Generic;
using System.Text;
using Alternet.Drawing;

namespace NativeApi.Api
{
	//https://docs.wxwidgets.org/3.2/overview_propgrid.html
	//https://docs.wxwidgets.org/3.2/classwx_property_grid.html
	public partial class PropertyGrid : Control
	{
        public static string NameAsLabel { get; }
        public bool HasBorder { get; set; }

        public static IntPtr CreateEx(long styles) => throw new Exception();

        public long CreateStyle { get; set; }

        /*
    void AddActionTrigger( int action, int keycode, int modifiers = 0 );

    void DedicateKey( int keycode )

    static void AutoGetTranslation( bool enable );

    bool ChangePropertyValue( wxPGPropArg id, wxVariant newValue );

    void CenterSplitter( bool enableAutoResizing = false );

    virtual void Clear();

    void ClearActionTriggers( int action );

    bool CommitChangesFromEditor( wxUint32 flags = 0 );

    void EditorsValueWasModified() { m_iFlags |= wxPG_FL_VALUE_MODIFIED; }

    void EditorsValueWasNotModified()

    bool EnableCategories( bool enable );

    bool EnsureVisible( wxPGPropArg id );

    wxSize FitColumns()

    wxWindow* GetPanel()

    wxColour GetCaptionBackgroundColour() const { return m_colCapBack; }

    wxFont& GetCaptionFont() { return m_captionFont; }

    wxColour GetCaptionForegroundColour() const { return m_colCapFore; }

    wxColour GetCellBackgroundColour() const { return m_colPropBack; }

    wxColour GetCellDisabledTextColour() const { return m_colDisPropFore; }

    wxColour GetCellTextColour() const { return m_colPropFore; }

    unsigned int GetColumnCount() const

    wxColour GetEmptySpaceColour() const { return m_colEmptySpace; }

    int GetFontHeight() const { return m_fontHeight; }

    wxPropertyGrid* GetGrid() { return this; }

    wxRect GetImageRect( wxPGProperty* p, int item ) const;

    wxSize GetImageSize( wxPGProperty* p = NULL, int item = -1 ) const;

    wxPGProperty* GetLastItem( int flags = wxPG_ITERATE_DEFAULT )

    wxPGProperty* GetLastItem( int flags = wxPG_ITERATE_DEFAULT )

    wxColour GetLineColour() const { return m_colLine; }

    wxColour GetMarginColour() const { return m_colMargin; }

    int GetMarginWidth() const { return m_marginWidth; }

    wxVariant GetUncommittedPropertyValue();

    wxPGProperty* GetRoot() const { return m_pState->m_properties; }

    int GetRowHeight() const { return m_lineHeight; }

    wxPGProperty* GetSelectedProperty() const { return GetSelection(); }

    wxColour GetSelectionBackgroundColour() const { return m_colSelBack; }

    wxColour GetSelectionForegroundColour() const { return m_colSelFore; }

    int GetSplitterPosition( unsigned int splitterIndex = 0 ) const

    wxTextCtrl* GetEditorTextCtrl() const;

    wxPGValidationInfo& GetValidationInfo()

    int GetVerticalSpacing() const { return (int)m_vspacing; }

    bool IsEditorFocused() const;

    bool IsEditorsValueModified()

    wxPropertyGridHitTestResult HitTest( const wxPoint& pt ) const;

    bool IsAnyModified() const

    void OnTLPChanging( wxWindow* newTLP );

    void RefreshProperty( wxPGProperty* p );

    static wxPGEditor* RegisterEditorClass( wxPGEditor* editor,
                                            bool noDefCheck = false )

    static wxPGEditor* DoRegisterEditorClass( wxPGEditor* editorClass,
                                              const wxString& editorName,
                                              bool noDefCheck = false );
    void ResetColours();

    void ResetColumnSizes( bool enableAutoResizing = false );

    bool SelectProperty( wxPGPropArg id, bool focus = false );

    void SetSelection( const wxArrayPGProperty& newSelection )

    bool AddToSelection( wxPGPropArg id )

    bool RemoveFromSelection( wxPGPropArg id )

    void MakeColumnEditable( unsigned int column, bool editable = true );

    void BeginLabelEdit( unsigned int column = 0 );

    void EndLabelEdit( bool commit = true );

    wxTextCtrl* GetLabelEditor() const

    void SetCaptionBackgroundColour(const wxColour& col);

    void SetCaptionTextColour(const wxColour& col);

    void SetCellBackgroundColour(const wxColour& col);

    void SetCellDisabledTextColour(const wxColour& col);

    void SetCellTextColour(const wxColour& col);

    void SetColumnCount( int colCount );

    void SetCurrentCategory( wxPGPropArg id );

    void SetEmptySpaceColour(const wxColour& col);

    void SetLineColour(const wxColour& col);

    void SetMarginColour(const wxColour& col);

    void SetSelectionBackgroundColour(const wxColour& col);

    void SetSelectionTextColour(const wxColour& col);

    void SetSplitterPosition( int newXPos, int col = 0 );

    void SetSortFunction( wxPGSortCallback sortFunction );

    wxPGSortCallback GetSortFunction();

    void SetUnspecifiedValueAppearance( const wxPGCell& cell );

    const wxPGCell& GetUnspecifiedValueAppearance() const

    wxString GetUnspecifiedValueText( int argFlags = 0 ) const;

    void SetVirtualWidth( int width );

    void SetSplitterLeft( bool privateChildrenToo = false );

    void SetVerticalSpacing( int vspacing );

    void ShowPropertyError( wxPGPropArg id, const wxString& msg );

    bool HasVirtualWidth() const

    const wxPGCommonValue* GetCommonValue( unsigned int i ) const

    unsigned int GetCommonValueCount() const

    wxString GetCommonValueLabel( unsigned int i ) const

    int GetUnspecifiedCommonValue() const { return m_cvUnspecified; }

    void SetUnspecifiedCommonValue( int index ) { m_cvUnspecified = index; }

    wxWindow* GenerateEditorButton( const wxPoint& pos, const wxSize& sz );

    void FixPosForTextCtrl( wxWindow* ctrl,
                            unsigned int forColumn = 1,
                            const wxPoint& offset = wxPoint(0, 0) );

    wxWindow* GenerateEditorTextCtrl( const wxPoint& pos,
                                      const wxSize& sz,
                                      const wxString& value,
                                      wxWindow* secondary,
                                      int extraStyle = 0,
                                      int maxLen = 0,
                                      unsigned int forColumn = 1 );

    wxWindow* GenerateEditorTextCtrlAndButton( const wxPoint& pos,
        const wxSize& sz, wxWindow** psecondary, int limited_editing,
        wxPGProperty* property );

    wxPoint GetGoodEditorDialogPosition( wxPGProperty* p,
                                         const wxSize& sz );

    static wxString& ExpandEscapeSequences( wxString& dst_str,
                                            const wxString& src_str );

    static wxString& CreateEscapeSequences( wxString& dst_str,
                                            const wxString& src_str );

    static bool IsSmallScreen();

    static wxBitmap RescaleBitmap(const wxBitmap& srcBmp, double scaleX, double scaleY);

    wxRect GetPropertyRect( const wxPGProperty* p1,
                            const wxPGProperty* p2 ) const;

    wxWindow* GetEditorControl() const;

    wxWindow* GetPrimaryEditor() const

    wxWindow* GetEditorControlSecondary() const

    void RefreshEditor();

    void SwitchState( wxPropertyGridPageState* pNewState );

    bool WasValueChangedInEvent() const

    int GetSpacingY() const { return m_spacingy; }

    void SetupTextCtrlValue( const wxString& text ) { m_prevTcValue = text; }

    bool UnfocusEditor();

        */
    }
}