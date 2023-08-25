#include "PropertyGrid.h"

namespace Alternet::UI
{
    class wxPropertyGrid2 : public wxPropertyGrid, public wxWidgetExtender
    {
    public:
        wxPropertyGrid2(wxWindow* parent, wxWindowID id = wxID_ANY,
            const wxPoint& pos = wxDefaultPosition,
            const wxSize& size = wxDefaultSize,
            long style = wxPG_DEFAULT_STYLE,
            const wxString& name = wxASCII_STR(wxPropertyGridNameStr))
            : wxPropertyGrid(parent, id, pos, size, style, name)
        {
        }
    };

    PropertyGrid::PropertyGrid(long styles)
    {
        _createStyle = styles;
        bindScrollEvents = false;
    }

    /*static*/ void* PropertyGrid::CreateEx(int64_t styles)
    {
        return new PropertyGrid(styles);
    }

    int64_t PropertyGrid::GetCreateStyle()
    {
        return _createStyle;
    }

    void PropertyGrid::SetCreateStyle(int64_t value)
    {
        if (_createStyle == value)
            return;
        _createStyle = value;
        RecreateWxWindowIfNeeded();
    }

    string PropertyGrid::GetNameAsLabel()
    {
        return wxStr(wxPG_LABEL);
    }

    bool PropertyGrid::GetHasBorder()
    {
        return _hasBorder;
    }

    void PropertyGrid::SetHasBorder(bool value)
    {
        if (_hasBorder == value)
            return;
        _hasBorder = value;
        RecreateWxWindowIfNeeded();
    }

    wxWindow* PropertyGrid::CreateWxWindowCore(wxWindow* parent)
    {
        long style = _createStyle;

        if (!_hasBorder)
            style |= wxBORDER_NONE;

        auto result = new wxPropertyGrid2(parent, wxID_ANY,
            wxDefaultPosition,
            wxDefaultSize,
            style);

        //toolbar->Bind(wxEVT_LEFT_DOWN, &AuiToolBar::OnLeftDown, this);
        return result;
    }

	PropertyGrid::PropertyGrid()
	{
        bindScrollEvents = false;
	}

    wxPropertyGridInterface* PropertyGrid::GetPropGridInterface()
    {
        return dynamic_cast<wxPropertyGridInterface*>(GetWxWindow());
    }
    
    wxPropertyGrid* PropertyGrid::GetPropGrid()
    {
        return dynamic_cast<wxPropertyGrid*>(GetWxWindow());
    }

	PropertyGrid::~PropertyGrid()
	{
        if (IsWxWindowCreated())
        {
            auto window = GetWxWindow();
            if (window != nullptr)
            {
                //window->Unbind(wxEVT_LEFT_DOWN, &AuiToolBar::OnLeftDown, this);    
            }
        }
    }

    void* PropertyGrid::CreateStringProperty(const string& label, const string& name,
        const string& value)
    {
        return new wxStringProperty(wxStr(label), wxStr(name), wxStr(value));
    }

    void* PropertyGrid::CreateBoolProperty(const string& label, const string& name, bool value)
    {
        return new wxBoolProperty(wxStr(label), wxStr(name), value);
    }

    void* PropertyGrid::CreateIntProperty(const string& label, const string& name, int64_t value)
    {
        return new wxIntProperty(wxStr(label), wxStr(name), (long)value);
    }

    void* PropertyGrid::CreateFloatProperty(const string& label, const string& name, double value)
    {
        return new wxFloatProperty(wxStr(label), wxStr(name), value);
    }

    void* PropertyGrid::CreateUIntProperty(const string& label, const string& name, uint64_t value)
    {
        return new wxUIntProperty(wxStr(label), wxStr(name), (unsigned long)value);
    }

    void* PropertyGrid::CreateLongStringProperty(const string& label, const string& name,
        const string& value)
    {
        return new wxLongStringProperty(wxStr(label), wxStr(name), wxStr(value));
    }

