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

        void SetId(int value)
        {
            _id = value;
        }

        int GetId()
        {
            return _id;
        }

        void AddRef()
        {
            if (_referenceCount < 0)
                throwEx("Reference count cannot be less than 0");

            _referenceCount++;
        }

        void Release()
        {
            _referenceCount--;

            if (_referenceCount < 0)
                throwEx("Reference count cannot be less than 0");

            if (_referenceCount == 0)
                delete this;
        }

    protected:
        wxString _container;

        static wxString _containerStatic;

        virtual bool EventsSuspended()
        {
            return false;
        }

    private:
        int _referenceCount;
        int _id = 0;
    };
}

