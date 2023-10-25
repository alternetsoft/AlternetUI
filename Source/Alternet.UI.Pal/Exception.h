#pragma once

#include "Common.Strings.h"
#include "ErrorDiagnostics.h"
#include "OptionalInclude.h"

namespace Alternet::UI
{
    class Exception
    {
    public:
        class Origin
        {
        public:
            std::string functionName;
            std::string sourceFileName;
            int sourceLineNumber;
        };

        Exception(const string& message_, int errorCode_, const optional<Origin>& origin_) : message(message_), errorCode(errorCode_), origin(origin_) {}
        ~Exception() {}

        inline const string& GetMessageText() const { return message; }
        inline int GetErrorCode() const { return errorCode; }
        inline optional<Origin> GetOrigin() const { return origin; }

        inline string ToString() const
        {
            wxString sb;
            sb.Append(wxStr(message));
            if (origin.has_value())
            {
                auto& o = origin.value();
                if (!message.empty())
                    sb.Append("\n");

                sb.Append("At function: ").Append(o.functionName).
                    Append(", file: ").Append(o.sourceFileName).
                    Append(", line: ").Append(wxString::Format("%i", o.sourceLineNumber)).
                    Append(".");
            }

            return wxStr(sb);
        }

    private:
        int errorCode = 0;
        string message;
        optional<Origin> origin;
    };

    class InvalidOperationException : public Exception
    {
    public:
        InvalidOperationException(const string& message_, int errorCode_, const optional<Origin>& origin_) : Exception(message_, errorCode_, origin_) {}
    };

    class ThreadStateException : public Exception
    {
    public:
        ThreadStateException(const string& message_, int errorCode_, const optional<Origin>& origin_) : Exception(message_, errorCode_, origin_) {}
    };

    class FormatException : public Exception
    {
    public:
        FormatException(const string& message_, int errorCode_, const optional<Origin>& origin_) : Exception(message_, errorCode_, origin_) {}
    };

    class ArgumentNullException : public Exception
    {
    public:
        ArgumentNullException(const string& message_, int errorCode_, const optional<Origin>& origin_) : Exception(message_, errorCode_, origin_) {}
    };

    class ArgumentException : public Exception
    {
    public:
        ArgumentException(const string& message_, int errorCode_, const optional<Origin>& origin_) : Exception(message_, errorCode_, origin_) {}
    };
}

namespace _PreprocessorDetail
{
    constexpr bool IsPathSeparator(char c)
    {
        return c == '/' || c == '\\';
    }

    constexpr const char* GetFileNameImpl(const char* path)
    {
        const char* fileName = path;

        while (*path != '\0')
        {
            if (IsPathSeparator(*path))
            {
                fileName = path + 1;
            }

            path++;
        }

        return fileName;
    }
}

#define CURRENT_FILE_WITHOUT_PATH _PreprocessorDetail::GetFileNameImpl(__FILE__)

#define ExceptionOrigin Alternet::UI::Exception::Origin { __func__, CURRENT_FILE_WITHOUT_PATH, __LINE__ }

#define throwEx(message) throw Alternet::UI::Exception((message), 0, ExceptionOrigin)
#define throwExTyped(exceptionType, message) throw exceptionType((message), 0, ExceptionOrigin)
#define throwExNoInfo throw Alternet::UI::Exception(u"", 0, ExceptionOrigin)
#define throwExInvalidArgNoInfo(argument) throw Alternet::UI::ArgumentException(string(u"Invalid argument: ") + (u###argument), 0, ExceptionOrigin)
#define throwExInvalidArg(argument, message) throw Alternet::UI::ArgumentException(string(message) + string(u". Argument name: ") + (u###argument), 0, ExceptionOrigin)
#define throwExInvalidArgEnumValue(argument) throw Alternet::UI::ArgumentException(string(u"Unexpected enum value. Argument name: ") + (u###argument), 0, ExceptionOrigin)
#define throwExInvalidOp throw Alternet::UI::InvalidOperationException(u"", 0, ExceptionOrigin)
#define throwExInvalidOpWithInfo(message) throw Alternet::UI::InvalidOperationException(string(message), 0, ExceptionOrigin)


#define DebugLogInvalidArg(argument, message) Alternet::UI::Application::Log(string(message) + string(u". Argument name: ") + (u###argument))


#ifdef PLATFORM_WINDOWS
#define ThrowOnFail(hr) \
	if (FAILED((hr))) \
		throw Alternet::UI::Exception( \
			Alternet::UI::GetErrorCodeAndDescription((hr)), (hr), ExceptionOrigin);

#define ThrowWin32Error(errorCode) \
	throw Alternet::UI::Exception( \
		Alternet::UI::GetErrorCodeAndDescription((errorCode)), (errorCode), ExceptionOrigin);

#define ThrowLastError \
	throw Alternet::UI::Exception( \
		Alternet::UI::GetErrorCodeAndDescription(GetLastError()), GetLastError(), ExceptionOrigin);
#endif