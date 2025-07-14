#pragma once
#include "Common.h"
#include "Control.h"

namespace Alternet::UI
{
    class GroupBox : public Control
    {
#include "Api/GroupBox.inc"
    public:
        string GetText() override;
        void SetText(const string& value) override;

        wxWindow* CreateWxWindowCore(wxWindow* parent) override;
        wxWindow* CreateWxWindowUnparented() override;

        SizeD GetAutoPaddingLeftTop() override;
        SizeD GetAutoPaddingRightBottom() override;

    private:
        wxStaticBox* GetStaticBox();

        DelayedValue<GroupBox, string> _title;

        string RetrieveTitle();
        void ApplyTitle(const string& value);

        Thickness GetIntrinsicPadding(bool preferredSize);
    };
}
