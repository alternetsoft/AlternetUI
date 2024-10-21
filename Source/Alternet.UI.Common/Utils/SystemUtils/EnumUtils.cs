using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties related to enums.
    /// </summary>
    public static class EnumUtils
    {
        /// <summary>
        /// Converts <see cref="ModalResult"/> to <see cref="DialogResult"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DialogResult Convert(ModalResult value)
        {
            switch (value)
            {
                case ModalResult.None:
                default:
                    return DialogResult.None;
                case ModalResult.Canceled:
                    return DialogResult.Cancel;
                case ModalResult.Accepted:
                    return DialogResult.OK;
            }
        }

        /// <summary>
        /// Gets maximum value of the enum using max enum element.
        /// </summary>
        /// <typeparam name="T">Type of the enum.</typeparam>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetMaxValueUseMax<T>()
            where T : struct, Enum
        {
            var result = Enum.GetValues(typeof(T)).Cast<T>().Max();
            return result;
        }

        /// <summary>
        /// Gets whether the specified enum value is the largest.
        /// This method uses last enum element to gets maximum value of the enum.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns></returns>
        public static bool IsMaxValueUseLast<T>(T value)
            where T : struct, Enum
        {
            return value.Equals(EnumUtils.GetMaxValueUseLast<T>());
        }

        /// <summary>
        /// Gets maximum value of the enum using last enum element.
        /// </summary>
        /// <typeparam name="T">Type of the enum.</typeparam>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetMaxValueUseLast<T>()
            where T : struct, Enum
        {
            var enumValues = Enum.GetValues(typeof(T));
            var result = (T)enumValues.GetValue(enumValues.Length - 1);
            return result;
        }

        /// <summary>
        /// Returns <paramref name="maxValue"/> if it's specified; otherwise uses
        /// <see cref="GetMaxValueUseLastAsInt{T}()"/> in order to obtain the result.
        /// </summary>
        /// <typeparam name="T">Type of the enum.</typeparam>
        /// <param name="maxValue">Max value in the enum.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetMaxValueUseLastOrDefault<T>(T? maxValue)
            where T : struct, Enum
        {
            int result;

            if (maxValue is null)
                result = EnumUtils.GetMaxValueUseLastAsInt<T>();
            else
                result = System.Convert.ToInt32(maxValue.Value);

            return result;
        }

        /// <summary>
        /// Gets maximum value of the enum as integer using last enum element.
        /// </summary>
        /// <typeparam name="T">Type of the enum.</typeparam>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetMaxValueUseLastAsInt<T>()
            where T : struct, Enum
        {
            var result = GetMaxValueUseLast<T>();
            return System.Convert.ToInt32(result);
        }

        /// <summary>
        /// Gets maximum value of the enum as integer using max enum element.
        /// </summary>
        /// <typeparam name="T">Type of the enum.</typeparam>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetMaxValueUseMaxAsInt<T>()
            where T : struct, Enum
        {
            var result = Enum.GetValues(typeof(T)).Cast<int>().Max();
            return result;
        }
    }
}