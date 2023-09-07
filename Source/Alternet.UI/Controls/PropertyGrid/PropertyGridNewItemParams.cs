﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class PropertyGridNewItemParams : IPropertyGridNewItemParams
    {
        public static readonly IPropertyGridNewItemParams Default =
            PropertyGrid.CreateNewItemParams(null!);

        private PropertyInfo? propInfo;

        public PropertyGridNewItemParams(PropertyInfo? propInfo)
        {
            this.propInfo = propInfo;
        }

        public PropertyInfo? PropInfo { get => propInfo; set => propInfo = value; }

        public string? Label { get; set; }

        public bool? IsNullable { get; set; }
    }
}
