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

        struct Thickness_C
        {
            float Left, Top, Right, Bottom;
        };

        struct Color_C
        {
            uint8_t R, G, B, A, state;
        };
    }

#pragma pack(pop)

    struct Size
    {
        int Width = 0, Height = 0;

        Size(const wxSize& s) : Width(s.x), Height(s.y)
        {
        }

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

        inline SizeF operator+(const SizeF& rhs) const { return SizeF(Width + rhs.Width, Height + rhs.Height); }
        inline SizeF& operator+=(const SizeF& rhs) 
        {
            Width += rhs.Width;
            Height += rhs.Height;
            return *this;
        }

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

        inline PointF operator+(const SizeF& value) const { return PointF(X + value.Width, Y + value.Height); }

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

        inline RectangleF Offset(const SizeF& value) const { return RectangleF(X + value.Width, Y + value.Height, Width, Height); }

        operator RectangleF_C() { return RectangleF_C{ X, Y, Width, Height }; }

        bool operator==(const RectangleF& rhs) { return X == rhs.X && Y == rhs.Y && Width == rhs.Width && Height == rhs.Height; }
        bool operator!=(const RectangleF& rhs) { return !(*this == rhs); }
    };

    struct Thickness
    {
        float Left = 0, Top = 0, Right = 0, Bottom = 0;

        Thickness() {}

        Thickness(float left, float top, float right, float bottom) : Left(left), Top(top), Right(right), Bottom(bottom) {}

        operator Thickness_C() { return Thickness_C{ Left, Top, Right, Bottom }; }

        bool operator==(const Thickness& rhs) { return Left == rhs.Left && Top == rhs.Top && Right == rhs.Right && Bottom == rhs.Bottom; }
        bool operator!=(const Thickness& rhs) { return !(*this == rhs); }
    };

    struct Color
    {
    public:
        uint8_t R, G, B, A;
    private:
        uint8_t state = 0;
    public:
        bool IsEmpty() const { return state == 0; }

        Color() : R(0), G(0), B(0), A(0), state(0)
        {
        }

        Color(const wxColor& c) : R(c.Red()), G(c.Green()), B(c.Blue()), A(c.Alpha()), state(1)
        {
        }

        operator Color_C() { return Color_C{ R, G, B, A, state }; }

        bool operator==(const Color& rhs) { return R == rhs.R && G == rhs.G && B == rhs.B && A == rhs.A && state == rhs.state; }
        bool operator!=(const struct Color& rhs) { return !(*this == rhs); }

        operator wxColor() const { return IsEmpty()? wxColor() : wxColor(R, G, B, A); }
    };

    class ParkingWindow
    {
    public:
        inline static bool IsCreated()
        {
            return s_parkingWindow != nullptr;
        }

        inline static void Destroy()
        {
            if (s_parkingWindow != nullptr)
            {
                s_parkingWindow->Destroy();
                s_parkingWindow = nullptr;
            }
        }

        inline static wxWindow* GetWindow()
        {
            if (s_parkingWindow == nullptr)
            {
                s_parkingWindow = new wxFrame();
                s_parkingWindow->Create(0, wxID_ANY, _T("AlterNET UI Parking Window"));
            }

            return s_parkingWindow;
        }

    private:
        inline static wxFrame* s_parkingWindow = nullptr;
    };

    inline double GetDPIScaleFactor(wxWindow* window)
    {
#if defined(__WXMSW__)
        return window->GetDPIScaleFactor();
#else
        return 1;
#endif
    }

    inline int fromDip(float value, wxWindow* window)
    {
        if (window == nullptr)
            window = ParkingWindow::GetWindow();

        return wxRound(value * GetDPIScaleFactor(window));
    }

    inline double fromDipF(float value, wxWindow* window)
    {
        if (window == nullptr)
            window = ParkingWindow::GetWindow();

        return value * GetDPIScaleFactor(window);
    }

    inline float toDip(int value, wxWindow* window)
    {
        if (window == nullptr)
            window = ParkingWindow::GetWindow();

        return value / GetDPIScaleFactor(window);
    }

    inline wxRect fromDip(const RectangleF& value, wxWindow* window)
    {
        if (window == nullptr)
            window = ParkingWindow::GetWindow();

        return wxRect(
            fromDip(value.X, window),
            fromDip(value.Y, window),
            fromDip(value.Width, window),
            fromDip(value.Height, window));
    };

    inline RectangleF fromDipF(const RectangleF& value, wxWindow* window)
    {
        if (window == nullptr)
            window = ParkingWindow::GetWindow();

        return RectangleF(
            fromDipF(value.X, window),
            fromDipF(value.Y, window),
            fromDipF(value.Width, window),
            fromDipF(value.Height, window));
    };

    inline RectangleF toDip(const wxRect& value, wxWindow* window)
    {
        if (window == nullptr)
            window = ParkingWindow::GetWindow();

        return RectangleF(
            toDip(value.x, window),
            toDip(value.y, window),
            toDip(value.width, window),
            toDip(value.height, window));
    };

    inline wxSize fromDip(const SizeF& value, wxWindow* window)
    {
        if (window == nullptr)
            window = ParkingWindow::GetWindow();

        return wxSize(
            fromDip(value.Width, window),
            fromDip(value.Height, window));
    };

    inline SizeF fromDipF(const SizeF& value, wxWindow* window)
    {
        if (window == nullptr)
            window = ParkingWindow::GetWindow();

        return SizeF(
            fromDipF(value.Width, window),
            fromDipF(value.Height, window));
    };

    inline SizeF toDip(const wxSize& value, wxWindow* window)
    {
        if (window == nullptr)
            window = ParkingWindow::GetWindow();

        return SizeF(
            toDip(value.x, window),
            toDip(value.y, window));
    };

    inline Thickness toDip(const Thickness& value, wxWindow* window)
    {
        if (window == nullptr)
            window = ParkingWindow::GetWindow();

        return Thickness(
            toDip(value.Left, window),
            toDip(value.Top, window),
            toDip(value.Right, window),
            toDip(value.Bottom, window));
    };

    inline wxPoint fromDip(const PointF& value, wxWindow* window)
    {
        if (window == nullptr)
            window = ParkingWindow::GetWindow();

        return wxPoint(
            fromDip(value.X, window),
            fromDip(value.Y, window));
    };

    inline PointF fromDipF(const PointF& value, wxWindow* window)
    {
        if (window == nullptr)
            window = ParkingWindow::GetWindow();

        return PointF(
            fromDipF(value.X, window),
            fromDipF(value.Y, window));
    };

    inline PointF toDip(const wxPoint& value, wxWindow* window)
    {
        if (window == nullptr)
            window = ParkingWindow::GetWindow();

        return PointF(
            toDip(value.x, window),
            toDip(value.y, window));
    };
}