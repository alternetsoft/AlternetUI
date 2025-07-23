#include "PropertyGrid.h"

namespace Alternet::UI
{
	class wxPropertyGrid2 : public wxPropertyGrid, public wxWidgetExtender
	{
	public:
		wxPropertyGrid2(){}
		wxPropertyGrid2(wxWindow* parent, wxWindowID id = wxID_ANY,
			const wxPoint& pos = wxDefaultPosition,
			const wxSize& size = wxDefaultSize,
			long style = wxPG_DEFAULT_STYLE,
			const wxString& name = wxASCII_STR(wxPropertyGridNameStr))
			: wxPropertyGrid(parent, id, pos, size, style, name)
		{
		}
	};

	wxWindow* PropertyGrid::CreateWxWindowUnparented()
	{
		return new wxPropertyGrid2();
	}

	void PropertyGrid::KnownColorsClear()
	{
		if (wxAlternetColourProperty::KnownColorLabels.capacity() == 0)
		{
#define numColors 200
			wxAlternetColourProperty::KnownColorLabels.Alloc(numColors);
			wxAlternetColourProperty::KnownColorValues.Alloc(numColors);
			wxAlternetColourProperty::KnownColorColors.Alloc(numColors);
		}
		else
		{
			wxAlternetColourProperty::KnownColorLabels.Clear();
			wxAlternetColourProperty::KnownColorValues.Clear();
			wxAlternetColourProperty::KnownColorColors.Clear();
		}
		wxAlternetColourProperty::gs_wxColourProperty_choicesCache.Clear();
	}

	void PropertyGrid::KnownColorsAdd(const string& name, const string& title,
		const Color& value, int knownColor)
	{
		auto wxtitle = wxStr(title);
		auto index = wxAlternetColourProperty::KnownColorLabels.Add(wxtitle);
		wxAlternetColourProperty::KnownColorValues.Add(index);
		wxAlternetColourProperty::KnownColorColors.Add(wxPG_COLOUR(value.R, value.G, value.B));
		wxAlternetColourProperty::gs_wxColourProperty_choicesCache.Add(wxtitle, index);
	}

	wxString PropertyGrid::CustomColorTitle = "Custom";

	void PropertyGrid::KnownColorsSetCustomColorTitle(const string& value)
	{
		CustomColorTitle = wxStr(value);
	}

	void PropertyGrid::KnownColorsApply()
	{
		wxAlternetColourProperty::KnownColorLabels.Add(CustomColorTitle);
		wxAlternetColourProperty::KnownColorValues.Add(wxPG_COLOUR_CUSTOM);
		wxAlternetColourProperty::KnownColorColors.Add(wxPG_COLOUR(0, 0, 0));
		wxAlternetColourProperty::gs_wxColourProperty_choicesCache.Add(CustomColorTitle,
			wxPG_COLOUR_CUSTOM);
	}

	PropertyGrid::PropertyGrid()
	{
		bindScrollEvents = false;
	}

	PropertyGrid::PropertyGrid(long styles) : PropertyGrid()
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

	int64_t PropertyGrid::GetCreateStyleEx()
	{
		return GetPropGrid()->GetExtraStyle();
	}

	void PropertyGrid::SetCreateStyleEx(int64_t value)
	{
		GetPropGrid()->SetExtraStyle(value);
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

		result->Bind(wxEVT_PG_SELECTED, &PropertyGrid::OnSelected, this);
		result->Bind(wxEVT_PG_CHANGED, &PropertyGrid::OnChanged, this);
		result->Bind(wxEVT_PG_CHANGING, &PropertyGrid::OnChanging, this);
		result->Bind(wxEVT_PG_HIGHLIGHTED, &PropertyGrid::OnHighlighted, this);
		result->Bind(wxEVT_PG_RIGHT_CLICK, &PropertyGrid::OnRightClick, this);
		result->Bind(wxEVT_PG_DOUBLE_CLICK, &PropertyGrid::OnDoubleClick, this);
		result->Bind(wxEVT_PG_ITEM_COLLAPSED, &PropertyGrid::OnItemCollapsed, this);
		result->Bind(wxEVT_PG_ITEM_EXPANDED, &PropertyGrid::OnItemExpanded, this);
		result->Bind(wxEVT_PG_LABEL_EDIT_BEGIN, &PropertyGrid::OnLabelEditBegin, this);
		result->Bind(wxEVT_PG_LABEL_EDIT_ENDING, &PropertyGrid::OnLabelEditEnding, this);
		result->Bind(wxEVT_PG_COL_BEGIN_DRAG, &PropertyGrid::OnColBeginDrag, this);
		result->Bind(wxEVT_PG_COL_DRAGGING, &PropertyGrid::OnColDragging, this);
		result->Bind(wxEVT_PG_COL_END_DRAG, &PropertyGrid::OnColEndDrag, this);
		result->Bind(wxEVT_BUTTON, &PropertyGrid::OnButton, this);

		return result;
	}