    void* PropertyGrid::CreateDateProperty(const string& label, const string& name,
        const DateTime& value)
    {
        wxDateTime dt = value;
        return new wxDateProperty(wxStr(label), wxStr(name), dt);
    }

    void PropertyGrid::Clear()
    {
        GetPropGrid()->Clear();
    }

    void* PropertyGrid::Append(void* property)
    {
        return GetPropGrid()->Append((wxPGProperty*)property);
    }

    bool PropertyGrid::ClearSelection(bool validation)
    {
        return GetPropGrid()->ClearSelection(validation);
    }

    void PropertyGrid::ClearModifiedStatus()
    {
        GetPropGrid()->ClearModifiedStatus();
    }

    bool PropertyGrid::CollapseAll()
    {
        return GetPropGrid()->CollapseAll();
    }

    bool PropertyGrid::EditorValidate()
    {
        return GetPropGrid()->EditorValidate();
    }

    bool PropertyGrid::ExpandAll(bool expand)
    {
        return GetPropGrid()->ExpandAll(expand);
    }

    void* PropertyGrid::CreateEnumProperty(const string& label, const string& name,
        void* choices, int value)
    {
        auto pgc = (PropertyGridChoices*)choices;

        return new wxEnumProperty(wxStr(label), wxStr(name), pgc->choices, value);
    }

    void* PropertyGrid::CreateColorProperty(const string& label, const string& name,
        const Color& value)
    {
        wxColor wxc = value;
        return new wxColourProperty(wxStr(label), wxStr(name), wxc);
    }

    void* PropertyGrid::CreateFlagsProperty(const string& label, const string& name,
        void* choices, int value)
    {
        auto pgc = (PropertyGridChoices*)choices;

        return new wxFlagsProperty(wxStr(label), wxStr(name), pgc->choices, value);
    }

    void PropertyGrid::SetBoolChoices(const string& trueChoice, const string& falseChoice)
    {
        wxPropertyGrid::SetBoolChoices(wxStr(trueChoice), wxStr(falseChoice));
    }

    void PropertyGrid::InitAllTypeHandlers()
    {
        wxPropertyGrid::InitAllTypeHandlers();
    }

    void PropertyGrid::RegisterAdditionalEditors()
    {
        wxPropertyGrid::RegisterAdditionalEditors();
    }

    void* PropertyGrid::CreatePropCategory(const string& label, const string& name)
    {
        return new wxPropertyCategory(wxStr(label), wxStr(name));
    }

    void* PropertyGrid::GetFirst(int flags)
    {
        return GetPropGridInterface()->GetFirst(flags);
    }

    void* PropertyGrid::GetProperty(const string& name)
    {
        return GetPropGrid()->GetProperty(wxStr(name));
    }

    void* PropertyGrid::GetPropertyByLabel(const string& label)
    {
        return GetPropGrid()->GetPropertyByLabel(wxStr(label));
    }

    void* PropertyGrid::GetPropertyByName(const string& name) 
    {
        return GetPropGrid()->GetPropertyByName(wxStr(name));
    }

    void* PropertyGrid::GetPropertyByNameAndSubName(const string& name, const string& subname) 
    {
        return GetPropGrid()->GetPropertyByName(wxStr(name), wxStr(subname));
    }

    void* PropertyGrid::GetSelection() 
    {
        return GetPropGrid()->GetSelection();
    }

    string PropertyGrid::GetPropertyName(void* property) 
    {
        return wxStr(GetPropGrid()->GetPropertyName((wxPGProperty*)property));
    }

    bool PropertyGrid::RestoreEditableState(const string& src, int restoreStates) 
    {
        return GetPropGrid()->RestoreEditableState(wxStr(src), restoreStates);
    }

    string PropertyGrid::SaveEditableState(int includedStates) 
    {
        return wxStr(GetPropGrid()->SaveEditableState(includedStates));
    }

