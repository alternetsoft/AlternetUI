#pragma once

#include <functional>
#include <vector>
#include <string.h>
#include "Exception.h"
#include "ExceptionsMarshal.h"

extern NativeExceptionCallbackType unhandledExceptionCallback;
extern NativeExceptionCallbackType caughtExceptionCallback;

inline void SetExceptionCallbackImpl(NativeExceptionCallbackType unhandledExceptionCallback_, NativeExceptionCallbackType caughtExceptionCallback_)
{
    unhandledExceptionCallback = unhandledExceptionCallback_;
    caughtExceptionCallback = caughtExceptionCallback_;
}

template<typename TResult> TResult MarshalExceptions(std::function<TResult()> action)
{
    return MarshalExceptions(action, [&] { return unhandledExceptionCallback; });
}

template<typename TResult> TResult CatchAndMarshalThreadExceptions(std::function<TResult()> action)
{
    return MarshalExceptions(action, [&] { return caughtExceptionCallback; });
}