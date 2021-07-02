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

        void OnSelectionChanged(wxCommandEvent& event);

    protected:
        void OnWxWindowCreated() override;
        void OnWxWindowDestroying() override;

    private:

        std::vector<string> _items;

        DelayedValue<ComboBox, int> _selectedIndex;
        DelayedValue<ComboBox, string> _text;

        bool _isEditable = true;

        long GetComboBoxStyle();

        void ApplyItems();
        void ReceiveItems();

        int RetrieveSelectedIndex();
        void ApplySelectedIndex(const int& value);

        string RetrieveText();
        void ApplyText(const string& value);

        wxComboBox* GetComboBox();
    };
}
