#pragma once

#ifdef __WXOSX_COCOA__
#include "OptionalPolyfill.h"
#else
#include <optional>
#endif

// macOS 10.13 doesnt have libc++ support for std::optional, we have to use a polyfill on mac.

namespace Alternet::UI
{
#ifdef __WXOSX_COCOA__
template< typename T >
using optional = nonstd::optional<T>;

using nullopt_t = nonstd::nullopt_t;
inline auto& nullopt = nonstd::nullopt;
#else
template< typename T >
using optional = std::optional<T>;

using nullopt_t = std::nullopt_t;
inline auto& nullopt = std::nullopt;
#endif
}