using ApiCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ApiGenerator.Api
{
    internal class ApiMethod
    {
        public ApiMethod(MethodInfo method)
        {
            Method = method;
        }

        public MethodInfo Method { get; }
    }
}