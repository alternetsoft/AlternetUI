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
        /// Adds enum mapping.
        /// </summary>
        /// <param name="from">From value.</param>
        /// <param name="to">To value.</param>
        public abstract void Add(TSource from, TDest to);

        /// <summary>
        /// Removes enum mapping.
        /// </summary>
        /// <param name="from"></param>
        public abstract void Remove(TSource from);

        /// <summary>
        /// Converts source enum value to destination enum value.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="defaultValue">Default value used if mapping not found.</param>
        /// <returns></returns>
        public abstract TDest Convert(TSource value, TDest defaultValue = default);
    }
}
