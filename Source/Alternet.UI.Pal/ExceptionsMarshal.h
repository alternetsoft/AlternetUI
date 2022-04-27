#pragma once

#include <functional>
#include <vector>
#include <string.h>
#include "Exception.h"

enum class ExceptionType
{
    ExternalException,
    InvalidOperationException,
    FormatException,
    ArgumentNullException,
    ThreadStateException,
};

typedef void(*NativeExceptionCallbackType)(ExceptionType exceptionType, const char16_t* message, int errorCode);

template<typename TResult> TResult MarshalExceptions(
    std::function<TResult()> action,
    std::function<NativeExceptionCallbackType()> getNativeExceptionCallback)
{
    auto reportException = [&](ExceptionType exceptionType, const char16_t* message, int errorCode)
    {
        auto callback = getNativeExceptionCallback();
        if (callback != nullptr)
            callback(exceptionType, message, errorCode);
    };

    try
    {
        return action();
    }
    catch (Alternet::UI::InvalidOperationException& e)
    {
        reportException(ExceptionType::InvalidOperationException, e.ToString().c_str(), e.GetErrorCode());
        return TResult();
    }
    catch (Alternet::UI::ThreadStateException& e)
    {
        reportException(ExceptionType::ThreadStateException, e.ToString().c_str(), e.GetErrorCode());
        return TResult();
    }
    catch (Alternet::UI::FormatException & e)
    {
        reportException(ExceptionType::FormatException, e.ToString().c_str(), e.GetErrorCode());
        return TResult();
    }
    catch (Alternet::UI::ArgumentNullException & e)
    {
        reportException(ExceptionType::ArgumentNullException, e.ToString().c_str(), e.GetErrorCode());
        return TResult();
    }
    catch (Alternet::UI::Exception& e)
    {
        reportException(ExceptionType::ExternalException, e.ToString().c_str(), e.GetErrorCode());
        return TResult();
    }
    catch (...)
    {
        reportException(ExceptionType::ExternalException, u"Unknown exception.", 0);
        return TResult();
    }
}