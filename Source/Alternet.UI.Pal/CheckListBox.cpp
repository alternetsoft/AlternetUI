#include "CheckListBox.h"

namespace Alternet::UI
{
    CheckListBox::CheckListBox()
    {
    }

    CheckListBox::~CheckListBox()
    {
        if (IsWxWindowCreated())
        {
            auto window = GetWxWindow();
            if (window != nullptr)
            {
                window->Unbind(wxEVT_CHECKLISTBOX, &CheckListBox::OnCheckedChanged, this);
                window->Unbind(wxEVT_LISTBOX, &CheckListBox::OnSelectionChanged, this);
            }
        }
    }


    wxWindow* CheckListBox::CreateWxWindowCore(wxWindow* parent)
    {
        auto value = new wxCheckListBox(
            parent,
            wxID_ANY,
            wxDefaultPosition,
            wxDefaultSize,
            0,
            NULL,
            GetSelectionStyle());

        value->Bind(wxEVT_CHECKLISTBOX, &CheckListBox::OnCheckedChanged, this);
        value->Bind(wxEVT_LISTBOX, &CheckListBox::OnSelectionChanged, this);

        return value;
    }

    void CheckListBox::OnCheckedChanged(wxCommandEvent& event)
    {
        RaiseEvent(CheckListBoxEvent::CheckedChanged);
    }

    void CheckListBox::OnSelectionChanged(wxCommandEvent& event)
    {
        RaiseEvent(CheckListBoxEvent::SelectionChanged);
    }

    void CheckListBox::OnWxWindowCreated()
    {
        ListBox::OnWxWindowCreated();
    }

    void CheckListBox::OnBeforeDestroyWxWindow()
    {
        ListBox::OnBeforeDestroyWxWindow();
    }


    void* CheckListBox::OpenCheckedIndicesArray()
    {
        auto array = new wxArrayInt();
        if (IsWxWindowCreated())
        {
            GetCheckListBox()->GetCheckedItems(*array);
        }
        else
        {
            for (size_t i = 0; i < _checkedIndices.size(); i++)
                array->Add(_checkedIndices[i]);
        }

        return array;
    }

    void CheckListBox::CloseCheckedIndicesArray(void* array)
    {
        delete ((wxArrayInt*)array);
    }

    int CheckListBox::GetCheckedIndicesItemCount(void* array)
    {
        return ((wxArrayInt*)array)->GetCount();
    }

    int CheckListBox::GetCheckedIndicesItemAt(void* array, int index)
    {
        return (*((wxArrayInt*)array))[index];
    }


    void CheckListBox::ClearChecked()
    {
        if (IsWxWindowCreated())
        {
            UncheckAll();
        }
        else
            _checkedIndices.clear();
    }

    void CheckListBox::SetChecked(int index, bool value)
    {
        if (IsWxWindowCreated())
        {
            auto listBox = GetCheckListBox();
            listBox->Check(index, value);
        }
        else
        {
            auto existingIndex = std::find(_checkedIndices.begin(), _checkedIndices.end(), index);

            if (value)
            {
                if (existingIndex == _checkedIndices.end())
                    _checkedIndices.push_back(index);
            }
            else
            {
                if (existingIndex != _checkedIndices.end())
                    _checkedIndices.erase(existingIndex);
            }
        }
    }

    bool CheckListBox::IsChecked(int index)
    {
        auto listBox = GetCheckListBox();
        return listBox->IsChecked(index);
    }

    void CheckListBox::UncheckAll()
    {
        auto listBox = GetCheckListBox();

        for (unsigned i = 0; i < listBox->GetCount(); i++)
            listBox->Check(i, false);
    }

    void CheckListBox::ApplyCheckedIndices()
    {
        UncheckAll();

        if (_checkedIndices.empty())
            return;

        auto listBox = GetCheckListBox();
        
        for (auto index : _checkedIndices)
            listBox->Check(index, true);

        _checkedIndices.clear();
    }

    void CheckListBox::ReceiveCheckedIndices()
    {
        auto listBox = GetCheckListBox();
        _checkedIndices.clear();

        if (listBox->GetCount() == 0)
            return;

        for (unsigned i = 0; i < listBox->GetCount(); i++)
            if (listBox->IsChecked(i))
                _checkedIndices.push_back(i);
    }

    wxCheckListBox* CheckListBox::GetCheckListBox()
    {
        return dynamic_cast<wxCheckListBox*>(GetWxWindow());
    }

    std::vector<int> CheckListBox::GetCheckedIndices()
    {
        auto array = OpenCheckedIndicesArray();
        int count = GetCheckedIndicesItemCount(array);

        std::vector<int> indices(count);
        for (int i = 0; i < count; i++)
            indices[i] = GetCheckedIndicesItemAt(array, i);

        CloseCheckedIndicesArray(array);

        return indices;
    }

    void CheckListBox::SetCheckedIndices(const std::vector<int>& value)
    {
        ClearChecked(); 
        for (auto index : value)
            SetChecked(index, true);
    }
}
