﻿using System;
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
    public struct EnumArray<TKey, TValue>
        where TKey : struct, Enum, IConvertible
    {
        /// <summary>
        /// Array with data.
        /// </summary>
        public TValue[] Data;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumArray{TKey, TValue}"/> struct.
        /// </summary>
        public EnumArray()
            : this(EnumUtils.GetMaxValueUseMax<TKey>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumArray{TKey, TValue}"/> struct.
        /// </summary>
        /// <param name="maxKeyValue">Maximal value in the enum.</param>
        public EnumArray(TKey maxKeyValue)
        {
            var maxKeyValueAsInt = System.Convert.ToInt32(maxKeyValue);
            Data = new TValue[maxKeyValueAsInt + 1];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumArray{TKey, TValue}"/> struct.
        /// </summary>
        /// <param name="maxKeyValue">Maximal value in the enum.</param>
        public EnumArray(int maxKeyValue)
        {
            Data = new TValue[maxKeyValue + 1];
        }

        /// <summary>
        /// Gets or sets array element.
        /// </summary>
        /// <param name="index">Index of the array element.</param>
        /// <returns></returns>
        public TValue this[TKey index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return Data[System.Convert.ToInt32(index)];
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                Data[System.Convert.ToInt32(index)] = value;
            }
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

        /// <summary>
        /// Sets the same value to all the items with the specified keys.
        /// </summary>
        /// <param name="keys">Array of keys.</param>
        public TValue this[TKey[] keys]
        {
            set
            {
                SetValues(value, keys);
            }
        }

        /// <summary>
        /// Resets all data.
        /// </summary>
        public void Reset()
        {
            Data = new TValue[Data.Length];
        }

        /// <summary>
        /// Sets the same value to all the items with the specified keys.
        /// </summary>
        /// <param name="keys">Array of keys.</param>
        /// <param name="value">Value to set.</param>
        public void SetValues(TValue value, TKey[] keys)
        {
            foreach (var key in keys)
                this[key] = value;
        }

        /// <summary>
        /// Assigns properties of this object with the properties of another object.
        /// </summary>
        /// <param name="assignFrom">Object which properties will be assigned to this object.</param>
        public void Assign(EnumArray<TKey, TValue> assignFrom)
        {
            var length = Math.Min(Data.Length, assignFrom.Data.Length);
            Array.Copy(assignFrom.Data, 0, Data, 0, length);
        }

        /// <summary>
        /// Creates clone of this object.
        /// </summary>
        /// <returns></returns>
        public EnumArray<TKey, TValue> Clone()
        {
            EnumArray<TKey, TValue> result = new();
            result.Assign(this);
            return result;
        }
    }
}
