using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public abstract class AbstractEnumMapping<TSource, TDest> : BaseObject
        where TSource : struct, Enum
        where TDest : struct, Enum
    {
        public abstract void Add(TSource from, TDest to);

        public abstract void Remove(TSource from);

        public abstract TDest Convert(TSource value, TDest defaultValue = default);
    }
}
