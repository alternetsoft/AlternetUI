#include "Control.h"

namespace Alternet::UI
{
    Control::Control()
    {
    }

    Control::~Control()
    {
        //delete GetControl();
    }

    wxWindow* Control::CreateWxWindow(wxWindow* parent) { return nullptr; }
}