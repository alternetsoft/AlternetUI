#pragma once
#include "Common.h"
#include "Control.h"

namespace Alternet::UI
{
    class ComboBox : public Control
    {
#include "Api/ComboBox.inc"
    public:
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;

        void OnSelectedItemChanged(wxCommandEvent& event);
        void OnTextChanged(wxCommandEvent& event);

        void ApplyMinimumSize(const Size& value) override {}
        void ApplyMaximumSize(const Size& value) override {}

    protected:
        void OnWxWindowCreated() override;
        void OnBeforeDestroyWxWindow() override;

    private:
        bool IsUsingComboBoxControl();
        bool IsUsingChoiceControl();
        bool hasBorder = true;

        std::vector<string> _items;

        DelayedValue<ComboBox, int> _selectedIndex;
        DelayedValue<ComboBox, string> _text;

        bool _isEditable = true;

        void ApplyItems();
        void ReceiveItems();

        int RetrieveSelectedIndex();
        void ApplySelectedIndex(const int& value);

        string RetrieveText();
        void ApplyText(const string& value);

        wxComboBox* GetComboBox();
        wxChoice* GetChoice();
        wxItemContainer* GetItemContainer();
    };
}
