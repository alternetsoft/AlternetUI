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

        /* Fixed for MACOS ControlsSample[2411:15866] This application is trying
        to draw a very large combo box, 30 points tall.Vertically resizable combo
        boxes are not supported, but it happens that 10.4 and previous drew
        something that looked kind of sort of okay.The art in 10.5 does
        not break up in a way that supports that drawing.This application should be
        revised to stop using large combo boxes.This warning will appear once per app launch.*/
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
