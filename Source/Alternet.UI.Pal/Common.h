#pragma once

#include "Common.Base.h"
#include "Common.Strings.h"
#include "Exception.h"

#include "Api/DrawingTypes.h"
#include "DelayedValue.h"

namespace Alternet::UI
{
    namespace Collections
    {
        template <typename T> size_t IndexOf(const std::vector<T>& vector, const T& value)
        {
            auto it = std::find(vector.begin(), vector.end(), value);
            if (it == vector.end())
                return -1;

            return distance(vector.begin(), it);
        }
    }

    inline int ConvertToIntChecked(size_t value)
    {
        if (value > INT_MAX)
            throw std::overflow_error("size_t to int conversion overflow.");
        return static_cast<int>(value);
    }
}