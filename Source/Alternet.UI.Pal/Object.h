#pragma once

#include "Common.h"

namespace Alternet::UI
{
    class Object
    {
    public:
        Object() : _referenceCount(1)
        {
        }

        virtual ~Object()
        {
        }

        void AddRef()
        {
            wxASSERT(_referenceCount >= 0);
            _referenceCount++;
        }

        void Release()
        {
            _referenceCount--;
            wxASSERT(_referenceCount >= 0);
            if (_referenceCount == 0)
                delete this;
        }

    protected:

    private:
        int _referenceCount;
    };
}

