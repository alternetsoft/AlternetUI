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

    //--------------------------------------

    void PropertyGrid::SetPropertyLabel(void* id, const string & newproplabel){}
    void PropertyGrid::SetPropertyName(void* id, const string& newName) {}
    void PropertyGrid::SetPropertyHelpString(void* id, const string& helpString) {}
    bool PropertyGrid::SetPropertyMaxLength(void* id, int maxLen) { return false; }
    void PropertyGrid::SetPropertyValueAsLong(void* id, int64_t value) {}
    void PropertyGrid::SetPropertyValueAsInt(void* id, int value) {}
    void PropertyGrid::SetPropertyValueAsDouble(void* id, double value) {}
    void PropertyGrid::SetPropertyValueAsBool(void* id, bool value) {}
    void PropertyGrid::SetPropertyValueAsStr(void* id, const string& value) {}
    void PropertyGrid::SetPropertyValueAsDateTime(void* id, const DateTime& value) {}
    void PropertyGrid::SetValidationFailureBehavior(int vfbFlags) {}
    void PropertyGrid::SortChildren(void* id, int flags) {}
    /*static*/ void* PropertyGrid::GetEditorByName(const string& editorName) { return nullptr; }
    void PropertyGrid::SetPropertyReadOnly(void* id, bool set, int flags) {}
    void PropertyGrid::SetPropertyValueUnspecified(void* id) {}
    void* PropertyGrid::AppendIn(void* id, void* newproperty) { return nullptr; }
    void PropertyGrid::BeginAddChildren(void* id) {}
    bool PropertyGrid::Collapse(void* id) { return false; }
    void PropertyGrid::DeleteProperty(void* id) {}
    void* PropertyGrid::RemoveProperty(void* id) { return nullptr; }
    bool PropertyGrid::DisableProperty(void* id) { return false; }
    bool PropertyGrid::EnableProperty(void* id, bool enable) { return false; }
    void PropertyGrid::EndAddChildren(void* id) {}
    bool PropertyGrid::Expand(void* id) { return false; }
    void* PropertyGrid::GetFirstChild(void* id) { return nullptr; }
    void* PropertyGrid::GetPropertyCategory(void* id) { return nullptr; }
    void* PropertyGrid::GetPropertyClientData(void* id) { return nullptr; }
    string PropertyGrid::GetPropertyHelpString(void* id) { return wxStr(wxEmptyString); }
    void* PropertyGrid::GetPropertyImage(void* id) { return nullptr; }
    string PropertyGrid::GetPropertyLabel(void* id) { return wxStr(wxEmptyString); }
    void* PropertyGrid::GetPropertyParent(void* id) { return nullptr; }
    string PropertyGrid::GetPropertyValueAsString(void* id) { return wxStr(wxEmptyString); }
    int64_t PropertyGrid::GetPropertyValueAsLong(void* id) { return 0; }
    uint64_t PropertyGrid::GetPropertyValueAsULong(void* id) { return 0; }
    int PropertyGrid::GetPropertyValueAsInt(void* id) { return 0; }
    bool PropertyGrid::GetPropertyValueAsBool(void* id) { return false; }
    double PropertyGrid::GetPropertyValueAsDouble(void* id) { return 0; }
    DateTime PropertyGrid::GetPropertyValueAsDateTime(void* id) { return DateTime(); }
    bool PropertyGrid::HideProperty(void* id, bool hide, int flags) { return false; }
    void* PropertyGrid::Insert(void* priorThis, void* newproperty) { return nullptr; }
    void* PropertyGrid::InsertByIndex(void* parent, int index, void* newproperty) { return nullptr; }
    bool PropertyGrid::IsPropertyCategory(void* id) { return false; }
    bool PropertyGrid::IsPropertyEnabled(void* id) { return false; }
    bool PropertyGrid::IsPropertyExpanded(void* id) { return false; }
    bool PropertyGrid::IsPropertyModified(void* id) { return false; }
    bool PropertyGrid::IsPropertySelected(void* id) { return false; }
    bool PropertyGrid::IsPropertyShown(void* id) { return false; }
    bool PropertyGrid::IsPropertyValueUnspecified(void* id) { return false; }
    void PropertyGrid::LimitPropertyEditing(void* id, bool limit) {}
    void* PropertyGrid::ReplaceProperty(void* id, void* property) { return nullptr; }
    void PropertyGrid::SetPropertyBackgroundColor(void* id, const Color& color, int flags) {}
    void PropertyGrid::SetPropertyColorsToDefault(void* id, int flags) {}
    void PropertyGrid::SetPropertyTextColor(void* id, const Color& col, int flags) {}
    Color PropertyGrid::GetPropertyBackgroundColor(void* id) { return Color(); }
    Color PropertyGrid::GetPropertyTextColor(void* id) { return Color(); }
    void PropertyGrid::SetPropertyClientData(void* id, void* clientData) {}
    void PropertyGrid::SetPropertyEditor(void* id, void* editor) {}
    void PropertyGrid::SetPropertyEditorByName(void* id, const string& editorName) {}
}