    bool PropertyGrid::SetColumnProportion(uint32_t column, int proportion) 
    {
        return GetPropGrid()->SetColumnProportion(column, proportion);
    }

    int PropertyGrid::GetColumnProportion(uint32_t column) 
    {
        return GetPropGrid()->GetColumnProportion(column);
    }

    void PropertyGrid::Sort(int flags) 
    {
        return GetPropGrid()->Sort(flags);
    }

    void PropertyGrid::RefreshProperty(void* p) 
    {
        return GetPropGrid()->RefreshProperty((wxPGProperty*)p);
    }

    void PropertyGrid::ToPropArg(void * id)
    {
        _propArg = wxPGPropArgCls((wxPGProperty*)id);
    }

    void PropertyGrid::SetPropertyLabel(void* id, const string & newproplabel)
    {
        ToPropArg(id);
        GetPropGrid()->SetPropertyLabel(_propArg, wxStr(newproplabel));
    }

    void PropertyGrid::SetPropertyName(void* id, const string& newName)
    {
        ToPropArg(id);
        GetPropGrid()->SetPropertyName(_propArg, wxStr(newName));
    }

    void PropertyGrid::SetPropertyHelpString(void* id, const string& helpString) 
    {
        ToPropArg(id);
        GetPropGrid()->SetPropertyHelpString(_propArg, wxStr(helpString));
    }

    bool PropertyGrid::SetPropertyMaxLength(void* id, int maxLen)
    {
        ToPropArg(id);
        return GetPropGrid()->SetPropertyMaxLength(_propArg, maxLen);
    }

    void PropertyGrid::SetPropertyValueAsLong(void* id, int64_t value)
    {
        ToPropArg(id);
        GetPropGrid()->SetPropertyValue(_propArg, value);
    }

    void PropertyGrid::SetPropertyValueAsInt(void* id, int value)
    {
        ToPropArg(id);
        GetPropGrid()->SetPropertyValue(_propArg, value);
    }

    void PropertyGrid::SetPropertyValueAsDouble(void* id, double value)
    {
        ToPropArg(id);
        GetPropGrid()->SetPropertyValue(_propArg, value);
    }

    void PropertyGrid::SetPropertyValueAsBool(void* id, bool value)
    {
        ToPropArg(id);
        GetPropGrid()->SetPropertyValue(_propArg, value);
    }

    void PropertyGrid::SetPropertyValueAsStr(void* id, const string& value)
    {
        ToPropArg(id);
        GetPropGrid()->SetPropertyValue(_propArg, wxStr(value));
    }

    void PropertyGrid::SetPropertyValueAsDateTime(void* id, const DateTime& value)
    {
        ToPropArg(id);
        GetPropGrid()->SetPropertyValue(_propArg, value);
    }

    void PropertyGrid::SetValidationFailureBehavior(int vfbFlags)
    {
        GetPropGrid()->SetValidationFailureBehavior(vfbFlags);
    }

    void PropertyGrid::SortChildren(void* id, int flags) 
    {
        ToPropArg(id);
        GetPropGrid()->SortChildren(_propArg, flags);
    }

    /*static*/ void* PropertyGrid::GetEditorByName(const string& editorName)
    {
        return wxPropertyGrid::GetEditorByName(wxStr(editorName));
    }
    
    void PropertyGrid::SetPropertyReadOnly(void* id, bool set, int flags)
    {
        ToPropArg(id);
        GetPropGrid()->SetPropertyReadOnly(_propArg, set, flags);
    }

    void PropertyGrid::SetPropertyValueUnspecified(void* id)
    {
        ToPropArg(id);
        GetPropGrid()->SetPropertyValueUnspecified(_propArg);
    }

    void* PropertyGrid::AppendIn(void* id, void* newproperty)
    { 
        ToPropArg(id);
        return GetPropGrid()->AppendIn(_propArg, (wxPGProperty*)newproperty);
    }
    
    void PropertyGrid::BeginAddChildren(void* id)
    {
        ToPropArg(id);
        GetPropGrid()->BeginAddChildren(_propArg);
    }

