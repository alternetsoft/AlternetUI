using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Defines structure with internal array which elements
    /// can be accessed using enum values as the indexes.
    /// </summary>
    /// <typeparam name="TKey">Type of the enum used as the index in the array.</typeparam>
    /// <typeparam name="TValue">Type of the array elements.</typeparam>
    internal readonly struct EnumArray<TKey, TValue>
        where TKey : struct, Enum, IConvertible
    {
        /// <summary>
        /// Array with data.
        /// </summary>
        public readonly TValue[] Data;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumArray{TKey, TValue}"/> struct.
        /// </summary>
        /// <param name="maxKeyValue">Maximal value in the enum. Optional. If not specified,
        /// <see cref="EnumUtils.GetMaxValueAsInt{T}()"/> is used.</param>
        public EnumArray(TKey maxKeyValue)
        {
            var m = EnumUtils.GetMaxValueOrDefault<TKey>(maxKeyValue);
            Data = new TValue[m + 1];
        }

        /// <summary>
        /// Gets or sets array element.
        /// </summary>
        /// <param name="index">Index of the array element.</param>
        /// <returns></returns>
        public TValue this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return Data[index];
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                Data[index] = value;
            }
        }
    }
}
