// This file is NOT auto generated.

#pragma once

#include "Exceptions.h"
#include "ApiUtils.h"

using namespace Alternet::UI;

ALTERNET_UI_API void SetExceptionCallback(NativeExceptionCallbackType unhandledExceptionCallback_, NativeExceptionCallbackType caughtExceptionCallback_)
{
    SetExceptionCallbackImpl(unhandledExceptionCallback_, caughtExceptionCallback_);
}