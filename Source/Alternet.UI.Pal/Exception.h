#pragma once

#include "Common.Strings.h"
#include "ErrorDiagnostics.h"
#include "OptionalInclude.h"

namespace Alternet::UI
{
    class Exception
    {
    public:
        inline static wxString _exception = wxEmptyString;        
        inline static std::function<void(wxString)> _logMessageProc;

        class Origin
        {
        public:
            wxString functionName;
            wxString sourceFileName;
            int sourceLineNumber;
        };

        Exception(const wxString& message_, int errorCode_, const optional<Origin>& origin_)
            : message(message_), errorCode(errorCode_), origin(origin_)
        {
            if(_logMessageProc != nullptr)
                _logMessageProc(ToString());
        }
        ~Exception() {}

        inline const wxString& GetMessageText() const { return message; }
        inline int GetErrorCode() const { return errorCode; }
        inline optional<Origin> GetOrigin() const { return origin; }

        inline std::u16string ToStdString() const
        {
            wxString s = ToString();
            std::u16string u16(s.wc_str(), s.wc_str() + s.length());
            return u16;
        }

        inline wxString ToString() const
        {
            wxString sb;
            sb.Append(message);
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

            return sb;
        }

    private:
        int errorCode = 0;
        wxString message;
        optional<Origin> origin;
    };

    class InvalidOperationException : public Exception
    {
    public:
        InvalidOperationException(
            const wxString& message_, int errorCode_, const optional<Origin>& origin_)
            : Exception(message_, errorCode_, origin_)
        {
        }
    };

    class ThreadStateException : public Exception
    {
    public:
        ThreadStateException(const wxString& message_, int errorCode_, const optional<Origin>& origin_) : Exception(message_, errorCode_, origin_) {}
    };

    class FormatException : public Exception
    {
    public:
        FormatException(const wxString& message_, int errorCode_, const optional<Origin>& origin_) : Exception(message_, errorCode_, origin_) {}
    };

    class ArgumentNullException : public Exception
    {
    public:
        ArgumentNullException(const wxString& message_, int errorCode_, const optional<Origin>& origin_) : Exception(message_, errorCode_, origin_) {}
    };

    class ArgumentException : public Exception
    {
    public:
        ArgumentException(const wxString& message_, int errorCode_, const optional<Origin>& origin_) : Exception(message_, errorCode_, origin_) {}
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
#define throwExNoInfo throw Alternet::UI::Exception("", 0, ExceptionOrigin)
#define throwExInvalidArgNoInfo(argument) throw Alternet::UI::ArgumentException("Invalid argument: " + argument, 0, ExceptionOrigin)
#define throwExInvalidArgEnumValue(argument) throw Alternet::UI::ArgumentException("Unexpected enum value. Argument name: " + argument, 0, ExceptionOrigin)
#define throwExInvalidOp throw Alternet::UI::InvalidOperationException("", 0, ExceptionOrigin)
#define throwExInvalidOpWithInfo(message) throw Alternet::UI::InvalidOperationException(message, 0, ExceptionOrigin)

#define DebugLogInvalidArg(argument, message) Alternet::UI::Application::Log(message + ". Argument name: " + argument)


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