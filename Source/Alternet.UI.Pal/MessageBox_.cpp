#include "MessageBox_.h"

namespace Alternet::UI
{
    /*static*/ void MessageBox_::Show(const string& text, optional<string> caption)
    {
        wxMessageBox(wxStr(text), wxStr(caption.value_or(u"")));
    }

}