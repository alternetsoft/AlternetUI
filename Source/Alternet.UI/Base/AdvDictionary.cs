using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class AdvDictionary<TKey, TValue> : Dictionary<TKey, TValue>
        where TKey : notnull
    {
    }
}
