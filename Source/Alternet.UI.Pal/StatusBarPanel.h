#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Control.h"

namespace Alternet::UI
{
    class StatusBar;

    class StatusBarPanel : public Control
    {
#include "Api/StatusBarPanel.inc"
    public:
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;

        void SetParentStatusBar(StatusBar* value, optional<int> index);
        StatusBar* GetParentStatusBar();

        optional<int> GetIndex();
    protected:
        void ShowCore() override;
        Rect RetrieveBounds() override;
        void ApplyBounds(const Rect& value) override;
        Size SizeToClientSize(const Size& size) override;
        void UpdateWxWindowParent() override;

    private:
        string _text;
        StatusBar* _parentStatusBar = nullptr;

        optional<int> _index;
    };
}