	void PropertyGrid::OnButton(wxCommandEvent& event)
	{
		auto id = event.GetId();
		_eventColumn = 1;

		auto prop = GetPropGrid()->GetSelection();
		_eventProperty = prop;

		if (prop == nullptr)
		{
			_eventPropertyName = wxStr(wxEmptyString);
			_eventValue->variant = wxVariant();
		}
		else
		{
			_eventPropertyName = wxStr(prop->GetName());
			_eventValue->variant = prop->GetValue();
		}

		RaiseEvent(PropertyGridEvent::ButtonClick);
		event.Skip();
	}

	void PropertyGrid::OnSelected(wxPropertyGridEvent& event)
	{
		FromEventData(PropertyGridEvent::Selected, event);
		RaiseEvent(PropertyGridEvent::Selected);
		ToEventData(PropertyGridEvent::Selected, event);
		event.Skip();
	}

	void PropertyGrid::OnChanged(wxPropertyGridEvent& event)
	{
		FromEventData(PropertyGridEvent::Changed, event);
		RaiseEvent(PropertyGridEvent::Changed);
		ToEventData(PropertyGridEvent::Changed, event);
		event.Skip();
	}

	void PropertyGrid::OnChanging(wxPropertyGridEvent& event)
	{
		FromEventData(PropertyGridEvent::Changing, event);
		bool c = RaiseEvent(PropertyGridEvent::Changing);
		ToEventData(PropertyGridEvent::Changing, event);
		if (c)
			event.Veto();
		else
			event.Skip();
	}

	void PropertyGrid::OnHighlighted(wxPropertyGridEvent& event)
	{
		FromEventData(PropertyGridEvent::Highlighted, event);
		RaiseEvent(PropertyGridEvent::Highlighted);
		ToEventData(PropertyGridEvent::Highlighted, event);
		event.Skip();
	}

	void PropertyGrid::OnRightClick(wxPropertyGridEvent& event)
	{
		FromEventData(PropertyGridEvent::RightClick, event);
		RaiseEvent(PropertyGridEvent::RightClick);
		ToEventData(PropertyGridEvent::RightClick, event);
		event.Skip();
	}

	void PropertyGrid::OnDoubleClick(wxPropertyGridEvent& event)
	{
		FromEventData(PropertyGridEvent::DoubleClick, event);
		RaiseEvent(PropertyGridEvent::DoubleClick);
		ToEventData(PropertyGridEvent::DoubleClick, event);
		event.Skip();
	}

	void PropertyGrid::OnItemCollapsed(wxPropertyGridEvent& event)
	{
		FromEventData(PropertyGridEvent::ItemCollapsed, event);
		RaiseEvent(PropertyGridEvent::ItemCollapsed);
		ToEventData(PropertyGridEvent::ItemCollapsed, event);
		event.Skip();
	}

	void PropertyGrid::OnItemExpanded(wxPropertyGridEvent& event)
	{
		FromEventData(PropertyGridEvent::ItemExpanded, event);
		RaiseEvent(PropertyGridEvent::ItemExpanded);
		ToEventData(PropertyGridEvent::ItemExpanded, event);
		event.Skip();
	}

	void PropertyGrid::OnLabelEditBegin(wxPropertyGridEvent& event)
	{
		FromEventData(PropertyGridEvent::LabelEditBegin, event);
		bool c = RaiseEvent(PropertyGridEvent::LabelEditBegin);
		ToEventData(PropertyGridEvent::LabelEditBegin, event);
		if (c)
			event.Veto();
		else
			event.Skip();
	}

