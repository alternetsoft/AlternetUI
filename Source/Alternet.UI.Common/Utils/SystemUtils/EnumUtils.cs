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
        /// Converts the values of a specified enumeration type
        /// into a collection of <see cref="ListControlItem"/>
        /// objects.
        /// </summary>
        /// <remarks>Each <see cref="ListControlItem"/> in the returned collection
        /// has its <see cref="ListControlItem.Value"/> property set to the enumeration
        /// value and its <see cref="ListControlItem.Text"/> property set to the
        /// string representation provided by <paramref name="valueToString"/>.</remarks>
        /// <param name="enumType">The type of the enumeration to convert.
        /// Must be a valid enumeration type. If <c>null</c>, the method returns
        /// <c>null</c>.</param>
        /// <param name="valueToString">A function that converts an enumeration value
        /// to its string representation. Cannot be <c>null</c>.</param>
        /// <param name="isValueIncluded">A function that determines whether an enumeration
        /// value is included in the result collection.</param>
        /// <returns>A <see cref="BaseCollection{T}"/> containing
        /// <see cref="ListControlItem"/> objects, where each item
        /// represents a value from the specified enumeration. Returns <c>null</c>
        /// if <paramref name="enumType"/> is <c>null</c>.</returns>
        public static BaseCollection<ListControlItem>? GetEnumItemsAsListItems(
            Type? enumType,
            Func<object?, string?>? valueToString = null,
            Func<object?, bool>? isValueIncluded = null)
        {
            if (enumType is null)
                return null;

            valueToString ??= (obj) =>
            {
                return obj?.ToString();
            };

            var collection = new BaseCollection<ListControlItem>();
            var values = Enum.GetValues(enumType);
            foreach (var v in values)
            {
                if(isValueIncluded != null && !isValueIncluded(v))
                    continue;

                ListControlItem item = new();
                item.Value = v;
                item.Text = valueToString(v) ?? string.Empty;
                collection.Add(item);
            }

            return collection;
        }

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
            var result = (T)enumValues.GetValue(enumValues.Length - 1)!;
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