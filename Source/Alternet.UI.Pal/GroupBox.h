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

        Thickness GetIntrinsicPadding() override;

    private:
        wxStaticBox* GetStaticBox();

        DelayedValue<GroupBox, optional<string>> _title;

        optional<string> RetrieveTitle();
        void ApplyTitle(const optional<string>& value);
    };
}
