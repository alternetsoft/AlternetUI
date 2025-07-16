#include "TreeView.h"
#include "Application.h"

#include <wx/generic/treectlg.h>

namespace Alternet::UI
{
	void TreeView::SetItemBold(void* handle, void* item, bool bold)
	{
		wxTreeItemId itemId(item);

		((wxTreeCtrl*)handle)->SetItemBold(itemId, bold);
	}

	Color TreeView::GetItemTextColor(void* handle, void* item)
	{
		wxTreeItemId itemId(item);
		return ((wxTreeCtrl*)handle)->GetItemTextColour(itemId);
	}

	Color TreeView::GetItemBackgroundColor(void* handle, void* item)
	{
		wxTreeItemId itemId(item);
		return ((wxTreeCtrl*)handle)->GetItemBackgroundColour(itemId);
	}

	void TreeView::SetItemTextColor(void* handle, void* item, const Color& color)
	{
		wxTreeItemId itemId(item);
		((wxTreeCtrl*)handle)->SetItemTextColour(itemId, color);
	}

	void TreeView::SetItemBackgroundColor(void* handle, void* item, const Color& color)
	{
		wxTreeItemId itemId(item);
		((wxTreeCtrl*)handle)->SetItemBackgroundColour(itemId, color);
	}

	void TreeView::ResetItemTextColor(void* handle, void* item)
	{
		wxTreeItemId itemId(item);
		((wxTreeCtrl*)handle)->SetItemTextColour(itemId, wxColour());
	}

	void TreeView::ResetItemBackgroundColor(void* handle, void* item)
	{
		wxTreeItemId itemId(item);
		((wxTreeCtrl*)handle)->SetItemBackgroundColour(itemId, wxColour());
	}

	class TreeViewItemData : public wxTreeItemData
	{
	public:
		TreeViewItemData(int64_t id) : uniqueId(id) {}

		int64_t uniqueId;
	};

	void TreeView::SetNodeUniqueId(void* node, int64_t uniqueId)
	{
		auto tree = GetTreeCtrl();
		wxTreeItemId itemId(node);
		auto itemData = tree->GetItemData(itemId);

		if (itemData == nullptr)
		{
			auto newData = new TreeViewItemData(uniqueId);
			newData->SetId(itemId);
			tree->SetItemData(itemId, newData);
		}
		else
		{
			auto data = static_cast<TreeViewItemData*>(itemData);
			data->uniqueId = uniqueId;
		}
	}

	int64_t TreeView::GetNodeUniqueId(void* node)
	{
		wxTreeItemId itemId(node);
		auto itemData = GetTreeCtrl()->GetItemData(itemId);

		if (itemData == nullptr)
			return -1;
		else
		{
			auto data = static_cast<TreeViewItemData*>(itemData);
			return data->uniqueId;
		}
	}

	TreeView::TreeView()
	{
		bindScrollEvents = false;
		CreateWxWindow();
	}

	bool TreeView::GetHasBorder()
	{
		return hasBorder;
	}

	void TreeView::SetHasBorder(bool value)
	{
		if (hasBorder == value)
			return;
		hasBorder = value;
		RecreateWxWindowIfNeeded();
	}

	TreeView::~TreeView()
	{
		if (IsWxWindowCreated())
		{
			auto window = GetTreeCtrl();
			if (window != nullptr)
			{
				window->Unbind(wxEVT_TREE_SEL_CHANGED, &TreeView::OnSelectionChanged, this);
				window->Unbind(wxEVT_TREE_ITEM_EXPANDED, &TreeView::OnItemExpanded, this);
				window->Unbind(wxEVT_TREE_ITEM_COLLAPSED, &TreeView::OnItemCollapsed, this);
				window->Unbind(wxEVT_TREE_ITEM_COLLAPSING, &TreeView::OnItemCollapsing, this);
				window->Unbind(wxEVT_TREE_ITEM_EXPANDING, &TreeView::OnItemExpanding, this);
				window->Unbind(wxEVT_TREE_BEGIN_LABEL_EDIT, &TreeView::OnItemBeginLabelEdit, this);
				window->Unbind(wxEVT_TREE_END_LABEL_EDIT, &TreeView::OnItemEndLabelEdit, this);
			}
		}

		if (_imageList != nullptr)
			_imageList->Release();
	}

	ImageList* TreeView::GetImageList()
	{
		if (_imageList != nullptr)
			_imageList->AddRef();
		return _imageList;
	}

	void TreeView::SetImageList(ImageList* value)
	{
		if (_imageList != nullptr)
			_imageList->Release();
		_imageList = value;
		if (_imageList != nullptr)
			_imageList->AddRef();
		if (IsWxWindowCreated())
			ApplyImageList(GetTreeCtrl());
	}

