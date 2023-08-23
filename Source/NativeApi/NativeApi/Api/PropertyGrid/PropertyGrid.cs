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

    void CenterSplitter( bool enableAutoResizing = false );

    void ClearActionTriggers( int action );

    bool CommitChangesFromEditor( wxUint32 flags = 0 );

    void EditorsValueWasModified() { m_iFlags |= wxPG_FL_VALUE_MODIFIED; }

    void EditorsValueWasNotModified()

    bool EnableCategories( bool enable );

    wxSize FitColumns()

    wxColour GetCaptionBackgroundColour() const { return m_colCapBack; }

    wxColour GetCaptionForegroundColour() const { return m_colCapFore; }

    wxColour GetCellBackgroundColour() const { return m_colPropBack; }

    wxColour GetCellDisabledTextColour() const { return m_colDisPropFore; }

    wxColour GetCellTextColour() const { return m_colPropFore; }

    unsigned int GetColumnCount() const

    wxColour GetEmptySpaceColour() const { return m_colEmptySpace; }

    int GetFontHeight() const { return m_fontHeight; }

    wxColour GetLineColour() const { return m_colLine; }

    wxColour GetMarginColour() const { return m_colMargin; }

    int GetMarginWidth() const { return m_marginWidth; }

    int GetRowHeight() const { return m_lineHeight; }

    wxColour GetSelectionBackgroundColour() const { return m_colSelBack; }

    wxColour GetSelectionForegroundColour() const { return m_colSelFore; }

    int GetSplitterPosition( unsigned int splitterIndex = 0 ) const

    int GetVerticalSpacing() const { return (int)m_vspacing; }

    bool IsEditorFocused() const;

    bool IsEditorsValueModified()

    bool IsAnyModified() const

    void ResetColours();

    void ResetColumnSizes( bool enableAutoResizing = false );

    void MakeColumnEditable( unsigned int column, bool editable = true );

    void BeginLabelEdit( unsigned int column = 0 );

    void EndLabelEdit( bool commit = true );

    void SetCaptionBackgroundColour(const wxColour& col);

    void SetCaptionTextColour(const wxColour& col);

    void SetCellBackgroundColour(const wxColour& col);

    void SetCellDisabledTextColour(const wxColour& col);

    void SetCellTextColour(const wxColour& col);

    void SetColumnCount( int colCount );

    void SetEmptySpaceColour(const wxColour& col);

    void SetLineColour(const wxColour& col);

    void SetMarginColour(const wxColour& col);

    void SetSelectionBackgroundColour(const wxColour& col);

    void SetSelectionTextColour(const wxColour& col);

    void SetSplitterPosition( int newXPos, int col = 0 );

    wxString GetUnspecifiedValueText( int argFlags = 0 ) const;

    void SetVirtualWidth( int width );

    void SetSplitterLeft( bool privateChildrenToo = false );

    void SetVerticalSpacing( int vspacing );

    bool HasVirtualWidth() const

    unsigned int GetCommonValueCount() const

    wxString GetCommonValueLabel( unsigned int i ) const

    int GetUnspecifiedCommonValue() const { return m_cvUnspecified; }

    void SetUnspecifiedCommonValue( int index ) { m_cvUnspecified = index; }

    static bool IsSmallScreen();

    void RefreshEditor();

    bool WasValueChangedInEvent() const

    int GetSpacingY() const { return m_spacingy; }

    void SetupTextCtrlValue( const wxString& text ) { m_prevTcValue = text; }

    bool UnfocusEditor();
====
    bool ChangePropertyValue( wxPGPropArg id, wxVariant newValue );

    bool EnsureVisible( wxPGPropArg id );

    wxWindow* GetPanel()

    wxFont& GetCaptionFont() { return m_captionFont; }

    wxPropertyGrid* GetGrid() { return this; }

    wxRect GetImageRect( wxPGProperty* p, int item ) const;

    wxSize GetImageSize( wxPGProperty* p = NULL, int item = -1 ) const;

    wxPGProperty* GetLastItem( int flags = wxPG_ITERATE_DEFAULT )

    wxPGProperty* GetLastItem( int flags = wxPG_ITERATE_DEFAULT )

    wxVariant GetUncommittedPropertyValue();

    wxPGProperty* GetRoot() const { return m_pState->m_properties; }

    wxPGProperty* GetSelectedProperty() const { return GetSelection(); }

    wxTextCtrl* GetEditorTextCtrl() const;

    wxPGValidationInfo& GetValidationInfo()

    wxPropertyGridHitTestResult HitTest( const wxPoint& pt ) const;

    void OnTLPChanging( wxWindow* newTLP );

    void RefreshProperty( wxPGProperty* p );

    static wxPGEditor* RegisterEditorClass( wxPGEditor* editor,
                                            bool noDefCheck = false )

    static wxPGEditor* DoRegisterEditorClass( wxPGEditor* editorClass,
                                              const wxString& editorName,
                                              bool noDefCheck = false );
    bool SelectProperty( wxPGPropArg id, bool focus = false );

    void SetSelection( const wxArrayPGProperty& newSelection )

    bool AddToSelection( wxPGPropArg id )

    bool RemoveFromSelection( wxPGPropArg id )

    wxTextCtrl* GetLabelEditor() const

    void SetCurrentCategory( wxPGPropArg id );

    void SetSortFunction( wxPGSortCallback sortFunction );

    wxPGSortCallback GetSortFunction();

    void SetUnspecifiedValueAppearance( const wxPGCell& cell );

    const wxPGCell& GetUnspecifiedValueAppearance() const

    void ShowPropertyError( wxPGPropArg id, const wxString& msg );

    const wxPGCommonValue* GetCommonValue( unsigned int i ) const

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

    static wxBitmap RescaleBitmap(const wxBitmap& srcBmp, double scaleX, double scaleY);

    wxRect GetPropertyRect( const wxPGProperty* p1,
                            const wxPGProperty* p2 ) const;

    wxWindow* GetEditorControl() const;

    wxWindow* GetPrimaryEditor() const

    wxWindow* GetEditorControlSecondary() const

    void SwitchState( wxPropertyGridPageState* pNewState );
        */
    }
}