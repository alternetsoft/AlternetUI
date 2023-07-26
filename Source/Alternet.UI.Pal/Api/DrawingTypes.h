#pragma once

#include <stdint.h>

namespace Alternet::UI
{
#pragma pack(push, 1)

    extern "C"
    {
        struct Int32Size_C
        {
            int Width, Height;
        };

        struct Size_C
        {
            double Width, Height;
        };

        struct Int32Point_C
        {
            int X, Y;
        };

        struct Point_C
        {
            double X, Y;
        };

        struct Int32Rect_C
        {
            int X, Y, Width, Height;
        };

        struct Rect_C
        {
            double X, Y, Width, Height;
        };

        struct Thickness_C
        {
            double Left, Top, Right, Bottom;
        };

        struct Color_C
        {
            uint8_t R, G, B, A, state;
        };

        struct DateTime_C
        {
            int Year, Month, Day, H, M, S, MS;
        };
    }

#pragma pack(pop)

    struct Int32Size
    {
        int Width = 0, Height = 0;

        Int32Size(const wxSize& s) : Width(s.x), Height(s.y)
        {
        }

        Int32Size(int width, int height) : Width(width), Height(height) {}

        operator Int32Size_C() { return Int32Size_C{ Width, Height }; }

        bool operator==(const Int32Size& rhs) { return Width == rhs.Width && Height == rhs.Height; }
        bool operator!=(const Int32Size& rhs) { return !(*this == rhs); }
    };

    struct Size
    {
        double Width = 0, Height = 0;

        Size() {}

        Size(double width, double height) : Width(width), Height(height) {}

        inline Size operator+(const Size& rhs) const { return Size(Width + rhs.Width, Height + rhs.Height); }
        inline Size& operator+=(const Size& rhs) 
        {
            Width += rhs.Width;
            Height += rhs.Height;
            return *this;
        }

        operator Size_C() { return Size_C{ Width, Height }; }

        bool operator==(const Size& rhs) { return Width == rhs.Width && Height == rhs.Height; }
        bool operator!=(const Size& rhs) { return !(*this == rhs); }
    };

    struct Int32Point
    {
        int X = 0, Y = 0;

        Int32Point() {}

        Int32Point(int x, int y) : X(x), Y(y) {}

        operator Int32Point_C() const { return Int32Point_C{ X, Y }; }

        bool operator==(const Int32Point& rhs) { return X == rhs.X && Y == rhs.Y; }
        bool operator!=(const Int32Point& rhs) { return !(*this == rhs); }
    };

    struct Point
    {
        double X = 0, Y = 0;

        Point() {}

        Point(double x, double y) : X(x), Y(y) {}

        operator Point_C() const { return Point_C{ X, Y }; }

        inline Point operator+(const Size& value) const { return Point(X + value.Width, Y + value.Height); }
        inline Point operator-(const Size& value) const { return Point(X - value.Width, Y - value.Height); }

        bool operator==(const Point& rhs) { return X == rhs.X && Y == rhs.Y; }
        bool operator!=(const Point& rhs) { return !(*this == rhs); }
    };

    struct Int32Rect
    {
        int X = 0, Y = 0, Width = 0, Height = 0;

        Int32Rect() {}

        Int32Rect(int x, int y, int width, int height) : X(x), Y(y), Width(width), Height(height) {}

        Int32Rect(const Int32Point& location, const Int32Size& size) : Int32Rect(location.X, location.Y, size.Width, size.Height) {}

        inline Int32Point GetLocation() const { return Int32Point(X, Y); };
        inline Int32Size GetSize() const { return Int32Size(Width, Height); };

        operator Int32Rect_C() { return Int32Rect_C{ X, Y, Width, Height }; }

        bool operator==(const Int32Rect& rhs) { return X == rhs.X && Y == rhs.Y && Width == rhs.Width && Height == rhs.Height; }
        bool operator!=(const Int32Rect& rhs) { return !(*this == rhs); }
    };

