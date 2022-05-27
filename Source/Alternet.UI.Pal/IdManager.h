#pragma once

#include "Common.h"

namespace Alternet::UI
{
    class IdManager
    {
    public:
        inline static int AllocateId()
        {
            if (_nextAvailableId == INT_MAX)
            {
                if (_freedIds.empty())
                    throwEx(u"Cannot allocate an ID. No available IDs left.");

                auto id = _freedIds.top();
                _freedIds.pop();
                return id;
            }
            else
            {
                return _nextAvailableId++;
            }
        }

        inline static void FreeId(int id)
        {
            _freedIds.push(id);
        }

    private:
        IdManager() {};

        inline static std::stack<int> _freedIds;
        inline static int _nextAvailableId = wxID_HIGHEST + 1;
    };
}
