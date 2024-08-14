using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Abstract class which extends <see cref="AbstractEnumMapping{TSource,TDest}"/>. It defines
    /// methods and properties allowing to map values
    /// of <typeparamref name="TSource"/> enum type
    /// to/from <typeparamref name="TDest"/> enum type.
    /// </summary>
    /// <typeparam name="TSource">Type of source enum.</typeparam>
    /// <typeparam name="TDest">Type of destination enum.</typeparam>
    public abstract class AbstractTwoWayEnumMapping<TSource, TDest> : BaseObject
        where TSource : struct, Enum
        where TDest : struct, Enum
    {
        /// <summary>
        /// Source to destination mapping.
        /// </summary>
        public abstract AbstractEnumMapping<TSource, TDest> SourceToDest { get; }

        /// <summary>
        /// Destination to source mapping.
        /// </summary>
        public abstract AbstractEnumMapping<TDest, TSource> DestToSource { get; }

        /// <summary>
        /// Registers two-way mapping.
        /// </summary>
        /// <param name="from">From value.</param>
        /// <param name="to">To value.</param>
        public virtual void Add(TSource from, TDest to)
        {
            SourceToDest.Add(from, to);
            DestToSource.Add(to, from);
        }

        /// <summary>
        /// Logs enum mappings to file.
        /// </summary>
        public virtual void Log()
        {
            SourceToDest.Log();
            DestToSource.Log();
        }
    }
}
