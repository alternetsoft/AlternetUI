using System.Runtime.InteropServices;
using System.Security;

namespace Alternet.UI.Native
{
    [SuppressUnmanagedCodeSecurity]
    static class NativeApiTypes
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

            public static implicit operator System.Drawing.Size(Size v) => new System.Drawing.Size(v.Width, v.Height);

            public static implicit operator Size(System.Drawing.Size v) => new Size(v.Width, v.Height);
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

            public static implicit operator System.Drawing.SizeF(SizeF v) => new System.Drawing.SizeF(v.Width, v.Height);

            public static implicit operator SizeF(System.Drawing.SizeF v) => new SizeF(v.Width, v.Height);
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

            public static implicit operator System.Drawing.Point(Point v) => new System.Drawing.Point(v.X, v.Y);

            public static implicit operator Point(System.Drawing.Point v) => new Point(v.X, v.Y);
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

            public static implicit operator System.Drawing.PointF(PointF v) => new System.Drawing.PointF(v.X, v.Y);

            public static implicit operator PointF(System.Drawing.PointF v) => new PointF(v.X, v.Y);
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

            public static implicit operator System.Drawing.Rectangle(Rectangle v) => new System.Drawing.Rectangle(v.X, v.Y, v.Width, v.Height);

            public static implicit operator Rectangle(System.Drawing.Rectangle v) => new Rectangle(v.X, v.Y, v.Width, v.Height);
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

            public static implicit operator System.Drawing.RectangleF(RectangleF v) => new System.Drawing.RectangleF(v.X, v.Y, v.Width, v.Height);

            public static implicit operator RectangleF(System.Drawing.RectangleF v) => new RectangleF(v.X, v.Y, v.Width, v.Height);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Color
        {
            public byte R, G, B, A;

            public Color(byte r, byte g, byte b, byte a)
            {
                R = r;
                G = g;
                B = b;
                A = a;
            }

            public static implicit operator System.Drawing.Color(Color v) => System.Drawing.Color.FromArgb(v.A, v.R, v.G, v.B);

            public static implicit operator Color(System.Drawing.Color color) => new Color(color.R, color.G, color.B, color.A);
        }
    }
}