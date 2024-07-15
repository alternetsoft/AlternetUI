using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Defines methods allowing to map values
    /// of <typeparamref name="TSource"/> enum type
    /// to/from <typeparamref name="TDest"/> enum type.
    /// </summary>
    /// <typeparam name="TSource">Type of source enum.</typeparam>
    /// <typeparam name="TDest">Type of destination enum.</typeparam>
    public class TwoWayEnumMapping<TSource, TDest> : AbstractTwoWayEnumMapping<TSource, TDest>
        where TSource : struct, Enum
        where TDest : struct, Enum
    {
        private readonly AbstractEnumMapping<TSource, TDest> sourceToDest;
        private readonly AbstractEnumMapping<TDest, TSource> destToSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="TwoWayEnumMapping{TSource, TDest}"/> class.
        /// </summary>
        /// <param name="sourceMaxValue">Maximal value of the source enum. Optional.</param>
        /// <param name="destMaxValue">Maximal value of the destination enum. Optional.</param>
        /// <remarks>
        /// If maximal enum value is not specified, <see cref="EnumUtils.GetMaxValueAsInt{T}()"/> is used.
        /// </remarks>
        public TwoWayEnumMapping(TSource? sourceMaxValue = null, TDest? destMaxValue = null)
        {
            sourceToDest = CreateEnumMapping<TSource, TDest>(sourceMaxValue);
            destToSource = CreateEnumMapping<TDest, TSource>(destMaxValue);
        }

        /// <inheritdoc/>
        public override void Add(TSource from, TDest to)
        {
            sourceToDest.Add(from, to);
            destToSource.Add(to, from);
        }

        /// <inheritdoc/>
        public override void Remove(TSource from)
        {
            sourceToDest.Remove(from);
        }

        /// <inheritdoc/>
        public override void Remove(TDest to)
        {
            destToSource.Remove(to);
        }

        /// <inheritdoc/>
        public override TDest Convert(TSource value, TDest defaultValue = default)
        {
            return sourceToDest.Convert(value, defaultValue);
        }

        /// <inheritdoc/>
        public override TSource Convert(TDest value, TSource defaultValue = default)
        {
            return destToSource.Convert(value, defaultValue);
        }

        /// <summary>
        /// Creates enum mapping object.
        /// </summary>
        /// <typeparam name="T1">Type of source enum.</typeparam>
        /// <typeparam name="T2">Type of destination enum.</typeparam>
        /// <param name="maxValue">Maximal value of the source enum. Optional.
        /// If not specified, <see cref="EnumUtils.GetMaxValueAsInt{T}()"/> is used.</param>
        /// <returns></returns>
        protected virtual AbstractEnumMapping<T1, T2> CreateEnumMapping<T1, T2>(T1? maxValue)
            where T1 : struct, Enum
            where T2 : struct, Enum
        {
            return new EnumMapping<T1, T2>(maxValue);
        }
    }
}
