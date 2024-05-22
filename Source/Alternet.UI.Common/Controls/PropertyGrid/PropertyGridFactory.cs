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
            get => PropertyGrid.DefaultCreateStyle;
            set => PropertyGrid.DefaultCreateStyle = value;
        }

        public bool SetCustomLabel<T>(string propName, string label)
            where T : class
        {
            return PropertyGrid.SetCustomLabel<T>(propName, label);
        }

        public IPropertyGridNewItemParams GetNewItemParams(Type type, PropertyInfo propInfo)
        {
            return PropertyGrid.GetNewItemParams(type, propInfo);
        }

        public IPropertyGridPropInfoRegistry GetPropRegistry(Type type, PropertyInfo propInfo)
        {
            return PropertyGrid.GetPropRegistry(type, propInfo);
        }

        public string? GetCustomLabel<T>(string propName)
            where T : class
        {
            return PropertyGrid.GetCustomLabel<T>(propName);
        }

        public IPropertyGridChoices GetChoices<T>()
            where T : Enum
        {
            return PropertyGrid.GetChoices<T>();
        }

        public IPropertyGridNewItemParams CreateNewItemParams(PropertyInfo? propInfo = null)
        {
            return PropertyGrid.CreateNewItemParams(null, propInfo);
        }

        public IPropertyGridChoices CreateChoices()
        {
            return PropertyGrid.CreateChoices();
        }

        public IPropertyGridChoices CreateChoices(Type enumType)
        {
            return PropertyGrid.CreateChoices(enumType);
        }

        public IPropertyGridChoices CreateChoicesOnce(Type enumType)
        {
            return PropertyGrid.CreateChoicesOnce(enumType);
        }

        public IPropertyGrid CreatePropertyGrid()
        {
            return PropertyGrid.CreatePropertyGrid();
        }

        public IPropertyGridTypeRegistry GetTypeRegistry(Type type)
        {
            return PropertyGrid.GetTypeRegistry(type);
        }

        public void RegisterPropCreateFunc(Type type, PropertyGridItemCreate func)
        {
            PropertyGrid.RegisterPropCreateFunc(type, func);
        }
    }
}
