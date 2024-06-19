using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class TwoWayEnumMapping<TSource, TDest> : AbstractTwoWayEnumMapping<TSource, TDest>
        where TSource : struct, Enum
        where TDest : struct, Enum
    {
        private readonly AbstractEnumMapping<TSource, TDest> sourceToDest;
        private readonly AbstractEnumMapping<TDest, TSource> destToSource;

        public TwoWayEnumMapping(TSource? sourceMaxValue = null, TDest? destMaxValue = null)
        {
            sourceToDest = CreateEnumMapping<TSource, TDest>(sourceMaxValue);
            destToSource = CreateEnumMapping<TDest, TSource>(destMaxValue);
        }

        public override void Add(TSource from, TDest to)
        {
            sourceToDest.Add(from, to);
            destToSource.Add(to, from);
        }

        public override void Remove(TSource from)
        {
            sourceToDest.Remove(from);
        }

        public override void Remove(TDest to)
        {
            destToSource.Remove(to);
        }

        public override TDest Convert(TSource value, TDest defaultValue = default)
        {
            return sourceToDest.Convert(value, defaultValue);
        }

        public override TSource Convert(TDest value, TSource defaultValue = default)
        {
            return destToSource.Convert(value, defaultValue);
        }

        protected virtual AbstractEnumMapping<T1, T2> CreateEnumMapping<T1, T2>(T1? maxValue)
            where T1 : struct, Enum
            where T2 : struct, Enum
        {
            return new EnumMapping<T1, T2>(maxValue);
        }
    }
}
