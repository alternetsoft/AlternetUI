#pragma once
#include "Common.h"
#include "Control.h"

namespace Alternet::UI
{
    class GroupBox : public Control
    {
#include "Api/GroupBox.inc"
    public:
        virtual void RecreateWxWindowIfNeeded() override;
        string GetText() override;
        void SetText(const NativeStringSpan& value) override;

        wxWindow* CreateWxWindowCore(wxWindow* parent) override;
        wxWindow* CreateWxWindowUnparented() override;

        float GetAutoPaddingLeft() override;
        float GetAutoPaddingRight() override;
        float GetAutoPaddingTop() override;
        float GetAutoPaddingBottom() override;

    private:
        wxStaticBox* GetStaticBox();

        Thickness GetIntrinsicPadding(bool preferredSize);
    };
}
