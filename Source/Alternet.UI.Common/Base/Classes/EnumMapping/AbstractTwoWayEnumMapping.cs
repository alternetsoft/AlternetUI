using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Abstract class which extends <see cref="AbstractEnumMapping{TSource,TDest}"/>. It defines
    /// methods allowing to map values
    /// of <typeparamref name="TSource"/> enum type
    /// to/from <typeparamref name="TDest"/> enum type.
    /// </summary>
    /// <typeparam name="TSource">Type of source enum.</typeparam>
    /// <typeparam name="TDest">Type of destination enum.</typeparam>
    public abstract class AbstractTwoWayEnumMapping<TSource, TDest> : AbstractEnumMapping<TSource, TDest>
        where TSource : struct, Enum
        where TDest : struct, Enum
    {
        /// <summary>
        /// Removes dest to source enum mapping.
        /// </summary>
        /// <param name="value"></param>
        public abstract void Remove(TDest value);

        /// <summary>
        /// Converts <typeparamref name="TDest"/> enum value to <typeparamref name="TSource"/> enum value.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="defaultValue">Default value used if mapping not found.</param>
        /// <returns></returns>
        public abstract TSource Convert(TDest value, TSource defaultValue = default);
    }
}
