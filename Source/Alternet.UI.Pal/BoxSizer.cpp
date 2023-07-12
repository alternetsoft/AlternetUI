#include "BoxSizer.h"
#include "wx/sizer.h"

namespace Alternet::UI
{
    BoxSizer::BoxSizer()
    {
        sizer = new wxBoxSizer(wxVERTICAL);
    }

    BoxSizer::~BoxSizer()
    {
    }

}
