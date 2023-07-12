#include "Sizer.h"

namespace Alternet::UI
{
    Sizer::Sizer()
    {
    }

    Sizer::~Sizer()
    {
    }

    void* Sizer::AddWindow(void* window, int proportion, int flag, int border, void* userData)
    {
        return sizer->Add((wxWindow*)window, proportion, flag, border, (wxObject*)userData);
    }
}
