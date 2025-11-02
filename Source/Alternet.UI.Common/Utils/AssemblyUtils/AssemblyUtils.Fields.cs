using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Alternet.UI
{
    public partial class AssemblyUtils
    {
        /// <summary>
        /// Gets initialized instance of the <see cref="BaseObject"/> for the different purposes.
        /// </summary>
        public static readonly BaseObject Default = new();

        /// <summary>
        /// Gets <c>true</c> value as <see cref="object"/>.
        /// </summary>
        public static readonly object True = true;

        /// <summary>
        /// Gets <c>false</c> value as <see cref="object"/>.
        /// </summary>
        public static readonly object False = false;

        /// <summary>
        /// Gets maximal possible variable value for the <see cref="sbyte"/> type
        /// as <see cref="object"/>.
        /// </summary>
        public static readonly object MaxValueSByte = sbyte.MaxValue;

        /// <summary>
        /// Gets maximal possible variable value for the <see cref="byte"/> type
        /// as <see cref="object"/>.
        /// </summary>
        public static readonly object MaxValueByte = byte.MaxValue;

        /// <summary>
        /// Gets maximal possible variable value for the <see cref="short"/> type
        /// as <see cref="object"/>.
        /// </summary>
        public static readonly object MaxValueInt16 = short.MaxValue;

        /// <summary>
        /// Gets maximal possible variable value for the <see cref="ushort"/> type
        /// as <see cref="object"/>.
        /// </summary>
        public static readonly object MaxValueUInt16 = ushort.MaxValue;

        /// <summary>
        /// Gets maximal possible variable value for the <see cref="int"/> type
        /// as <see cref="object"/>.
        /// </summary>
        public static readonly object MaxValueInt32 = int.MaxValue;

        /// <summary>
        /// Gets maximal possible variable value for the <see cref="uint"/> type
        /// as <see cref="object"/>.
        /// </summary>
        public static readonly object MaxValueUInt32 = uint.MaxValue;

        /// <summary>
        /// Gets maximal possible variable value for the <see cref="long"/> type
        /// as <see cref="object"/>.
        /// </summary>
        public static readonly object MaxValueInt64 = long.MaxValue;

        /// <summary>
        /// Gets maximal possible variable value for the <see cref="ulong"/> type
        /// as <see cref="object"/>.
        /// </summary>
        public static readonly object MaxValueUInt64 = ulong.MaxValue;

        /// <summary>
        /// Gets maximal possible variable value for the <see cref="float"/> type
        /// as <see cref="object"/>.
        /// </summary>
        public static readonly object MaxValueSingle = float.MaxValue;

        /// <summary>
        /// Gets maximal possible variable value for the <see cref="double"/> type
        /// as <see cref="object"/>.
        /// </summary>
        public static readonly object MaxValueDouble = double.MaxValue;

        /// <summary>
        /// Gets maximal possible variable value for the <see cref="decimal"/> type
        /// as <see cref="object"/>.
        /// </summary>
        public static readonly object MaxValueDecimal = decimal.MaxValue;

        /// <summary>
        /// Gets maximal possible variable value for the <see cref="DateTime"/> type
        /// as <see cref="object"/>.
        /// </summary>
        public static readonly object MaxValueDateTime = DateTime.MaxValue;

        /// <summary>
        /// Gets minimal possible variable value for the <see cref="sbyte"/> type
        /// as <see cref="object"/>.
        /// </summary>
        public static readonly object MinValueSByte = sbyte.MinValue;

        /// <summary>
        /// Gets minimal possible variable value for the <see cref="byte"/> type
        /// as <see cref="object"/>.
        /// </summary>
        public static readonly object MinValueByte = byte.MinValue;

        /// <summary>
        /// Gets minimal possible variable value for the <see cref="short"/> type
        /// as <see cref="object"/>.
        /// </summary>
        public static readonly object MinValueInt16 = short.MinValue;

        /// <summary>
        /// Gets minimal possible variable value for the <see cref="ushort"/> type
        /// as <see cref="object"/>.
        /// </summary>
        public static readonly object MinValueUInt16 = ushort.MinValue;

        /// <summary>
        /// Gets minimal possible variable value for the <see cref="int"/> type
        /// as <see cref="object"/>.
        /// </summary>
        public static readonly object MinValueInt32 = int.MinValue;

        /// <summary>
        /// Gets minimal possible variable value for the <see cref="uint"/> type
        /// as <see cref="object"/>.
        /// </summary>
        public static readonly object MinValueUInt32 = uint.MinValue;

        /// <summary>
        /// Gets minimal possible variable value for the <see cref="long"/> type
        /// as <see cref="object"/>.
        /// </summary>
        public static readonly object MinValueInt64 = long.MinValue;

        /// <summary>
        /// Gets minimal possible variable value for the <see cref="ulong"/> type
        /// as <see cref="object"/>.
        /// </summary>
        public static readonly object MinValueUInt64 = ulong.MinValue;

        /// <summary>
        /// Gets minimal possible variable value for the <see cref="float"/> type
        /// as <see cref="object"/>.
        /// </summary>
        public static readonly object MinValueSingle = float.MinValue;

        /// <summary>
        /// Gets minimal possible variable value for the <see cref="double"/> type
        /// as <see cref="object"/>.
        /// </summary>
        public static readonly object MinValueDouble = double.MinValue;

        /// <summary>
        /// Gets minimal possible variable value for the <see cref="decimal"/> type
        /// as <see cref="object"/>.
        /// </summary>
        public static readonly object MinValueDecimal = decimal.MinValue;

        /// <summary>
        /// Gets minimal possible variable value for the <see cref="DateTime"/> type
        /// as <see cref="object"/>.
        /// </summary>
        public static readonly object MinValueDateTime = DateTime.MinValue;

        /// <summary>
        /// Gets default value for the <see cref="sbyte"/> type as <see cref="object"/>.
        /// </summary>
        public static readonly object DefaultSByte = default(sbyte);

        /// <summary>
        /// Gets default value for the <see cref="byte"/> type as <see cref="object"/>.
        /// </summary>
        public static readonly object DefaultByte = default(byte);

        /// <summary>
        /// Gets default value for the <see cref="short"/> type as <see cref="object"/>.
        /// </summary>
        public static readonly object DefaultInt16 = default(short);

        /// <summary>
        /// Gets default value for the <see cref="ushort"/> type as <see cref="object"/>.
        /// </summary>
        public static readonly object DefaultUInt16 = default(ushort);

        /// <summary>
        /// Gets default value for the <see cref="int"/> type as <see cref="object"/>.
        /// </summary>
        public static readonly object DefaultInt32 = default(int);

        /// <summary>
        /// Gets default value for the <see cref="uint"/> type as <see cref="object"/>.
        /// </summary>
        public static readonly object DefaultUInt32 = default(uint);

        /// <summary>
        /// Gets default value for the <see cref="long"/> type as <see cref="object"/>.
        /// </summary>
        public static readonly object DefaultInt64 = default(long);

        /// <summary>
        /// Gets default value for the <see cref="ulong"/> type as <see cref="object"/>.
        /// </summary>
        public static readonly object DefaultUInt64 = default(ulong);

        /// <summary>
        /// Gets default value for the <see cref="float"/> type as <see cref="object"/>.
        /// </summary>
        public static readonly object DefaultSingle = default(float);

        /// <summary>
        /// Gets default value for the <see cref="double"/> type as <see cref="object"/>.
        /// </summary>
        public static readonly object DefaultDouble = default(double);

        /// <summary>
        /// Gets default value for the <see cref="decimal"/> type as <see cref="object"/>.
        /// </summary>
        public static readonly object DefaultDecimal = default(decimal);

        /// <summary>
        /// Gets default value for the <see cref="DateTime"/> type as <see cref="object"/>.
        /// </summary>
        public static readonly object DefaultDateTime = default(DateTime);

        private static PropertyInfo? specialDummyPropertyInfo;

        /// <summary>
        /// Special dummy property for the internal purposes.
        /// </summary>
        public static object? SpecialDummyProperty { get; set; }

        /// <summary>
        /// Property info for the <see cref="SpecialDummyProperty"/>.
        /// </summary>
        public static PropertyInfo SpecialDummyPropertyInfo
        {
            get
            {
                return specialDummyPropertyInfo
                    ??= typeof(AssemblyUtils).GetProperty(nameof(SpecialDummyProperty))!;
            }
        }
    }
}
