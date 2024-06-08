#include "BoxSizer.h"

#include <wx/sizer.h>

namespace Alternet::UI
{
    void* BoxSizer::CreateBoxSizer(int orient)
    {
        return new wxBoxSizer(orient);
    }

    void* BoxSizer::AddSpacer(void* handle, int size)
    {
        return ((wxBoxSizer*)handle)->AddSpacer(size);
    }

    Int32Size BoxSizer::CalcMin(void* handle)
    {
        return ((wxBoxSizer*)handle)->CalcMin();
    }

    int BoxSizer::GetOrientation(void* handle)
    {
        return ((wxBoxSizer*)handle)->GetOrientation();
    }

    void BoxSizer::SetOrientation(void* handle, int orient)
    {
        ((wxBoxSizer*)handle)->SetOrientation(orient);
    }

    void BoxSizer::RepositionChildren(void* handle, const Int32Size& minSize)
    {
        ((wxBoxSizer*)handle)->RepositionChildren(minSize);
    }

    BoxSizer::BoxSizer()
    {
    }

    BoxSizer::~BoxSizer()
    {
    }
}
