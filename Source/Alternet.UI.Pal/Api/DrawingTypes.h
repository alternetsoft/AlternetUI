#pragma once

#include <stdint.h>
#include <wx/display.h>

namespace Alternet::UI
{
#pragma pack(push, 1)

	extern "C"
	{
		struct SizeI_C
		{
			int Width = 0, Height = 0;
		};

		struct SizeD_C
		{
			double Width = 0, Height = 0;
		};

		struct PointI_C
		{
			int X = 0, Y = 0;
		};

		struct PointD_C
		{
			double X = 0, Y = 0;
		};

		struct RectI_C
		{
			int X = 0, Y = 0, Width = 0, Height = 0;
		};

		struct RectD_C
		{
			double X = 0, Y = 0, Width = 0, Height = 0;
		};

		struct Thickness_C
		{
			double Left = 0, Top = 0, Right = 0, Bottom = 0;
		};

		struct Color_C
		{
			uint8_t R = 0, G = 0, B = 0, A = 0, state = 0;
		};

		struct DateTime_C
		{
			int Year = 0, Month = 0, Day = 0, Hour = 0, Minute = 0, Second = 0, Millisecond = 0;
		};
	}

	struct Int32Size : SizeI_C
	{
		Int32Size()
		{
		}

		Int32Size(const wxSize& s)
		{
			Width = s.x;
			Height = s.y;
		}

		Int32Size(int width, int height)
		{
			Width = width;
			Height = height;
		}

		/*inline operator SizeI_C() { return SizeI_C{Width, Height}; }*/

		inline operator wxSize() const { return wxSize(Width, Height); }

		inline bool operator==(const Int32Size& rhs) { return Width == rhs.Width && Height == rhs.Height; }
		inline bool operator!=(const Int32Size& rhs) { return !(*this == rhs); }

		std::string ToString() const
		{
			std::string w = std::to_string(Width);
			std::string h = std::to_string(Height);

			std::string prefix("(");
			std::string suffix(")");
			std::string comma(", ");

			std::string result = prefix + w + comma + h + suffix;

			return result;
		}
	};

#define SizeI Int32Size

	struct Size	: SizeD_C
	{
		Size() {}

		Size(double width, double height)
		{
			Width = width;
			Height = height;
		}

		Size(wxSize size)
		{
			Width = size.x;
			Height = size.y;
		}

		inline Size operator+(const Size& rhs) const {
			return Size(Width + rhs.Width, Height + rhs.Height);
		}
		inline Size& operator+=(const Size& rhs)
		{
			Width += rhs.Width;
			Height += rhs.Height;
			return *this;
		}

		/*inline operator SizeD_C() { return SizeD_C{Width, Height}; }*/

		inline bool operator==(const Size& rhs) { return Width == rhs.Width && Height == rhs.Height; }
		inline bool operator!=(const Size& rhs) { return !(*this == rhs); }

	public:
		std::string ToString() const
		{
			std::string w = std::to_string(Width);
			std::string h = std::to_string(Height);

			std::string prefix("(");
			std::string suffix(")");
			std::string comma(", ");

			std::string result = prefix + w + comma + h + suffix;

			return result;
		}
	};

#define SizeD Size

	struct Int32Point : PointI_C
	{
		Int32Point() {}

		Int32Point(int x, int y)
		{
			X = x;
			Y = y;
		}

		Int32Point(wxPoint p)
		{
			X = p.x;
			Y = p.y;
		}

		/*inline operator PointI_C() const { return PointI_C{X, Y}; }*/

		inline operator wxPoint() const { return wxPoint{ X, Y }; }

		inline bool operator==(const Int32Point& rhs) { return X == rhs.X && Y == rhs.Y; }
		inline bool operator!=(const Int32Point& rhs) { return !(*this == rhs); }

		std::string ToString() const
		{
			std::string x = std::to_string(X);
			std::string y = std::to_string(Y);

			std::string prefix("(");
			std::string suffix(")");
			std::string comma(", ");

			std::string result = prefix + x + comma + y + suffix;

			return result;
		}
	};

#define PointI Int32Point

	struct Point : PointD_C
	{
		Point() {}

