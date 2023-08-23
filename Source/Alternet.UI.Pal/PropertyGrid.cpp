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

    wxPropertyGrid* PropertyGrid::GetPropGrid()
    {
        return dynamic_cast<wxPropertyGrid2*>(GetWxWindow());
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

    void* PropertyGrid::CreateFlagsProperty(const string& label, const string& name,
        void* choices, int value)
    {
        auto pgc = (PropertyGridChoices*)choices;

        return new wxFlagsProperty(wxStr(label), wxStr(name), pgc->choices, value);
    }

}
