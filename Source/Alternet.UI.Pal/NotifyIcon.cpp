#include "NotifyIcon.h"

namespace Alternet::UI
{
    NotifyIcon::NotifyIcon()
    {
    }

    NotifyIcon::~NotifyIcon()
    {
    }

    optional<string> NotifyIcon::GetText()
    {
        return optional<string>();
    }

    void NotifyIcon::SetText(optional<string> value)
    {
    }

    Image* NotifyIcon::GetIcon()
    {
        return nullptr;
    }

    void NotifyIcon::SetIcon(Image* value)
    {
    }

    Menu* NotifyIcon::GetMenu()
    {
        return nullptr;
    }

    void NotifyIcon::SetMenu(Menu* value)
    {
    }

    bool NotifyIcon::GetVisible()
    {
        return false;
    }

    void NotifyIcon::SetVisible(bool value)
    {
    }
}