	void* TreeView::GetRootItem()
	{
		auto item = GetTreeCtrl()->GetRootItem();
		return item;
	}

	TreeViewSelectionMode TreeView::GetSelectionMode()
	{
		return _selectionMode;
	}

	wxTreeCtrlBase* TreeView::GetTreeCtrl()
	{
		return dynamic_cast<wxTreeCtrlBase*>(GetWxWindow());
	}

	void TreeView::SetSelectionMode(TreeViewSelectionMode value)
	{
		if (_selectionMode == value)
			return;

		_selectionMode = value;
		_skipSelectionChangedEvent = true;
		_skipExpandedEvent = true;
		RecreateWxWindowIfNeeded();
		_skipSelectionChangedEvent = false;
		_skipExpandedEvent = false;
	}

	void* TreeView::OpenSelectedItemsArray()
	{
		auto array = new wxArrayTreeItemIds();
		GetTreeCtrl()->GetSelections(*array);

		return array;
	}

	int TreeView::GetSelectedItemsItemCount(void* array)
	{
		return ((wxArrayTreeItemIds*)array)->GetCount();
	}

	void* TreeView::GetSelectedItemsItemAt(void* array, int index)
	{
		return (*((wxArrayTreeItemIds*)array))[index];
	}

	void TreeView::CloseSelectedItemsArray(void* array)
	{
		delete ((wxArrayTreeItemIds*)array);
	}

	int TreeView::GetItemCount(void* parentItem)
	{
		wxTreeItemId parentItemId(parentItem);
		return ConvertToIntChecked(GetTreeCtrl()->GetChildrenCount(
			parentItemId, /*recursively:*/ false));
	}

	void* TreeView::InsertItem(void* parentItem, void* insertAfter,
		const string& text, int imageIndex, bool parentIsExpanded)
	{
		wxTreeItemId parentItemId(parentItem);
		wxTreeItemId insertAfterId(insertAfter);
		auto control = GetTreeCtrl();
		auto item = control->InsertItem(parentItem, insertAfterId,
			wxStr(text), imageIndex);

		if (parentItemId != control->GetRootItem())
		{
			if (parentIsExpanded)
				control->Expand(parentItemId);
			else
				control->Collapse(parentItemId);
		}

		return item;
	}

	void TreeView::RemoveItem(void* item)
	{
		wxTreeItemId itemId(item);
		auto control = GetTreeCtrl();
		GetTreeCtrl()->Delete(itemId);
	}

	void TreeView::ClearItems(void* parentItem)
	{
		wxTreeItemId parentItemId(parentItem);
		GetTreeCtrl()->DeleteChildren(parentItem);
	}

	void TreeView::ClearSelected()
	{
		GetTreeCtrl()->UnselectAll();
	}

	void TreeView::SetSelected(void* item, bool value)
	{
		wxTreeItemId itemId(item);
		GetTreeCtrl()->SelectItem(itemId, value);
	}

	void TreeView::ApplyImageList(wxTreeCtrlBase* value)
	{
		value->SetImageList(_imageList == nullptr ? nullptr :
			_imageList->GetImageList());
	}

	class wxGenericTreeCtrl2 : public wxGenericTreeCtrl, public wxWidgetExtender
	{
	public:
		wxGenericTreeCtrl2() {}
		wxGenericTreeCtrl2(wxWindow* parent, wxWindowID id = wxID_ANY,
			const wxPoint& pos = wxDefaultPosition,
			const wxSize& size = wxDefaultSize,
			long style = wxTR_HAS_BUTTONS | wxTR_LINES_AT_ROOT,
			const wxValidator& validator = wxDefaultValidator,
			const wxString& name = wxASCII_STR(wxTreeCtrlNameStr))
		{
			Create(parent, id, pos, size, style, validator, name);
		}
	protected:
	};

	class wxTreeCtrl2 : public wxTreeCtrl, public wxWidgetExtender
	{
	public:
		wxTreeCtrl2() {}
		wxTreeCtrl2(wxWindow* parent, wxWindowID id = wxID_ANY,
			const wxPoint& pos = wxDefaultPosition,
			const wxSize& size = wxDefaultSize,
			long style = wxTR_HAS_BUTTONS | wxTR_LINES_AT_ROOT,
			const wxValidator& validator = wxDefaultValidator,
			const wxString& name = wxASCII_STR(wxTreeCtrlNameStr))
		{
			Create(parent, id, pos, size, style, validator, name);
		}
	protected:
	};

