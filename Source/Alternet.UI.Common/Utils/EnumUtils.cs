using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public static class EnumUtils
    {
        public static T GetMaxValue<T>()
            where T: struct, Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>().Last();
        }

        public static int GetMaxValueAsInt<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<int>().Last();
        }
    }
}
