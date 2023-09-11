using System.Runtime.InteropServices;
using System.Security;

namespace Alternet.UI.Native
{
    [SuppressUnmanagedCodeSecurity]
    internal static class NativeApiTypes
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct Color
        {
            public static readonly Color Empty = new();

            public byte R;
            public byte G;
            public byte B;
            public byte A;
            private readonly byte state;

            public Color(byte r, byte g, byte b, byte a)
            {
                R = r;
                G = g;
                B = b;
                A = a;
                state = 1;
            }

            public readonly bool IsEmpty => state == 0;

            public static implicit operator Alternet.Drawing.Color(Color v)
            {
                if (v.IsEmpty)
                    return Alternet.Drawing.Color.Empty;
                else
                    return Alternet.Drawing.Color.FromArgb(v.A, v.R, v.G, v.B);
            }

            public static implicit operator Color(Alternet.Drawing.Color color)
            {
                if (color.IsEmpty)
                    return Color.Empty;
                else
                    return new Color(color.R, color.G, color.B, color.A);
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
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

            public static implicit operator System.DateTime(DateTime v)
            {
                if (v.Year == 0)
                    return System.DateTime.MinValue;
                return new(v.Year, v.Month, v.Day, v.Hour, v.Minute, v.Second, v.Millisecond);
            }

            public static implicit operator DateTime(System.DateTime v) =>
                new(v.Year, v.Month, v.Day, v.Hour, v.Minute, v.Second,
                    v.Millisecond);
        }
    }
}