    bool PropertyGrid::Collapse(void* id)
    {
        ToPropArg(id);
        return GetPropGrid()->Collapse(_propArg);
    }
    
    void PropertyGrid::DeleteProperty(void* id) 
    {
        ToPropArg(id);
        GetPropGrid()->DeleteProperty(_propArg);
    }
    
    void* PropertyGrid::RemoveProperty(void* id)
    { 
        ToPropArg(id);
        return GetPropGrid()->RemoveProperty(_propArg);
    }
    
    bool PropertyGrid::DisableProperty(void* id)
    { 
        ToPropArg(id);
        return GetPropGrid()->DisableProperty(_propArg);
    }
    
    bool PropertyGrid::EnableProperty(void* id, bool enable)
    {
        ToPropArg(id);
        return GetPropGrid()->EnableProperty(_propArg, enable);
    }

    void PropertyGrid::EndAddChildren(void* id)
    {
        ToPropArg(id);
        GetPropGrid()->EndAddChildren(_propArg);
    }

    bool PropertyGrid::Expand(void* id)
    { 
        ToPropArg(id);
        return GetPropGrid()->Expand(_propArg);
    }

    void* PropertyGrid::GetFirstChild(void* id)
    { 
        ToPropArg(id);
        return GetPropGrid()->GetFirstChild(_propArg);
    }

    void* PropertyGrid::GetPropertyCategory(void* id)
    { 
        ToPropArg(id);
        return GetPropGrid()->GetPropertyCategory(_propArg);
    }

    void* PropertyGrid::GetPropertyClientData(void* id)
    { 
        ToPropArg(id);
        return GetPropGrid()->GetPropertyClientData(_propArg);
    }

    string PropertyGrid::GetPropertyHelpString(void* id)
    { 
        ToPropArg(id);
        return wxStr(GetPropGrid()->GetPropertyHelpString(_propArg));
    }
    
    void* PropertyGrid::GetPropertyImage(void* id)
    { 
        ToPropArg(id);
        return GetPropGrid()->GetPropertyImage(_propArg);
    }

    string PropertyGrid::GetPropertyLabel(void* id)
    { 
        ToPropArg(id);
        return wxStr(GetPropGrid()->GetPropertyLabel(_propArg));
    }

    void* PropertyGrid::GetPropertyParent(void* id)
    { 
        ToPropArg(id);
        return GetPropGrid()->GetPropertyParent(_propArg);
    }

    string PropertyGrid::GetPropertyValueAsString(void* id)
    { 
        ToPropArg(id);
        return wxStr(GetPropGrid()->GetPropertyValueAsString(_propArg));
    }

    int64_t PropertyGrid::GetPropertyValueAsLong(void* id)
    { 
        ToPropArg(id);
        return GetPropGrid()->GetPropertyValueAsLong(_propArg);
    }

    uint64_t PropertyGrid::GetPropertyValueAsULong(void* id)
    { 
        ToPropArg(id);
        return GetPropGrid()->GetPropertyValueAsULong(_propArg);
    }
    
    int PropertyGrid::GetPropertyValueAsInt(void* id)
    { 
        ToPropArg(id);
        return GetPropGrid()->GetPropertyValueAsInt(_propArg);
    }

    bool PropertyGrid::GetPropertyValueAsBool(void* id)
    { 
        ToPropArg(id);
        return GetPropGrid()->GetPropertyValueAsBool(_propArg);
    }

    double PropertyGrid::GetPropertyValueAsDouble(void* id)
    { 
        ToPropArg(id);
        return GetPropGrid()->GetPropertyValueAsDouble(_propArg);
    }

    DateTime PropertyGrid::GetPropertyValueAsDateTime(void* id)
    { 
        ToPropArg(id);
        return GetPropGrid()->GetPropertyValueAsDateTime(_propArg);
    }

