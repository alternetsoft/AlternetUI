using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Alternet.UI
{
    internal class PropertyValueSource : IValueSource<object>
    {
        private readonly PropertyInfo propertyInfo;
        private readonly object? propertyContainer;

        public PropertyValueSource(
            Type containerType,
            string propertyName)
        {
            propertyInfo = AssemblyUtils.GetPropertySafe(containerType, propertyName)
                ?? AssemblyUtils.SpecialDummyPropertyInfo;
        }

        public PropertyValueSource(
            object propertyContainer,
            string propertyName)
            : this(propertyContainer.GetType(), propertyName)
        {
            this.propertyContainer = propertyContainer;
        }

        public event EventHandler? ValueChanged;

        public Type ValueType => propertyInfo?.PropertyType ?? typeof(object);

        public PropertyInfo PropInfo => propertyInfo;

        public object? Value
        {
            get
            {
                var result = propertyInfo?.GetValue(propertyContainer);
                return result;
            }

            set
            {
                if (Value == value)
                    return;
                propertyInfo?.SetValue(propertyContainer, value);
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