		Point(double x, double y)
		{
			X = x;
			Y = y;
		}

		/*inline operator PointD_C() const { return PointD_C{X, Y}; }*/

		inline Point operator+(const Size& value) const {
			return Point(X + value.Width, Y + value.Height);
		}
		inline Point operator-(const Size& value) const {
			return Point(X - value.Width, Y - value.Height);
		}

		inline bool operator==(const Point& rhs) { return X == rhs.X && Y == rhs.Y; }
		inline bool operator!=(const Point& rhs) { return !(*this == rhs); }

		std::string ToString() const
		{
			std::string x = std::to_string(X);
			std::string y = std::to_string(Y);

			std::string prefix("(");
			std::string suffix(")");
			std::string comma(", ");

			std::string result = prefix + x + comma + y + suffix;

			return result;
		}
	};

#define PointD Point

	struct Int32Rect : RectI_C
	{
		Int32Rect() {}

		Int32Rect(wxRect r)
		{
			X = r.x;
			Y = r.y;
			Width = r.width;
			Height = r.height;
		}

		Int32Rect(int x, int y, int width, int height)
		{
			X = x;
			Y = y;
			Width = width;
			Height = height;
		}

		Int32Rect(const Int32Point& location, const Int32Size& size) :
			Int32Rect(location.X, location.Y, size.Width, size.Height) {}

		inline Int32Point GetLocation() const { return Int32Point(X, Y); };
		inline Int32Size GetSize() const { return Int32Size(Width, Height); };

		inline bool IsZero() const
		{
			return (Width == 0) && (Height == 0) && (X == 0) && (Y == 0);
		}

		inline operator wxRect() const { return wxRect{ X, Y, Width, Height }; }

		/*inline operator RectI_C() const { return RectI_C{X, Y, Width, Height}; }*/

		inline bool operator==(const Int32Rect& rhs)
		{
			return X == rhs.X && Y == rhs.Y && Width == rhs.Width && Height == rhs.Height;
		}

		inline bool operator!=(const Int32Rect& rhs) { return !(*this == rhs); }

		std::string ToString() const
		{
			std::string x = std::to_string(X);
			std::string y = std::to_string(Y);
			std::string width = std::to_string(Width);
			std::string height = std::to_string(Height);

			std::string prefix("(");
			std::string suffix(")");
			std::string comma(", ");

			std::string result = prefix + x + comma + y + comma + width + comma + height + suffix;

			return result;
		}
	};

#define RectI Int32Rect

	struct Rect : RectD_C
	{
		Rect() {}

		Rect(double x, double y, double width, double height)
		{
			X = x;
			Y = y;
			Width = width;
			Height = height;
		}

		Rect(wxRect rect)			
		{
			X = rect.x;
			Y = rect.y;
			Width = rect.width;
			Height = rect.height;
		}

		Rect(const Point& location, const Size& size)
			: Rect(location.X, location.Y, size.Width, size.Height) {}

		bool IsEmpty() const
		{
			return X == 0 && Y == 0 && Width == 0 && Height == 0;
		}

		inline Point GetLocation() const { return Point(X, Y); };
		inline Size GetSize() const { return Size(Width, Height); };

		inline Rect Offset(const Size& value) const
		{
			return Rect(X + value.Width, Y + value.Height, Width, Height);
		}

		/*inline operator RectD_C() { return RectD_C{X, Y, Width, Height}; }*/

		inline bool operator==(const Rect& rhs)
		{
			return X == rhs.X && Y == rhs.Y && Width == rhs.Width && Height == rhs.Height;
		}

		inline bool operator!=(const Rect& rhs) { return !(*this == rhs); }

		std::string ToString() const
		{
			std::string x = std::to_string(X);
			std::string y = std::to_string(Y);
			std::string width = std::to_string(Width);
			std::string height = std::to_string(Height);

			std::string prefix("(");
			std::string suffix(")");
			std::string comma(", ");

			std::string result = prefix + x + comma + y + comma + width + comma + height + suffix;

			return result;
		}
	};

#define RectD Rect

	struct Thickness : Thickness_C
	{
		Thickness() {}

