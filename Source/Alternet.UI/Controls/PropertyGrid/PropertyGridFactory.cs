using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class PropertyGridFactory : IPropertyGridFactory
    {
        public PropertyGridCreateStyle DefaultCreateStyle
        {
            get => PropertyGridUtils.DefaultCreateStyle;
            set => PropertyGridUtils.DefaultCreateStyle = value;
        }

        public bool SetCustomLabel<T>(string propName, string label)
            where T : class
        {
            return PropertyGridUtils.SetCustomLabel<T>(propName, label);
        }

        public IPropertyGridNewItemParams GetNewItemParams(Type type, PropertyInfo propInfo)
        {
            return PropertyGridUtils.GetNewItemParams(type, propInfo);
        }

        public IPropertyGridPropInfoRegistry GetPropRegistry(Type type, PropertyInfo propInfo)
        {
            return PropertyGridUtils.GetPropRegistry(type, propInfo);
        }

        public string? GetCustomLabel<T>(string propName)
            where T : class
        {
            return PropertyGridUtils.GetCustomLabel<T>(propName);
        }

        public IPropertyGridChoices GetChoices<T>()
            where T : Enum
        {
            return PropertyGridUtils.GetChoices<T>();
        }

        public IPropertyGridNewItemParams CreateNewItemParams(PropertyInfo? propInfo = null)
        {
            return PropertyGridUtils.CreateNewItemParams(null, propInfo);
        }

        public IPropertyGridChoices CreateChoices()
        {
            return PropertyGridUtils.CreateChoices();
        }

        public IPropertyGridChoices CreateChoices(Type enumType)
        {
            return PropertyGridUtils.CreateChoices(enumType);
        }

        public IPropertyGridChoices CreateChoicesOnce(Type enumType)
        {
            return PropertyGridUtils.CreateChoicesOnce(enumType);
        }

        public IPropertyGrid CreatePropertyGrid()
        {
            return PropertyGrid.CreatePropertyGrid();
        }

        public IPropertyGridTypeRegistry GetTypeRegistry(Type type)
        {
            return PropertyGridUtils.GetTypeRegistry(type);
        }

        public void RegisterPropCreateFunc(Type type, PropertyGridItemCreate func)
        {
            PropertyGridUtils.RegisterPropCreateFunc(type, func);
        }
    }
}
