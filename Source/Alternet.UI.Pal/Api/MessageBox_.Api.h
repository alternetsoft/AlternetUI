#pragma once

#include "ApiUtils.h"
#include "MessageBox_.h"

using namespace Alternet::UI;

ALTERNET_UI_API void MessageBox_Show(const char16_t* text, const char16_t* caption)
{
    MessageBox_::Show(text, caption);
}

