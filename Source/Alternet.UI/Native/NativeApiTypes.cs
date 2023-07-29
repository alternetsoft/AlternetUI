using System.Runtime.InteropServices;
using System.Security;

namespace Alternet.UI.Native
{
    [SuppressUnmanagedCodeSecurity]
    internal static class NativeApiTypes
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct Int32Size
        {
            public int Width, Height;

            public Int32Size(int width, int height)
            {
                Width = width;
                Height = height;
            }

            public static implicit operator Alternet.Drawing.Int32Size(Int32Size v)
                => new(v.Width, v.Height);

            public static implicit operator Int32Size(Alternet.Drawing.Int32Size v)
                => new(v.Width, v.Height);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Size
        {
            public double Width;
            public double Height;

            public Size(double width, double height)
            {
                Width = width;
                Height = height;
            }

            public static implicit operator
                Alternet.Drawing.Size(Size v) => new (v.Width, v.Height);

            public static implicit operator Size(Alternet.Drawing.Size v) =>
                new(v.Width, v.Height);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Int32Point
        {
            public int X;
            public int Y;

            public Int32Point(int x, int y)
            {
                X = x;
                Y = y;
            }

            public static implicit operator
                Alternet.Drawing.Int32Point(Int32Point v) => new(v.X, v.Y);

            public static implicit operator
                Int32Point(Alternet.Drawing.Int32Point v) => new(v.X, v.Y);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Point
        {
            public double X;
            public double Y;

            public Point(double x, double y)
            {
                X = x;
                Y = y;
            }

            public static implicit operator Alternet.Drawing.Point(Point v) =>
                new(v.X, v.Y);

            public static implicit operator Point(Alternet.Drawing.Point v)
                => new(v.X, v.Y);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Int32Rect
        {
            public int X;
            public int Y;
            public int Width;
            public int Height;

            public Int32Rect(int x, int y, int width, int height)
            {
                X = x;
                Y = y;
                Width = width;
                Height = height;
            }

            public static implicit operator
                Alternet.Drawing.Int32Rect(Int32Rect v) =>
                    new(v.X, v.Y, v.Width, v.Height);

            public static implicit operator Int32Rect(Alternet.Drawing.Int32Rect v)
                => new(v.X, v.Y, v.Width, v.Height);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public double X;
            public double Y;
            public double Width;
            public double Height;

            public Rect(double x, double y, double width, double height)
            {
                X = x;
                Y = y;
                Width = width;
                Height = height;
            }

            public static implicit operator Alternet.Drawing.Rect(Rect v) =>
                new(v.X, v.Y, v.Width, v.Height);

            public static implicit operator Rect(Alternet.Drawing.Rect v) =>
                new(v.X, v.Y, v.Width, v.Height);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Thickness
        {
            public double Left;
            public double Top;
            public double Right;
            public double Bottom;

            public Thickness(double left, double top, double right, double bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public static implicit operator UI.Thickness(Thickness v) =>
                new(v.Left, v.Top, v.Right, v.Bottom);

            public static implicit operator Thickness(UI.Thickness v) =>
                new(v.Left, v.Top, v.Right, v.Bottom);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Color
        {
            public byte R;
            public byte G;
            public byte B;
            public byte A;

            private readonly byte state;

            public readonly bool IsEmpty => state == 0;

            public static readonly Color Empty = new();

            public Color(byte r, byte g, byte b, byte a)
            {
                R = r;
                G = g;
                B = b;
                A = a;
                state = 1;
            }

            public static implicit operator
                Alternet.Drawing.Color(Color v) =>
                v.IsEmpty ? Alternet.Drawing.Color.Empty :
                Alternet.Drawing.Color.FromArgb(v.A, v.R, v.G, v.B);

            public static implicit operator Color(Alternet.Drawing.Color color) =>
                color.IsEmpty ? Color.Empty :
                new Color(color.R, color.G, color.B, color.A);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DateTime
        {
            public int Year; /* 1-9999 */
            public int Month; /* 1-12 */
            public int Day; /* 1-31 */
            public int Hour; /* 0-23 */
            public int Minute; /* 0-59 */
            public int Second; /* 0-59 */
            public int Millisecond; /* 0-999 */

            public DateTime(
                int year,
                int month,
                int day,
                int hour,
                int minute,
                int second,
                int millisecond)
            {
                Year = year;
                Month = month;
                Day = day;
                Hour = hour;
                Minute = minute;
                Second = second;
                Millisecond = millisecond;
            }

            public static implicit operator System.DateTime(DateTime v) =>
                new(v.Year, v.Month, v.Day, v.Hour, v.Minute,
                v.Second, v.Millisecond);

            public static implicit operator DateTime(System.DateTime v) =>
                new(v.Year, v.Month, v.Day, v.Hour, v.Minute, v.Second,
                    v.Millisecond);
        }
    }
}