#pragma once

namespace Alternet::UI
{
    namespace Errors
    {
        namespace { using str = const char16_t* const; }

        constexpr str UnexpectedNullPointer = u"Unexpected null pointer.";
        constexpr str InvalidObjectState = u"Invalid object state.";
        constexpr str UnexpectedEnumValue = u"Unexpected enum value.";
        constexpr str UnexpectedValue = u"Unexpected value.";
        constexpr str NotImplemented = u"Not implemented.";
        constexpr str NotSupported= u"Not supported.";
    }
}