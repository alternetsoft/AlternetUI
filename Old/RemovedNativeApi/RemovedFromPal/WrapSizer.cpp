#include "WrapSizer.h"

namespace Alternet::UI
{
    void* WrapSizer::CreateWrapSizer(int orient, int flags)
    {
        return new wxWrapSizer(orient, flags);
    }

    void WrapSizer::RepositionChildren(void* handle, const Int32Size& minSize)
    {
        ((wxWrapSizer*)handle)->RepositionChildren(minSize);
    }

    Int32Size WrapSizer::CalcMin(void* handle)
    {
        return ((wxWrapSizer*)handle)->CalcMin();
    }

    WrapSizer::WrapSizer()
    {
    }

    WrapSizer::~WrapSizer()
    {
    }

}
