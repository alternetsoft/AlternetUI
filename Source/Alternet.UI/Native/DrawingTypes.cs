using System.Runtime.InteropServices;
using System.Security;

namespace Alternet.UI.Native
{
    [SuppressUnmanagedCodeSecurity]
    internal static class NativeApiTypes
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct Size
        {
            public int Width, Height;

            public Size(int width, int height)
            {
                Width = width;
                Height = height;
            }

            public static implicit operator Drawing.Size(Size v) => new Drawing.Size(v.Width, v.Height);

            public static implicit operator Size(Drawing.Size v) => new Size(v.Width, v.Height);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SizeF
        {
            public float Width, Height;

            public SizeF(float width, float height)
            {
                Width = width;
                Height = height;
            }

            public static implicit operator Drawing.SizeF(SizeF v) => new Drawing.SizeF(v.Width, v.Height);

            public static implicit operator SizeF(Drawing.SizeF v) => new SizeF(v.Width, v.Height);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Point
        {
            public int X, Y;

            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }

            public static implicit operator Drawing.Point(Point v) => new Drawing.Point(v.X, v.Y);

            public static implicit operator Point(Drawing.Point v) => new Point(v.X, v.Y);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PointF
        {
            public float X, Y;

            public PointF(float x, float y)
            {
                X = x;
                Y = y;
            }

            public static implicit operator Drawing.PointF(PointF v) => new Drawing.PointF(v.X, v.Y);

            public static implicit operator PointF(Drawing.PointF v) => new PointF(v.X, v.Y);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Rectangle
        {
            public int X, Y, Width, Height;

            public Rectangle(int x, int y, int width, int height)
            {
                X = x;
                Y = y;
                Width = width;
                Height = height;
            }

            public static implicit operator Drawing.Rectangle(Rectangle v) => new Drawing.Rectangle(v.X, v.Y, v.Width, v.Height);

            public static implicit operator Rectangle(Drawing.Rectangle v) => new Rectangle(v.X, v.Y, v.Width, v.Height);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RectangleF
        {
            public float X, Y, Width, Height;

            public RectangleF(float x, float y, float width, float height)
            {
                X = x;
                Y = y;
                Width = width;
                Height = height;
            }

            public static implicit operator Drawing.RectangleF(RectangleF v) => new Drawing.RectangleF(v.X, v.Y, v.Width, v.Height);

            public static implicit operator RectangleF(Drawing.RectangleF v) => new RectangleF(v.X, v.Y, v.Width, v.Height);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Thickness
        {
            public float Left, Top, Right, Bottom;

            public Thickness(float left, float top, float right, float bottom)
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