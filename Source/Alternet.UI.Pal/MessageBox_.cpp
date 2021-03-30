#include "MessageBox_.h"

namespace Alternet::UI
{
    /*static*/ void MessageBox_::Show(string text, string caption)
    {
        wxMessageBox(wxStr(text), wxStr(caption));
    }

}