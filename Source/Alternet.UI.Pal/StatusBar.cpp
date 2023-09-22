#include "StatusBar.h"
#include "Window.h"

namespace Alternet::UI
{
    void* StatusBar::GetRealHandle()
    {
        return GetWxStatusBar();
    }

    StatusBar::StatusBar()
    {
    }

    StatusBar::~StatusBar()
    {
        DestroyWxStatusBar();
    }

    wxWindow* StatusBar::CreateWxWindowCore(wxWindow* parent)
    {
        return new wxDummyPanel("statusbar");
    }

    wxStatusBar* StatusBar::GetWxStatusBar()
    {
        return _wxStatusBar;
    }

    void StatusBar::SetOwnerWindow(Window* window)
    {
        _ownerWindow = window;
        RecreateWxStatusBar(window);
    }

    /*void StatusBar::ApplyItems(size_t startIndex, optional<int> count)
    {
        return;

        auto sb = GetWxStatusBar();
        if (sb == nullptr)
            return;

        auto itemsCount = _items.size();

        auto panesCount = sb->GetFieldsCount();

        if (panesCount != itemsCount)
        {
            auto newCount = itemsCount;
            if (newCount < 1)
                newCount = 1;
            sb->SetFieldsCount(newCount);
        }

        if (itemsCount == 0)
            return;

        for (size_t i = startIndex;
            i < itemsCount && ((!count.has_value()) || i < startIndex + count.value());
            i++)
        {
            _items[i]->SetParentStatusBar(this, i);
            sb->SetStatusText(wxStr(_items[i]->GetText()), i);
        }
    }*/

    void StatusBar::CreateWxStatusBar(Window* window)
    {
        if (_wxStatusBar != nullptr)
            throwExNoInfo;

        if (_ownerWindow == nullptr)
            return;

        auto getStyle = [&]
        {
            auto style = (wxSTB_ELLIPSIZE_END | wxSTB_SHOW_TIPS | wxFULL_REPAINT_ON_RESIZE);

            if (_sizingGripperVisible)
                style |= wxSTB_SIZEGRIP;

            return style;
        };

        _wxStatusBar = window->GetFrame()->CreateStatusBar(1, getStyle());
    }

    void StatusBar::DestroyWxStatusBar()
    {
        if (_wxStatusBar != nullptr)
        {
            delete _wxStatusBar;
            _wxStatusBar = nullptr;
        }
    }

    void StatusBar::RecreateWxStatusBar(Window* window)
    {
        DestroyWxStatusBar();
        CreateWxStatusBar(window);
    }

    bool StatusBar::GetSizingGripVisible()
    {
        return _sizingGripperVisible;
    }

    void StatusBar::SetSizingGripVisible(bool value)
    {
        if (_sizingGripperVisible == value)
            return;

        _sizingGripperVisible = value;
        
        if (_ownerWindow != nullptr)
            RecreateWxStatusBar(_ownerWindow);
    }
}
