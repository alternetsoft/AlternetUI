using ApiCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ApiGenerator.Api
{
    [Flags]
    internal enum ApiPropertyFlags
    {
        None = 0,
        ManagedArrayAccessor = 1 << 0
    }
}