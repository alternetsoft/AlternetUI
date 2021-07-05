using ApiCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ApiGenerator.Api
{
    internal class ApiProperty
    {
        public ApiProperty(PropertyInfo property, ApiPropertyFlags flags)
        {
            Property = property;
            Flags = flags;
        }

        public PropertyInfo Property { get; }

        public ApiPropertyFlags Flags { get; }
    }
}