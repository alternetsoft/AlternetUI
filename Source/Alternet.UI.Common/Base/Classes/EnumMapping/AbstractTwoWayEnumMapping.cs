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
    public abstract class AbstractTwoWayEnumMapping<TSource, TDest> : DisposableObject
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
        /// Maximal value of the source enum as integer.
        /// </summary>
        public TSource SourceMaxValue => SourceToDest.SourceMaxValue;

        /// <summary>
        /// Maximal value of the destination enum as integer.
        /// </summary>
        public TDest DestMaxValue => DestToSource.SourceMaxValue;

        /// <summary>
        /// Maximal value of the source enum as integer.
        /// </summary>
        public int SourceMaxValueAsInt => SourceToDest.SourceMaxValueAsInt;

        /// <summary>
        /// Maximal value of the destination enum as integer.
        /// </summary>
        public int DestMaxValueAsInt => DestToSource.SourceMaxValueAsInt;

        /// <summary>
        /// Converts the source enum value to the destination enum value.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns></returns>
        public TDest Convert(TSource value)
        {
            return SourceToDest.Convert(value);
        }

        /// <summary>
        /// Converts the destination enum value to the source enum value.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns></returns>
        public TSource Convert(TDest value)
        {
            return DestToSource.Convert(value);
        }

        /// <summary>
        /// Registers one way mapping from the source type to the destionation type.
        /// </summary>
        public void AddOneWay(TSource from, TDest to)
        {
            SourceToDest.Add(from, to);
        }

        /// <summary>
        /// Registers one way mapping from the destination type to the source type.
        /// </summary>
        public void AddOneWay(TDest from, TSource to)
        {
            DestToSource.Add(from, to);
        }

        /// <summary>
        /// Registers two-way mapping.
        /// </summary>
        /// <param name="from">From value.</param>
        /// <param name="to">To value.</param>
        public void Add(TSource from, TDest? to)
        {
            SourceToDest.Add(from, to);
            if(to is not null)
                DestToSource.Add(to.Value, from);
        }

        /// <summary>
        /// Logs enum mappings to file.
        /// </summary>
        public virtual void LogToFile()
        {
            SourceToDest.LogToFile();
            DestToSource.LogToFile();
        }
    }
}