	void PropertyGrid::OnLabelEditEnding(wxPropertyGridEvent& event)
	{
		FromEventData(PropertyGridEvent::LabelEditEnding, event);
		bool c = RaiseEvent(PropertyGridEvent::LabelEditEnding);
		ToEventData(PropertyGridEvent::LabelEditEnding, event);
		if (c)
			event.Veto();
		else
			event.Skip();
	}

	void PropertyGrid::OnColBeginDrag(wxPropertyGridEvent& event)
	{
		FromEventData(PropertyGridEvent::ColBeginDrag, event);
		bool c = RaiseEvent(PropertyGridEvent::ColBeginDrag);
		ToEventData(PropertyGridEvent::ColBeginDrag, event);
		if (c)
			event.Veto();
		else
			event.Skip();
	}

	void PropertyGrid::OnColDragging(wxPropertyGridEvent& event)
	{
		FromEventData(PropertyGridEvent::ColDragging, event);
		RaiseEvent(PropertyGridEvent::ColDragging);
		ToEventData(PropertyGridEvent::ColDragging, event);
		event.Skip();
	}

	void PropertyGrid::OnColEndDrag(wxPropertyGridEvent& event)
	{
		FromEventData(PropertyGridEvent::ColEndDrag, event);
		RaiseEvent(PropertyGridEvent::ColEndDrag);
		ToEventData(PropertyGridEvent::ColEndDrag, event);
		event.Skip();
	}

	void PropertyGrid::ToEventData(PropertyGridEvent evType, wxPropertyGridEvent& event)
	{
		if (evType == PropertyGridEvent::Changing)
		{
			event.SetValidationFailureBehavior(_eventValidationFailureBehavior);
			event.SetValidationFailureMessage(wxStr(_eventValidationFailureMessage));
		}
	}

	void* PropertyGrid::GetEventPropValue()
	{
		return _eventValue;
	}

