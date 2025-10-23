#include "ListBox.h"

namespace Alternet::UI
{
    int ListBox::GetCheckedIndexesCount()
    {
        return (int)_checked.GetCount();
    }

    int ListBox::GetCheckedIndexesItem(int index)
    {
        return _checked.Item(index);
    }

    void ListBox::UpdateCheckedIndexes()
    {
		GetCheckListBox()->GetCheckedItems(_checked);
    }

    void* ListBox::CreateListBox(ListBoxHandlerCreateFlags createFlags)
    {
		return new ListBox(createFlags);
    }

    ListBox::ListBox(ListBoxHandlerCreateFlags createFlags)
    {
		_createFlags = createFlags;
    }

    ListBox::ListBox()
    {
    }

    bool ListBox::GetHasBorder()
    {
        return hasBorder;
    }

    void ListBox::SetHasBorder(bool value)
    {
        if (hasBorder == value)
            return;
        hasBorder = value;
        RecreateWxWindowIfNeeded();
    }

    wxListBoxBase* ListBox::GetListBoxBase()
    {
        auto value = dynamic_cast<wxListBoxBase*>(GetWxWindow());
        if (value == nullptr)
            throwExInvalidOp;
        return value;
    }

    wxListBox* ListBox::GetListBox()
    {
        auto value = dynamic_cast<wxListBox*>(GetWxWindow());
        if (value == nullptr)
            throwExInvalidOp;
        return value;
    }

    wxCheckListBox* ListBox::GetCheckListBox()
    {
        auto value = dynamic_cast<wxCheckListBox*>(GetWxWindow());
        if (value == nullptr)
            throwExInvalidOp;
        return value;
    }

    wxItemContainer* ListBox::GetItemContainer()
    {
        auto value = dynamic_cast<wxItemContainer*>(GetWxWindow());
        if (value == nullptr)
            throwExInvalidOp;
        return value;
    }

    wxWindow* ListBox::CreateWxWindowUnparented()
    {
        auto value = new wxListBox2();
        return value;
    }

    wxWindow* ListBox::CreateWxWindowCore(wxWindow* parent)
    {
        long style = (long)_flags;

        if (!hasBorder)
            style = style | wxBORDER_NONE;

        wxListBox* value;

        if (HasCheckBoxes())
        {
            value = new wxCheckListBox2(
                parent,
                wxID_ANY,
                wxDefaultPosition,
                wxDefaultSize,
                0,
                nullptr,
                style);
            value->Bind(wxEVT_CHECKLISTBOX, &ListBox::OnCheckedChanged, this);
        }
        else
        {
            value = new wxListBox2(
                parent,
                wxID_ANY,
                wxDefaultPosition,
                wxDefaultSize,
                0,
                nullptr,
                style);
        }

        value->Bind(wxEVT_LISTBOX, &ListBox::OnSelectedChanged, this);
        return value;
    }

    ListBox::~ListBox()
    {
        if (IsWxWindowCreated())
        {
            auto window = GetListBox();
            window->Unbind(wxEVT_LISTBOX,
                &ListBox::OnSelectedChanged, this);
            if (HasCheckBoxes())
            {
                window->Unbind(wxEVT_CHECKLISTBOX,
                    &ListBox::OnCheckedChanged, this);
            }
        }
    }

    void ListBox::Check(int item, bool check)
    {
		GetCheckListBox()->Check(item, check);
    }

    bool ListBox::IsChecked(int item)
    {
		return GetCheckListBox()->IsChecked(item);
    }

    void ListBox::OnSelectedChanged(wxCommandEvent& event)
    {
        event.Skip();
        RaiseEvent(ListBoxEvent::SelectionChanged);
    }

    void ListBox::OnCheckedChanged(wxCommandEvent& event)
    {
        event.Skip();
        RaiseEvent(ListBoxEvent::CheckedChanged);
    }

    ListBoxHandlerFlags ListBox::GetFlags()
    {
        return _flags;
	}

    void ListBox::SetFlags(ListBoxHandlerFlags flags)
    {
        if(_flags == flags)
			return;
		_flags = flags;
        RecreateWxWindowIfNeeded();
    }

    int ListBox::GetSelection()
    {
		return GetListBoxBase()->GetSelection();
    }

    bool ListBox::IsSelected(int n)
    {
        return GetListBox()->IsSelected(n);
	}

    bool ListBox::IsSorted()
    {
        return GetItemContainer()->IsSorted();
	}

    int ListBox::GetCountPerPage()
    {
		return GetListBoxBase()->GetCountPerPage();
	}

    int ListBox::GetTopItem()
    {
        return GetListBoxBase()->GetTopItem();
    }

    void ListBox::Deselect(int n)
    {
		GetListBoxBase()->Deselect(n);
    }

    void ListBox::EnsureVisible(int n)
    {
		GetListBoxBase()->EnsureVisible(n);
    }
    void ListBox::SetFirstItem(int n)
    {
		GetListBoxBase()->SetFirstItem(n);
    }

    void ListBox::SetFirstItemStr(const string& s)
    {
        GetListBoxBase()->SetFirstItem(wxStr(s));
    }

    void ListBox::SetSelection(int n)
    {
		GetListBoxBase()->SetSelection(n);
    }

    void ListBox::Clear()
    {
		GetItemContainer()->Clear();
    }

    bool ListBox::SetStringSelection(const string& s, bool select)
    {
        return GetListBoxBase()->SetStringSelection(wxStr(s), select);
    }

    void ListBox::SetString(uint32_t n, const string& s)
    {
		GetItemContainer()->SetString(n, wxStr(s));
    }

    string ListBox::GetString(uint32_t n)
    {
		return wxStr(GetItemContainer()->GetString(n));
    }

    uint32_t ListBox::GetCount()
    {
		return GetItemContainer()->GetCount();
    }

    void ListBox::Delete(uint32_t n)
    {
		GetItemContainer()->Delete(n);
    }

    int ListBox::Append(const string& s)
    {
		return GetItemContainer()->Append(wxStr(s));
    }

    int ListBox::Insert(const string& item, uint32_t pos)
    {
		return GetItemContainer()->Insert(wxStr(item), pos);
    }

    int ListBox::FindString(const string& s, bool bCase)
    {
		return GetItemContainer()->FindString(wxStr(s), bCase);
    }

    int ListBox::HitTest(const PointD& point)
    {
        auto listBox = GetListBoxBase();
        auto p = fromDip(point, listBox);
		return listBox->HitTest(p);
    }

    void ListBox::SetItemSelection(int index, bool select)
    {
        auto listBox = GetListBoxBase();
		listBox->SetSelection(index, select);
    }

    int ListBox::GetSelectionsCount()
    {
		return (int)_selections.GetCount();
    }

    int ListBox::GetSelectionsItem(int index)
    {
		return _selections.Item(index);
    }

    void ListBox::UpdateSelections()
    {
        GetListBox()->GetSelections(_selections);
    }
}
