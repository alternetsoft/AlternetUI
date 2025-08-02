using System.Diagnostics;
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

            public Color(Alternet.Drawing.Color color)
            {
                if (color is null || color.IsEmpty)
                    return;
                color.GetArgbValues(out A, out R, out G, out B);
                state = 1;
            }

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
                    return new Alternet.Drawing.Color(v.A, v.R, v.G, v.B);
            }

            public static explicit operator Alternet.Drawing.ColorStruct(Color v)
            {
                if (v.IsEmpty)
                    return Alternet.Drawing.ColorStruct.Default;
                else
                    return new Alternet.Drawing.ColorStruct(v.A, v.R, v.G, v.B);
            }

            public static implicit operator Color(Alternet.Drawing.Color color)
            {
                return new(color);
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