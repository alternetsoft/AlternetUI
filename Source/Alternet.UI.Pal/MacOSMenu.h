#pragma once
#include "Common.h"
#include "MainMenu.h"

#ifdef __WXOSX_COCOA__


namespace
{
}

namespace Alternet::UI::MacOSMenu
{
    void EnsureHelpItemIsLast(wxMenuBar* menuBar)
    {
        // macOS appends "Window" menu to the main menu automatically.
        // If our menu has "Help" menu, move it to the end, after the "Window" menu.
        // This only needs to be done when there is no "Window" menu explicitly added by the user.

        auto windowMenuIndex = menuBar->FindMenu("Window");
        if (windowMenuIndex != wxNOT_FOUND)
            return;

        auto helpMenuIndex = menuBar->FindMenu("Help");
        if (helpMenuIndex == wxNOT_FOUND)
            return;

        auto helpMenu = menuBar->GetMenu(helpMenuIndex);
        auto helpMenuLabel = menuBar->GetMenuLabel(helpMenuIndex);

        menuBar->Remove(helpMenuIndex);
        menuBar->Append(helpMenu, helpMenuLabel);
    }

    void ApplyItemRoles(MainMenu* mainMenu)
    {

    }
}

#endif