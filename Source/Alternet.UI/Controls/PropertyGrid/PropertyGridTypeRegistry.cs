﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class PropertyGridTypeRegistry : IPropertyGridTypeRegistry
    {
        private readonly AdvDictionary<PropertyInfo, IPropertyGridPropInfoRegistry> registry = new();
        private readonly Type type;
        private IPropertyGridTypeRegistry? baseTypeRegistry;
        private List<(string, Action)>? simpleActions;

        public PropertyGridTypeRegistry(Type type)
        {
            this.type = type;
        }

        /// <summary>
        /// Occurs when button is clicked in the property editor.
        /// </summary>
        public event EventHandler? ButtonClick;

        /// <summary>
        /// Gets or sets whether property editor has ellipsis button.
        /// </summary>
        public bool? HasEllipsis { get; set; }

        public NumberStyles? NumberStyles { get; set; }

        public IFormatProvider? FormatProvider { get; set; }

        public IObjectToString? Converter { get; set; }

        public string? DefaultFormat { get; set; }

        public IPropertyGridTypeRegistry? BaseTypeRegistry
        {
            get
            {
                if(baseTypeRegistry == null)
                {
                    if (type == typeof(object))
                        return null;
                    Type? baseType = type.BaseType;
                    if (baseType == null)
                        return null;
                    else
                        baseTypeRegistry = PropertyGrid.GetTypeRegistry(baseType);
                    return baseTypeRegistry;
                }

                return baseTypeRegistry;
            }
        }

        public Type InstanceType => type;

        public PropertyGridItemCreate? CreateFunc { get; set; }

        public IPropertyGridPropInfoRegistry? GetPropRegistryOrNull(PropertyInfo propInfo)
        {
            return registry.GetValueOrDefault(propInfo);
        }

        public IPropertyGridPropInfoRegistry GetPropRegistry(string propName)
        {
            return GetPropRegistry(type.GetProperty(propName)!);
        }

        public IPropertyGridPropInfoRegistry? GetPropRegistryOrNull(string propName)
        {
            var propInfo = type.GetProperty(propName);
            if (propInfo == null)
                return null;
            return GetPropRegistryOrNull(propInfo);
        }

        public IPropertyGridPropInfoRegistry GetPropRegistry(PropertyInfo propInfo)
        {
            return registry.GetOrCreate(
                propInfo,
                () => { return new PropertyGridPropInfoRegistry(this, propInfo); });
        }

        public void AddSimpleAction(string name, Action action)
        {
            simpleActions ??= new();
            simpleActions.Add((name, action));
        }

        public IEnumerable<(string, Action)>? GetSimpleActions()
        {
            return simpleActions;
        }

        /// <summary>
        /// Raises <see cref="ButtonClick"/> event.
        /// </summary>
        public void RaiseButtonClick(IPropertyGridItem item)
        {
            ButtonClick?.Invoke(item, EventArgs.Empty);
        }

        private class NullType
        {
        }
    }
}
