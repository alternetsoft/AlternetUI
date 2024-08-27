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
        /// Gets maximum value of the enum.
        /// </summary>
        /// <typeparam name="T">Type of the enum.</typeparam>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetMaxValue<T>()
            where T : struct, Enum
        {
            var enumValues = Enum.GetValues(typeof(T));
            var result = (T)enumValues.GetValue(enumValues.Length - 1);
            return result;
        }

        /// <summary>
        /// Returns <paramref name="maxValue"/> if it's specified; otherwise uses
        /// <see cref="GetMaxValueAsInt{T}()"/> in order to obtain the result.
        /// </summary>
        /// <typeparam name="T">Type of the enum.</typeparam>
        /// <param name="maxValue">Max value in the enum.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetMaxValueOrDefault<T>(T? maxValue)
            where T : struct, Enum
        {
            int result;

            if (maxValue is null)
                result = EnumUtils.GetMaxValueAsInt<T>();
            else
                result = System.Convert.ToInt32(maxValue.Value);

            return result;
        }

        /// <summary>
        /// Gets maximum value of the enum as integer.
        /// </summary>
        /// <typeparam name="T">Type of the enum.</typeparam>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetMaxValueAsInt<T>()
            where T : struct, Enum
        {
            var result = GetMaxValue<T>();
            return System.Convert.ToInt32(result);
        }
    }
}