    struct Rect
    {
        double X = 0, Y = 0, Width = 0, Height = 0;

        Rect() {}

        Rect(double x, double y, double width, double height) : X(x), Y(y), Width(width), Height(height) {}

        Rect(const Point& location, const Size& size) : Rect(location.X, location.Y, size.Width, size.Height) {}

        inline Point GetLocation() const { return Point(X, Y); };
        inline Size GetSize() const { return Size(Width, Height); };

        inline Rect Offset(const Size& value) const { return Rect(X + value.Width, Y + value.Height, Width, Height); }

        operator Rect_C() { return Rect_C{ X, Y, Width, Height }; }

        bool operator==(const Rect& rhs) { return X == rhs.X && Y == rhs.Y && Width == rhs.Width && Height == rhs.Height; }
        bool operator!=(const Rect& rhs) { return !(*this == rhs); }
    };

    struct Thickness
    {
        double Left = 0, Top = 0, Right = 0, Bottom = 0;

        Thickness() {}

        Thickness(double left, double top, double right, double bottom) : Left(left), Top(top), Right(right), Bottom(bottom) {}

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

    struct DateTime
    {
        int Year = 0, Month = 0, Day = 0;
        int Hour = 0, Minute = 0, Second = 0, Millisecond = 0;

        DateTime()
            :DateTime(wxDateTime::Now())
        {
        }

        DateTime(const wxDateTime& c)
        {
            wxDateTime::Tm cc = c.GetTm();

            Year = cc.year; // tm_year field is the offset from 1900
            Month = cc.mon + 1; // mon should be < 12
            Day = cc.mday; // mday - Day of the month in 1..31 range
            Hour = cc.hour;
            Minute = cc.min;
            Second = cc.sec;
            Millisecond = cc.msec;
        }

        operator DateTime_C() { 
            return DateTime_C{ Year, Month, // was (uint8_t)(Month + 1)
                Day, Hour, Minute, Second, Millisecond }; }

        bool operator==(const DateTime& rhs) { 
            return Hour == rhs.Hour && Minute == rhs.Minute && Second == rhs.Second 
                && Millisecond == rhs.Millisecond && Year == rhs.Year && 
                Month == rhs.Month && Day == rhs.Day; }
        bool operator!=(const DateTime& rhs) { return !(*this == rhs); }

        operator wxDateTime() const { 

            /*
            inline wxDateTime::wxDateTime(wxDateTime_t day,
                Month        month,
                int          year,
                wxDateTime_t hour,
                wxDateTime_t minute,
                wxDateTime_t second,
                wxDateTime_t millisec)
            */

            return wxDateTime(Day, (wxDateTime::Month)(Month - 1), 
                Year, Hour, Minute, Second, Millisecond); }
        };

    class wxFrame2 : public wxFrame
    {
    public:
        wxFrame2()
        {
        }

        wxFrame2(wxWindow* parent,
            wxWindowID id,
            const wxString& title,
            const wxPoint& pos = wxDefaultPosition,
            const wxSize& size = wxDefaultSize,
            long style = wxDEFAULT_FRAME_STYLE,
            const wxString& name = wxASCII_STR(wxFrameNameStr))
        {
            Create(parent, id, title, pos, size, style, name);
        }
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
                s_parkingWindow->Unbind(wxEVT_CLOSE_WINDOW, &ParkingWindow::OnClose);
                s_parkingWindow->Destroy();
                s_parkingWindow = nullptr;
            }
        }
       
        inline static wxWindow* GetWindow()
        {
            if (s_parkingWindow == nullptr)
            {
                s_parkingWindow = new wxFrame();
                s_parkingWindow->Create(0, wxID_ANY, 
                    _T("AlterNET UI Parking Window"));
                s_parkingWindow->Bind(wxEVT_CLOSE_WINDOW, &ParkingWindow::OnClose);
                s_parkingWindow->Bind(wxEVT_IDLE, &ParkingWindow::OnIdle);
            }

            return s_parkingWindow;
        }

