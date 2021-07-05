using ApiCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ApiGenerator.Api
{
    internal class ApiType
    {
        public ApiType(Type type, ApiProperty[] properties, ApiMethod[] methods)
        {
            Type = type;
            Properties = properties;
            Methods = methods;
        }

        public Type Type { get; }
        
        public ApiProperty[] Properties { get; }
        
        public ApiMethod[] Methods { get; }
    }
}