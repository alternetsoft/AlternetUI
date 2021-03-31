#include "MessageBox_.h"

namespace Alternet::UI
{
    /*static*/ void MessageBox_::Show(const string& text, const string& caption)
    {
        wxMessageBox(wxStr(text), wxStr(caption));
    }

}