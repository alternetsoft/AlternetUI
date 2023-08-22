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
        public IntPtr CreateStringProperty(string label, string name, string value) =>
            throw new Exception();

        public IntPtr CreateBoolProperty(string label, string name, bool value = false) =>
            throw new Exception();

        public IntPtr CreateIntProperty(string label, string name, long value = 0) =>
            throw new Exception();

        public IntPtr CreateFloatProperty(string label, string name, double value = 0.0) =>
            throw new Exception();

        public IntPtr CreateUIntProperty(string label, string name, ulong value = 0) =>
            throw new Exception();

        public IntPtr CreateLongStringProperty(string label, string name, string value) =>
            throw new Exception();

        public IntPtr CreateDateProperty(string label, string name, DateTime value) =>
            throw new Exception();

        public void Clear() => throw new Exception();

        public IntPtr Append(IntPtr property) => throw new Exception();

        /*public IntPtr CreateFlagsProperty(
            string label,
            string name,
            string[] labels,
            int[] values,
            int value = 0) => throw new Exception();

        public IntPtr CreateEnumProperty(
            string label,
            string name,
            string[] labels,
            int[] values,
            int value = 0) => throw new Exception();*/

        /*
        public bool ClearSelection(bool validation = false) => throw new Exception();

        public void ClearModifiedStatus() => throw new Exception();

        public bool CollapseAll() => throw new Exception();

        public bool EditorValidate() => throw new Exception();

        public bool ExpandAll(bool expand = true) => throw new Exception();



        ====
            wxPGProperty* AppendIn( wxPGPropArg id, wxPGProperty* newproperty );

            void BeginAddChildren( wxPGPropArg id );

            bool Collapse( wxPGPropArg id );

            bool ChangePropertyValue( wxPGPropArg id, wxVariant newValue );

            void DeleteProperty( wxPGPropArg id );

            wxPGProperty* RemoveProperty( wxPGPropArg id );

            bool DisableProperty( wxPGPropArg id )

            bool EnableProperty( wxPGPropArg id, bool enable = true );

            void EndAddChildren( wxPGPropArg id );

            bool Expand( wxPGPropArg id );

            wxPGProperty* GetFirstChild( wxPGPropArg id )

            wxPropertyGridIterator GetIterator( int flags = wxPG_ITERATE_DEFAULT,
                                                wxPGProperty* firstProp = NULL )

            wxPropertyGridConstIterator
            GetIterator( int flags = wxPG_ITERATE_DEFAULT,
                         wxPGProperty* firstProp = NULL ) const

            wxPropertyGridIterator GetIterator( int flags, int startPos )

            wxPropertyGridConstIterator GetIterator( int flags, int startPos ) const

            wxPGProperty* GetFirst( int flags = wxPG_ITERATE_ALL )

            const wxPGProperty* GetFirst( int flags = wxPG_ITERATE_ALL ) const

            wxPGProperty* GetProperty( const wxString& name ) const

            const wxPGAttributeStorage& GetPropertyAttributes( wxPGPropArg id ) const

            void GetPropertiesWithFlag( wxArrayPGProperty* targetArr,
                                        wxPGProperty::FlagType flags,
                                        bool inverse = false,
                                        int iterFlags = wxPG_ITERATE_PROPERTIES |
                                                        wxPG_ITERATE_HIDDEN |
                                                        wxPG_ITERATE_CATEGORIES) const;
            wxVariant GetPropertyAttribute( wxPGPropArg id,
                                            const wxString& attrName ) const

            wxPropertyCategory* GetPropertyCategory( wxPGPropArg id ) const

            void* GetPropertyClientData( wxPGPropArg id ) const

            wxPGProperty* GetPropertyByLabel( const wxString& label ) const;

            wxPGProperty* GetPropertyByName( const wxString& name ) const;

            wxPGProperty* GetPropertyByName( const wxString& name,
                                             const wxString& subname ) const;

            const wxPGEditor* GetPropertyEditor( wxPGPropArg id ) const

            wxString GetPropertyHelpString( wxPGPropArg id ) const

            wxBitmap* GetPropertyImage( wxPGPropArg id ) const

            const wxString& GetPropertyLabel( wxPGPropArg id )

            wxString GetPropertyName( wxPGProperty* property )

            wxPGProperty* GetPropertyParent( wxPGPropArg id )

            wxVariant GetPropertyValue(wxPGPropArg id);

            wxString GetPropertyValueAsString( wxPGPropArg id ) const;
            long GetPropertyValueAsLong( wxPGPropArg id ) const;
            unsigned long GetPropertyValueAsULong( wxPGPropArg id ) const

            int GetPropertyValueAsInt( wxPGPropArg id ) const
                { return (int)GetPropertyValueAsLong(id); }

            bool GetPropertyValueAsBool( wxPGPropArg id ) const;

            double GetPropertyValueAsDouble( wxPGPropArg id ) const;

            wxArrayString GetPropertyValueAsArrayString(wxPGPropArg id) const;

            wxLongLong_t GetPropertyValueAsLongLong(wxPGPropArg id) const;

            wxULongLong_t GetPropertyValueAsULongLong(wxPGPropArg id) const;

            wxArrayInt GetPropertyValueAsArrayInt(wxPGPropArg id) const;

            wxDateTime GetPropertyValueAsDateTime(wxPGPropArg id) const;

            wxVariant GetPropertyValues( const wxString& listname = wxEmptyString,
                wxPGProperty* baseparent = NULL, long flags = 0 ) const

            wxPGProperty* GetSelection() const;

            const wxArrayPGProperty& GetSelectedProperties() const

            wxPropertyGridPageState* GetState() const { return m_pState; }

            virtual wxPGVIterator GetVIterator( int flags ) const;

            bool HideProperty( wxPGPropArg id,
                               bool hide = true,
                               int flags = wxPG_RECURSE );

            static void InitAllTypeHandlers()

            wxPGProperty* Insert( wxPGPropArg priorThis, wxPGProperty* newproperty );

            wxPGProperty* Insert( wxPGPropArg parent,
                                  int index,
                                  wxPGProperty* newproperty );

            bool IsPropertyCategory( wxPGPropArg id ) const

            bool IsPropertyEnabled( wxPGPropArg id ) const

            bool IsPropertyExpanded( wxPGPropArg id ) const;

            bool IsPropertyModified( wxPGPropArg id ) const

            bool IsPropertySelected( wxPGPropArg id ) const

            bool IsPropertyShown( wxPGPropArg id ) const

            bool IsPropertyValueUnspecified( wxPGPropArg id ) const

            void LimitPropertyEditing( wxPGPropArg id, bool limit = true );

            virtual void RefreshGrid( wxPropertyGridPageState* state = NULL );

            static void RegisterAdditionalEditors();

            wxPGProperty* ReplaceProperty( wxPGPropArg id, wxPGProperty* property );

            enum EditableStateFlags
            {
                // Include selected property.
                SelectionState   = 0x01,
                // Include expanded/collapsed property information.
                ExpandedState    = 0x02,
                // Include scrolled position.
                ScrollPosState   = 0x04,
                // Include selected page information.
                // Only applies to wxPropertyGridManager.
                PageState        = 0x08,
                // Include splitter position. Stored for each page.
                SplitterPosState = 0x10,
                // Include description box size.
                // Only applies to wxPropertyGridManager.
                DescBoxState     = 0x20,

                // Include all supported user editable state information.
                // This is usually the default value.
                AllStates        = SelectionState |
                                   ExpandedState |
                                   ScrollPosState |
                                   PageState |
                                   SplitterPosState |
                                   DescBoxState
            };

            bool RestoreEditableState( const wxString& src,
                                       int restoreStates = AllStates );

            wxString SaveEditableState( int includedStates = AllStates ) const;

            static void SetBoolChoices( const wxString& trueChoice,
                                        const wxString& falseChoice );

            bool SetColumnProportion( unsigned int column, int proportion );

            int GetColumnProportion( unsigned int column ) const

            void SetPropertyAttribute( wxPGPropArg id,
                                       const wxString& attrName,
                                       wxVariant value,
                                       long argFlags = 0 )

            void SetPropertyAttributeAll( const wxString& attrName, wxVariant value );

            void SetPropertyBackgroundColour( wxPGPropArg id,
                                              const wxColour& colour,
                                              int flags = wxPG_RECURSE );

            void SetPropertyColoursToDefault(wxPGPropArg id, int flags = wxPG_DONT_RECURSE);

            void SetPropertyTextColour( wxPGPropArg id,
                                        const wxColour& col,
                                        int flags = wxPG_RECURSE );

            wxColour GetPropertyBackgroundColour( wxPGPropArg id ) const

            wxColour GetPropertyTextColour( wxPGPropArg id ) const

            void SetPropertyCell( wxPGPropArg id,
                                  int column,
                                  const wxString& text = wxEmptyString,
                                  const wxBitmapBundle& bitmap = wxBitmapBundle(),
                                  const wxColour& fgCol = wxNullColour,
                                  const wxColour& bgCol = wxNullColour );

            void SetPropertyClientData( wxPGPropArg id, void* clientData )

            void SetPropertyEditor( wxPGPropArg id, const wxPGEditor* editor )

            void SetPropertyEditor( wxPGPropArg id, const wxString& editorName )

            void SetPropertyLabel( wxPGPropArg id, const wxString& newproplabel );

            void SetPropertyName( wxPGPropArg id, const wxString& newName )

            void SetPropertyReadOnly( wxPGPropArg id,
                                      bool set = true,
                                      int flags = wxPG_RECURSE );

            void SetPropertyValueUnspecified( wxPGPropArg id )

            void SetPropertyValues( const wxVariantList& list,
                                    wxPGPropArg defaultCategory = wxNullProperty )

            void SetPropertyValues( const wxVariant& list,
                                    wxPGPropArg defaultCategory = wxNullProperty )

            void SetPropertyHelpString( wxPGPropArg id, const wxString& helpString )

            void SetPropertyImage( wxPGPropArg id, const wxBitmapBundle& bmp )

            bool SetPropertyMaxLength( wxPGPropArg id, int maxLen );

            void SetPropertyValidator( wxPGPropArg id, const wxValidator& validator )

            void SetPropertyValue( wxPGPropArg id, long value )

            void SetPropertyValue( wxPGPropArg id, int value )

            void SetPropertyValue( wxPGPropArg id, double value )

            void SetPropertyValue( wxPGPropArg id, bool value )

            void SetPropertyValue( wxPGPropArg id, const wchar_t* value )

            void SetPropertyValue( wxPGPropArg id, const char* value )

            void SetPropertyValue( wxPGPropArg id, const wxString& value )

            void SetPropertyValue( wxPGPropArg id, const wxArrayString& value )

            void SetPropertyValue( wxPGPropArg id, const wxDateTime& value )

            void SetPropertyValue( wxPGPropArg id, wxObject* value )

            void SetPropertyValue( wxPGPropArg id, wxObject& value )
            void SetPropertyValue(wxPGPropArg id, wxLongLong_t value)

            void SetPropertyValue( wxPGPropArg id, wxLongLong value )

            void SetPropertyValue(wxPGPropArg id, wxULongLong_t value)

            void SetPropertyValue( wxPGPropArg id, wxULongLong value )

            void SetPropertyValue( wxPGPropArg id, const wxArrayInt& value )
            void SetPropertyValueString( wxPGPropArg id, const wxString& value );
            void SetPropertyValue( wxPGPropArg id, wxVariant value )

            void SetPropVal( wxPGPropArg id, wxVariant& value );

            void SetValidationFailureBehavior( int vfbFlags );

            void Sort( int flags = 0 );
            void SortChildren( wxPGPropArg id, int flags = 0 )

            wxPGProperty* GetPropertyByNameA( const wxString& name ) const;

            static wxPGEditor* GetEditorByName( const wxString& editorName );

            virtual void RefreshProperty( wxPGProperty* p ) = 0;

         */
    }
}