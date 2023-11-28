#pragma once
#include "Common.h"
#include "Control.h"

namespace Alternet::UI
{
    class GroupBox : public Control
    {
#include "Api/GroupBox.inc"
    public:
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;
        wxWindow* CreateWxWindowUnparented() override;

        Thickness GetIntrinsicLayoutPadding() override;
        Thickness GetIntrinsicPreferredSizePadding() override;

    private:
        wxStaticBox* GetStaticBox();

        DelayedValue<GroupBox, optional<string>> _title;

        optional<string> RetrieveTitle();
        void ApplyTitle(const optional<string>& value);

        Thickness GetIntrinsicPadding(bool preferredSize);
    };
}