	wxWindow* TreeView::CreateWxWindowUnparented()
	{
		return new wxTreeCtrl2();
	}

	int64_t TreeView::GetCreateStyle()
	{
		return 0;
	}

	void TreeView::SetCreateStyle(int64_t value)
	{

	}

	void TreeView::MakeAsListBox()
	{
		_fullRowSelect = true;
		_showRootLines = false;
		_showLines = false;
		_twistButtons = false;
		_rowLines = false;
		_showExpandButtons = false;
		_hideRoot = true;
		_variableRowHeight = false;
		RecreateWxWindowIfNeeded();
		SetStateImageSpacing(0);
		if (GetIndentation() > 3)
			SetIndentation(3);
	}

	wxWindow* TreeView::CreateWxWindowCore(wxWindow* parent)
	{
		long style = GetStyle();

		if (!hasBorder)
			style = style | wxBORDER_NONE;

		auto value = new wxTreeCtrl2(
			parent,
			wxID_ANY,
			wxDefaultPosition,
			wxDefaultSize,
			style);

		value->AddRoot("");

		ApplyImageList(value);

		value->Bind(wxEVT_TREE_SEL_CHANGED, &TreeView::OnSelectionChanged, this);
		value->Bind(wxEVT_TREE_ITEM_EXPANDED, &TreeView::OnItemExpanded, this);
		value->Bind(wxEVT_TREE_ITEM_COLLAPSED, &TreeView::OnItemCollapsed, this);
		value->Bind(wxEVT_TREE_BEGIN_LABEL_EDIT, &TreeView::OnItemBeginLabelEdit, this);
		value->Bind(wxEVT_TREE_END_LABEL_EDIT, &TreeView::OnItemEndLabelEdit, this);
		value->Bind(wxEVT_TREE_ITEM_COLLAPSING, &TreeView::OnItemCollapsing, this);
		value->Bind(wxEVT_TREE_ITEM_EXPANDING, &TreeView::OnItemExpanding, this);

		return value;
	}

	void TreeView::OnSelectionChanged(wxCommandEvent& event)
	{
		event.Skip();
		auto window = dynamic_cast<wxWindow*>(event.GetEventObject());
		if (window->IsBeingDeleted())
			return;

		if (!_skipSelectionChangedEvent)
			RaiseEvent(TreeViewEvent::SelectionChanged);
	}

	void TreeView::OnItemCollapsed(wxTreeEvent& event)
	{
		event.Skip();
		TreeViewItemEventData data{ event.GetItem() };
		RaiseEvent(TreeViewEvent::ItemCollapsed, &data);
	}

	void TreeView::OnItemExpanded(wxTreeEvent& event)
	{
		event.Skip();
		if (_skipExpandedEvent)
			return;

		TreeViewItemEventData data{ event.GetItem() };
		RaiseEvent(TreeViewEvent::ItemExpanded, &data);
	}

	void TreeView::OnItemCollapsing(wxTreeEvent& event)
	{
		TreeViewItemEventData data{ event.GetItem() };
		if (RaiseEventWithPointerResult(TreeViewEvent::ItemCollapsing, &data) != 0)
			event.Veto();
		else
			event.Skip();
	}

	void TreeView::OnItemExpanding(wxTreeEvent& event)
	{
		if (_skipExpandedEvent)
		{
			event.Skip();
			return;
		}

		TreeViewItemEventData data{ event.GetItem() };
		if (RaiseEventWithPointerResult(TreeViewEvent::ItemExpanding, &data) != 0)
			event.Veto();
		else
			event.Skip();
	}

	void TreeView::OnItemBeginLabelEdit(wxTreeEvent& event)
	{
		event.Skip();
		OnItemLabelEditEvent(event, TreeViewEvent::BeforeItemLabelEdit);
	}

	void TreeView::OnItemLabelEditEvent(wxTreeEvent& event, TreeViewEvent e)
	{
		TreeViewItemLabelEditEventData data{ 0 };

		data.editCancelled = event.IsEditCancelled();
		data.item = event.GetItem();
		auto label = wxStr(event.GetLabel());
		data.label = const_cast<char16_t*>(label.c_str());

		auto result = RaiseEventWithPointerResult(e, &data);

		if (result != 0)
			event.Veto();
		else
			event.Skip();
	}

	void TreeView::OnItemEndLabelEdit(wxTreeEvent& event)
	{
		event.Skip();
		auto window = GetWxWindow();
		window->Refresh();
		window->Update();

		OnItemLabelEditEvent(event, TreeViewEvent::AfterItemLabelEdit);
	}

	void TreeView::SetItemText(void* item, const string& text)
	{
		GetTreeCtrl()->SetItemText(item, wxStr(text));
	}