    bool PropertyGrid::HideProperty(void* id, bool hide, int flags)
    { 
        ToPropArg(id);
        return GetPropGrid()->HideProperty(_propArg, hide, flags);
    }

    void* PropertyGrid::Insert(void* priorThis, void* newproperty)
    { 
        ToPropArg(priorThis);
        return GetPropGrid()->Insert(_propArg, (wxPGProperty*)newproperty);
    }

    void* PropertyGrid::InsertByIndex(void* parent, int index, void* newproperty)
    { 
        ToPropArg(parent);
        return GetPropGrid()->Insert(_propArg, index, (wxPGProperty*)newproperty);
    }

    bool PropertyGrid::IsPropertyCategory(void* id)
    {
        ToPropArg(id);
        return GetPropGrid()->IsPropertyCategory(_propArg);
    }

    bool PropertyGrid::IsPropertyEnabled(void* id)
    {
        ToPropArg(id);
        return GetPropGrid()->IsPropertyEnabled(_propArg);
    }

    bool PropertyGrid::IsPropertyExpanded(void* id)
    {
        ToPropArg(id);
        return GetPropGrid()->IsPropertyExpanded(_propArg);
    }

    bool PropertyGrid::IsPropertyModified(void* id)
    {
        ToPropArg(id);
        return GetPropGrid()->IsPropertyModified(_propArg);
    }

    bool PropertyGrid::IsPropertySelected(void* id)
    {
        ToPropArg(id);
        return GetPropGrid()->IsPropertySelected(_propArg);
    }

    bool PropertyGrid::IsPropertyShown(void* id)
    {
        ToPropArg(id);
        return GetPropGrid()->IsPropertyShown(_propArg);
    }

    bool PropertyGrid::IsPropertyValueUnspecified(void* id)
    {
        ToPropArg(id);
        return GetPropGrid()->IsPropertyValueUnspecified(_propArg);
    }

    void PropertyGrid::LimitPropertyEditing(void* id, bool limit)
    {
        ToPropArg(id);
        GetPropGrid()->LimitPropertyEditing(_propArg, limit);
    }

    void* PropertyGrid::ReplaceProperty(void* id, void* property)
    { 
        ToPropArg(id);
        return GetPropGrid()->ReplaceProperty(_propArg, (wxPGProperty*)property);
    }

    void PropertyGrid::SetPropertyBackgroundColor(void* id, const Color& color, int flags)
    {
        ToPropArg(id);
        GetPropGrid()->SetPropertyBackgroundColour(_propArg, color, flags);
    }

    void PropertyGrid::SetPropertyColorsToDefault(void* id, int flags)
    {
        ToPropArg(id);
        GetPropGrid()->SetPropertyColoursToDefault(_propArg, flags);
    }

    void PropertyGrid::SetPropertyTextColor(void* id, const Color& col, int flags)
    {
        ToPropArg(id);
        GetPropGrid()->SetPropertyTextColour(_propArg, col, flags);
    }

    Color PropertyGrid::GetPropertyBackgroundColor(void* id)
    { 
        ToPropArg(id);
        return GetPropGrid()->GetPropertyBackgroundColour(_propArg);
    }

    Color PropertyGrid::GetPropertyTextColor(void* id)
    { 
        ToPropArg(id);
        return GetPropGrid()->GetPropertyTextColour(_propArg);
    }

    void PropertyGrid::SetPropertyClientData(void* id, void* clientData)
    {
        ToPropArg(id);
        GetPropGrid()->SetPropertyClientData(_propArg, clientData);
    }

    void PropertyGrid::SetPropertyEditor(void* id, void* editor)
    {
        ToPropArg(id);
        GetPropGrid()->SetPropertyEditor(_propArg, (wxPGEditor*)editor);
    }

    void PropertyGrid::SetPropertyEditorByName(void* id, const string& editorName)
    {
        ToPropArg(id);
        GetPropGrid()->SetPropertyEditor(_propArg, wxStr(editorName));
    }

}
