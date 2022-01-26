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

            public static implicit operator Drawing.Int32Size(Int32Size v) => new Drawing.Int32Size(v.Width, v.Height);

            public static implicit operator Int32Size(Drawing.Int32Size v) => new Int32Size(v.Width, v.Height);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Size
        {
            public double Width, Height;

            public Size(double width, double height)
            {
                Width = width;
                Height = height;
            }

            public static implicit operator Drawing.Size(Size v) => new Drawing.Size(v.Width, v.Height);

            public static implicit operator Size(Drawing.Size v) => new Size(v.Width, v.Height);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Int32Point
        {
            public int X, Y;

            public Int32Point(int x, int y)
            {
                X = x;
                Y = y;
            }

            public static implicit operator Drawing.Int32Point(Int32Point v) => new Drawing.Int32Point(v.X, v.Y);

            public static implicit operator Int32Point(Drawing.Int32Point v) => new Int32Point(v.X, v.Y);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Point
        {
            public double X, Y;

            public Point(double x, double y)
            {
                X = x;
                Y = y;
            }

            public static implicit operator Drawing.Point(Point v) => new Drawing.Point(v.X, v.Y);

            public static implicit operator Point(Drawing.Point v) => new Point(v.X, v.Y);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Int32Rect
        {
            public int X, Y, Width, Height;

            public Int32Rect(int x, int y, int width, int height)
            {
                X = x;
                Y = y;
                Width = width;
                Height = height;
            }

            public static implicit operator Drawing.Int32Rect(Int32Rect v) => new Drawing.Int32Rect(v.X, v.Y, v.Width, v.Height);

            public static implicit operator Int32Rect(Drawing.Int32Rect v) => new Int32Rect(v.X, v.Y, v.Width, v.Height);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public double X, Y, Width, Height;

            public Rect(double x, double y, double width, double height)
            {
                X = x;
                Y = y;
                Width = width;
                Height = height;
            }

            public static implicit operator Drawing.Rect(Rect v) => new Drawing.Rect(v.X, v.Y, v.Width, v.Height);

            public static implicit operator Rect(Drawing.Rect v) => new Rect(v.X, v.Y, v.Width, v.Height);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Thickness
        {
            public double Left, Top, Right, Bottom;

            public Thickness(double left, double top, double right, double bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public static implicit operator UI.Thickness(Thickness v) => new UI.Thickness(v.Left, v.Top, v.Right, v.Bottom);

            public static implicit operator Thickness(UI.Thickness v) => new Thickness(v.Left, v.Top, v.Right, v.Bottom);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Color
        {
            public byte R, G, B, A;

            private readonly byte state;

            public bool IsEmpty => state == 0;

            public static readonly Color Empty = new Color();

            public Color(byte r, byte g, byte b, byte a)
            {
                R = r;
                G = g;
                B = b;
                A = a;
                state = 1;
            }

            public static implicit operator Drawing.Color(Color v) => v.IsEmpty ? Drawing.Color.Empty : Drawing.Color.FromArgb(v.A, v.R, v.G, v.B);

            public static implicit operator Color(Drawing.Color color) => color.IsEmpty ? Color.Empty : new Color(color.R, color.G, color.B, color.A);
        }
    }
}