	string TreeView::GetItemText(void* item)
	{
		return wxStr(GetTreeCtrl()->GetItemText(item));
	}

	void TreeView::SetItemImageIndex(void* item, int imageIndex)
	{
		GetTreeCtrl()->SetItemImage(item, imageIndex);
	}

	int TreeView::GetItemImageIndex(void* item)
	{
		return GetTreeCtrl()->GetItemImage(item);
	}

	void TreeView::SetFocused(void* item, bool value)
	{
		auto treeCtrl = GetTreeCtrl();

		if (value)
			treeCtrl->SetFocusedItem(item);
		else
			treeCtrl->ClearFocusedItem();
	}

	bool TreeView::IsItemFocused(void* item)
	{
		return GetTreeCtrl()->GetFocusedItem() == item;
	}

	bool TreeView::GetShowLines()
	{
		return _showLines;
	}

	void TreeView::SetShowLines(bool value)
	{
		if (_showLines == value)
			return;

		_showLines = value;
		RecreateWxWindowIfNeeded();
	}

	bool TreeView::GetShowRootLines()
	{
		return _showRootLines;
	}

	void TreeView::SetShowRootLines(bool value)
	{
		if (_showRootLines == value)
			return;

		_showRootLines = value;
		RecreateWxWindowIfNeeded();
	}

	bool TreeView::GetShowExpandButtons()
	{
		return _showExpandButtons;
	}

	void TreeView::SetShowExpandButtons(bool value)
	{
		if (_showExpandButtons == value)
			return;

		_showExpandButtons = value;
		RecreateWxWindowIfNeeded();
	}

	void* TreeView::GetTopItem()
	{
		return GetTreeCtrl()->GetFirstVisibleItem();
	}

	bool TreeView::GetFullRowSelect()
	{
		return _fullRowSelect;
	}

	void TreeView::SetFullRowSelect(bool value)
	{
		if (_fullRowSelect == value)
			return;

		_fullRowSelect = value;
		RecreateWxWindowIfNeeded();
	}

	bool TreeView::GetAllowLabelEdit()
	{
		return _allowLabelEdit;
	}

	void TreeView::SetAllowLabelEdit(bool value)
	{
		if (_allowLabelEdit == value)
			return;

		_allowLabelEdit = value;
		RecreateWxWindowIfNeeded();
	}

	void TreeView::ExpandAll()
	{
		GetTreeCtrl()->ExpandAll();
	}

	void TreeView::CollapseAll()
	{
		GetTreeCtrl()->CollapseAll();
	}

	void* TreeView::ItemHitTest(const Point& point)
	{
		auto treeCtrl = GetTreeCtrl();
		int flags = 0;
		auto item = treeCtrl->HitTest(fromDip(point, treeCtrl), flags);

		auto result = new HitTestResult();
		result->item = item;
		result->locations = GetHitTestLocationsFromWxFlags(flags);
		return result;
	}

	/*static*/ TreeViewHitTestLocations TreeView::GetHitTestLocationsFromWxFlags(int flags)
	{
		TreeViewHitTestLocations result = (TreeViewHitTestLocations)0;

		if ((flags & wxTREE_HITTEST_ABOVE) != 0) result |= TreeViewHitTestLocations::AboveClientArea;
		if ((flags & wxTREE_HITTEST_BELOW) != 0) result |= TreeViewHitTestLocations::BelowClientArea;
		if ((flags & wxTREE_HITTEST_NOWHERE) != 0) result |= TreeViewHitTestLocations::None;
		if ((flags & wxTREE_HITTEST_ONITEMBUTTON) != 0) result |= TreeViewHitTestLocations::ItemExpandButton;
		if ((flags & wxTREE_HITTEST_ONITEMICON) != 0) result |= TreeViewHitTestLocations::ItemImage;
		if ((flags & wxTREE_HITTEST_ONITEMINDENT) != 0) result |= TreeViewHitTestLocations::ItemIndent;
		if ((flags & wxTREE_HITTEST_ONITEMLABEL) != 0) result |= TreeViewHitTestLocations::ItemLabel;
		if ((flags & wxTREE_HITTEST_ONITEMRIGHT) != 0) result |= TreeViewHitTestLocations::RightOfItemLabel;
		if ((flags & wxTREE_HITTEST_ONITEMSTATEICON) != 0) result |= TreeViewHitTestLocations::ItemStateImage;
		if ((flags & wxTREE_HITTEST_TOLEFT) != 0) result |= TreeViewHitTestLocations::LeftOfClientArea;
		if ((flags & wxTREE_HITTEST_TORIGHT) != 0) result |= TreeViewHitTestLocations::RightOfClientArea;
		if ((flags & wxTREE_HITTEST_ONITEMUPPERPART) != 0) result |= TreeViewHitTestLocations::ItemUpperPart;
		if ((flags & wxTREE_HITTEST_ONITEMLOWERPART) != 0) result |= TreeViewHitTestLocations::ItemLowerPart;

		if (result == (TreeViewHitTestLocations)0)
			result = TreeViewHitTestLocations::None;

		return result;
	}

