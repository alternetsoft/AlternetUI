#pragma once
#include "wx/log.h"

namespace Alternet::UI
{
    // https://docs.wxwidgets.org/3.2/classwx_log_formatter.html
    class wxAlternetLogFormatter : public wxLogFormatter
    {
        virtual wxString Format(wxLogLevel level,
            const wxString& msg,
            const wxLogRecordInfo& info) const
        {
            return wxLogFormatter::Format(level, msg, info);
            //return wxString::Format("%s(%d) : %s", info.filename, info.line, msg);
        }
    };
}