		Thickness(double left, double top, double right, double bottom)
		{
			Left = left;
			Top = top;
			Right = right;
			Bottom = bottom;
		}

		/*inline operator Thickness_C() { return Thickness_C{Left, Top, Right, Bottom}; }*/

		inline bool operator==(const Thickness& rhs) {
			return Left == rhs.Left
				&& Top == rhs.Top && Right == rhs.Right && Bottom == rhs.Bottom;
		}
		inline bool operator!=(const Thickness& rhs) { return !(*this == rhs); }

		std::string ToString() const
		{
			std::string v1 = std::to_string(Left);
			std::string v2 = std::to_string(Top);
			std::string v3 = std::to_string(Right);
			std::string v4 = std::to_string(Bottom);

			std::string prefix("(");
			std::string suffix(")");
			std::string comma(", ");

			std::string result = prefix + v1 + comma + v2 + comma + v3 + comma + v4 + suffix;

			return result;
		}
	};

	struct Color : Color_C
	{
	public:
		inline bool IsEmpty() const { return state == 0; }

		bool IsBlack() const
		{
			return R == 0 && G == 0 && B == 0 && A == 255;
		}

		Color(uint8_t alpha, uint8_t red, uint8_t green, uint8_t blue)			
		{
			R = red;
			G = green;
			B = blue;
			A = alpha;
			state = 1;
		}

		Color()
		{
			R = 0;
			G = 0;
			B = 0;
			A = 0;
			state = 0;
		}

		Color(const wxColor& c)
		{
			if (c.IsOk())
			{
				R = c.Red();
				G = c.Green();
				B = c.Blue();
				A = c.Alpha();
				state = 1;
			}
			else
			{
				R = 0;
				G = 0;
				B = 0;
				A = 0;
				state = 0;
			}
		}

		/*inline operator Color_C() { return Color_C{R, G, B, A, state}; }*/

		bool operator==(const Color& rhs)
		{
			return R == rhs.R && G == rhs.G && B == rhs.B && A == rhs.A &&
				state == rhs.state;
		}

		inline bool operator!=(const struct Color& rhs) { return !(*this == rhs); }

		operator wxColor() const
		{
			if (IsEmpty())
				return wxColor();
			else
				return wxColor(R, G, B, A);
		}
	};

	struct DateTime : DateTime_C
	{
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

		void Reset()
		{
			Year = 0;
			Month = 0;
			Day = 0;
			Hour = 0;
			Minute = 0;
			Second = 0;
			Millisecond = 0;
		}

		/*operator DateTime_C() {
			return DateTime_C{ Year, Month, // was (uint8_t)(Month + 1)
				Day, Hour, Minute, Second, Millisecond };
		}*/

		bool operator==(const DateTime& rhs) {
			return Hour == rhs.Hour && Minute == rhs.Minute && Second == rhs.Second
				&& Millisecond == rhs.Millisecond && Year == rhs.Year &&
				Month == rhs.Month && Day == rhs.Day;
		}
		inline bool operator!=(const DateTime& rhs) { return !(*this == rhs); }

		inline operator wxDateTime() const {

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
				Year, Hour, Minute, Second, Millisecond);
		}
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

	/*
		static wxSize GetDPIHelper(const wxWindowBase* w)
		{
			wxSize dpi;

			if (w)
				dpi = w->GetDPI();
			if (!dpi.x || !dpi.y)
				dpi = wxScreenDC().GetPPI();
			if (!dpi.x || !dpi.y)
				dpi = wxDisplay::GetStdPPI();

			return dpi;
		}
	*/

	inline double GetDPIScaleFactor(wxWindow* window)
	{
#if defined(__WXMSW__)
		if (window == nullptr)
			window = ParkingWindow::GetWindow();

		return window->GetDPIScaleFactor();
#else
		return 1;
#endif
	}

	inline int fromDip(double value, wxWindow* window)
	{
		return wxRound(value * GetDPIScaleFactor(window));
	}

	inline double fromDipF(double value, wxWindow* window)
	{
		return value * GetDPIScaleFactor(window);
	}

	inline double toDipF(double value, wxWindow* window)
	{
		return value / GetDPIScaleFactor(window);
	}

