﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class PropertyGridPropInfoRegistry : IPropertyGridPropInfoRegistry
    {
        private readonly PropertyInfo propInfo;
        private readonly IPropertyGridTypeRegistry owner;
        private IPropertyGridNewItemParams? newItemParams;

        public PropertyGridPropInfoRegistry(IPropertyGridTypeRegistry owner, PropertyInfo propInfo)
        {
            this.propInfo = propInfo;
            this.owner = owner;
        }

        /// <summary>
        /// Gets or sets <see cref="CultureInfo"/> used for the conversion.
        /// </summary>
        public CultureInfo? Culture { get; set; }

        /// <summary>
        /// Gets or sets <see cref="ITypeDescriptorContext"/> used for the conversion.
        /// </summary>
        public ITypeDescriptorContext? Context { get; set; }

        /// <summary>
        /// Gets or sets whether to use invariant conversion.
        /// </summary>
        public bool? UseInvariantCulture { get; set; }

        public NumberStyles? NumberStyles { get; set; }

        public IFormatProvider? FormatProvider { get; set; }

        public IObjectToString? Converter { get; set; }

        public string? DefaultFormat { get; set; }

        public PropertyInfo PropInfo => propInfo;

        public IPropertyGridTypeRegistry Owner => owner;

        public IPropertyGridNewItemParams NewItemParams
        {
            get
            {
                newItemParams ??= PropertyGrid.CreateNewItemParams(this, propInfo);
                return newItemParams;
            }

            set
            {
                newItemParams = value;
            }
        }

        public bool HasNewItemParams => newItemParams != null;

        public Type? ListEditSourceType { get; set; }
    }
}