	TreeViewHitTestLocations TreeView::GetHitTestResultLocations(void* hitTestResult)
	{
		return ((HitTestResult*)hitTestResult)->locations;
	}

	void* TreeView::GetHitTestResultItem(void* hitTestResult)
	{
		return ((HitTestResult*)hitTestResult)->item;
	}

	void TreeView::FreeHitTestResult(void* hitTestResult)
	{
		delete (HitTestResult*)hitTestResult;
	}

	bool TreeView::IsItemSelected(void* item)
	{
		return GetTreeCtrl()->IsSelected(item);
	}

	void TreeView::BeginLabelEdit(void* item)
	{
		GetTreeCtrl()->EditLabel(item);
	}

	void TreeView::EndLabelEdit(void* item, bool cancel)
	{
		GetTreeCtrl()->EndEditLabel(item, cancel);
	}

	void TreeView::ExpandAllChildren(void* item)
	{
		GetTreeCtrl()->ExpandAllChildren(item);
	}

	void TreeView::CollapseAllChildren(void* item)
	{
		GetTreeCtrl()->CollapseAllChildren(item);
	}

	void TreeView::EnsureVisible(void* item)
	{
		GetTreeCtrl()->EnsureVisible(item);
	}

	void TreeView::ScrollIntoView(void* item)
	{
		GetTreeCtrl()->ScrollTo(item);
	}

	long TreeView::GetStyle()
	{
		long style =
			(_hideRoot ? wxTR_HIDE_ROOT : 0) |
			(_selectionMode == TreeViewSelectionMode::Single ?
				wxTR_SINGLE : wxTR_MULTIPLE) |
			(_allowLabelEdit ? wxTR_EDIT_LABELS : 0) |
			(_showRootLines ? wxTR_LINES_AT_ROOT : 0) |
			(_showLines ? 0 : wxTR_NO_LINES) |
			(_fullRowSelect ? wxTR_FULL_ROW_HIGHLIGHT : 0) |
			(_showExpandButtons ? wxTR_HAS_BUTTONS : 0) |
			(_twistButtons ? wxTR_TWIST_BUTTONS : 0) |
			(_variableRowHeight ? wxTR_HAS_VARIABLE_ROW_HEIGHT : 0) |
			(_rowLines ? wxTR_ROW_LINES : 0);

		return style;
	}

	void TreeView::RecreateWxWindowIfNeeded()
	{
		if (_ignoreRecreate)
			return;
		Control::RecreateWxWindowIfNeeded();
		RaiseEvent(TreeViewEvent::ControlRecreated);
	}

	bool TreeView::GetHideRoot()
	{
		return _hideRoot;
	}

	void TreeView::SetHideRoot(bool value)
	{
		if (_hideRoot == value)
			return;

		_hideRoot = value;
		RecreateWxWindowIfNeeded();
	}

	bool TreeView::GetVariableRowHeight()
	{
		return _variableRowHeight;
	}

	void TreeView::SetVariableRowHeight(bool value)
	{
		if (_variableRowHeight == value)
			return;

		_variableRowHeight = value;
		RecreateWxWindowIfNeeded();
	}

	bool TreeView::GetTwistButtons()
	{
		return _twistButtons;
	}

	void TreeView::SetTwistButtons(bool value)
	{
		if (_twistButtons == value)
			return;

		_twistButtons = value;
		RecreateWxWindowIfNeeded();
	}

	uint32_t TreeView::GetStateImageSpacing()
	{
		return GetTreeCtrl()->GetSpacing();
	}

	void TreeView::SetStateImageSpacing(uint32_t value)
	{
		GetTreeCtrl()->SetSpacing(value);
	}

	uint32_t TreeView::GetIndentation()
	{
		return GetTreeCtrl()->GetIndent();
	}

	void TreeView::SetIndentation(uint32_t value)
	{
		GetTreeCtrl()->SetIndent(value);
	}

	bool TreeView::GetRowLines()
	{
		return _rowLines;
	}

	void TreeView::SetRowLines(bool value)
	{
		if (_rowLines == value)
			return;

		_rowLines = value;
		RecreateWxWindowIfNeeded();
	}
}