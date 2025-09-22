#pragma once

#include "Common.Base.h"
#include "Common.Strings.h"
#include "Exception.h"

#include "Api/DrawingTypes.h"
#include "DelayedValue.h"


namespace Alternet::UI
{
    constexpr auto DialogCancelGuid = "5DB20A10B5974CD4885CFCF346AF0F81";

    const Coord DegToRad = M_PI_Coord / 180;

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

    template<typename TFlagsEnum>
    class FlagsAccessor
    {
    public:
        FlagsAccessor(TFlagsEnum defaultValue) : _value(defaultValue)
        {
        }

        inline bool IsSet(TFlagsEnum flag)
        {
            return (_value & flag) != (TFlagsEnum)0;
        }

        inline void Set(TFlagsEnum flag, bool value)
        {
            if (value)
                _value |= flag;
            else
                _value &= ~flag;
        }

    private:
        TFlagsEnum _value;
    };

    class ScopeGuard
    {
        std::function<void()> onExit;
        BYREF_ONLY(ScopeGuard);
    public:
        ScopeGuard(std::function<void()> onEnter, std::function<void()> onExit_) : onExit(onExit_)
        {
            onEnter();
        }

        ScopeGuard(std::function<void()> onExit_) : ScopeGuard([] {}, onExit_)
        {
        }

        virtual ~ScopeGuard()
        {
            onExit();
        }
    };
}