        inline static void SetIdleCallback(std::function<void()> idleCallback)
        {
            _idleCallback = idleCallback;
        }

    private:
        inline static wxFrame* s_parkingWindow = nullptr;

        inline static std::function<void()> _idleCallback;

        static void OnClose(wxCloseEvent& event)
        {
            event.Veto();
        }

        static void OnIdle(wxIdleEvent& event)
        {
            event.Skip();
            
            if (_idleCallback)
                _idleCallback();
        }
    };

    inline double GetDPIScaleFactor(wxWindow* window)
    {
#if defined(__WXMSW__)
        return window->GetDPIScaleFactor();
#else
        return 1;
#endif
    }

    inline int fromDip(double value, wxWindow* window)
    {
        if (window == nullptr)
            window = ParkingWindow::GetWindow();

        return wxRound(value * GetDPIScaleFactor(window));
    }

    inline double fromDipF(double value, wxWindow* window)
    {
        if (window == nullptr)
            window = ParkingWindow::GetWindow();

        return value * GetDPIScaleFactor(window);
    }

    inline double toDip(int value, wxWindow* window)
    {
        if (window == nullptr)
            window = ParkingWindow::GetWindow();

        return value / GetDPIScaleFactor(window);
    }

    inline wxRect fromDip(const Rect& value, wxWindow* window)
    {
        if (window == nullptr)
            window = ParkingWindow::GetWindow();

        return wxRect(
            fromDip(value.X, window),
            fromDip(value.Y, window),
            fromDip(value.Width, window),
            fromDip(value.Height, window));
    };

    inline Rect fromDipF(const Rect& value, wxWindow* window)
    {
        if (window == nullptr)
            window = ParkingWindow::GetWindow();

        return Rect(
            fromDipF(value.X, window),
            fromDipF(value.Y, window),
            fromDipF(value.Width, window),
            fromDipF(value.Height, window));
    };

    inline Rect toDip(const wxRect& value, wxWindow* window)
    {
        if (window == nullptr)
            window = ParkingWindow::GetWindow();

        return Rect(
            toDip(value.x, window),
            toDip(value.y, window),
            toDip(value.width, window),
            toDip(value.height, window));
    };

    inline wxSize fromDip(const Size& value, wxWindow* window)
    {
        if (window == nullptr)
            window = ParkingWindow::GetWindow();

        return wxSize(
            fromDip(value.Width, window),
            fromDip(value.Height, window));
    };

    inline Size fromDipF(const Size& value, wxWindow* window)
    {
        if (window == nullptr)
            window = ParkingWindow::GetWindow();

        return Size(
            fromDipF(value.Width, window),
            fromDipF(value.Height, window));
    };

    inline Size toDip(const wxSize& value, wxWindow* window)
    {
        if (window == nullptr)
            window = ParkingWindow::GetWindow();

        return Size(
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

    inline wxPoint fromDip(const Point& value, wxWindow* window)
    {
        if (window == nullptr)
            window = ParkingWindow::GetWindow();

        return wxPoint(
            fromDip(value.X, window),
            fromDip(value.Y, window));
    };

    inline Point fromDipF(const Point& value, wxWindow* window)
    {
        if (window == nullptr)
            window = ParkingWindow::GetWindow();

        return Point(
            fromDipF(value.X, window),
            fromDipF(value.Y, window));
    };

    inline Point toDip(const wxPoint& value, wxWindow* window)
    {
        if (window == nullptr)
            window = ParkingWindow::GetWindow();

        return Point(
            toDip(value.x, window),
            toDip(value.y, window));
    };

    inline Point toDip(const wxPoint2DDouble& value, wxWindow* window)
    {
        if (window == nullptr)
            window = ParkingWindow::GetWindow();

        return Point(
            toDip(value.m_x, window),
            toDip(value.m_y, window));
    };  
}