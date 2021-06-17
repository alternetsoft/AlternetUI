#pragma once
#include "Common.h"
#include "Control.h"

namespace Alternet::UI
{
    class TabControl;

    class TabPage : public Control
    {
#include "Api/TabPage.inc"
    public:
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;

        TabControl* GetOwnerTabControl();

        void OnParentWxWindowChanged(wxWindow* oldParent, wxWindow* newParent) override;
    private:
        DelayedValue<TabPage, string> _title;

        string RetrieveTitle();
        void ApplyTitle(const string& value);
    };
}