	void PropertyGrid::FromEventData(PropertyGridEvent evType, wxPropertyGridEvent& event)
	{
		if (evType == PropertyGridEvent::Changing)
		{
			_eventValidationFailureBehavior = event.GetValidationFailureBehavior();
			_eventValidationFailureMessage = wxStr(event.GetValidationInfo().GetFailureMessage());
		}
		_eventColumn = event.GetColumn();
		_eventProperty = event.GetProperty();
		_eventPropertyName = wxStr(event.GetPropertyName());
		_eventValue->variant = event.GetPropertyValue();
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
		PropertyGridVariant::Delete(_eventValue);
		if (IsWxWindowCreated())
		{
			auto window = GetWxWindow();
			if (window != nullptr)
			{
				window->Unbind(wxEVT_BUTTON, &PropertyGrid::OnButton, this);
				window->Unbind(wxEVT_PG_SELECTED, &PropertyGrid::OnSelected, this);
				window->Unbind(wxEVT_PG_CHANGED, &PropertyGrid::OnChanged, this);
				window->Unbind(wxEVT_PG_CHANGING, &PropertyGrid::OnChanging, this);
				window->Unbind(wxEVT_PG_HIGHLIGHTED, &PropertyGrid::OnHighlighted, this);
				window->Unbind(wxEVT_PG_RIGHT_CLICK, &PropertyGrid::OnRightClick, this);
				window->Unbind(wxEVT_PG_DOUBLE_CLICK, &PropertyGrid::OnDoubleClick, this);
				window->Unbind(wxEVT_PG_ITEM_COLLAPSED, &PropertyGrid::OnItemCollapsed, this);
				window->Unbind(wxEVT_PG_ITEM_EXPANDED, &PropertyGrid::OnItemExpanded, this);
				window->Unbind(wxEVT_PG_LABEL_EDIT_BEGIN, &PropertyGrid::OnLabelEditBegin, this);
				window->Unbind(wxEVT_PG_LABEL_EDIT_ENDING, &PropertyGrid::OnLabelEditEnding, this);
				window->Unbind(wxEVT_PG_COL_BEGIN_DRAG, &PropertyGrid::OnColBeginDrag, this);
				window->Unbind(wxEVT_PG_COL_DRAGGING, &PropertyGrid::OnColDragging, this);
				window->Unbind(wxEVT_PG_COL_END_DRAG, &PropertyGrid::OnColEndDrag, this);
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

	class wxFloatProperty2 : public wxFloatProperty
	{
	public:
		wxFloatProperty2(const wxString& label = wxPG_LABEL,
			const wxString& name = wxPG_LABEL,
			double value = 0.0)
			:wxFloatProperty(label, name, value)
		{
		}

		virtual wxString ValueToString(wxVariant& value, int argFlags = 0) const wxOVERRIDE;
	};

	wxString wxFloatProperty2::ValueToString(wxVariant& value, int argFlags) const
	{
		auto dbl = value.GetDouble();
		if (wxIsNaN(dbl))
			return "nan";

		auto result = wxFloatProperty::ValueToString(value, (wxPGPropValFormatFlags)argFlags);
		return result;
	}

	void* PropertyGrid::CreateFloatProperty(const string& label, const string& name, double value)
	{
		return new wxFloatProperty2(wxStr(label), wxStr(name), value);
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

	void* PropertyGrid::CreateEditEnumProperty(const string& label, const string& name,
		void* choices, const string& value)
	{
		auto pgc = (PropertyGridChoices*)choices;

		return new wxEditEnumProperty(wxStr(label), wxStr(name), pgc->choices, wxStr(value));
	}

	void* PropertyGrid::CreateEnumProperty(const string& label, const string& name,
		void* choices, int value)
	{
		auto pgc = (PropertyGridChoices*)choices;

		return new wxEnumProperty(wxStr(label), wxStr(name), pgc->choices, value);
	}

	void* PropertyGrid::CreateFilenameProperty(const string& label, const string& name,
		const string& value)
	{
		return new wxFileProperty(wxStr(label), wxStr(name), wxStr(value));
	}

	void* PropertyGrid::CreateDirProperty(const string& label, const string& name,
		const string& value)
	{
		return new wxDirProperty(wxStr(label), wxStr(name), wxStr(value));
	}

	void* PropertyGrid::CreateImageFilenameProperty(const string& label, const string& name,
		const string& value)
	{
		return new wxImageFileProperty(wxStr(label), wxStr(name), wxStr(value));
	}

	void* PropertyGrid::CreateSystemColorProperty(const string& label, const string& name,
		const Color& value, uint32_t kind)
	{
		wxColor wxc = value;
		wxColourPropertyValue wxProp = wxColourPropertyValue(kind, wxc);
		return new wxAlternetSystemColourProperty(wxStr(label), wxStr(name), wxProp);
	}

	void* PropertyGrid::CreateCursorProperty(const string& label, const string& name, int value)
	{
		return new wxCursorProperty(wxStr(label), wxStr(name), value);
	}

	void* PropertyGrid::CreateColorProperty(const string& label, const string& name,
		const Color& value)
	{
		wxColor wxc = value;
		return new wxAlternetColourProperty(wxStr(label), wxStr(name), wxc);
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
		return GetPropGrid()->Sort((wxPGPropertyValuesFlags)flags);
	}

	void PropertyGrid::RefreshProperty(void* p)
	{
		return GetPropGrid()->RefreshProperty((wxPGProperty*)p);
	}

	void PropertyGrid::ToPropArg(void* id)
	{
		_propArg = wxPGPropArgCls((wxPGProperty*)id);
	}

	void PropertyGrid::SetPropertyLabel(void* id, const string& newproplabel)
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
		GetPropGrid()->SetValidationFailureBehavior((wxPGVFBFlags)vfbFlags);
	}

	void PropertyGrid::SortChildren(void* id, int flags)
	{
		ToPropArg(id);
		GetPropGrid()->SortChildren(_propArg, (wxPGPropertyValuesFlags)flags);
	}

	/*static*/ void* PropertyGrid::GetEditorByName(const string& editorName)
	{
		return wxPropertyGrid::GetEditorByName(wxStr(editorName));
	}

	void PropertyGrid::SetPropertyReadOnly(void* id, bool set, int flags)
	{
		ToPropArg(id);
		GetPropGrid()->SetPropertyReadOnly(_propArg, set, (wxPGPropertyValuesFlags)flags);
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

	void* PropertyGrid::GetPropertyValueAsVariant(void* id)
	{
		ToPropArg(id);
		auto variant = GetPropGrid()->GetPropertyValue(_propArg);

		auto result = new PropertyGridVariant(variant);
		return result;
	}

	string PropertyGrid::GetPropertyValueAsString(void* id)
	{
		ToPropArg(id);
		return wxStr(GetPropGrid()->GetPropertyValueAsString(_propArg));
	}

	int64_t PropertyGrid::GetPropertyValueAsLong(void* id)
	{
		ToPropArg(id);		
		return GetPropGrid()->GetPropertyValueAsLongLong(_propArg);
	}

	uint64_t PropertyGrid::GetPropertyValueAsULong(void* id)
	{
		ToPropArg(id);
		return GetPropGrid()->GetPropertyValueAsULongLong(_propArg);
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
		return GetPropGrid()->HideProperty(_propArg, hide, (wxPGPropertyValuesFlags)flags);
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
		GetPropGrid()->SetPropertyBackgroundColour(_propArg, color, (wxPGPropertyValuesFlags)flags);
	}

	void PropertyGrid::SetPropertyColorsToDefault(void* id, int flags)
	{
		ToPropArg(id);
		GetPropGrid()->SetPropertyColoursToDefault(_propArg, (wxPGPropertyValuesFlags)flags);
	}

	void PropertyGrid::SetPropertyTextColor(void* id, const Color& col, int flags)
	{
		ToPropArg(id);
		GetPropGrid()->SetPropertyTextColour(_propArg, col, (wxPGPropertyValuesFlags)flags);
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

	int PropertyGrid::GetEventValidationFailureBehavior()
	{
		return (int)_eventValidationFailureBehavior;
	}

	void PropertyGrid::SetEventValidationFailureBehavior(int value)
	{
		_eventValidationFailureBehavior = (wxPGVFBFlags)value;
	}

	int PropertyGrid::GetEventColumn()
	{
		return _eventColumn;
	}

	void* PropertyGrid::GetEventProperty()
	{
		return _eventProperty;
	}

	string PropertyGrid::GetEventPropertyName()
	{
		return _eventPropertyName;
	}

	string PropertyGrid::GetEventValidationFailureMessage()
	{
		return _eventValidationFailureMessage;
	}

	void PropertyGrid::SetEventValidationFailureMessage(const string& value)
	{
		_eventValidationFailureMessage = value;
	}

	string PropertyGrid::GetUnspecifiedValueText(int argFlags)
	{
		return wxStr(GetPropGrid()->GetUnspecifiedValueText((wxPGPropValFormatFlags)argFlags));
	}

	void PropertyGrid::SetVirtualWidth(int width)
	{
		GetPropGrid()->SetVirtualWidth(width);
	}

	void PropertyGrid::SetSplitterLeft(bool privateChildrenToo)
	{
		GetPropGrid()->SetSplitterLeft(privateChildrenToo);
	}

	void PropertyGrid::SetVerticalSpacing(int vspacing)
	{
		GetPropGrid()->SetVerticalSpacing(vspacing);
	}

	bool PropertyGrid::HasVirtualWidth()
	{
		return GetPropGrid()->HasVirtualWidth();
	}

	uint32_t PropertyGrid::GetCommonValueCount()
	{
		return GetPropGrid()->GetCommonValueCount();
	}

	string PropertyGrid::GetCommonValueLabel(uint32_t i)
	{
		return wxStr(GetPropGrid()->GetCommonValueLabel(i));
	}

	int PropertyGrid::GetUnspecifiedCommonValue()
	{
		return GetPropGrid()->GetUnspecifiedCommonValue();
	}

	void PropertyGrid::SetUnspecifiedCommonValue(int index)
	{
		GetPropGrid()->SetUnspecifiedCommonValue(index);
	}

	/*static*/ bool PropertyGrid::IsSmallScreen()
	{
		return wxPropertyGrid::IsSmallScreen();
	}

	void PropertyGrid::RefreshEditor()
	{
		GetPropGrid()->RefreshEditor();
	}

	bool PropertyGrid::WasValueChangedInEvent()
	{
		return GetPropGrid()->WasValueChangedInEvent();
	}

	int PropertyGrid::GetSpacingY()
	{
		return GetPropGrid()->GetSpacingY();
	}

	void PropertyGrid::SetupTextCtrlValue(const string& text)
	{
		GetPropGrid()->SetupTextCtrlValue(wxStr(text));
	}

	bool PropertyGrid::UnfocusEditor()
	{
		return GetPropGrid()->UnfocusEditor();
	}

	void* PropertyGrid::GetLastItem(int flags)
	{
		return GetPropGrid()->GetLastItem(flags);
	}

	void* PropertyGrid::GetRoot()
	{
		return GetPropGrid()->GetRoot();
	}

	void* PropertyGrid::GetSelectedProperty()
	{
		return GetPropGrid()->GetSelectedProperty();
	}

	bool PropertyGrid::EnsureVisible(void* propArg)
	{
		ToPropArg(propArg);
		return GetPropGrid()->EnsureVisible(_propArg);
	}

	bool PropertyGrid::SelectProperty(void* propArg, bool focus)
	{
		ToPropArg(propArg);
		return GetPropGrid()->SelectProperty(_propArg, focus);
	}

	bool PropertyGrid::AddToSelection(void* propArg)
	{
		ToPropArg(propArg);
		return GetPropGrid()->AddToSelection(_propArg);
	}

	bool PropertyGrid::RemoveFromSelection(void* propArg)
	{
		ToPropArg(propArg);
		return GetPropGrid()->RemoveFromSelection(_propArg);
	}

	void PropertyGrid::SetCurrentCategory(void* propArg)
	{
		ToPropArg(propArg);
		return GetPropGrid()->SetCurrentCategory(_propArg);
	}

	Int32Rect PropertyGrid::GetImageRect(void* p, int item)
	{
		return GetPropGrid()->GetImageRect((wxPGProperty*)p, item);
	}

	Int32Size PropertyGrid::GetImageSize(void* p, int item)
	{
		return GetPropGrid()->GetImageSize((wxPGProperty*)p, item);
	}

	void PropertyGrid::AddActionTrigger(int action, int keycode, int modifiers)
	{
		GetPropGrid()->AddActionTrigger(
			(wxPGKeyboardAction)action, Keyboard::KeyToWxKey((Key)keycode), modifiers);
	}

	void PropertyGrid::DedicateKey(int keycode)
	{
		GetPropGrid()->DedicateKey(Keyboard::KeyToWxKey((Key)keycode));
	}

	/*static*/ void PropertyGrid::AutoGetTranslation(bool enable)
	{
		wxPropertyGrid::AutoGetTranslation(enable);
	}

	void PropertyGrid::CenterSplitter(bool enableAutoResizing)
	{
		GetPropGrid()->CenterSplitter(enableAutoResizing);
	}

	void PropertyGrid::ClearActionTriggers(int action)
	{
		GetPropGrid()->ClearActionTriggers((wxPGKeyboardAction)action);
	}

	bool PropertyGrid::CommitChangesFromEditor(uint32_t flags)
	{
		return GetPropGrid()->CommitChangesFromEditor((wxPGSelectPropertyFlags)flags);
	}

	void PropertyGrid::EditorsValueWasModified()
	{
		GetPropGrid()->EditorsValueWasModified();
	}

	void PropertyGrid::EditorsValueWasNotModified()
	{
		GetPropGrid()->EditorsValueWasNotModified();
	}

	bool PropertyGrid::EnableCategories(bool enable)
	{
		return GetPropGrid()->EnableCategories(enable);
	}

	Size PropertyGrid::FitColumns()
	{
		return GetPropGrid()->FitColumns();
	}

	Color PropertyGrid::GetCaptionBackgroundColor()
	{
		return GetPropGrid()->GetCaptionBackgroundColour();
	}

	Color PropertyGrid::GetCaptionForegroundColor()
	{
		return GetPropGrid()->GetCaptionForegroundColour();
	}

	Color PropertyGrid::GetCellBackgroundColor()
	{
		return GetPropGrid()->GetCellBackgroundColour();
	}

	Color PropertyGrid::GetCellDisabledTextColor()
	{
		return GetPropGrid()->GetCellDisabledTextColour();
	}

	Color PropertyGrid::GetCellTextColor()
	{
		return GetPropGrid()->GetCellTextColour();
	}

	uint32_t PropertyGrid::GetColumnCount()
	{
		return GetPropGrid()->GetColumnCount();
	}

	Color PropertyGrid::GetEmptySpaceColor()
	{
		return GetPropGrid()->GetEmptySpaceColour();
	}

	int PropertyGrid::GetFontHeight()
	{
		return GetPropGrid()->GetFontHeight();
	}

	Color PropertyGrid::GetLineColor()
	{
		return GetPropGrid()->GetLineColour();
	}

	Color PropertyGrid::GetMarginColor()
	{
		return GetPropGrid()->GetMarginColour();
	}

	int PropertyGrid::GetMarginWidth()
	{
		return GetPropGrid()->GetMarginWidth();
	}

	int PropertyGrid::GetRowHeight()
	{
		return GetPropGrid()->GetRowHeight();
	}

	Color PropertyGrid::GetSelectionBackgroundColor()
	{
		return GetPropGrid()->GetSelectionBackgroundColour();
	}

	Color PropertyGrid::GetSelectionForegroundColor()
	{
		return GetPropGrid()->GetSelectionForegroundColour();
	}

	int PropertyGrid::GetSplitterPosition(uint32_t splitterIndex)
	{
		return GetPropGrid()->GetSplitterPosition(splitterIndex);
	}

	int PropertyGrid::GetVerticalSpacing()
	{
		return GetPropGrid()->GetVerticalSpacing();
	}

	bool PropertyGrid::IsEditorFocused()
	{
		return GetPropGrid()->IsEditorFocused();
	}

	bool PropertyGrid::IsEditorsValueModified()
	{
		return GetPropGrid()->IsEditorsValueModified();
	}

	bool PropertyGrid::IsAnyModified()
	{
		return GetPropGrid()->IsAnyModified();
	}

	void PropertyGrid::ResetColors()
	{
		GetPropGrid()->ResetColours();
	}

	void PropertyGrid::ResetColumnSizes(bool enableAutoResizing)
	{
		GetPropGrid()->ResetColumnSizes(enableAutoResizing);
	}

	void PropertyGrid::MakeColumnEditable(uint32_t column, bool editable)
	{
		GetPropGrid()->MakeColumnEditable(column, editable);
	}

	void PropertyGrid::BeginLabelEdit(uint32_t column)
	{
		GetPropGrid()->BeginLabelEdit(column);
	}

	void PropertyGrid::EndLabelEdit(bool commit)
	{
		GetPropGrid()->EndLabelEdit(commit);
	}

	void PropertyGrid::SetCaptionBackgroundColor(const Color& col)
	{
		GetPropGrid()->SetCaptionBackgroundColour(col);
	}

	void PropertyGrid::SetCaptionTextColor(const Color& col)
	{
		GetPropGrid()->SetCaptionTextColour(col);
	}

	void PropertyGrid::SetCellBackgroundColor(const Color& col)
	{
		GetPropGrid()->SetCellBackgroundColour(col);
	}

	void PropertyGrid::SetCellDisabledTextColor(const Color& col)
	{
		GetPropGrid()->SetCellDisabledTextColour(col);
	}

	void PropertyGrid::SetCellTextColor(const Color& col)
	{
		GetPropGrid()->SetCellTextColour(col);
	}

	void PropertyGrid::SetColumnCount(int colCount)
	{
		GetPropGrid()->SetColumnCount(colCount);
	}

	void PropertyGrid::SetEmptySpaceColor(const Color& col)
	{
		GetPropGrid()->SetEmptySpaceColour(col);
	}

	void PropertyGrid::SetLineColor(const Color& col)
	{
		GetPropGrid()->SetLineColour(col);
	}

	void PropertyGrid::SetMarginColor(const Color& col)
	{
		GetPropGrid()->SetMarginColour(col);
	}

	void PropertyGrid::SetSelectionBackgroundColor(const Color& col)
	{
		GetPropGrid()->SetSelectionBackgroundColour(col);
	}

	void PropertyGrid::SetSelectionTextColor(const Color& col)
	{
		GetPropGrid()->SetSelectionTextColour(col);
	}

	void PropertyGrid::SetSplitterPosition(int newXPos, int col)
	{
		GetPropGrid()->SetSplitterPosition(newXPos, col);
	}

	bool PropertyGrid::ChangePropertyValue(void* id, void* variant)
	{
		ToPropArg(id);
		wxVariant v = PropertyGridVariant::ToVar(variant);
		return GetPropGrid()->ChangePropertyValue(_propArg, v);
	}

	void PropertyGrid::SetPropertyImage(void* id, ImageSet* bmp)
	{
		ToPropArg(id);
		GetPropGrid()->SetPropertyImage(_propArg, ImageSet::BitmapBundle(bmp));
	}

	void PropertyGrid::SetPropertyAttribute(void* id, const string& attrName, 
		void* variant, int64_t argFlags)
	{
		ToPropArg(id);
		wxVariant v = PropertyGridVariant::ToVar(variant);
		GetPropGrid()->SetPropertyAttribute(
			_propArg, wxStr(attrName), v, (wxPGPropertyValuesFlags)argFlags);
	}

	void PropertyGrid::SetPropertyValueAsVariant(void* id, void* variant)
	{
		ToPropArg(id);
		wxVariant v = PropertyGridVariant::ToVar(variant);
		GetPropGrid()->SetPropertyValue(_propArg, v);
	}

	void PropertyGrid::SetPropertyAttributeAll(const string& attrName, void* variant)
	{
		wxVariant v = PropertyGridVariant::ToVar(variant);
		GetPropGrid()->SetPropertyAttributeAll(wxStr(attrName), v);
	}

	void* PropertyGrid::GetPropertyValidator(void* prop)
	{
		ToPropArg(prop);
		auto validator = GetPropGrid()->GetPropertyValidator(_propArg);
		return validator;
	}

	void PropertyGrid::SetPropertyFlag(void* prop, int flag, bool value)
	{
		wxPGProperty* pg = (wxPGProperty*)prop;
		
		pg->ChangeFlag((wxPGFlags)flag, value);
	}

	void PropertyGrid::SetPropertyValidator(void* prop, void* validator)
	{
		ToPropArg(prop);
		if(validator == nullptr)
			GetPropGrid()->SetPropertyValidator(_propArg, wxDefaultValidator);
		else
			GetPropGrid()->SetPropertyValidator(_propArg, *(wxValidator*)validator);
	}

	void* PropertyGrid::ColorDatabaseCreate()
	{
		return new wxColourDatabase();
	}

	void PropertyGrid::ColorDatabaseDelete(void* handle)
	{
		delete (wxColourDatabase*)handle;
	}

	void PropertyGrid::ColorDatabaseSetGlobal(void* handle)
	{
		wxTheColourDatabase = (wxColourDatabase*)handle;
	}

	void PropertyGrid::ColorDatabaseAdd(void* handle, const string& name, const Color& color)
	{
		((wxColourDatabase*)handle)->AddColour(wxStr(name), color);
	}

	Color PropertyGrid::ColorDatabaseFind(void* handle, const string& name)
	{
		return ((wxColourDatabase*)handle)->Find(wxStr(name));
	}

	string PropertyGrid::ColorDatabaseFindName(void* handle, const Color& color)
	{
		return wxStr(((wxColourDatabase*)handle)->FindName(color));
	}

	PointI PropertyGrid::CalcUnscrolledPosition(const PointI& point)
	{
		auto result = GetPropGrid()->CalcUnscrolledPosition(point);
		return result;
	}

	PointI PropertyGrid::CalcScrolledPosition(const PointI& point)
	{
		auto result = GetPropGrid()->CalcScrolledPosition(point);
		return result;
	}

	int PropertyGrid::GetHitTestColumn(const PointI& point)
	{
		auto result = GetPropGrid()->HitTest(point);
		return result.GetColumn();
	}

	void* PropertyGrid::GetHitTestProp(const PointI& point)
	{
		auto result = GetPropGrid()->HitTest(point);
		auto prop = result.GetProperty();
		return prop;
	}
}