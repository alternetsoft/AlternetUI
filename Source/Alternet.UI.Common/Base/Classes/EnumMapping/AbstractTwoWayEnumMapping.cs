using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public abstract class AbstractTwoWayEnumMapping<TSource, TDest> : AbstractEnumMapping<TSource, TDest>
        where TSource : struct, Enum
        where TDest : struct, Enum
    {
        public abstract void Remove(TDest value);

        public abstract TSource Convert(TDest value, TSource defaultValue = default);
    }
}
