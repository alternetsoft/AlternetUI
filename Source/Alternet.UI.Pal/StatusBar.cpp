#include "StatusBar.h"
#include "StatusBarPanel.h"
#include "Window.h"

namespace Alternet::UI
{
    StatusBar::StatusBar()
    {
    }

    StatusBar::~StatusBar()
    {
        DestroyWxStatusBar();
    }

    int StatusBar::GetPanelCount()
    {
        return _items.size();
    }

    void StatusBar::InsertPanelAt(int index, StatusBarPanel* item)
    {
        _items.insert(_items.begin() + index, item);
        ApplyItems(index);
    }

    void StatusBar::RemovePanelAt(int index)
    {
        auto it = _items.begin() + index;
        auto item = *it;
        _items.erase(it);
        ApplyItems(index);
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

    void StatusBar::OnItemChanged(int index)
    {
        ApplyItems(index, 1);
    }

    void StatusBar::ApplyEnabled(bool value)
    {
    }

    void StatusBar::ApplyBounds(const Rect& value)
    {
    }

    void StatusBar::ApplyItems(size_t startIndex, optional<int> count/* = nullopt*/)
    {
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
    }

    void StatusBar::CreateWxStatusBar(Window* window)
    {
        if (_wxStatusBar != nullptr)
            throwExNoInfo;

        if (_ownerWindow == nullptr)
            return;

        auto getStyle = [&]
        {
            auto style = wxFULL_REPAINT_ON_RESIZE;

            if (_sizingGripperVisible)
                style |= wxSTB_SIZEGRIP;

            return style;
        };

        _wxStatusBar = window->GetFrame()->CreateStatusBar(1, getStyle());

        ApplyItems(0);
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
