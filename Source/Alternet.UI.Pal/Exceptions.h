#pragma once

#include <functional>
#include <vector>
#include <string.h>
#include "Exception.h"
#include "ExceptionsMarshal.h"

extern NativeExceptionCallbackType unhandledExceptionCallback;

inline void SetExceptionCallbackImpl(NativeExceptionCallbackType unhandledExceptionCallback_)
{
    unhandledExceptionCallback = unhandledExceptionCallback_;
}

template<typename TResult> TResult MarshalExceptions(std::function<TResult()> action)
{
    return MarshalExceptions(action, [&] { return unhandledExceptionCallback; });
}