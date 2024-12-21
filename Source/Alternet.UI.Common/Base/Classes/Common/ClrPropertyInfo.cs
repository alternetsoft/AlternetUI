using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Alternet.UI.Port
{
    internal class ClrPropertyInfo : IPropertyInfo
    {
        private readonly Func<object, object?>? getter;
        private readonly Action<object, object?>? setter;

        public ClrPropertyInfo(
            string name,
            Func<object, object?>? getter,
            Action<object, object?>? setter,
            Type propertyType)
        {
            this.getter = getter;
            this.setter = setter;
            PropertyType = propertyType;
            Name = name;
        }

        public bool CanSet => setter != null;

        public bool CanGet => getter != null;

        public string Name { get; }

        public Type PropertyType { get; }

        public object? Get(object target)
        {
            if (getter == null)
                throw new NotSupportedException($"Property '{Name}' doesn't have a getter");
            return getter(target);
        }

        public void Set(object target, object? value)
        {
            if (setter == null)
                throw new NotSupportedException($"Property '{Name}' doesn't have a setter");
            setter(target, value);
        }
    }
}