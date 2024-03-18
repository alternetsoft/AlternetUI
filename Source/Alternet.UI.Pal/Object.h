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

        void SetId(uint64_t value)
        {
            _id = value;
        }

        uint64_t GetId()
        {
            return _id;
        }

        void AddRef()
        {
            if (_referenceCount < 0)
                throwEx(u"Reference count cannot be less than 0");

            _referenceCount++;
        }

        void Release()
        {
            _referenceCount--;

            if (_referenceCount < 0)
                throwEx(u"Reference count cannot be less than 0");

            if (_referenceCount == 0)
                delete this;
        }

    protected:
        virtual bool EventsSuspended()
        {
            return false;
        }

    private:
        int _referenceCount;
        uint64_t _id;
    };
}

