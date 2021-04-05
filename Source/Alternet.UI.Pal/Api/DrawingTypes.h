#pragma once

#include <stdint.h>

namespace Alternet::UI
{
#pragma pack(push, 1)
    extern "C"
    {
        struct Size
        {
            int Width, Height;
        };

        struct SizeF
        {
            float Width, Height;
        };

        struct Point
        {
            int X, Y;
        };

        struct PointF
        {
            float X, Y;
        };

        struct Rectangle
        {
            int X, Y, Width, Height;
        };

        struct RectangleF
        {
            float X, Y, Width, Height;
        };

        struct Color
        {
            uint8_t R, G, B, A;
        };
    }
#pragma pack(pop)
}