using ApiCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ApiGenerator.Api
{
    enum ApiTypeCreationMode
    {
        ManagedApiClass,
        ManagedPInvokeClass,
        NativeCApi,
        NativeCppApi
    }
}