	inline double toDip(int value, wxWindow* window)
	{
		return value / GetDPIScaleFactor(window);
	}

	inline wxRect fromDip(const Rect& value, wxWindow* window)
	{
		return wxRect(
			fromDip(value.X, window),
			fromDip(value.Y, window),
			fromDip(value.Width, window),
			fromDip(value.Height, window));
	};

	inline Rect fromDipF(const Rect& value, wxWindow* window)
	{
		return Rect(
			fromDipF(value.X, window),
			fromDipF(value.Y, window),
			fromDipF(value.Width, window),
			fromDipF(value.Height, window));
	};

	inline Rect toDip(const wxRect& value, wxWindow* window)
	{
		return Rect(
			toDip(value.x, window),
			toDip(value.y, window),
			toDip(value.width, window),
			toDip(value.height, window));
	};

	inline wxSize fromDip(const Size& value, wxWindow* window)
	{
		return wxSize(
			fromDip(value.Width, window),
			fromDip(value.Height, window));
	};

	inline Size fromDipF(const Size& value, wxWindow* window)
	{
		return Size(
			fromDipF(value.Width, window),
			fromDipF(value.Height, window));
	};

	inline Size toDip(const wxSize& value, wxWindow* window)
	{
		return Size(
			toDip(value.x, window),
			toDip(value.y, window));
	};

	inline Thickness toDip(const Thickness& value, wxWindow* window)
	{
		return Thickness(
			toDip(value.Left, window),
			toDip(value.Top, window),
			toDip(value.Right, window),
			toDip(value.Bottom, window));
	};

	inline wxPoint fromDip(const Point& value, wxWindow* window)
	{
		return wxPoint(
			fromDip(value.X, window),
			fromDip(value.Y, window));
	};

	inline Point fromDipF(const Point& value, wxWindow* window)
	{
		return Point(
			fromDipF(value.X, window),
			fromDipF(value.Y, window));
	};

	inline Point toDip(const wxPoint& value, wxWindow* window)
	{
		return Point(
			toDip(value.x, window),
			toDip(value.y, window));
	};

	inline Point toDip(const wxPoint2DDouble& value, wxWindow* window)
	{
		return Point(
			toDip(value.m_x, window),
			toDip(value.m_y, window));
	};

	enum KnownSystemColor
	{
		// 0 - reserved for "not a known color"

		KnownSystemColor_ActiveBorder = 1,
		KnownSystemColor_ActiveCaption = 2,
		KnownSystemColor_ActiveCaptionText = 3,
		KnownSystemColor_AppWorkspace = 4,
		KnownSystemColor_Control = 5,
		KnownSystemColor_ControlDark = 6,
		KnownSystemColor_ControlDarkDark = 7,
		KnownSystemColor_ControlLight = 8,
		KnownSystemColor_ControlLightLight = 9,
		KnownSystemColor_ControlText = 10,
		KnownSystemColor_Desktop = 11,
		KnownSystemColor_GrayText = 12,
		KnownSystemColor_Highlight = 13,
		KnownSystemColor_HighlightText = 14,
		KnownSystemColor_HotTrack = 15,
		KnownSystemColor_InactiveBorder = 16,
		KnownSystemColor_InactiveCaption = 17,
		KnownSystemColor_InactiveCaptionText = 18,
		KnownSystemColor_Info = 19,
		KnownSystemColor_InfoText = 20,
		KnownSystemColor_Menu = 21,
		KnownSystemColor_MenuText = 22,
		KnownSystemColor_ScrollBar = 23,
		KnownSystemColor_Window = 24,
		KnownSystemColor_WindowFrame = 25,
		KnownSystemColor_WindowText = 26,
		KnownSystemColor_Transparent = 27,

		KnownSystemColor_ButtonFace = 168,
		KnownSystemColor_ButtonHighlight = 169,
		KnownSystemColor_ButtonShadow = 170,
		KnownSystemColor_GradientActiveCaption = 171,
		KnownSystemColor_GradientInactiveCaption = 172,
		KnownSystemColor_MenuBar = 173,
		KnownSystemColor_MenuHighlight = 174,
	};

#pragma pack(pop)
}