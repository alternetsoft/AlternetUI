using System;
using System.ComponentModel;

namespace Alternet.UI.Port
{
    /// <summary>
    /// This is a helper class used by the DateTimeConverter2 to call the DateTimeValueSerializer.
    /// It provides no functionality.
    /// </summary>
    internal class DateTimeValueSerializerContext : IValueSerializerContext
    {
        public PropertyDescriptor? PropertyDescriptor
        {
            get
            {
                return null;
            }
        }

        public IContainer? Container
        {
            get
            {
                return null;
            }
        }

        public object? Instance
        {
            get
            {
                return null;
            }
        }

        public ValueSerializer? GetValueSerializerFor(PropertyDescriptor descriptor)
        {
            return null;
        }

        public ValueSerializer? GetValueSerializerFor(Type type)
        {
            return null;
        }

        public void OnComponentChanged()
        {
        }

        public bool OnComponentChanging()
        {
            return false;
        }

        public object? GetService(Type serviceType)
        {
            return null;
        }
    }
}