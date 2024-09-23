using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Abstract class which defines methods allowing to map values
    /// of <typeparamref name="TSource"/> enum type
    /// to <typeparamref name="TDest"/> enum type.
    /// </summary>
    /// <typeparam name="TSource">Type of source enum.</typeparam>
    /// <typeparam name="TDest">Type of destination enum.</typeparam>
    public abstract class AbstractEnumMapping<TSource, TDest> : BaseObject
        where TSource : struct, Enum
        where TDest : struct, Enum
    {
        /// <summary>
        /// Maximal value of the source enum as integer.
        /// </summary>
        public abstract int SourceMaxValueAsInt { get; }

        /// <summary>
        /// Maximal value of the source enum.
        /// </summary>
        public abstract TSource SourceMaxValue { get; }

        /// <summary>
        /// Adds enum mapping.
        /// </summary>
        /// <param name="from">From value.</param>
        /// <param name="to">To value.</param>
        public abstract void Add(TSource from, TDest? to);

        /// <summary>
        /// Removes enum mapping.
        /// </summary>
        /// <param name="from"></param>
        public abstract void Remove(TSource from);

        /// <summary>
        /// Converts source enum value to the destination enum value.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="defaultValue">Default value used if mapping not found.</param>
        /// <returns></returns>
        public abstract TDest Convert(TSource value, TDest defaultValue = default);

        /// <summary>
        /// Logs enum mappings to file.
        /// </summary>
        public abstract void LogToFile(string? logFileName = default);

        /// <summary>
        /// Converts source enum value to the destination enum value if mapping exists.
        /// Returns <c>null</c> if no mapping is specified.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns></returns>
        public abstract TDest? ConvertOrNull(TSource value);

        /// <summary>
        /// Gets whether source to destination mapping is registered.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns></returns>
        public abstract bool HasMapping(TSource value);
    }
}
