#pragma once

#include <stdint.h>

namespace Alternet::UI
{
#pragma pack(push, 1)

    extern "C"
    {
        struct Size_C
        {
            int Width, Height;
        };

        struct SizeF_C
        {
            float Width, Height;
        };

        struct Point_C
        {
            int X, Y;
        };

        struct PointF_C
        {
            float X, Y;
        };

        struct Rectangle_C
        {
            int X, Y, Width, Height;
        };

        struct RectangleF_C
        {
            float X, Y, Width, Height;
        };

        struct Color_C
        {
            uint8_t R, G, B, A;
        };
    }

#pragma pack(pop)

    struct Size
    {
        int Width = 0, Height = 0;

        Size() {}

        Size(int width, int height) : Width(width), Height(height) {}

        operator Size_C() { return Size_C{ Width, Height }; }

        bool operator==(const Size& rhs) { return Width == rhs.Width && Height == rhs.Height; }
        bool operator!=(const Size& rhs) { return !(*this == rhs); }
    };

    struct SizeF
    {
        float Width = 0, Height = 0;

        SizeF() {}

        SizeF(float width, float height) : Width(width), Height(height) {}

        operator SizeF_C() { return SizeF_C{ Width, Height }; }

        bool operator==(const SizeF& rhs) { return Width == rhs.Width && Height == rhs.Height; }
        bool operator!=(const SizeF& rhs) { return !(*this == rhs); }
    };

    struct Point
    {
        int X = 0, Y = 0;

        Point() {}

        Point(int x, int y) : X(x), Y(y) {}

        operator Point_C() const { return Point_C{ X, Y }; }

        bool operator==(const Point& rhs) { return X == rhs.X && Y == rhs.Y; }
        bool operator!=(const Point& rhs) { return !(*this == rhs); }
    };

    struct PointF
    {
        float X = 0, Y = 0;

        PointF() {}

        PointF(float x, float y) : X(x), Y(y) {}

        operator PointF_C() const { return PointF_C{ X, Y }; }

        bool operator==(const PointF& rhs) { return X == rhs.X && Y == rhs.Y; }
        bool operator!=(const PointF& rhs) { return !(*this == rhs); }
    };

    struct Rectangle
    {
        int X = 0, Y = 0, Width = 0, Height = 0;

        Rectangle() {}

        Rectangle(int x, int y, int width, int height) : X(x), Y(y), Width(width), Height(height) {}

        Rectangle(const Point& location, const Size& size) : Rectangle(location.X, location.Y, size.Width, size.Height) {}

        inline Point GetLocation() const { return Point(X, Y); };
        inline Size GetSize() const { return Size(Width, Height); };

        operator Rectangle_C() { return Rectangle_C{ X, Y, Width, Height }; }

        bool operator==(const Rectangle& rhs) { return X == rhs.X && Y == rhs.Y && Width == rhs.Width && Height == rhs.Height; }
        bool operator!=(const Rectangle& rhs) { return !(*this == rhs); }
    };

    struct RectangleF
    {
        float X = 0, Y = 0, Width = 0, Height = 0;

        RectangleF() {}

        RectangleF(float x, float y, float width, float height) : X(x), Y(y), Width(width), Height(height) {}

        RectangleF(const PointF& location, const SizeF& size) : RectangleF(location.X, location.Y, size.Width, size.Height) {}

        inline PointF GetLocation() const { return PointF(X, Y); };
        inline SizeF GetSize() const { return SizeF(Width, Height); };

        operator RectangleF_C() { return RectangleF_C{ X, Y, Width, Height }; }

        bool operator==(const RectangleF& rhs) { return X == rhs.X && Y == rhs.Y && Width == rhs.Width && Height == rhs.Height; }
        bool operator!=(const RectangleF& rhs) { return !(*this == rhs); }
    };

    struct Color
    {
        uint8_t R = 0, G = 0, B = 0, A = 0;

        operator Color_C() { return Color_C{ R, G, B, A }; }

        bool operator==(const Color& rhs) { return R == rhs.R && G == rhs.G && B == rhs.B && A == rhs.A; }
        bool operator!=(const struct Color& rhs) { return !(*this == rhs); }

        operator wxColor() const { return wxColor(R, G, B, A); }
    };

    inline wxWindow* getParkingWindow()
    {
        static wxWindow* value = nullptr;

        if (value == nullptr)
        {
            value = new wxFrame();
            value->Hide();
        }

        return value;
    }

    inline int fromDip(float value, wxWindow* window)
    {
        if (window == nullptr)
            window = getParkingWindow();

        return wxRound(value * window->GetDPIScaleFactor());
    }

    inline float toDip(int value, wxWindow* window)
    {
        if (window == nullptr)
            window = getParkingWindow();

        return value / window->GetDPIScaleFactor();
    }

    inline wxRect fromDip(const RectangleF& value, wxWindow* window)
    {
        if (window == nullptr)
            window = getParkingWindow();

        return wxRect(
            fromDip(value.X, window),
            fromDip(value.Y, window),
            fromDip(value.Width, window),
            fromDip(value.Height, window));
    };

    inline RectangleF toDip(const wxRect& value, wxWindow* window)
    {
        if (window == nullptr)
            window = getParkingWindow();

        return RectangleF(
            toDip(value.x, window),
            toDip(value.y, window),
            toDip(value.width, window),
            toDip(value.height, window));
    };

    inline wxSize fromDip(const SizeF& value, wxWindow* window)
    {
        if (window == nullptr)
            window = getParkingWindow();

        return wxSize(
            fromDip(value.Width, window),
            fromDip(value.Height, window));
    };

    inline SizeF toDip(const wxSize& value, wxWindow* window)
    {
        if (window == nullptr)
            window = getParkingWindow();

        return SizeF(
            toDip(value.x, window),
            toDip(value.y, window));
    };

    inline wxPoint fromDip(const PointF& value, wxWindow* window)
    {
        if (window == nullptr)
            window = getParkingWindow();

        return wxPoint(
            fromDip(value.X, window),
            fromDip(value.Y, window));
    };

    inline PointF toDip(const wxPoint& value, wxWindow* window)
    {
        if (window == nullptr)
            window = getParkingWindow();

        return PointF(
            toDip(value.x, window),
            toDip(value.y, window));
    };
}